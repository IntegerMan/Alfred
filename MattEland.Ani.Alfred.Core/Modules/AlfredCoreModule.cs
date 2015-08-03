// ---------------------------------------------------------
// AlfredCoreModule.cs
// 
// Created on:      08/02/2015 at 4:56 PM
// Last Modified:   08/03/2015 at 2:43 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Widgets;

namespace MattEland.Ani.Alfred.Core.Modules
{
    /// <summary>
    ///     A module intended for the control and monitoring of Alfred
    /// </summary>
    public sealed class AlfredCoreModule : AlfredModule
    {
        [NotNull]
        public const string NoAlfredProviderMessage = "Alfred Provider has not been set";

        [NotNull]
        private readonly ButtonWidget _initializeButton;

        [NotNull]
        private readonly ButtonWidget _shutdownButton;

        [NotNull]
        private readonly TextWidget _statusWidget;

        [CanBeNull]
        private AlfredProvider _alfredProvider;

        /// <summary>
        ///     Initializes a new instance of the
        ///     <see
        ///         cref="AlfredCoreModule" />
        ///     class.
        /// </summary>
        /// <exception
        ///     cref="System.ArgumentNullException">
        /// </exception>
        /// <param
        ///     name="platformProvider">
        ///     The platform provider.
        /// </param>
        public AlfredCoreModule([NotNull] IPlatformProvider platformProvider) : base(platformProvider)
        {
            _statusWidget = new TextWidget(NoAlfredProviderMessage);

            var initializeCommand = platformProvider.CreateCommand(() => _alfredProvider?.Initialize());
            _initializeButton = new ButtonWidget("Initialize", initializeCommand);

            var shutdownCommand = platformProvider.CreateCommand(() => _alfredProvider?.Shutdown());
            _shutdownButton = new ButtonWidget("Shut Down", shutdownCommand);
        }

        /// <summary>
        ///     Initializes a new instance of the
        ///     <see
        ///         cref="AlfredCoreModule" />
        ///     class.
        /// </summary>
        /// <param
        ///     name="platformProvider">
        ///     The collection provider.
        /// </param>
        /// <param
        ///     name="alfredProvider">
        ///     The alfred provider.
        /// </param>
        public AlfredCoreModule(
            [NotNull] IPlatformProvider platformProvider,
            [CanBeNull] AlfredProvider alfredProvider) : this(platformProvider)
        {
            AlfredProvider = alfredProvider;
        }

        /// <summary>
        ///     Gets the name of the module.
        /// </summary>
        /// <value>The name of the module.</value>
        [NotNull]
        public override string Name
        {
            get { return "Alfred Core"; }
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
        ///     Gets or sets the alfred provider. This will update the display of the widget accordingly.
        /// </summary>
        /// <value>The alfred provider.</value>
        [CanBeNull]
        public AlfredProvider AlfredProvider
        {
            get { return _alfredProvider; }
            set
            {
                _alfredProvider = value;

                UpdateAlfredProviderStatus();

                if (value != null)
                {
                    // Just in case we're changing providers midstream or after shutdown, clear things out
                    Widgets?.Clear();

                    // Add the appropriate UI elements
                    if (value.Status == AlfredStatus.Online)
                    {
                        AddOnlineWidgets();
                    }
                    else
                    {
                        AddOfflineWidgets();
                    }
                }
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
        protected override void InitializeProtected()
        {
            AddOnlineWidgets();
        }

        /// <summary>
        ///     Adds the widgets used while in online mode.
        /// </summary>
        private void AddOnlineWidgets()
        {
            RegisterWidget(_statusWidget);
            RegisterWidget(ShutdownButton);
        }

        /// <summary>
        ///     Handles module shutdown events
        /// </summary>
        protected override void ShutdownProtected()
        {
            // We're going to re-register the status widget since it's always present - even when the system is offline
            AddOfflineWidgets();
        }

        /// <summary>
        ///     Adds the widgets used in offline mode.
        /// </summary>
        private void AddOfflineWidgets()
        {
            RegisterWidget(_statusWidget);
            RegisterWidget(InitializeButton);
        }

        /// <summary>
        ///     Handles updating the module as needed
        /// </summary>
        protected override void UpdateProtected()
        {
            UpdateAlfredProviderStatus();
        }

        /// <summary>
        ///     Updates the alfred provider status text.
        /// </summary>
        private void UpdateAlfredProviderStatus()
        {
            // If we don't have a provider, use a static message, otherwise display the provided status.
            _statusWidget.Text = AlfredProvider == null
                                     ? NoAlfredProviderMessage
                                     : $"{AlfredProvider.NameAndVersion} is currently {AlfredProvider.Status}";
        }
    }
}