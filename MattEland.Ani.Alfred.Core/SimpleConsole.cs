using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

namespace MattEland.Ani.Alfred.Core
{
    /// <summary>
    /// A simple console used for unit testing and designer window purposes
    /// </summary>
    public class SimpleConsole : IConsole
    {
        /// <summary>
        /// Logs the specified message to the console.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="body">The body.</param>
        public void Log(string title, string body)
        {
            var evt = new ConsoleEvent(title, body);

            _events.Add(evt);
        }

        private readonly List<ConsoleEvent> _events = new List<ConsoleEvent>();

        /// <summary>
        /// Gets the console events.
        /// </summary>
        /// <value>The console events.</value>
        public IList<ConsoleEvent> Events => _events;
    }
}