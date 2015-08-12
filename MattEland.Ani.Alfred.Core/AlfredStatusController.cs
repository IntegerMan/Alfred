// ---------------------------------------------------------
// AlfredStatusController.cs
// 
// Created on:      08/03/2015 at 3:09 PM
// Last Modified:   08/09/2015 at 9:38 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.Globalization;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Common;

namespace MattEland.Ani.Alfred.Core
{
    /// <summary>
    ///     A utility class that helps control Alfred's status and monitors the initialization and shutdown processes.
    /// </summary>
    public sealed class AlfredStatusController : IStatusController
    {
        [CanBeNull]
        private IAlfred _alfred;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AlfredStatusController" /> class.
        /// </summary>
        public AlfredStatusController() : this(null)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AlfredStatusController" /> class.
        /// </summary>
        /// <param name="alfred">The alfred provider.</param>
        public AlfredStatusController([CanBeNull] IAlfred alfred)
        {
            Alfred = alfred;
        }

        /// <summary>
        ///     Gets or sets the alfred framework.
        /// </summary>
        /// <value>The alfred framework.</value>
        public IAlfred Alfred
        {
            get { return _alfred; }
            set { _alfred = value; }
        }

        /// <summary>
        ///     Tells Alfred it's okay to start itself up and begin operating.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        ///     Thrown if Alfred is already Online
        /// </exception>
        public void Initialize()
        {
            var alfred = Alfred;
            if (alfred == null)
            {
                throw new InvalidOperationException("Alfred is not set. Please set Alfred first");
            }

            var header = Resources.AlfredStatusController_Initialize_LogHeader.NonNull();

            var console = alfred.Console;

            // Handle case on initialize but already initializing or online
            EnsureAlfredOffline(alfred, console, header);

            // Inform things that we're setting up right now
            console?.Log(header, Resources.AlfredStatusController_Initialize_Initializing.NonNull(), LogLevel.Verbose);
            alfred.Status = AlfredStatus.Initializing;

            // Boot up items and give them a provider
            foreach (var item in alfred.Subsystems)
            {
                InitializeComponent(console, item, header);
            }

            // We're done. Let the world know.
            alfred.Status = AlfredStatus.Online;
            console?.Log(header,
                         Resources.AlfredStatusController_Initialize_InitilizationCompleted.NonNull(),
                         LogLevel.Verbose);

            // Notify each item that startup was completed
            foreach (var item in alfred.Subsystems)
            {
                item.OnInitializationCompleted();
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
            var alfred = Alfred;
            if (alfred == null)
            {
                throw new InvalidOperationException("Alfred is not set. Please set Alfred first");
            }

            var header = Resources.AlfredStatusController_Shutdown_LogHeader.NonNull();

            var console = alfred.Console;

            // Handle cases where shutdown shouldn't be allowed
            EnsureAlfredOnline(alfred, console, header);

            // Indicate status so the UI can keep busy
            console?.Log(header, Resources.AlfredStatusController_Shutdown_Shutting_down.NonNull(), LogLevel.Verbose);
            alfred.Status = AlfredStatus.Terminating;

            // Shut down items and decouple them from Alfred
            foreach (var item in alfred.Subsystems)
            {
                ShutdownComponent(console, item, header);
            }

            // We're done here. Tell the world.
            alfred.Status = AlfredStatus.Offline;
            console?.Log(header, Resources.AlfredStatusController_Shutdown_Completed.NonNull(), LogLevel.Info);

            // Notify each item that shutdown was completed
            foreach (var item in alfred.Subsystems)
            {
                item.OnShutdownCompleted();
            }
        }

        /// <summary>
        ///     Ensures that alfred is offline and throws an InvalidOperationException if it isn't.
        /// </summary>
        /// <param name="alfred">The alfred.</param>
        /// <param name="console">The console.</param>
        /// <param name="header">The header.</param>
        private static void EnsureAlfredOffline([NotNull] IAlfred alfred,
                                                [CanBeNull] IConsole console,
                                                [NotNull] string header)
        {
            if (alfred.Status == AlfredStatus.Online)
            {
                var message = Resources.AlfredStatusController_Initialize_ErrorAlreadyOnline.NonNull();
                console?.Log(header, message, LogLevel.Verbose);

                throw new InvalidOperationException(message);
            }
        }

        /// <summary>
        ///     Initializes the component.
        /// </summary>
        /// <param name="console">The console.</param>
        /// <param name="item">The item.</param>
        /// <param name="header">The header.</param>
        private void InitializeComponent([CanBeNull] IConsole console,
                                         [NotNull] IAlfredComponent item,
                                         [NotNull] string header)
        {

            // Log the initialization
            var initLogFormat = Resources.AlfredStatusController_InitializingComponent.NonNull();
            console?.Log(header,
                         string.Format(CultureInfo.CurrentCulture, initLogFormat, item.NameAndVersion),
                         LogLevel.Verbose);

            // Actually initialize the component
            item.Initialize(Alfred);

            // Log the completion
            var initializedLogFormat = Resources.AlfredStatusController_InitializeComponentInitialized.NonNull();
            console?.Log(header,
                         string.Format(CultureInfo.CurrentCulture, initializedLogFormat, item.NameAndVersion),
                         LogLevel.Verbose);
        }

        /// <summary>
        ///     Shutdowns the subsystem.
        /// </summary>
        /// <param name="console">The console.</param>
        /// <param name="item">The item.</param>
        /// <param name="header">The header.</param>
        private static void ShutdownComponent([CanBeNull] IConsole console,
                                              [NotNull] IAlfredComponent item,
                                              [NotNull] string header)
        {

            var culture = CultureInfo.CurrentCulture;

            var shuttingDownMessage = Resources.AlfredStatusController_ShuttingDownComponent.NonNull();
            console?.Log(header, string.Format(culture, shuttingDownMessage, item.NameAndVersion), LogLevel.Verbose);

            item.Shutdown();

            var shutDownMessage = Resources.AlfredStatusController_ComponentOffline.NonNull();
            console?.Log(header, string.Format(culture, shutDownMessage, item.NameAndVersion), LogLevel.Verbose);
        }

        /// <summary>
        ///     Ensures that alfred online.
        /// </summary>
        /// <param name="alfred">The alfred.</param>
        /// <param name="console">The console.</param>
        /// <param name="header">The header.</param>
        private static void EnsureAlfredOnline([NotNull] IAlfred alfred,
                                               [CanBeNull] IConsole console,
                                               [NotNull] string header)
        {
            switch (alfred.Status)
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
        }
    }
}