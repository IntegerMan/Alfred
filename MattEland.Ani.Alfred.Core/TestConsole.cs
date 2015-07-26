using System.Collections.Generic;

namespace MattEland.Ani.Alfred.Core
{
    /// <summary>
    /// A simple console used for unit testing purposes
    /// </summary>
    public class TestConsole : IConsole
    {
        private List<ConsoleEvent> _events = new List<ConsoleEvent>();

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
    }
}