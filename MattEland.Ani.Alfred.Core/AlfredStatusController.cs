// ---------------------------------------------------------
// AlfredStatusController.cs
// 
// Created on:      08/19/2015 at 9:31 PM
// Last Modified:   09/03/2015 at 1:52 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Common;

namespace MattEland.Ani.Alfred.Core
{
    /// <summary>
    ///     A utility class that helps control Alfred's status and monitors the initialization and
    ///     shutdown processes.
    /// </summary>
    internal sealed class AlfredStatusController : IStatusController
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AlfredStatusController" /> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <param name="container"> The container. </param>
        internal AlfredStatusController([NotNull] IAlfredContainer container)
        {
            //- Validate
            if (container == null) { throw new ArgumentNullException(nameof(container)); }

            Container = container;
            Alfred = container.Alfred;

            Locale = container.Locale;
            Console = container.TryProvide<IConsole>();
        }

        /// <summary>
        ///     Gets the localization culture.
        /// </summary>
        /// <value>
        /// The localization culture.
        /// </value>
        [NotNull]
        private CultureInfo Locale { get; }

        /// <summary>
        ///     Gets the console associated with this item.
        /// </summary>
        /// <value>
        /// The console.
        /// </value>
        private IConsole Console { get; }

        /// <summary>
        ///     Gets the container.
        /// </summary>
        /// <value>
        /// The container.
        /// </value>
        public IAlfredContainer Container { get; }

        /// <summary>
        ///     Gets or sets the Alfred framework.
        /// </summary>
        /// <value>
        /// The Alfred framework.
        /// </value>
        [NotNull]
        public IAlfred Alfred { get; }

        /// <summary>
        ///     Gets the components to initialize / shut down. These will include subsystems and other
        ///     helper components.
        /// </summary>
        /// <value>
        ///     The components.
        /// </value>
        [NotNull]
        [ItemNotNull]
        private IEnumerable<IAlfredComponent> Components
        {
            get { return Alfred.Components; }
        }

        /// <summary>
        ///     Tells <see cref="Alfred"/> it's okay to start itself up and begin operating.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown if <see cref="Alfred"/> is already Offline
        /// </exception>
        public void Initialize()
        {
            var statusTarget = Alfred;

            var header = Resources.AlfredStatusController_Initialize_LogHeader.NonNull();

            // Handle case on initialize but already initializing or online
            EnsureAlfredOffline(statusTarget, header);

            // Inform things that we're setting up right now
            "Initializing subsystems and modules...".Log(header, LogLevel.Verbose, Console);

            statusTarget.Status = AlfredStatus.Initializing;

            // Boot up items and give them a provider
            foreach (var item in Components) { InitializeComponent(item, header); }

            // We're done. Let the world know.
            statusTarget.Status = AlfredStatus.Online;
            "Initialization Completed; notifying modules and subsystems.".Log(header,
                                                                              LogLevel.Verbose,
                                                                              Console);

            // Notify each item that startup was completed
            foreach (var item in Components) { item.OnInitializationCompleted(); }

            // Log the completion
            "Alfred is now Online.".Log(header, LogLevel.Info, Console);
        }

        /// <summary>
        ///     Tells <see cref="Alfred"/> to go ahead and shut down.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown if <see cref="Alfred"/> is already Offline
        /// </exception>
        public void Shutdown()
        {
            var statusTarget = Alfred;

            var header = Resources.AlfredStatusController_Shutdown_LogHeader.NonNull();

            // Handle cases where shutdown shouldn't be allowed
            EnsureAlfredOnline(statusTarget, header);

            // Indicate status so the UI can keep busy
            "Shutting down...".Log(header, LogLevel.Verbose, Console);
            statusTarget.Status = AlfredStatus.Terminating;

            // Shut down items and decouple them from Alfred
            foreach (var item in Components) { ShutdownComponent(item, header); }

            // We're done here. Tell the world.
            statusTarget.Status = AlfredStatus.Offline;
            "Shut down completed.".Log(header, LogLevel.Info, Console);

            // Notify each item that shutdown was completed
            foreach (var item in Components) { item.OnShutdownCompleted(); }
        }

        /// <summary>
        ///     Ensures that <paramref name="alfred"/> is offline and throws an
        ///     <see cref="InvalidOperationException" /> if it isn't.
        /// </summary>
        /// <param name="alfred">The alfred.</param>
        /// <param name="header">The header.</param>
        /// <exception cref="InvalidOperationException">Thrown if the system was online</exception>
        private void EnsureAlfredOffline([NotNull] IHasStatus alfred, [NotNull] string header)
        {
            if (alfred.Status == AlfredStatus.Online)
            {
                var message =
                    Resources.AlfredStatusController_Initialize_ErrorAlreadyOnline.NonNull();
                message.Log(header, LogLevel.Verbose, Console);

                throw new InvalidOperationException(message);
            }
        }

        /// <summary>
        ///     Initializes the component.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="header">The header.</param>
        private void InitializeComponent([NotNull] IAlfredComponent item, [NotNull] string header)
        {
            // Log the initialization event
            var initLogFormat = "Initializing {0}".NonNull();
            var message = string.Format(Locale, initLogFormat, item.NameAndVersion);
            message.Log(header, LogLevel.Verbose, Console);

            // Actually initialize the component
            // TODO: Remove this parameter and have children get Alfred from Container when needed
            item.Initialize(Alfred);

            // Log that initialization completed
            var initializedMessage = string.Format(Locale,
                                                   "{0} is now initialized.",
                                                   item.NameAndVersion);
            initializedMessage.Log(header, LogLevel.Verbose, Console);
        }

        /// <summary>
        ///     Shutdowns the subsystem.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="header">The header.</param>
        private void ShutdownComponent([NotNull] IAlfredComponent item, [NotNull] string header)
        {
            var startedMessage = string.Format(Locale, "Shutting down {0}", item.NameAndVersion);
            startedMessage.Log(header, LogLevel.Verbose, Console);

            item.Shutdown();

            var completedMessage = string.Format(Locale, "{0} is now offline.", item.NameAndVersion);
            completedMessage.Log(header, LogLevel.Verbose, Console);
        }

        /// <summary>
        ///     Ensures that Alfred is online and throws an <see cref="InvalidOperationException" />
        ///     if it isn't.
        /// </summary>
        /// <param name="alfred">The Alfred instance.</param>
        /// <param name="header">The header.</param>
        /// <exception cref="InvalidOperationException">Thrown if the system was offline</exception>
        private void EnsureAlfredOnline([NotNull] IHasStatus alfred, [NotNull] string header)
        {
            switch (alfred.Status)
            {
                case AlfredStatus.Offline:
                    var offlineMessage = "Instructed to shut down but system is already offline";
                    offlineMessage.Log(header, LogLevel.Warning, Console);

                    throw new InvalidOperationException(offlineMessage);

                case AlfredStatus.Terminating:
                    var terminatingMessage =
                        "Instructed to shut down but system is already shutting down";
                    terminatingMessage.Log(header, LogLevel.Warning, Console);

                    throw new InvalidOperationException(terminatingMessage);
            }
        }
    }
}