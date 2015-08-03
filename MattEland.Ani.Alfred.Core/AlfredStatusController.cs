using System;

using JetBrains.Annotations;

namespace MattEland.Ani.Alfred.Core
{
    /// <summary>
    /// A utility class that helps control Alfred's status and monitors the initialization and shutdown processes.
    /// </summary>
    public sealed class AlfredStatusController
    {

        [NotNull]
        private readonly AlfredProvider _alfred;

        /// <summary>
        /// Initializes a new instance of the <see cref="AlfredStatusController" /> class.
        /// </summary>
        /// <param name="alfred">The alfred provider.</param>
        public AlfredStatusController([NotNull] AlfredProvider alfred)
        {
            if (alfred == null)
            {
                throw new ArgumentNullException(nameof(alfred));
            }

            _alfred = alfred;
        }

        /// <summary>
        /// Gets the alfred provider.
        /// </summary>
        /// <value>The alfred provider.</value>
        [NotNull]
        public AlfredProvider Alfred
        {
            get { return _alfred; }
        }

        /// <summary>
        ///     Tells Alfred it's okay to start itself up and begin operating.
        /// </summary>
        /// <exception
        ///     cref="InvalidOperationException">
        ///     Thrown if Alfred is already Online
        /// </exception>
        public void Initialize()
        {
            const string LogHeader = "Alfred.Initialize";

            var console = Alfred.Console;

            // Handle case on initialize but already initializing or online
            if (Alfred.Status == AlfredStatus.Online)
            {
                const string Message = "Instructed to initialize but system is already online";
                console?.Log(LogHeader, Message);

                throw new InvalidOperationException(Message);
            }

            // Inform things that we're setting up right now
            console?.Log(LogHeader, "Initializing...");
            Alfred.Status = AlfredStatus.Initializing;

            // Boot up Modules and give them a provider
            foreach (var module in Alfred.Modules)
            {
                console?.Log(LogHeader, $"Initializing {module.NameAndVersion}");
                module.Initialize(Alfred);
                console?.Log(LogHeader, $"{module.NameAndVersion} is now initialized.");
            }

            // We're done. Let the world know.
            Alfred.Status = AlfredStatus.Online;
            console?.Log(LogHeader, "Initilization Completed; notifying modules.");


            // Notify each module that startup was completed
            foreach (var module in Alfred.Modules)
            {
                module.OnInitializationCompleted();
            }

            console?.Log(LogHeader, "Alfred is now Online.");

        }

        /// <summary>
        ///     Tells Alfred to go ahead and shut down.
        /// </summary>
        /// <exception
        ///     cref="InvalidOperationException">
        ///     Thrown if Alfred is already Offline
        /// </exception>
        public void Shutdown()
        {
            const string LogHeader = "Alfred.Shutdown";

            var console = Alfred.Console;

            // Handle case on shutdown but already offline
            if (Alfred.Status == AlfredStatus.Offline)
            {
                const string Message = "Instructed to shut down but system is already offline";
                console?.Log(LogHeader, Message);

                throw new InvalidOperationException(Message);
            }

            // Handle case on shutdown but already terminating
            if (Alfred.Status == AlfredStatus.Terminating)
            {
                const string Message = "Instructed to shut down but system is already shutting down";
                console?.Log(LogHeader, Message);

                throw new InvalidOperationException(Message);
            }

            // Indicate status so the UI can keep busy
            console?.Log(LogHeader, "Shutting down...");
            Alfred.Status = AlfredStatus.Terminating;

            // Shut down modules and decouple them from Alfred
            foreach (var module in Alfred.Modules)
            {
                console?.Log(LogHeader, $"Shutting down {module.NameAndVersion}");
                module.Shutdown();
                console?.Log(LogHeader, $"{module.NameAndVersion} is now offline.");
            }

            // We're done here. Tell the world.
            Alfred.Status = AlfredStatus.Offline;
            console?.Log(LogHeader, "Shut down completed.");

            // Notify each module that shutdown was completed
            foreach (var module in Alfred.Modules)
            {
                module.OnShutdownCompleted();
            }

        }
    }
}