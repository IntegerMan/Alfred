// ---------------------------------------------------------
// AimlStatementHandler.cs
// 
// Created on:      08/10/2015 at 12:51 AM
// Last Modified:   08/10/2015 at 11:06 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;

using AIMLbot;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core;
using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.Core.Definitions;

namespace MattEland.Ani.Alfred.Chat
{
    /// <summary>
    ///     Handles user statements by parsing the statement through an open source AIML engine
    /// </summary>
    /// <remarks>
    ///     AIML stands for Artificial Intelligence Markup Language
    /// </remarks>
    public sealed class AimlStatementHandler : IUserStatementHandler, INotifyPropertyChanged
    {
        [NotNull]
        private readonly Bot _chatBot;

        [NotNull]
        private readonly string _logHeader;

        [NotNull]
        private readonly string _settingsPath;

        [NotNull]
        private readonly User _user;

        [CanBeNull]
        private IConsole _console;

        [CanBeNull]
        private string _input;

        private UserStatementResponse _response;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        public AimlStatementHandler()
            : this(null, Path.Combine(Environment.CurrentDirectory, @"Chat\config\settings.xml"))
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        /// <param name="console">The console.</param>
        /// <param name="settingsPath">The settings path.</param>
        /// <exception cref="ArgumentException"></exception>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "I really don't trust third party libraries to advertise thrown exception types")]
        public AimlStatementHandler([CanBeNull] IConsole console, [NotNull] string settingsPath)
        {
            // Validate / Store Settings
            if (string.IsNullOrWhiteSpace(settingsPath))
            {
                throw new ArgumentException(Resources.NoSettingsPathError, nameof(settingsPath));
            }
            _settingsPath = settingsPath;

            // Logging Housekeeping
            _logHeader = "Chat.Processing";
            _console = console;

            // Set up the chat bot
            _chatBot = new Bot();
            _user = new User("Batman", _chatBot);
            try
            {
                InitializeChatBot();
            }
            catch (Exception ex)
            {
                var errorFormat = Resources.ErrorInitializingChat.NonNull();
                var message = string.Format(CultureInfo.CurrentCulture, errorFormat, ex.Message);
                _console?.Log(_logHeader, message.NonNull(), LogLevel.Error);
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
            get
            {
                return _console;
            }
            [DebuggerStepThrough]
            set
            {
                _console = value;
            }
        }

        /// <summary>
        ///     Occurs when a property changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Handles a user statement.
        /// </summary>
        /// <param name="userInput">The user input.</param>
        /// <returns>The response to the user statement</returns>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "I really don't trust third party libraries to advertise thrown exception types")]
        public UserStatementResponse HandleUserStatement(string userInput)
        {
            // Log the input to the diagnostic log.
            _console?.Log("Chat.Input", userInput, LogLevel.UserInput);

            // We're calling 3rd party code - be extremely careful
            Result result = null;
            try
            {
                result = _chatBot.Chat(userInput, _user.UserID);
            }
            catch (Exception ex)
            {
                _console?.Log(_logHeader, ex.Message, LogLevel.Error);
                var errorFormat = string.Format(CultureInfo.CurrentCulture,
                                                Resources.ChatErrorLastLogMessageFormat,
                                                _chatBot.LastLogMessage);
                _console?.Log(_logHeader, errorFormat.NonNull(), LogLevel.Verbose);
            }

            // Handle the response keeping in mind it could have messed up
            var response = result == null
                               ? new UserStatementResponse(userInput, Resources.DefaultFailureResponseText, false)
                               : new UserStatementResponse(userInput, result.RawOutput, true);

            // Log the output to the diagnostic log.
            _console?.Log("Chat.Output", response.ResponseText, LogLevel.ChatResponse);

            // Update query properties
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

        private void InitializeChatBot()
        {
            _chatBot.WrittenToLog += OnWrittenToLog;
            _chatBot.loadSettings(_settingsPath);

            // Load AIML files
            _chatBot.isAcceptingUserInput = false;
            _chatBot.loadAIMLFromFiles();
            _chatBot.isAcceptingUserInput = true;
        }

        private void OnWrittenToLog()
        {
            if (!string.IsNullOrWhiteSpace(_chatBot.LastLogMessage))
            {
                _console?.Log(_logHeader, _chatBot.LastLogMessage, LogLevel.Verbose);
            }
        }

        /// <summary>
        ///     Called when a property changes.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        [SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed",
            Justification = "Using CallerMemberName to auto-default this value from any property caller")]
        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

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
    }
}