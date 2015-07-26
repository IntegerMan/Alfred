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
        /// Adds the event to the console.
        /// </summary>
        /// <param name="consoleEvent">The console event.</param>
        void AddEvent(ConsoleEvent consoleEvent);

    }
}
