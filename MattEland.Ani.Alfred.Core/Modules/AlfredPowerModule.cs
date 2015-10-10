// ---------------------------------------------------------
// AlfredPowerModule.cs
// 
// Created on:      08/19/2015 at 9:31 PM
// Last Modified:   08/24/2015 at 11:53 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Globalization;

using MattEland.Common.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Widgets;
using MattEland.Common;
using MattEland.Common.Providers;

namespace MattEland.Ani.Alfred.Core.Modules
{
    /// <summary>
    ///     A module intended for the control and monitoring of Alfred's status
    /// </summary>
    public sealed class AlfredPowerModule : AlfredModule
    {
        /// <summary>
        ///     Initializes a new instance of the
        ///     <see cref="AlfredPowerModule" />
        ///     class.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <param name="container"> The container. </param>
        public AlfredPowerModule([NotNull] IAlfredContainer container)
            : base(container)
        {
            if (container == null) { throw new ArgumentNullException(nameof(container)); }

            AlfredStatusWidget = new TextWidget(Resources.AlfredCoreModule_AlfredNotSet,
                                                BuildWidgetParameters(@"lblStatus"));

            var initializeCommand = container.BuildCommand(ExecuteInitializeCommand);
            InitializeButton = new ButtonWidget(Resources.InitializeButtonText,
                                                initializeCommand,
                                                BuildWidgetParameters(@"btnInitialize"));

            var shutdownCommand = container.BuildCommand(ExecuteShutdownCommand);
            ShutdownButton = new ButtonWidget(Resources.ShutdownButtonText,
                                              shutdownCommand,
                                              BuildWidgetParameters(@"btnShutdown"));
        }

        /// <summary>
        ///     Gets the name of the module.
        /// </summary>
        /// <value>The name of the module.</value>
        [NotNull]
        public override string Name
        {
            get { return Resources.AlfredCoreModule_Name.NonNull(); }
        }

        /// <summary>
        ///     Gets the Alfred status widget.
        /// </summary>
        /// <value>The Alfred status widget.</value>
        [NotNull]
        public TextWidget AlfredStatusWidget { get; }

        /// <summary>
        ///     Gets the initialize button.
        /// </summary>
        /// <value>The initialize button.</value>
        [NotNull]
        public ButtonWidget InitializeButton { get; }

        /// <summary>
        ///     Gets the shutdown button.
        /// </summary>
        /// <value>The shutdown button.</value>
        [NotNull]
        public ButtonWidget ShutdownButton { get; }

        /// <summary>
        ///     Handles the initialize command by initializing Alfred
        /// </summary>
        internal void ExecuteInitializeCommand()
        {
            AlfredInstance?.Initialize();
        }

        /// <summary>
        ///     Handles the shutdown command by shutting down Alfred
        /// </summary>
        internal void ExecuteShutdownCommand()
        {
            AlfredInstance?.Shutdown();
        }

        /// <summary>
        ///     Handles module initialization events
        /// </summary>
        /// <param name="alfred"></param>
        protected override void InitializeProtected(IAlfred alfred)
        {
            AddOnlineWidgets();
        }

        /// <summary>
        ///     Adds the widgets used while in online mode.
        /// </summary>
        private void AddOnlineWidgets()
        {
            ClearChildCollections();
            Register(AlfredStatusWidget);
            Register(ShutdownButton);
        }

        /// <summary>
        ///     Handles module shutdown events
        /// </summary>
        protected override void ShutdownProtected()
        {
            // We're going to re-register the status widget since it's always present - even when the system is offline
            AddOfflineWidgets();

            // We need to do this so that buttons render properly
            UpdateAlfredProviderStatus();
        }

        /// <summary>
        ///     Adds the widgets used in offline mode.
        /// </summary>
        private void AddOfflineWidgets()
        {
            ClearChildCollections();
            Register(AlfredStatusWidget);
            Register(InitializeButton);
        }

        /// <summary>
        ///     Handles updating the module as needed
        /// </summary>
        protected override void UpdateProtected()
        {
            UpdateAlfredProviderStatus();
        }

        /// <summary>
        ///     Called when the component is registered.
        /// </summary>
        /// <param name="alfred">The Alfred instance.</param>
        public override void OnRegistered(IAlfred alfred)
        {
            base.OnRegistered(alfred);

            UpdateAlfredProviderStatus();

            if (AlfredInstance != null)
            {
                // Add the appropriate UI elements
                if (AlfredInstance.Status == AlfredStatus.Online)
                {
                    AddOnlineWidgets();
                }
                else
                {
                    AddOfflineWidgets();
                }
            }
        }

        /// <summary>
        ///     Updates the Alfred provider status text.
        /// </summary>
        private void UpdateAlfredProviderStatus()
        {
            if (AlfredInstance == null)
            {
                // Update Text Message to a Nobody's Home sort of thing
                AlfredStatusWidget.Text = Resources.NoAlfredText;

                // Update Button Visibilities to hidden
                if (ShutdownButton.ClickCommand != null)
                {
                    ShutdownButton.ClickCommand.IsEnabled = false;
                }
                if (InitializeButton.ClickCommand != null)
                {
                    InitializeButton.ClickCommand.IsEnabled = false;
                }
            }
            else
            {
                // Display the current status
                var statusFormat = Resources.AlfredCoreModule_AlfredStatusText.NonNull();
                AlfredStatusWidget.Text = string.Format(CultureInfo.CurrentCulture,
                                                        statusFormat,
                                                        AlfredInstance.Name,
                                                        AlfredInstance.Status);

                // Show the shutdown button while online and initialize button while offline
                var isOnline = AlfredInstance.Status == AlfredStatus.Online;
                if (ShutdownButton.ClickCommand != null)
                {
                    ShutdownButton.ClickCommand.IsEnabled = isOnline;
                }
                if (InitializeButton.ClickCommand != null)
                {
                    InitializeButton.ClickCommand.IsEnabled = !isOnline;
                }
            }
        }

        /// <summary>
        ///     Processes an Alfred Command. If the command is handled, result should be modified accordingly
        ///     and the method should return true. Returning false will not stop the message from being
        ///     propagated.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="result">The result. If the command was handled, this should be updated.</param>
        /// <returns><c>True</c> if the command was handled; otherwise false.</returns>
        public override bool ProcessAlfredCommand(ChatCommand command, ICommandResult result)
        {
            if (command.Name.Matches("Shutdown"))
            {
                ExecuteShutdownCommand();
                return true;
            }

            return base.ProcessAlfredCommand(command, result);
        }

        /// <summary>
        ///     A notification method that is invoked when initialization for Alfred is complete so the UI can
        ///     be fully enabled or
        ///     adjusted
        /// </summary>
        public override void OnInitializationCompleted()
        {
            base.OnInitializationCompleted();

            UpdateAlfredProviderStatus();
        }

        /// <summary>
        ///     A notification method that is invoked when shutdown for Alfred is complete so the UI can be
        ///     fully enabled or
        ///     adjusted
        /// </summary>
        public override void OnShutdownCompleted()
        {
            base.OnShutdownCompleted();

            UpdateAlfredProviderStatus();
        }
    }
}