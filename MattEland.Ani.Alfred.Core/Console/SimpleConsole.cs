// ---------------------------------------------------------
// SimpleConsole.cs
// 
// Created on:      07/26/2015 at 2:23 PM
// Last Modified:   08/07/2015 at 12:41 AM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;

using JetBrains.Annotations;

using MattEland.Common.Providers;

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
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <param name="container"> The container. </param>
        public SimpleConsole([NotNull] IObjectContainer container) : this(container, new ConsoleEventFactory())
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SimpleConsole" /> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <param name="container"> The container. </param>
        /// <param name="factory"> The console event factory. </param>
        public SimpleConsole([NotNull] IObjectContainer container,
                             [NotNull] ConsoleEventFactory factory)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }
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
        ///     Gets the container.
        /// </summary>
        /// <value>
        ///     The container.
        /// </value>
        [NotNull]
        public IObjectContainer Container { get; }

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
        protected virtual void Log([NotNull] IConsoleEvent consoleEvent)
        {
            _events.Add(consoleEvent);
        }

        /// <summary>
        ///     Gets the number of events in the collection.
        /// </summary>
        /// <value>
        ///     The total number of events.
        /// </value>
        public int EventCount { get { return _events.Count; } }

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