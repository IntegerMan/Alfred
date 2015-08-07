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

namespace MattEland.Ani.Alfred.Core.Console
{
    /// <summary>
    ///     A simple console used for unit testing and designer window purposes
    /// </summary>
    public sealed class SimpleConsole : IConsole
    {
        [NotNull]
        private readonly ICollection<ConsoleEvent> _events;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SimpleConsole" /> class.
        /// </summary>
        public SimpleConsole() : this(new SimplePlatformProvider())
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SimpleConsole" /> class.
        /// </summary>
        /// <param name="provider">The platform provider used to initialize the collection of events.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public SimpleConsole([NotNull] IPlatformProvider provider)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            _events = provider.CreateCollection<ConsoleEvent>();
        }

        /// <summary>
        ///     Logs the specified message to the console.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="message">The message.</param>
        /// <param name="level">The logging level.</param>
        public void Log([NotNull] string title, [NotNull] string message, LogLevel level)
        {
            if (title == null)
            {
                throw new ArgumentNullException(nameof(title));
            }

            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var evt = new ConsoleEvent(title, message, level);

            _events.Add(evt);
        }

        /// <summary>
        ///     Gets the console events.
        /// </summary>
        /// <value>The console events.</value>
        [NotNull]
        [ItemNotNull]
        public IEnumerable<ConsoleEvent> Events
        {
            get { return _events; }
        }
    }
}