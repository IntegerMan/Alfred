namespace MattEland.Ani.Alfred.Core
{
    /// <summary>
    /// Coordinates providing personal assistance to a user interface and receiving settings and queries back from the user
    /// interface.
    /// </summary>
    public class AlfredProvider
    {
        /// <summary>
        /// Gets or sets the console.
        /// </summary>
        /// <value>The console.</value>
        public IConsole Console { get; set; }

        /// <summary>
        /// Gets or sets the name and version string for display purposes.
        /// </summary>
        /// <value>The name and version string.</value>
        public string NameAndVersionString => "Alfred 0.1 Alpha";

        /// <summary>
        /// Tells Alfred it's okay to start itself up and begin operating.
        /// </summary>
        public void Initialize()
        {

            const string LogHeader = "Alfred.Initialize";

            Console?.Log(LogHeader, "Initializing...");

            // TODO: Set things up here

            Console?.Log(LogHeader, "Initilization Completed.");

        }

        /// <summary>
        /// Tells Alfred to go ahead and shut down.
        /// </summary>
        public void Shutdown()
        {

            const string LogHeader = "Alfred.Shutdown";

            Console?.Log(LogHeader, "Shutting down...");

            // TODO: Tear things down

            Console?.Log(LogHeader, "Shut down completed.");

        }
    }
}