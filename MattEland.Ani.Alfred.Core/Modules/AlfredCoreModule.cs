// ---------------------------------------------------------
// AlfredCoreModule.cs
// 
// Created on:      08/02/2015 at 4:56 PM
// Last Modified:   08/02/2015 at 5:50 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;

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
        ///     cref="ArgumentNullException">
        /// </exception>
        /// <param
        ///     name="collectionProvider">
        ///     The collection provider.
        /// </param>
        public AlfredCoreModule([NotNull] ICollectionProvider collectionProvider) : base(collectionProvider)
        {
            _statusWidget = new TextWidget(NoAlfredProviderMessage);
        }

        /// <summary>
        ///     Initializes a new instance of the
        ///     <see
        ///         cref="AlfredCoreModule" />
        ///     class.
        /// </summary>
        /// <param
        ///     name="collectionProvider">
        ///     The collection provider.
        /// </param>
        /// <param
        ///     name="alfredProvider">
        ///     The alfred provider.
        /// </param>
        public AlfredCoreModule(
            [NotNull] ICollectionProvider collectionProvider,
            [CanBeNull] AlfredProvider alfredProvider) : this(collectionProvider)
        {
            AlfredProvider = alfredProvider;
        }

        /// <summary>
        ///     Gets the name and version of the Module.
        /// </summary>
        /// <value>The name and version.</value>
        [NotNull]
        public override string NameAndVersion
        {
            get { return "Alfred Core 1.0"; }
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
            }
        }

        /// <summary>
        ///     Handles module initialization events
        /// </summary>
        protected override void InitializeProtected()
        {
            RegisterWidget(_statusWidget);
        }

        /// <summary>
        ///     Handles module shutdown events
        /// </summary>
        protected override void ShutdownProtected()
        {
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