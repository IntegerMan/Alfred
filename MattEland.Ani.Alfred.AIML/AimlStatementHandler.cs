// ---------------------------------------------------------
// AimlStatementHandler.cs
// 
// Created on:      08/19/2015 at 9:31 PM
// Last Modified:   08/30/2015 at 4:37 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml;
using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Common;
using MattEland.Common.Providers;

namespace MattEland.Ani.Alfred.Chat
{
    /// <summary>
    ///     Handles user statements by parsing the statement through an open source AIML engine.
    /// </summary>
    /// <remarks>
    ///     AIML stands for Artificial Intelligence Markup Language.
    /// </remarks>
    public sealed class AimlStatementHandler : IChatProvider, INotifyPropertyChanged
    {

        [NotNull]
        private readonly ChatHandlersProvider _chatHandlerProvider;

        [NotNull]
        private readonly ChatHistoryProvider _chatHistory;

        [NotNull]
        private readonly User _user;

        [CanBeNull]
        private IConsole _console;

        private UserStatementResponse _response;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object" /> class using the current
        ///     directory for the settings path.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <param name="container"> The container to use for inversion of control. </param>
        /// <param name="engineName"> Name of the chat engine. </param>
        public AimlStatementHandler(
            [NotNull] IObjectContainer container,
            [NotNull] string engineName)
        {
            //- Validate
            if (engineName.IsEmpty()) { throw new ArgumentNullException(nameof(engineName)); }
            if (container == null) { throw new ArgumentNullException(nameof(container)); }

            // Set up simple internal fields
            Container = container;
            _console = container.TryProvide<IConsole>();
            _chatHistory = new ChatHistoryProvider(container);

            // Create and set up the chat engine
            ChatEngine = new ChatEngine(container, engineName);

            // Register the engine so other places can find it
            ChatEngine.RegisterAsProvidedInstance(container);

            // Create basic chat helpers / ancillary classes
            _user = new User(Resources.ChatUserName.NonNull());
            _chatHandlerProvider = new ChatHandlersProvider(container, ChatEngine);

            InitializeChatEngine();
        }

        /// <summary>
        ///     Gets the container.
        /// </summary>
        /// <value>
        ///     The container.
        /// </value>
        [NotNull]
        public IObjectContainer Container { get; }

