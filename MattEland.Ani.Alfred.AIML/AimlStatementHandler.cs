// ---------------------------------------------------------
// AimlStatementHandler.cs
// 
// Created on:      08/10/2015 at 12:51 AM
// Last Modified:   08/16/2015 at 11:49 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
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
        [CanBeNull]
        private readonly ChatEngine _chatEngine;

        [CanBeNull]
        private readonly User _user;

        [CanBeNull]
        private IConsole _console;

        [CanBeNull]
        private string _input;

        private UserStatementResponse _response;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object" /> class using the current
        ///     directory for the settings path.
        /// </summary>
        /// <param name="console">The console.</param>
        /// <exception cref="IOException">An I/O error occurred.</exception>
        /// <exception cref="DirectoryNotFoundException">Attempted to set a local path that cannot be found.</exception>
        /// <exception cref="SecurityException">The caller does not have the appropriate permission.</exception>
        public AimlStatementHandler([CanBeNull] IConsole console = null)
            : this(console, Path.Combine(Environment.CurrentDirectory, @"Chat\config"))
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        /// <param name="console">The console.</param>
        /// <param name="settingsDirectoryPath">The settings path.</param>
        /// <exception cref="System.ArgumentException"></exception>
        /// <exception cref="ArgumentException">settingsPath, aimlDirectoryPath</exception>
        public AimlStatementHandler([CanBeNull] IConsole console,
                                    [CanBeNull] string settingsDirectoryPath)
        {
            //- Validate / Store Settings
            if (string.IsNullOrWhiteSpace(settingsDirectoryPath))
            {
                throw new ArgumentException(Resources.NoSettingsPathError,
                                            nameof(settingsDirectoryPath));
            }

            SettingsDirectoryPath = settingsDirectoryPath;

            //- Logging Housekeeping
            _console = console;

            //+ Set up the chat engine
            try
            {
                _chatEngine = new ChatEngine(console);
                _user = new User(Resources.ChatUserName.NonNull(), _chatEngine);
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
        ///     Gets or sets the settings path.
        /// </summary>
        /// <value>The settings path.</value>
        [NotNull]
        public string SettingsDirectoryPath { get; set; }

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
                if (Equals(value, _console))
                {
                    return;
                }

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
        public ChatEngine ChatEngine
        {
            get { return _chatEngine; }
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
            if (_chatEngine == null || _user == null)
            {
                throw new InvalidOperationException(Resources.AimlStatementHandlerChatOffline);
            }

            //- Log the input to the diagnostic log.
            _console?.Log(Resources.ChatInputHeader, userInput, LogLevel.UserInput);

            // Give our input to the chat ChatEngine
            var result = GetChatResult(userInput);

            //- If it's a catastrophic failure, return a blankish object
            if (result == null)
            {
                return new UserStatementResponse(userInput, Resources.DefaultFailureResponseText, string.Empty, ChatCommand.Empty);
            }

            // Get the template out of the response so we can see if there are any OOB instructions
            var template = AimlCommandParser.GetResponseTemplate(result);

            // Grab the command from the template, if one was present
            var command = AimlCommandParser.GetCommandFromTemplate(template, _console);

            // Interpret the response 
            var output = result.Output;
            if (!string.IsNullOrWhiteSpace(output))
            {
                _console?.Log(Resources.ChatOutputHeader, output, LogLevel.ChatResponse);
            }

            //- Log the output to the diagnostic log. Sometimes - for redirect commands / etc. there's no response
            if (!string.IsNullOrWhiteSpace(template))
            {
                _console?.Log(Resources.ChatOutputHeader,
                              string.Format(CultureInfo.CurrentCulture,
                                            "Using Template: {0}",
                                            template),
                              LogLevel.Verbose);
            }

            //- Update query properties and return the result
            var response = new UserStatementResponse(userInput, output, template, command);
            LastResponse = response;
            LastInput = userInput;

            return response;
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
                if (Equals(value, _response))
                {
                    return;
                }
                _response = value;
                OnPropertyChanged();
            }
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
                if (value == _input)
                {
                    return;
                }
                _input = value;
                OnPropertyChanged();
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
        ///     Occurs when a property changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Logs an error encountered initializing chat.
        /// </summary>
        /// <param name="ex">The ex.</param>
        private void LogErrorInitializingChat([CanBeNull] Exception ex)
        {
            var errorFormat = Resources.ErrorInitializingChat;
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
            if (_chatEngine == null || _user == null)
            {
                throw new InvalidOperationException(Resources.AimlStatementHandlerChatOffline);
            }

            var result = _chatEngine.Chat(userInput, _user);

            return result;
        }

        /// <summary>
        ///     Sets up the chat engine
        /// </summary>
        private void InitializeChatEngine()
        {
            if (_chatEngine == null || _user == null)
            {
                throw new InvalidOperationException(Resources.AimlStatementHandlerChatOffline);
            }

            InitializeChatEngineSettings();

            AddApplicationAimlResourcesToChatEngine(_chatEngine);
        }

        private void InitializeChatEngineSettings()
        {
            if (_chatEngine == null)
            {
                throw new InvalidOperationException(Resources.AimlStatementHandlerChatOffline);
            }

            try
            {
                _chatEngine.LoadSettingsFromDirectory(SettingsDirectoryPath);
            }
            catch (UnauthorizedAccessException ex)
            {
                _console?.Log("ChatEngine.Initialize",
                              string.Format(CultureInfo.CurrentCulture,
                                            "Unauthorized access exception initializing chat settings: {0}",
                                            ex.Message),
                              LogLevel.Error);
            }
            catch (SecurityException ex)
            {
                _console?.Log("ChatEngine.Initialize",
                              string.Format(CultureInfo.CurrentCulture,
                                            "Security exception initializing chat settings: {0}",
                                            ex.Message),
                              LogLevel.Error);
            }
            catch (FileNotFoundException ex)
            {
                _console?.Log("ChatEngine.Initialize",
                              string.Format(CultureInfo.CurrentCulture,
                                            "File not found on file '{1}' initializing chat settings: {0}",
                                            ex.Message,
                                            ex.FileName),
                              LogLevel.Error);
            }
            catch (DirectoryNotFoundException ex)
            {
                _console?.Log("ChatEngine.Initialize",
                              string.Format(CultureInfo.CurrentCulture,
                                            "Directory not found initializing chat settings: {0}",
                                            ex.Message),
                              LogLevel.Error);
            }
            catch (IOException ex)
            {
                _console?.Log("ChatEngine.Initialize",
                              string.Format(CultureInfo.CurrentCulture,
                                            "IO exception initializing chat settings: {0}",
                                            ex.Message),
                              LogLevel.Error);
            }
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
        /// <exception cref="IOException">An I/O error occurred while opening the file.</exception>
        public static void AddApplicationAimlResourcesToChatEngine([NotNull] ChatEngine engine)
        {
            if (engine == null)
            {
                throw new ArgumentNullException(nameof(engine));
            }

            engine.LoadAimlFromString(Resources.AimlCoreDateTime);
            engine.LoadAimlFromString(Resources.AimlCorePower);
            engine.LoadAimlFromString(Resources.AimlFallback);
            engine.LoadAimlFromString(Resources.AimlGod);
            engine.LoadAimlFromString(Resources.AimlGreeting);
            engine.LoadAimlFromString(Resources.AimlThanks);
            engine.LoadAimlFromString(Resources.AimlHelp);
            engine.LoadAimlFromString(Resources.AimlShellNavigation);
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
    }

}