﻿// ---------------------------------------------------------
// AimlStatementHandler.cs
// 
// Created on:      08/10/2015 at 12:51 AM
// Last Modified:   08/11/2015 at 2:36 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
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
    public sealed class AimlStatementHandler : IChatProvider, INotifyPropertyChanged
    {
        [NotNull]
        private readonly Bot _chatBot;

        [CanBeNull]
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
            //- Validate / Store Settings
            if (string.IsNullOrWhiteSpace(settingsPath))
            {
                throw new ArgumentException(Resources.NoSettingsPathError, nameof(settingsPath));
            }
            _settingsPath = settingsPath;

            //- Logging Housekeeping
            _logHeader = Resources.ChatProcessingHeader;
            _console = console;

            //+ Set up the chat bot
            _chatBot = new Bot();
            _user = new User(Resources.ChatUserName, _chatBot);
            try
            {
                InitializeChatBot();
            }
            catch (Exception ex)
            {
                var errorFormat = Resources.ErrorInitializingChat;
                var message = string.Format(CultureInfo.CurrentCulture, errorFormat, ex.Message);
                _console?.Log(_logHeader, message, LogLevel.Error);
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
                if (Equals(value, _console))
                {
                    return;
                }

                _console = value;
                OnPropertyChanged();
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
            //- Log the input to the diagnostic log.
            _console?.Log(Resources.ChatInputHeader, userInput, LogLevel.UserInput);

            //+ We're calling 3rd party code - be extremely careful
            Result result = null;
            try
            {
                result = _chatBot.Chat(userInput, _user.UserID);
            }
            catch (Exception ex)
            {
                _console?.Log(Resources.ChatInputHeader, ex.Message, LogLevel.Error);
            }

            //+ Get the template out of the response so we can see if there are any OOB instructions
            var template = GetResponseTemplate(result);

            //+ Grab the command from the template, if one was present
            var command = AimlCommandParser.GetCommandFromTemplate(template, _console);

            // Interpret the response keeping in mind it could have messed up
            var output = result?.Output ?? Resources.DefaultFailureResponseText;
            if (!string.IsNullOrWhiteSpace(output))
            {
                _console?.Log(Resources.ChatOutputHeader, output, LogLevel.ChatResponse);
            }

            //- Log the output to the diagnostic log. Sometimes - for redirect commands / etc. there's no response
            if (!string.IsNullOrWhiteSpace(template))
            {
                var message = string.Format(CultureInfo.CurrentCulture, "Using Template: {0}", template);
                _console?.Log(Resources.ChatOutputHeader, message, LogLevel.Verbose);
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
        ///     Gets the response template from the AIML chat message result.
        /// </summary>
        /// <param name="result">The result of a chat message to the AIML interpreter.</param>
        /// <returns>The response template</returns>
        /// <remarks>
        ///     Result is not CLSCompliant so this method should not be made public
        /// </remarks>
        [CanBeNull]
        private static string GetResponseTemplate([CanBeNull] Result result)
        {
            // We want the last template as the other templates have redirected to it
            var subQuery = result?.SubQueries?.LastOrDefault();

            return subQuery?.Template;
        }

        private void InitializeChatBot()
        {
            _chatBot.WrittenToLog += OnWrittenToLog;
            _chatBot.loadSettings(_settingsPath);

            //+ Load AIML files
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