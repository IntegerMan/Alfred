// ---------------------------------------------------------
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
using System.Xml;
using System.Xml.Linq;

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
            _logHeader = Resources.ChatProcessingHeader.NonNull();
            _console = console;

            // Set up the chat bot
            _chatBot = new Bot();
            _user = new User(Resources.ChatUserName, _chatBot);
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
            // Log the input to the diagnostic log.
            _console?.Log(Resources.ChatInputHeader.NonNull(), userInput, LogLevel.UserInput);

            // We're calling 3rd party code - be extremely careful
            Result result = null;
            try
            {
                result = _chatBot.Chat(userInput, _user.UserID);
            }
            catch (Exception ex)
            {
                _console?.Log(Resources.ChatInputHeader.NonNull(), ex.Message, LogLevel.Error);
            }

            // Get the template out of the response so we can see if there are any OOB instructions
            var template = GetResponseTemplate(result);

            // Grab the command from the template, if one was present
            var command = GetCommandFromTemplate(template, _console);

            // Handle the response keeping in mind it could have messed up
            var output = result?.RawOutput ?? Resources.DefaultFailureResponseText;
            _console?.Log(Resources.ChatOutputHeader.NonNull(), output.NonNull(), LogLevel.ChatResponse);

            var response = new UserStatementResponse(userInput, output, template, command);

            // Log the output to the diagnostic log.
            if (!string.IsNullOrWhiteSpace(template))
            {
                var message = string.Format(CultureInfo.CurrentCulture, "Using Template: {0}", template).NonNull();
                _console?.Log(Resources.ChatOutputHeader.NonNull(), message, LogLevel.Verbose);
            }

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

        /// <summary>
        ///     Gets the first command present from the template and returns that node in XML format.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <param name="console">The console.</param>
        /// <returns>System.String.</returns>
        private static string GetCommandFromTemplate([CanBeNull] string template, [CanBeNull] IConsole console)
        {
            // Do a bunch of trimming and interpreting to find our XML
            var commandXml = GetCommandXmlFromTemplate(template, console);
            if (commandXml == null)
            {
                return null;
            }

            // Load the XML as a document so we can manipulate the elements easier
            XDocument xdoc;
            try
            {
                xdoc = XDocument.Parse(commandXml);
            }
            catch (XmlException ex)
            {
                var message = string.Format(CultureInfo.CurrentCulture,
                                            Resources.ErrorParsingCommand,
                                            ex.Message,
                                            commandXml);
                console?.Log(Resources.ChatOutputHeader.NonNull(),
                             message.NonNull(),
                             LogLevel.Error);
                return null;
            }

            // Grab the OOB root tag out of the document
            var oobElement = xdoc.Root;
            if (oobElement == null)
            {
                console?.Log(Resources.ChatOutputHeader.NonNull(), "OOB command had no root element", LogLevel.Error);
                return null;
            }

            // Return either the XML of the first node or the value of an text value
            var command = oobElement.FirstNode?.ToString() ?? oobElement.Value;

            console?.Log(Resources.ChatOutputHeader.NonNull(), "Received OOB Command: " + command, LogLevel.Info);

            return command;

        }

        /// <summary>
        ///     Gets the command XML from a template.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <param name="console">The console.</param>
        /// <returns>The XML without noise of periphery characters</returns>
        [SuppressMessage("ReSharper", "SuggestVarOrType_BuiltInTypes")]
        [CanBeNull]
        private static string GetCommandXmlFromTemplate([CanBeNull] string template, [CanBeNull] IConsole console)
        {
            // Early exit if it's empty
            if (template == null)
            {
                return null;
            }

            // Set up constants
            const StringComparison ComparisonType = StringComparison.OrdinalIgnoreCase;
            const string StartTag = "<oob";
            const string EndTag = "</oob>";
            const string SelfClosingTagEnd = "/>";

            // Figure out where we start
            var start = template.IndexOf(StartTag, ComparisonType);

            // If we don't have a tag, there's no command and that's fine
            if (start < 0)
            {
                return null;
            }

            // We don't care about the portion of the string before the start so chop it off now
            template = template.Substring(start);

            // Try to find self-closing tags first
            var selfClosingEnd = template.LastIndexOf(SelfClosingTagEnd, ComparisonType);
            if (selfClosingEnd >= 0)
            {
                // We're self-closing. Advance to the end of the tag
                selfClosingEnd = selfClosingEnd + SelfClosingTagEnd.Length;
            }

            // Look for an end tag for our command node
            var end = template.IndexOf(EndTag, ComparisonType);
            if (end >= 0)
            {
                end += EndTag.Length;
            }

            // If we have both a self-closing tag and an end tag, the self-closing probably belongs to
            // an inner XML element. In that case we want to go with the end tag. On the other hand, if
            // we have a self-closing tag and no end tag, we'll want to go with the self-closing tag.

            // That's what this is doing - taking the self-closing tag as the end tag
            if (end <= 0 && selfClosingEnd >= 0)
            {
                end = selfClosingEnd;
            }

            // If we don't have an end at this point, we need to bow out as a tag that was started but not finished
            if (end < 0)
            {
                var message = string.Format(CultureInfo.CurrentCulture, Resources.NoEndTagForOobCommand, template).NonNull();
                console?.Log(Resources.ChatOutputHeader.NonNull(), message, LogLevel.Error);

                return null;
            }

            // Now we can snip out the extra bits to get our template XML
            return template.Substring(0, end);
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