// ---------------------------------------------------------
// AlfredSpeechConsole.cs
// 
// Created on:      08/19/2015 at 9:31 PM
// Last Modified:   09/03/2015 at 12:49 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Console;
using MattEland.Common;
using MattEland.Common.Providers;

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
        ///     Initializes a new instance of the <see cref="AlfredSpeechConsole" /> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="console">The console that events should be logged to.</param>
        /// <param name="factory">The event factory.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when one or more required arguments are null.
        /// </exception>
        public AlfredSpeechConsole(
            [NotNull] IObjectContainer container,
            [CanBeNull] IConsole console,
            [CanBeNull] ConsoleEventFactory factory)
        {
            if (container == null) { throw new ArgumentNullException(nameof(container)); }
            Container = container;

            // This class can decorate other consoles, but for an empty implementation it can rely on an internal collection
            if (console == null) { console = new SimpleConsole(container); }
            _console = console;

            // Set up the event factory
            if (factory == null) { factory = new ConsoleEventFactory(); }
            EventFactory = factory;

            // Tell it what log levels we care about
            _speechEnabledLogLevels = new HashSet<LogLevel>
                                      {
                                          LogLevel.ChatResponse,
                                          LogLevel.ChatNotification,
                                          LogLevel.Warning,
                                          LogLevel.Error
                                      };

            try
            {
                // Give the speech provider the existing console and not this console since it won't be online yet
                _speech = new AlfredSpeechProvider(console);
            }
            catch (InvalidOperationException ex)
            {
                // On failure creating the speech provider, just have speech be null and we'll just be a decorator
                _speech = null;
                Log("Init.Console", $"Speech could not be initialized: {ex.Message}", LogLevel.Error);
            }
        }

        /// <summary>
        ///     Gets the log levels that warrant the user's attention.
        /// </summary>
        /// <value>
        /// The log levels that warrant user attention.
        /// </value>
        [NotNull]
        public HashSet<LogLevel> SpeechEnabledLogLevels
        {
            [DebuggerStepThrough]
            get
            {
                return _speechEnabledLogLevels;
            }
        }

        /// <summary>
        ///     Clears all events from the log
        /// </summary>
        public void Clear()
        {
            _console.Clear();
        }

        /// <summary>
        ///     Gets the number of events in the collection.
        /// </summary>
        /// <value>
        /// The total number of events.
        /// </value>
        public int EventCount
        {
            get { return _console.EventCount; }
        }

        /// <summary>
        ///     Gets the console event factory used for creating new events.
        /// </summary>
        /// <value>
        /// The console event factory.
        /// </value>
        public ConsoleEventFactory EventFactory { get; }

        /// <summary>
        ///     Gets the console events.
        /// </summary>
        /// <value>
        /// The console events.
        /// </value>
        public IEnumerable<IConsoleEvent> Events
        {
            [DebuggerStepThrough]
            get
            {
                return _console.Events;
            }
        }

        /// <summary>
        ///     Logs the specified <paramref name="message" /> to the console.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="message">The message.</param>
        /// <param name="level">The logging level.</param>
        public void Log([CanBeNull] string title, [CanBeNull] string message, LogLevel level)
        {
            // Always log things to the base logger
            _console.Log(title, message, level);

            // If it's a significant message, tell the user via voice
            if (SpeechEnabledLogLevels.Contains(level))
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
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting
        ///     unmanaged resources.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed",
            MessageId = "_speech")]
        public void Dispose()
        {
            _speech.TryDispose();
        }

        /// <summary>
        ///     Gets the container.
        /// </summary>
        /// <value>
        /// The container.
        /// </value>
        [NotNull]
        public IObjectContainer Container { get; }
    }
}