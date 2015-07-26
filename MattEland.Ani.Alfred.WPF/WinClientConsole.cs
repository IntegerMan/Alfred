using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core;

namespace MattEland.Ani.Alfred.WPF
{
    public class WinClientConsole : IConsole
    {
        private readonly ObservableCollection<ConsoleEvent> _events = new ObservableCollection<ConsoleEvent>();

        /// <summary>
        /// Logs an event with the specified title and body.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="body">The body.</param>
        /// <exception cref="System.ArgumentNullException">
        /// </exception>
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

            _events.Add(new ConsoleEvent(title, body));
        }

        /// <summary>
        /// Gets or sets the events.
        /// </summary>
        /// <value>The events.</value>
        [ItemNotNull]
        [NotNull]
        public IEnumerable<ConsoleEvent> Events => _events;
    }
}