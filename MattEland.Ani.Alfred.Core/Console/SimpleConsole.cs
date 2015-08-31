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
using MattEland.Common.Providers;

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
        public SimpleConsole() : this(CommonProvider.Container)
        {
        }

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
            try
            {
                _events.Add(consoleEvent);
            }
            catch (NotSupportedException)
            {
                /* TODO: I get this from dispatcher-based exceptions in VS logging. 
                   I think I need a Thread-Safe Observable Collection */
            }
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