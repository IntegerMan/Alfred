using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MattEland.Ani.Alfred.Core
{
    /// <summary>
    /// An interface describing the display console Alfred can interact with
    /// </summary>
    public interface IConsole
    {
        /// <summary>
        /// Logs the specified message to the console.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="body">The body.</param>
        void Log(string title, string body);

        /// <summary>
        /// Gets the console events.
        /// </summary>
        /// <value>The console events.</value>
        IList<ConsoleEvent> Events { get; }
    }
}
