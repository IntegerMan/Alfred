namespace MattEland.Ani.Alfred.Core
{
    /// <summary>
    ///     Coordinates providing personal assistance to a user interface and receiving settings and queries back from the user
    ///     interface.
    /// </summary>
    public class AlfredProvider
    {
        /// <summary>
        /// Gets or sets the console.
        /// </summary>
        /// <value>The console.</value>
        public IConsole Console { get; set; }

        /// <summary>
        /// Tells Alfred it's okay to start itself up and begin operating.
        /// </summary>
        public void Initialize()
        {
            const string LogHeader = "Alfred.Initialize";
            var console = Console;

            console?.Log(LogHeader, "Initializing System...");

            // TODO: Set things up here

            console?.Log(LogHeader, "Initilization Completed.");
        }
    }
}