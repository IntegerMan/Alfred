﻿// ---------------------------------------------------------
// AlfredSpeechConsole.cs
// 
// Created on:      08/07/2015 at 2:49 PM
// Last Modified:   08/07/2015 at 3:15 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Console;

namespace MattEland.Ani.Alfred.Core.Speech
{
    /// <summary>
    ///     A speech-enabled console implementation that notifies the user of significant events
    /// </summary>
    public class AlfredSpeechConsole : IConsole
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
            _speechEnabledLogLevels = new HashSet<LogLevel> { LogLevel.Info, LogLevel.Warning, LogLevel.Error };

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
        public IEnumerable<ConsoleEvent> Events
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
            // Always log things to the base logger
            _console.Log(title, message, level);

            // If it's a significant message, tell the user via voice
            if (_speechEnabledLogLevels.Contains(level))
            {
                _speech.Say(message);
            }
        }
    }
}