// ---------------------------------------------------------
// AlfredStatusController.cs
// 
// Created on:      08/03/2015 at 3:09 PM
// Last Modified:   08/05/2015 at 2:35 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.Globalization;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Console;

namespace MattEland.Ani.Alfred.Core
{
    /// <summary>
    ///     A utility class that helps control Alfred's status and monitors the initialization and shutdown processes.
    /// </summary>
    public sealed class AlfredStatusController
    {
        [NotNull]
        private readonly AlfredProvider _alfred;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AlfredStatusController" /> class.
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
        ///     Gets the alfred provider.
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
        /// <exception cref="InvalidOperationException">
        ///     Thrown if Alfred is already Online
        /// </exception>
        public void Initialize()
        {
            var header = Resources.AlfredStatusController_Initialize_LogHeader.NonNull();
            var culture = CultureInfo.CurrentCulture;

            var console = Alfred.Console;

            // Handle case on initialize but already initializing or online
            if (Alfred.Status == AlfredStatus.Online)
            {
                var message = Resources.AlfredStatusController_Initialize_ErrorAlreadyOnline.NonNull();
                console?.Log(header, message, LogLevel.Verbose);

                throw new InvalidOperationException(message);
            }

            // Inform things that we're setting up right now
            console?.Log(header, Resources.AlfredStatusController_Initialize_Initializing.NonNull(), LogLevel.Verbose);
            Alfred.Status = AlfredStatus.Initializing;

            // Boot up Modules and give them a provider
            foreach (var module in Alfred.Modules)
            {
                // Log the initialization
                var initLogFormat = Resources.AlfredStatusController_Initialize_InitializingModule.NonNull();
                console?.Log(header, string.Format(culture, initLogFormat, module.NameAndVersion), LogLevel.Verbose);

                // Actually initialize the module
                module.Initialize(Alfred);

                // Log the completion
                var initializedLogFormat = Resources.AlfredStatusController_Initialize__ModuleInitialized.NonNull();
                console?.Log(header, string.Format(culture, initializedLogFormat, module.NameAndVersion), LogLevel.Verbose);
            }

            // We're done. Let the world know.
            Alfred.Status = AlfredStatus.Online;
            console?.Log(header, Resources.AlfredStatusController_Initialize_InitilizationCompleted.NonNull(), LogLevel.Verbose);

            // Notify each module that startup was completed
            foreach (var module in Alfred.Modules)
            {
                module.OnInitializationCompleted();
            }

            // Log the completion
            console?.Log(header, Resources.AlfredStatusController_Initialize_AlfredOnline.NonNull(), LogLevel.Info);
        }

        /// <summary>
        ///     Tells Alfred to go ahead and shut down.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        ///     Thrown if Alfred is already Offline
        /// </exception>
        public void Shutdown()
        {
            var header = Resources.AlfredStatusController_Shutdown_LogHeader.NonNull();

            var console = Alfred.Console;

            // Handle cases where shutdown shouldn't be allowed
            switch (Alfred.Status)
            {
                case AlfredStatus.Offline:
                    var offlineMessage = Resources.AlfredStatusController_Shutdown_ErrorAlreadyOffline.NonNull();
                    console?.Log(header, offlineMessage, LogLevel.Verbose);

                    throw new InvalidOperationException(offlineMessage);

                case AlfredStatus.Terminating:
                    var terminatingMessage = Resources.AlfredStatusController_Shutdown_ErrorAlreadyTerminating.NonNull();
                    console?.Log(header, terminatingMessage, LogLevel.Verbose);

                    throw new InvalidOperationException(terminatingMessage);
            }

            // Indicate status so the UI can keep busy
            console?.Log(header, Resources.AlfredStatusController_Shutdown_Shutting_down.NonNull(), LogLevel.Verbose);
            Alfred.Status = AlfredStatus.Terminating;

            // Shut down modules and decouple them from Alfred
            foreach (var module in Alfred.Modules)
            {
                var culture = CultureInfo.CurrentCulture;

                var shuttingDownMessage = Resources.AlfredStatusController_Shutdown_ShuttingDownModule.NonNull();
                console?.Log(header, string.Format(culture, shuttingDownMessage, module.NameAndVersion), LogLevel.Verbose);

                module.Shutdown();

                var shutDownMessage = Resources.AlfredStatusController_Shutdown_ModuleOffline.NonNull();
                console?.Log(header, string.Format(culture, shutDownMessage, module.NameAndVersion), LogLevel.Verbose);
            }

            // We're done here. Tell the world.
            Alfred.Status = AlfredStatus.Offline;
            console?.Log(header, Resources.AlfredStatusController_Shutdown_Completed.NonNull(), LogLevel.Info);

            // Notify each module that shutdown was completed
            foreach (var module in Alfred.Modules)
            {
                module.OnShutdownCompleted();
            }

        }
    }
}