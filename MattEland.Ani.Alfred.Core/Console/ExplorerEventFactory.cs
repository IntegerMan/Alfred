namespace MattEland.Ani.Alfred.Core.Console
{
    /// <summary>
    /// A <see cref="ConsoleEventFactory"/> that returns <see cref="ConsoleEventExplorerDecorator"/> events.
    /// </summary>
    public class ExplorerEventFactory : ConsoleEventFactory
    {
        /// <summary>
        /// Creates a <see cref="IConsoleEvent"/>.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="message">The message.</param>
        /// <param name="level">The log level.</param>
        /// <returns>The <see cref="IConsoleEvent"/>.</returns>
        public override IConsoleEvent CreateEvent(string title, string message, LogLevel level)
        {
            var consoleEvent = base.CreateEvent(title, message, level);

            var decorator = new ConsoleEventExplorerDecorator(consoleEvent);

            return decorator;
        }
    }
}