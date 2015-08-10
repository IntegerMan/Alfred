// ---------------------------------------------------------
// AimlStatementHandler.cs
// 
// Created on:      08/10/2015 at 12:51 AM
// Last Modified:   08/10/2015 at 1:21 AM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.Diagnostics;
using System.IO;
using System.Xml;

using AIMLbot;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Console;

namespace MattEland.Ani.Alfred.Chat
{
    /// <summary>
    ///     Handles user statements by parsing the statement through an open source AIML engine
    /// </summary>
    /// <remarks>
    ///     AIML stands for Artificial Intelligence Markup Language
    /// </remarks>
    public class AimlStatementHandler : IUserStatementHandler
    {
        [NotNull]
        private readonly Bot _chatBot;

        [NotNull]
        private readonly User _user;

        [CanBeNull]
        private IConsole _console;

        [NotNull]
        private readonly string _logHeader;

        [NotNull]
        private readonly string _settingsPath;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        public AimlStatementHandler() : this(null, Path.Combine(Environment.CurrentDirectory, @"Chat\config\settings.xml"))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        /// <param name="console">The console.</param>
        /// <param name="settingsPath">The settings path.</param>
        /// <exception cref="System.ArgumentException"></exception>
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
            InitializeChatBot();
            _user = new User("Batman", _chatBot);

        }

        /// <summary>
        /// Loads the settings XML document and returns it.
        /// </summary>
        /// <param name="pathToSettings">The path to settings.</param>
        /// <returns>The XmlDocument</returns>
        /// <exception cref="System.IO.FileNotFoundException">
        /// </exception>
        [NotNull]
        private static XmlDocument LoadSettingsXml([NotNull] string pathToSettings)
        {
            if (pathToSettings != null && pathToSettings.Length <= 0)
                throw new FileNotFoundException();

            if (!new FileInfo(pathToSettings).Exists)
                throw new FileNotFoundException();

            var settingsXml = new XmlDocument();
            settingsXml.Load(pathToSettings);

            return settingsXml;
        }


        private void InitializeChatBot()
        {
            _chatBot.WrittenToLog += OnWrittenToLog;
            var settingsXml = LoadSettingsXml(_settingsPath);
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
        ///     Handles a user statement.
        /// </summary>
        /// <param name="userInput">The user input.</param>
        /// <returns>The response to the user statement</returns>
        public UserStatementResponse HandleUserStatement(string userInput)
        {
            // Log the input to the diagnostic log. Verbose should keep it from being spoken
            _console?.Log("Chat.Input", userInput, LogLevel.Verbose);

            // We're calling 3rd party code - be extremely careful
            Result result = null;
            try
            {
                result = _chatBot.Chat(userInput, _user.UserID);
            }
            catch (Exception ex)
            {
                _console?.Log(_logHeader, ex.Message, LogLevel.Error);
                _console?.Log(_logHeader, "Last Log Message:" + _chatBot.LastLogMessage, LogLevel.Verbose);
            }

            // Handle the response keeping in mind it could have messed up
            var response = result == null
                               ? new UserStatementResponse(userInput, Resources.DefaultFailureResponseText, false)
                               : new UserStatementResponse(userInput, result.RawOutput, true);

            // Log the output to the diagnostic log. Info should make it spoken if speech is on.
            _console?.Log("Chat.Output", response.ResponseText, LogLevel.Info);

            return response;

        }
    }
}