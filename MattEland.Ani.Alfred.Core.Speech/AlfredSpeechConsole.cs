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
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Console;
using MattEland.Common;

namespace MattEland.Ani.Alfred.Core.Speech
{
    /// <summary>
    ///     A speech-enabled console implementation that notifies the user of significant events
    /// </summary>
    public sealed class AlfredSpeechConsole : IConsole, IDisposable
    {
        [NotNull]
        private readonly IConsole _console;

        [CanBeNull]
        private readonly AlfredSpeechProvider _speech;

        [NotNull]
        private readonly HashSet<LogLevel> _speechEnabledLogLevels;

        /// <summary>
        /// Initializes a new instance of the <see cref="AlfredSpeechConsole" /> class.
        /// </summary>
        /// <param name="console">The console that events should be logged to.</param>
        /// <param name="factory">The event factory.</param>
        public AlfredSpeechConsole([CanBeNull] IConsole console, [CanBeNull] ConsoleEventFactory factory)
        {
            // This class can decorate other consoles, but for an empty implementation it can rely on an internal collection
            if (console == null)
            {
                console = new SimpleConsole();
            }
            _console = console;

            // Set up the event factory
            if (factory == null) { factory = new ConsoleEventFactory(); }
            EventFactory = factory;

            // Tell it what log levels we care about
            _speechEnabledLogLevels = new HashSet<LogLevel> { LogLevel.ChatResponse, LogLevel.Warning, LogLevel.Error };

            try
            {
                // Give the speech provider the existing console and not this console since it won't be online yet
                _speech = new AlfredSpeechProvider(console);
            }
            catch (InvalidOperationException ex)
            {
                // On failure creating the speech provider, just have speech be null and we'll just be a decorator
                _speech = null;
                console.Log("Init.Console", $"Speech could not be initialized: {ex.Message}", LogLevel.Error);
            }
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
        /// Gets or sets a value indicating whether speech synthesis is enabled.
        /// </summary>
        /// <value><c>true</c> if speech synthesis is enabled; otherwise, <c>false</c>.</value>
        public bool EnableSpeech { get; set; } = true;

        /// <summary>
        ///     Logs the specified <paramref name="message"/> to the console.
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
            if (EnableSpeech && SpeechEnabledLogLevels.Contains(level))
            {
                // For more serious items, have Alfred say the status beforehand
                if (level == LogLevel.Warning || level == LogLevel.Error)
                {
                    message = string.Format(CultureInfo.CurrentCulture, "{0}: {1}", level, message);
                }

                _speech?.Say(message.NonNull());
            }
        }

        /// <summary>
        /// Gets the console event factory used for creating new events.
        /// </summary>
        /// <value>The console event factory.</value>
        public ConsoleEventFactory EventFactory { get; }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "_speech")]
        public void Dispose()
        {
            _speech.TryDispose();
        }
    }
}