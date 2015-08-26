// ---------------------------------------------------------
// SimpleConsole.cs
// 
// Created on:      07/26/2015 at 2:23 PM
// Last Modified:   08/07/2015 at 12:41 AM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;

namespace MattEland.Ani.Alfred.Core.Console
{
    /// <summary>
    ///     A simple console used for unit testing and designer window purposes
    /// </summary>
    public sealed class SimpleConsole : IConsole
    {
        [NotNull]
        private readonly ICollection<IConsoleEvent> _events;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SimpleConsole" /> class.
        /// </summary>
        public SimpleConsole() : this(new SimplePlatformProvider(), new ConsoleEventFactory())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleConsole" /> class.
        /// </summary>
        /// <param name="provider">The platform provider used to initialize the collection of events.</param>
        /// <param name="factory">The console event factory.</param>
        /// <exception cref="System.ArgumentNullException">provider, factory
        /// </exception>
        public SimpleConsole([NotNull] IPlatformProvider provider,
                             [NotNull] ConsoleEventFactory factory)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }
            if (factory == null) { throw new ArgumentNullException(nameof(factory)); }

            _events = provider.CreateCollection<IConsoleEvent>();
            EventFactory = factory;
        }

        /// <summary>
        ///     Logs the specified message to the console.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="message">The message.</param>
        /// <param name="level">The logging level.</param>
        public void Log([CanBeNull] string title, [CanBeNull] string message, LogLevel level)
        {
            if (title == null)
            {
                title = "Unknown";
            }

            if (message == null)
            {
                return;
            }

            var evt = EventFactory.CreateEvent(title, message, level);

            Log(evt);
        }

        /// <summary>
        /// Logs the specified console event.
        /// </summary>
        /// <param name="consoleEvent">The console event.</param>
        private void Log([NotNull] IConsoleEvent consoleEvent)
        {
            _events.Add(consoleEvent);
        }

        /// <summary>
        ///     Gets the console events.
        /// </summary>
        /// <value>The console events.</value>
        [NotNull]
        [ItemNotNull]
        public IEnumerable<IConsoleEvent> Events
        {
            get { return _events; }
        }

        /// <summary>
        /// Gets the console event factory used for creating new events.
        /// </summary>
        /// <value>The console event factory.</value>
        [NotNull]
        public ConsoleEventFactory EventFactory { get; }
    }

}