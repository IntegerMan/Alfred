using JetBrains.Annotations;

namespace MattEland.Ani.Alfred.Core.Console
{
    /// <summary>
    /// A simple factory that creates console events. This factory can be inherited from allowing customizations to the event creation process.
    /// </summary>
    public class ConsoleEventFactory
    {
        /// <summary>
        /// Creates a <see cref="IConsoleEvent"/>.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="message">The message.</param>
        /// <param name="level">The log level.</param>
        /// <returns>The <see cref="IConsoleEvent"/>.</returns>
        [NotNull]
        public virtual IConsoleEvent CreateEvent(string title, string message, LogLevel level)
        {
            return new ConsoleEvent(title, message, level);
        }
    }

}