﻿// ---------------------------------------------------------
// AimlStatementHandler.cs
// 
// Created on:      08/10/2015 at 12:51 AM
// Last Modified:   08/16/2015 at 1:20 AM
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
        private readonly ChatEngine _chatChatEngine;

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
            Justification =
                "I really don't trust third party libraries to advertise thrown exception types")]
        public AimlStatementHandler([CanBeNull] IConsole console, [NotNull] string settingsPath)
        {
            //- Validate / Store Settings
            if (string.IsNullOrWhiteSpace(settingsPath))
            {
                throw new ArgumentException(Resources.NoSettingsPathError, nameof(settingsPath));
            }
            _settingsPath = settingsPath;

            //- Logging Housekeeping
            _console = console;

            //+ Set up the chat ChatEngine
            _chatChatEngine = new ChatEngine { Logger = console };

            // TODO: Use the currently logged in user's name instead?
            _user = new User(Resources.ChatUserName.NonNull(), _chatChatEngine);

            try
            {
                InitializeChatEngine();
            }
            catch (Exception ex)
            {
                var errorFormat = Resources.ErrorInitializingChat;
                var message = string.Format(CultureInfo.CurrentCulture, errorFormat, ex.Message);
                _console?.Log(Resources.ChatProcessingHeader, message, LogLevel.Error);
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
                if (Equals(value, _console))
                {
                    return;
                }

                _console = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Handles a user statement.
        /// </summary>
        /// <param name="userInput">The user input.</param>
        /// <returns>The response to the user statement</returns>
        public UserStatementResponse HandleUserStatement(string userInput)
        {
            //- Log the input to the diagnostic log.
            _console?.Log(Resources.ChatInputHeader, userInput, LogLevel.UserInput);

            //! Give our input to the chat ChatEngine
            var result = GetChatResult(userInput);

            //+ GetValue the template out of the response so we can see if there are any OOB instructions
            var template = AimlCommandParser.GetResponseTemplate(result);

            //! Grab the command from the template, if one was present
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
        ///     Gets the chat result.
        /// </summary>
        /// <param name="userInput">The user input.</param>
        /// <returns>The result of the communication to the chat ChatEngine</returns>
        private Result GetChatResult([NotNull] string userInput)
        {
            var result = _chatChatEngine.Chat(userInput, _user);

            return result;
        }

        /// <summary>
        ///     Sets up the chat engine
        /// </summary>
        private void InitializeChatEngine()
        {
            _chatChatEngine.Logger = _console;
            _chatChatEngine.LoadSettings(_settingsPath);
            _chatChatEngine.LoadAimlFromDirectory();
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