// ---------------------------------------------------------
// AimlStatementHandler.cs
// 
// Created on:      08/19/2015 at 9:31 PM
// Last Modified:   08/25/2015 at 3:24 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Xml;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml;
using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Common;

namespace MattEland.Ani.Alfred.Chat
{
    /// <summary>
    ///     Handles user statements by parsing the statement through an open source AIML engine
    /// </summary>
    /// <remarks>
    ///     AIML stands for Artificial Intelligence Markup Language
    /// </remarks>
    public sealed class AimlStatementHandler : IChatProvider, INotifyPropertyChanged
    {

        [NotNull]
        private readonly ChatHandlersProvider _chatHandlerProvider;

        [NotNull]
        private readonly ChatHistoryProvider _chatHistory;

        /// <summary>
        /// The <see cref="User"/> that represents the chat engine.
        /// </summary>
        [CanBeNull]
        private readonly User _engineUser;

        [CanBeNull]
        private readonly User _user;

        [CanBeNull]
        private IConsole _console;

        [CanBeNull]
        private string _input;

        private UserStatementResponse _response;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object" /> class using the current
        /// directory for the settings path.
        /// </summary>
        /// <param name="engineName">Name of the chat engine.</param>
        /// <param name="provider">The platform provider.</param>
        /// <param name="console">The console.</param>
        /// <exception cref="System.ArgumentNullException">
        /// </exception>
        public AimlStatementHandler(
            [NotNull] string engineName,
            [NotNull] IPlatformProvider provider,
            [CanBeNull] IConsole console = null)
        {

            //- Validate
            if (engineName == null || engineName.IsEmpty())
            {
                throw new ArgumentNullException(nameof(engineName));
            }
            if (provider == null) { throw new ArgumentNullException(nameof(provider)); }

            // Set up simple internal fields
            _console = console;
            _chatHistory = new ChatHistoryProvider(provider);
            _chatHandlerProvider = new ChatHandlersProvider();

            // Set up the chat engine
            try
            {
                ChatEngine = new ChatEngine(console);
                _user = new User(Resources.ChatUserName.NonNull());
                _engineUser = new User(engineName);
                InitializeChatEngine();
            }
            catch (IOException ex)
            {
                LogErrorInitializingChat(ex);
            }
            catch (SecurityException ex)
            {
                LogErrorInitializingChat(ex);
            }
        }

        /// <summary>
        ///     Gets or sets the console.
        /// </summary>
        /// <value>The console.</value>
        [CanBeNull]
        public IConsole Console
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
        /// <remarks>This is present largely for testing</remarks>
        /// <value>The chat engine.</value>
        [CanBeNull]
        public ChatEngine ChatEngine { get; }

        /// <summary>
        ///     Gets the property providers representing broad chat categories.
        /// </summary>
        /// <value>The property providers.</value>
        [NotNull, ItemNotNull]
        public IEnumerable<IPropertyProvider> PropertyProviders
        {
            get
            {
                yield return _chatHistory;
                yield return _chatHandlerProvider;
            }
        }

        /// <summary>
        ///     Performs an initial greeting by sending hi to the conversation system
        ///     and erasing it from the last input so the user sees Alfred greeting them.
        /// </summary>
        public void DoInitialGreeting()
        {
            // Send a "hi" into the system
            HandleUserStatement(Resources.InitialGreetingText.NonNull());

            // Clear out the input so it doesn't look like the user typed it
            LastInput = string.Empty;
        }

