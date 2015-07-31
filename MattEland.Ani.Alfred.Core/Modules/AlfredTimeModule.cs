using System;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Widgets;

namespace MattEland.Ani.Alfred.Core.Modules
{
    /// <summary>
    ///     A module that keeps track of time and date information and presents it to the user.
    /// </summary>
    public sealed class AlfredTimeModule : AlfredModule
    {
        /// <summary>
        ///     Initializes a new instance of the
        ///     <see
        ///         cref="AlfredModule" />
        ///     class.
        /// </summary>
        /// <param
        ///     name="collectionProvider">
        ///     The collection provider.
        /// </param>
        public AlfredTimeModule([NotNull] ICollectionProvider collectionProvider) : base(collectionProvider)
        {
            CurrentTimeWidget = new TextWidget();
            BedtimeAlertWidget = new WarningWidget { IsVisible = false };
        }

        /// <summary>
        ///     Gets the name and version of the module.
        /// </summary>
        /// <value>The name and version.</value>
        [NotNull]
        public override string NameAndVersion => "Time 0.1 Alpha";

        /// <summary>
        ///     Gets the current time user interface widget.
        /// </summary>
        /// <value>The current time user interface widget.</value>
        [NotNull]
        public TextWidget CurrentTimeWidget { get; }

        /// <summary>
        ///     Gets the bedtime alert widget.
        ///     This widget alerts users to go to bed after a certain time
        ///     and will be hidden until the user needs to go to bed.
        /// </summary>
        /// <value>The bedtime alert widget.</value>
        [NotNull]
        public WarningWidget BedtimeAlertWidget { get; }

        protected override void InitializeProtected()
        {
            RegisterWidget(CurrentTimeWidget);
            RegisterWidget(BedtimeAlertWidget);
        }

        /// <summary>
        ///     Handles updating the module
        /// </summary>
        protected override void UpdateProtected()
        {
            CurrentTimeWidget.Text = $"The time is now {DateTime.Now.ToString("t")}";

            // TODO: Update the visible status of the alert widget
        }

        /// <summary>
        ///     Handles module shutdown events
        /// </summary>
        protected override void ShutdownProtected()
        {
            CurrentTimeWidget.Text = null;
        }
    }
}