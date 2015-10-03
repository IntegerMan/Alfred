// ---------------------------------------------------------
// SimpleConsole.cs
// 
// Created on:      08/19/2015 at 9:31 PM
// Last Modified:   09/03/2015 at 12:49 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;

using JetBrains.Annotations;

using MattEland.Common.Providers;
using MattEland.Ani.Alfred.Core.Definitions;

namespace MattEland.Ani.Alfred.Core.Console
{
    /// <summary>
    ///     A simple console used for unit testing and designer window purposes
    /// </summary>
    public class SimpleConsole : IConsole
    {
        [NotNull]
        private readonly ICollection<IConsoleEvent> _events;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SimpleConsole" /> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when one or more required arguments are null.
        /// </exception>
        public SimpleConsole([NotNull] IAlfredContainer container)
            : this(container, new ConsoleEventFactory())
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SimpleConsole" /> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="factory">The console event factory.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when one or more required arguments are null.
        /// </exception>
        public SimpleConsole(
            [NotNull] IAlfredContainer container,
            [NotNull] ConsoleEventFactory factory)
        {
            if (container == null) { throw new ArgumentNullException(nameof(container)); }
            if (factory == null) { throw new ArgumentNullException(nameof(factory)); }

            _events = container.ProvideCollection<IConsoleEvent>();

            // This shouldn't happen, but I saw some weird mojo during 
            if (_events.Count > 0)
            {
                throw new InvalidOperationException("The events collection did not start empty.");
            }

            EventFactory = factory;
            Container = container;
        }

        /// <summary>
        ///     Clears all events from the log
        /// </summary>
        public void Clear()
        {
            _events.Clear();
        }

        /// <summary>
        ///     Gets the number of events in the collection.
        /// </summary>
        /// <value>
        /// The total number of events.
        /// </value>
        public int EventCount
        {
            get { return _events.Count; }
        }

        /// <summary>
        ///     Gets the console event factory used for creating new events.
        /// </summary>
        /// <value>
        /// The console event factory.
        /// </value>
        [NotNull]
        public ConsoleEventFactory EventFactory { get; }

        /// <summary>
        ///     Gets the console events.
        /// </summary>
        /// <value>
        /// The console events.
        /// </value>
        [NotNull]
        [ItemNotNull]
        public IEnumerable<IConsoleEvent> Events
        {
            get { return _events; }
        }

        /// <summary>
        ///     Logs the specified <paramref name="message"/> to the console.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="message">The message.</param>
        /// <param name="level">The logging level.</param>
        public void Log([CanBeNull] string title, [CanBeNull] string message, LogLevel level)
        {
            if (title == null) { title = "Unknown"; }

            if (message == null) { return; }

            var evt = EventFactory.CreateEvent(title, message, level);

            Log(evt);
        }

        /// <summary>
        ///     Gets the container.
        /// </summary>
        /// <value>
        /// The container.
        /// </value>
        [NotNull]
        public IAlfredContainer Container { get; }

        /// <summary>
        ///     Logs the specified console event.
        /// </summary>
        /// <param name="consoleEvent">The console event.</param>
        protected virtual void Log([NotNull] IConsoleEvent consoleEvent)
        {
            _events.Add(consoleEvent);
        }
    }

}