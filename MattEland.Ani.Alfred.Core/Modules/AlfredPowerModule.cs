// ---------------------------------------------------------
// AlfredPowerModule.cs
// 
// Created on:      08/02/2015 at 4:56 PM
// Last Modified:   08/12/2015 at 3:52 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System.Globalization;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Widgets;
using MattEland.Common;

namespace MattEland.Ani.Alfred.Core.Modules
{
    /// <summary>
    ///     A module intended for the control and monitoring of Alfred's status
    /// </summary>
    public sealed class AlfredPowerModule : AlfredModule
    {
        [NotNull]
        private readonly ButtonWidget _initializeButton;

        [NotNull]
        private readonly ButtonWidget _shutdownButton;

        [NotNull]
        private readonly TextWidget _statusWidget;

        /// <summary>
        ///     Initializes a new instance of the
        ///     <see cref="AlfredPowerModule" />
        ///     class.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">
        /// </exception>
        /// <param name="platformProvider">
        ///     The platform provider.
        /// </param>
        public AlfredPowerModule([NotNull] IPlatformProvider platformProvider) : base(platformProvider)
        {
            _statusWidget = new TextWidget(Resources.AlfredCoreModule_AlfredNotSet);

            var initializeCommand = platformProvider.CreateCommand(ExecuteInitializeCommand);
            _initializeButton = new ButtonWidget(Resources.InitializeButtonText, initializeCommand);

            var shutdownCommand = platformProvider.CreateCommand(ExecuteShutdownCommand);
            _shutdownButton = new ButtonWidget(Resources.ShutdownButtonText, shutdownCommand);
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
        ///     Gets the alfred status widget.
        /// </summary>
        /// <value>The alfred status widget.</value>
        [NotNull]
        public TextWidget AlfredStatusWidget
        {
            get { return _statusWidget; }
        }

        /// <summary>
        ///     Gets the initialize button.
        /// </summary>
        /// <value>The initialize button.</value>
        [NotNull]
        public ButtonWidget InitializeButton
        {
            get { return _initializeButton; }
        }

        /// <summary>
        ///     Gets the shutdown button.
        /// </summary>
        /// <value>The shutdown button.</value>
        [NotNull]
        public ButtonWidget ShutdownButton
        {
            get { return _shutdownButton; }
        }

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
            Register(_statusWidget);
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
            Register(_statusWidget);
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
        /// <param name="alfred">The alfred.</param>
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
        ///     Updates the alfred provider status text.
        /// </summary>
        private void UpdateAlfredProviderStatus()
        {
            if (AlfredInstance == null)
            {
                // Update Text Message to a Nobody's Home sort of thing
                _statusWidget.Text = Resources.NoAlfredText;

                // Update Button Visibilities to hidden
                if (_shutdownButton.ClickCommand != null)
                {
                    _shutdownButton.ClickCommand.IsEnabled = false;
                }
                if (_initializeButton.ClickCommand != null)
                {
                    _initializeButton.ClickCommand.IsEnabled = false;
                }
            }
            else
            {
                // Display the current status
                var statusFormat = Resources.AlfredCoreModule_AlfredStatusText.NonNull();
                _statusWidget.Text = string.Format(CultureInfo.CurrentCulture,
                                                   statusFormat,
                                                   AlfredInstance.Name,
                                                   AlfredInstance.Status);

                // Show the shutdown button while online and initialize button while offline
                if (_shutdownButton.ClickCommand != null)
                {
                    _shutdownButton.ClickCommand.IsEnabled = AlfredInstance.Status == AlfredStatus.Online;
                }
                if (_initializeButton.ClickCommand != null)
                {
                    _initializeButton.ClickCommand.IsEnabled = AlfredInstance.Status == AlfredStatus.Offline;
                }
            }
        }

        /// <summary>
        /// Processes an Alfred Command. If the command is handled, result should be modified accordingly and the method should return true. Returning false will not stop the message from being propogated.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="result">The result. If the command was handled, this should be updated.</param>
        /// <returns><c>True</c> if the command was handled; otherwise false.</returns>
        public override bool ProcessAlfredCommand(ChatCommand command, AlfredCommandResult result)
        {
            if (command.Command.Matches("Shutdown"))
            {
                ExecuteShutdownCommand();
                return true;
            }

            return base.ProcessAlfredCommand(command, result);
        }

        /// <summary>
        ///     A notification method that is invoked when initialization for Alfred is complete so the UI can be fully enabled or
        ///     adjusted
        /// </summary>
        public override void OnInitializationCompleted()
        {
            base.OnInitializationCompleted();

            UpdateAlfredProviderStatus();
        }

        /// <summary>
        ///     A notification method that is invoked when shutdown for Alfred is complete so the UI can be fully enabled or
        ///     adjusted
        /// </summary>
        public override void OnShutdownCompleted()
        {
            base.OnShutdownCompleted();

            UpdateAlfredProviderStatus();
        }
    }
}