        /// <summary>
        ///     Gets the console.
        /// </summary>
        /// <value>
        ///     The console.
        /// </value>
        [CanBeNull]
        internal IConsole Console
        {
            [DebuggerStepThrough]
            [UsedImplicitly]
            get
            {
                return _console;
            }
            [DebuggerStepThrough]
            set
            {
                if (Equals(value, _console)) { return; }

                _console = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Gets the chat engine.
        /// </summary>
        /// <remarks>
        ///     This is present largely for testing.
        /// </remarks>
        /// <value>
        ///     The chat engine.
        /// </value>
        [NotNull]
        public ChatEngine ChatEngine { get; }

        /// <summary>
        ///     Gets the property providers representing broad chat categories.
        /// </summary>
        /// <value>
        ///     The property providers.
        /// </value>
        [NotNull]
        [ItemNotNull]
        internal IEnumerable<IPropertyProvider> PropertyProviders
        {
            get
            {
                yield return _chatHistory;
                yield return _chatHandlerProvider;
            }
        }

        /// <summary>
        ///     Handles a user statement.
        /// </summary>
        /// <param name="userInput"> The user input. </param>
        /// <returns>
        ///     The <see cref="UserStatementResponse" />.
        /// </returns>
        public UserStatementResponse HandleUserStatement([CanBeNull] string userInput)
        {
            //- Trim for housekeeping purposes since this will be displayed / stored.
            userInput = userInput.NonNull().Trim();

            return HandleUserStatementInternal(userInput);
        }

        /// <summary>
        ///     Gets the last input from the user.
        /// </summary>
        /// <value>
        ///     The last input.
        /// </value>
        [CanBeNull]
        public string LastInput
        {
            get
            {
                var entry = _chatHistory.GetLastMessageFromUser(_user);
                return entry?.Message;
            }
        }

        /// <summary>
        ///     Gets the last response from the system.
        /// </summary>
        /// <value>
        ///     The last response.
        /// </value>
        [CanBeNull]
        public UserStatementResponse LastResponse
        {
            get { return _response; }
            private set
            {
                if (Equals(value, _response)) { return; }
                _response = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Occurs when a property changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     The internal implementation of <see cref="AimlStatementHandler.HandleUserStatement" />
        ///     which handles input text by submitting it to the chat engine as if from the current user
        ///     and then returning the result. This internal implementation also has the ability to not
        ///     save this statement to the log. This can be useful for automated events needing to
        ///     interact with the
        ///     <see cref="ChatEngine" /> as if they were from the user.
        /// </summary>
        /// <param name="userInput"> The user input. </param>
        /// <param name="saveToLog">
        ///     Whether or not the events should be saved to the log and have a
        ///     <see cref="ChatHistoryEntry" /> created for them.
        /// </param>
        /// <returns>
        ///     The <see cref="UserStatementResponse" />.
        /// </returns>
        [NotNull]
        private UserStatementResponse HandleUserStatementInternal(
            [NotNull] string userInput,
            bool saveToLog = true)
        {
            //- Log the input to the diagnostic log.
            Console?.Log(Resources.ChatInputHeader, userInput, LogLevel.UserInput);

            // Store the input in chat history (only if we're logging it)
            if (saveToLog) { AddHistoryEntry(_user, userInput); }

            // Give our input to the chat ChatEngine
            var result = GetChatResult(userInput);

            //- If it's a catastrophic failure, return a nearly empty object without storing the null response
            if (result == null)
            {
                return new UserStatementResponse(userInput,
                                                 Resources.DefaultFailureResponseText,
                                                 string.Empty,
                                                 ChatCommand.Empty,
                                                 null);
            }

            // Get the template out of the response so we can see if there are any OOB instructions
            var template = GetResponseTemplate(result);

            // Log and store the response 
            var output = result.Output;
            if (!string.IsNullOrWhiteSpace(output))
            {
                Console?.Log(Resources.ChatOutputHeader, output, LogLevel.ChatResponse);

                AddHistoryEntry(ChatEngine.SystemUser, output, result);
            }

            //- Log the output to the diagnostic log. Sometimes - for redirect commands / etc. there's no response
            if (!string.IsNullOrWhiteSpace(template))
            {
                Console?.Log(Resources.ChatOutputHeader,
                             string.Format(CultureInfo.CurrentCulture,
                                           Resources.AimlStatementHandlerUsingTemplate.NonNull(),
                                           template),
                             LogLevel.Verbose);
            }

            //- Update query properties and return the result
            var response = new UserStatementResponse(userInput, output, template, ChatCommand.Empty, result);
            LastResponse = response;

            return response;
        }

        /// <summary>Adds a new history entry.</summary>
        /// <param name="user">The user.</param>
        /// <param name="message">The message.</param>
        /// <param name="chatResult">The chat result, when the message is from the system</param>
        private void AddHistoryEntry([NotNull] User user, [NotNull] string message,
                                     [CanBeNull] ChatResult chatResult = null)
        {
            // Build out an entry
            var entry = new ChatHistoryEntry(Container, user, message, chatResult);

            // Add the entry to the collection
            _chatHistory.Add(entry);
        }

        /// <summary>
        ///     Updates the owner of the chat engine. Setting the owner allows Alfred Commands to reach
        ///     Alfred from the chat engine.
        /// </summary>
        /// <param name="owner">The owner.</param>
        internal void UpdateOwner([CanBeNull] IAlfredCommandRecipient owner)
        {
            ChatEngine.Owner = owner;
        }

        /// <summary>Gets the chat result.</summary>
        /// <param name="userInput">The user input.</param>
        /// <returns>The result of the communication to the chat ChatEngine</returns>
        [CanBeNull]
        private ChatResult GetChatResult([NotNull] string userInput)
        {
            if (ChatEngine == null || _user == null)
            {
                throw new InvalidOperationException(Resources.AimlStatementHandlerChatOffline);
            }

            var result = ChatEngine.Chat(userInput, _user);

            return result;
        }

        /// <summary>
        ///     Sets up the chat engine.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        ///     Thrown when the requested operation is invalid.
        /// </exception>
        private void InitializeChatEngine()
        {
            if (ChatEngine == null || _user == null)
            {
                throw new InvalidOperationException(Resources.AimlStatementHandlerChatOffline);
            }

            ChatEngine.LoadSettingsFromXml(Resources.ChatBotSettings,
                                           Resources.ChatBotPersonSubstitutions,
                                           Resources.ChatBotPerson2Substitutions,
                                           Resources.ChatBotGenderSubstitutions,
                                           Resources.ChatBotSubstitutions);

            AddApplicationAimlResourcesToChatEngine();
        }

        /// <summary>
        ///     Adds Alfred application AIML resources to the chat engine from the internal resource
        ///     files.
        /// </summary>
        private void AddApplicationAimlResourcesToChatEngine()
        {
            // TODO: Each assembly should provide their own set of resources

            // TODO: Find a nicer extension method way of doing this

            var loaded = 0;

            if (TryLoadAimlFile(Resources.AimlCoreDateTime)) { loaded++; }
            if (TryLoadAimlFile(Resources.AimlCorePower)) { loaded++; }
            if (TryLoadAimlFile(Resources.AimlFallback)) { loaded++; }
            if (TryLoadAimlFile(Resources.AimlGod)) { loaded++; }
            if (TryLoadAimlFile(Resources.AimlGreeting)) { loaded++; }
            if (TryLoadAimlFile(Resources.AimlThanks)) { loaded++; }
            if (TryLoadAimlFile(Resources.AimlHelp)) { loaded++; }
            if (TryLoadAimlFile(Resources.AimlShellNavigation)) { loaded++; }
            if (TryLoadAimlFile(Resources.AimlSysStatus)) { loaded++; }

            const string Singular = "1 AIML File Parsed";
            const string PluralFormat = "{0} AIML Files Parsed";

            var plural = string.Format(ChatEngine.Locale, PluralFormat, loaded);

            var message = loaded.Pluralize(Singular, plural);
            Console?.Log("Chat.Load", message, LogLevel.Verbose);
        }

        /// <summary>
        ///     Tries to load raw AIML <paramref name="markup"/> into the chat engine.
        /// </summary>
        /// <param name="markup"> The markup. </param>
        /// <returns>
        ///     True if the AIML was processed; otherwise false.
        /// </returns>
        private bool TryLoadAimlFile([CanBeNull] string markup)
        {
            try
            {
                ChatEngine.LoadAimlFromString(markup.NonNull());
                return true;
            }
            catch (XmlException ex)
            {
                Console?.Log("Chat.Load",
                             $"XmlException while loading Aiml: {ex.Message} for AIML: {markup}",
                             LogLevel.Warning);

                return false;
            }
        }

        /// <summary>
        ///     Called when a property changes.
        /// </summary>
        /// <param name="propertyName"> Name of the property. </param>
        [SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed",
            Justification =
                "Using CallerMemberName to auto-default this value from any property caller")]
        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        ///     Gets the response template from the last request spawned in the AIML chat message
        ///     <paramref name="chatResult" />.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="chatResult" /> is <see langword="null" />.
        /// </exception>
        /// <param name="chatResult"> The result of a chat message to the AIML interpreter. </param>
        /// <returns>
        ///     The response template.
        /// </returns>
        [CanBeNull]
        private static string GetResponseTemplate([NotNull] ChatResult chatResult)
        {
            if (chatResult == null) { throw new ArgumentNullException(nameof(chatResult)); }

            // We want the last template as the other templates have redirected to it
            var template = string.Empty;
            var request = chatResult.Request;
            while (request != null)
            {
                // Grab the template used for this request
                var query = request.ChatResult?.SubQueries.FirstOrDefault();
                if (query != null) { template = query.Template; }

                // If it has an inner request, we'll use that for next iteration, otherwise we're done.
                request = request.Child;
            }

            return template;
        }

        /// <summary>
        ///     Handles events from the framework.
        /// </summary>
        /// <param name="frameworkEvent"> The event. </param>
        public void HandleFrameworkEvent(FrameworkEvent frameworkEvent)
        {
            switch (frameworkEvent)
            {
                case FrameworkEvent.Initialize:

                    // Send a "hi" into the system, but hide it from the event log.
                    HandleUserStatementInternal("Hi", false);
                    break;

                case FrameworkEvent.Shutdown:

                    // Send a "Bye" into the system, but hide it from the event log.
                    HandleUserStatementInternal("Bye", false);
                    break;
            }
        }
    }

}