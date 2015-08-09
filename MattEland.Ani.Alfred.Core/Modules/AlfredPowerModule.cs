// ---------------------------------------------------------
// AlfredPowerModule.cs
// 
// Created on:      08/02/2015 at 4:56 PM
// Last Modified:   08/07/2015 at 11:55 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System.Globalization;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Widgets;

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

            var initializeCommand = platformProvider.CreateCommand(() => AlfredInstance?.Initialize());
            _initializeButton = new ButtonWidget("Initialize", initializeCommand);

            var shutdownCommand = platformProvider.CreateCommand(() => AlfredInstance?.Shutdown());
            _shutdownButton = new ButtonWidget("Shut Down", shutdownCommand);
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
        ///     Gets whether or not the module is visible to the user interface.
        /// </summary>
        /// <value>Whether or not the module is visible.</value>
        public override bool IsVisible
        {
            get
            {
                // The power module will always need to be visible
                return true;
            }
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
        ///     Handles module initialization events
        /// </summary>
        /// <param name="alfred"></param>
        protected override void InitializeProtected(AlfredProvider alfred)
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
        /// Called when the component is registered.
        /// </summary>
        /// <param name="alfred">The alfred.</param>
        public override void OnRegistered(AlfredProvider alfred)
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
                _statusWidget.Text = "Alfred Provider has not been set";

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