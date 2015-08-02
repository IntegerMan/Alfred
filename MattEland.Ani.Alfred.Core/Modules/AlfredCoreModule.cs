// ---------------------------------------------------------
// AlfredCoreModule.cs
// 
// Created on:      08/02/2015 at 4:56 PM
// Last Modified:   08/02/2015 at 5:06 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Widgets;

namespace MattEland.Ani.Alfred.Core.Modules
{
    /// <summary>
    /// A module intended for the control and monitoring of Alfred
    /// </summary>
    public sealed class AlfredCoreModule : AlfredModule
    {
        public const string NoAlfredProviderMessage = "Alfred Provider has not been set";

        [NotNull]
        private readonly TextWidget _statusWidget;

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
        ///     Gets the name and version of the Module.
        /// </summary>
        /// <value>The name and version.</value>
        [NotNull]
        public override string NameAndVersion
        {
            get { return "Alfred Core 1.0"; }
        }

        /// <summary>
        /// Gets the alfred status widget.
        /// </summary>
        /// <value>The alfred status widget.</value>
        [NotNull]
        public TextWidget AlfredStatusWidget
        {
            get { return _statusWidget; }
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
            // For now, we'll go with no alfred provider message all the time
            _statusWidget.Text = NoAlfredProviderMessage;
        }
    }
}