        /// <summary>
        ///     Handles a user statement.
        /// </summary>
        /// <param name="userInput">The user input.</param>
        /// <returns>The response to the user statement</returns>
        /// <exception cref="InvalidOperationException">
        ///     Cannot use the chat system when chat did not initialize
        ///     properly
        /// </exception>
        public UserStatementResponse HandleUserStatement(string userInput)
        {
            if (ChatEngine == null || _user == null || _engineUser == null)
            {
                throw new InvalidOperationException(Resources.AimlStatementHandlerChatOffline);
            }

            //- Trim for housekeeping purposes since this will be displayed / stored.
            userInput = userInput.NonNull().Trim();

            //- Log the input to the diagnostic log.
            Console?.Log(Resources.ChatInputHeader, userInput, LogLevel.UserInput);

            // Store the input in chat history
            AddHistoryEntry(_user, userInput);

            // Give our input to the chat ChatEngine
            var result = GetChatResult(userInput);

            //- If it's a catastrophic failure, return a nearly empty object without storing the null response
            if (result == null)
            {
                return new UserStatementResponse(userInput,
                                                 Resources.DefaultFailureResponseText,
                                                 string.Empty,
                                                 ChatCommand.Empty);
            }

            // Get the template out of the response so we can see if there are any OOB instructions
            var template = GetResponseTemplate(result);

            // Log and store the response 
            var output = result.Output;
            if (!string.IsNullOrWhiteSpace(output))
            {
                Console?.Log(Resources.ChatOutputHeader, output, LogLevel.ChatResponse);

                AddHistoryEntry(_engineUser, output);
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
            var response = new UserStatementResponse(userInput, output, template, ChatCommand.Empty);
            LastResponse = response;
            LastInput = userInput;

            return response;
        }

        /// <summary>
        /// Adds a new history entry.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="message">The message.</param>
        private void AddHistoryEntry([NotNull] User user, [NotNull] string message)
        {
            // Build out an entry
            var entry = new ChatHistoryEntry(user, message);

            // Add the entry to the collection
            _chatHistory.Add(entry);

        }

        /// <summary>
        ///     Gets the last input from the user.
        /// </summary>
        /// <value>The last input.</value>
        [CanBeNull]
        public string LastInput
        {
            get { return _input; }
            private set
            {
                if (value == _input) { return; }
                _input = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Gets the last response from the system.
        /// </summary>
        /// <value>The last response.</value>
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
        ///     Updates the owner of the chat engine. Setting the owner allows Alfred Commands to reach Alfred
        ///     from the chat engine.
        /// </summary>
        /// <param name="owner">The owner.</param>
        public void UpdateOwner(IAlfredCommandRecipient owner)
        {
            if (ChatEngine != null) { ChatEngine.Owner = owner; }
        }

        /// <summary>
        ///     Logs an error encountered initializing chat.
        /// </summary>
        /// <param name="ex">The ex.</param>
        private void LogErrorInitializingChat([CanBeNull] Exception ex)
        {
            var errorFormat = Resources.ErrorInitializingChat.NonNull();
            var message = string.Format(CultureInfo.CurrentCulture, errorFormat, ex?.Message);
            _console?.Log(Resources.ChatProcessingHeader, message, LogLevel.Error);
        }

        /// <summary>
        ///     Gets the chat result.
        /// </summary>
        /// <param name="userInput">The user input.</param>
        /// <returns>The result of the communication to the chat ChatEngine</returns>
        [CanBeNull]
        private Result GetChatResult([NotNull] string userInput)
        {
            if (ChatEngine == null || _user == null)
            {
                throw new InvalidOperationException(Resources.AimlStatementHandlerChatOffline);
            }

            var result = ChatEngine.Chat(userInput, _user);

            return result;
        }

        /// <summary>
        ///     Sets up the chat engine
        /// </summary>
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

            AddApplicationAimlResourcesToChatEngine(ChatEngine);
        }

        /// <summary>
        ///     Adds Alfred application AIML resources to the chat engine from the internal resource files.
        /// </summary>
        /// <remarks>
        ///     This method is public and static so that it can be easily accessed by testing libraries.
        /// </remarks>
        /// <param name="engine">The chat engine.</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="engine" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="XmlException">
        ///     There is a load or parse error in the XML. In this case, a
        ///     <see cref="T:System.IO.FileNotFoundException" /> is raised.
        /// </exception>
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public static void AddApplicationAimlResourcesToChatEngine([NotNull] ChatEngine engine)
        {
            if (engine == null) { throw new ArgumentNullException(nameof(engine)); }

            // TODO: Each assembly should provide their own set of resources

            engine.LoadAimlFromString(Resources.AimlCoreDateTime);
            engine.LoadAimlFromString(Resources.AimlCorePower);
            engine.LoadAimlFromString(Resources.AimlFallback);
            engine.LoadAimlFromString(Resources.AimlGod);
            engine.LoadAimlFromString(Resources.AimlGreeting);
            engine.LoadAimlFromString(Resources.AimlThanks);
            engine.LoadAimlFromString(Resources.AimlHelp);
            engine.LoadAimlFromString(Resources.AimlShellNavigation);
            engine.LoadAimlFromString(Resources.AimlSysStatus);
        }

        /// <summary>
        ///     Called when a property changes.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        [SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed",
            Justification =
                "Using CallerMemberName to auto-default this value from any property caller")]
        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        ///     Gets the response template from the last request spawned in the AIML chat message result.
        /// </summary>
        /// <param name="result">The result of a chat message to the AIML interpreter.</param>
        /// <returns>The response template</returns>
        /// <exception cref="ArgumentNullException"><paramref name="result" /> is <see langword="null" />.</exception>
        [CanBeNull]
        private static string GetResponseTemplate([NotNull] Result result)
        {
            if (result == null) { throw new ArgumentNullException(nameof(result)); }

            // We want the last template as the other templates have redirected to it
            var template = string.Empty;
            var request = result.Request;
            while (request != null)
            {
                // Grab the template used for this request
                var query = request.Result?.SubQueries.FirstOrDefault();
                if (query != null) { template = query.Template; }

                // If it has an inner request, we'll use that for next iteration, otherwise we're done.
                request = request.Child;
            }

            return template;
        }
    }

}