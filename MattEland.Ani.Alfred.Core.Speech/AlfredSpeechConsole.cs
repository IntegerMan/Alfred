// ---------------------------------------------------------
// AlfredSpeechConsole.cs
// 
// Created on:      08/07/2015 at 2:49 PM
// Last Modified:   08/07/2015 at 3:15 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.Core.Definitions;

namespace MattEland.Ani.Alfred.Core.Speech
{
    /// <summary>
    ///     A speech-enabled console implementation that notifies the user of significant events
    /// </summary>
    public sealed class AlfredSpeechConsole : IConsole, IDisposable
    {
        [NotNull]
        private readonly IConsole _console;

        [NotNull]
        private readonly AlfredSpeechProvider _speech;

        [NotNull]
        private readonly HashSet<LogLevel> _speechEnabledLogLevels;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AlfredSpeechConsole" /> class.
        /// </summary>
        /// <param name="console">The console that events should be logged to.</param>
        /// <exception cref="System.ArgumentNullException">console</exception>
        public AlfredSpeechConsole([CanBeNull] IConsole console)
        {
            // This class can decorate other consoles, but for an empty implementation it can rely on an internal collection
            if (console == null)
            {
                console = new SimpleConsole();
            }

            _console = console;

            // Tell it what log levels we care about
            _speechEnabledLogLevels = new HashSet<LogLevel> { LogLevel.ChatResponse, LogLevel.Warning, LogLevel.Error };

            // Give the speech provider the existing console and not this console since it won't be online yet
            _speech = new AlfredSpeechProvider(console);
        }

        /// <summary>
        ///     Gets the log levels that warrant the user's attention.
        /// </summary>
        /// <value>The log levels that warrant user attention.</value>
        [NotNull]
        public HashSet<LogLevel> SpeechEnabledLogLevels
        {
            [DebuggerStepThrough]
            get
            { return _speechEnabledLogLevels; }
        }

        /// <summary>
        ///     Gets the console events.
        /// </summary>
        /// <value>The console events.</value>
        public IEnumerable<IConsoleEvent> Events
        {
            [DebuggerStepThrough]
            get
            { return _console.Events; }
        }

        /// <summary>
        ///     Logs the specified message to the console.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="message">The message.</param>
        /// <param name="level">The logging level.</param>
        public void Log(string title, string message, LogLevel level)
        {
            if (title == null)
            {
                title = "Unknown";
            }

            if (message == null)
            {
                return;
            }

            // Always log things to the base logger
            _console.Log(title, message, level);

            // If it's a significant message, tell the user via voice
            if (_speechEnabledLogLevels.Contains(level))
            {
                // For more serious items, have Alfred say the status beforehand
                if (level == LogLevel.Warning || level == LogLevel.Error)
                {
                    message = string.Format(CultureInfo.CurrentCulture, "{0}: {1}", level, message);
                }

                _speech.Say(message.NonNull());
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _speech.Dispose();
        }
    }
}