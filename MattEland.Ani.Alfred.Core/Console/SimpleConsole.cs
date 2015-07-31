using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

namespace MattEland.Ani.Alfred.Core.Console
{
    /// <summary>
    /// A simple console used for unit testing and designer window purposes
    /// </summary>
    public sealed class SimpleConsole : IConsole
    {
        /// <summary>
        /// Logs the specified message to the console.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="body">The body.</param>
        public void Log([NotNull] string title, [NotNull] string body)
        {
            if (title == null)
            {
                throw new ArgumentNullException(nameof(title));
            }

            if (body == null)
            {
                throw new ArgumentNullException(nameof(body));
            }

            var evt = new ConsoleEvent(title, body);

            _events.Add(evt);
        }

        [NotNull]
        private readonly List<ConsoleEvent> _events = new List<ConsoleEvent>();

        /// <summary>
        /// Gets the console events.
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