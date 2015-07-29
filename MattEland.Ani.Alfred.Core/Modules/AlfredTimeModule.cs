using System;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Widgets;

namespace MattEland.Ani.Alfred.Core.Modules
{
    /// <summary>
    /// A module that keeps track of time and date information and presents it to the user.
    /// </summary>
    public class AlfredTimeModule : AlfredModule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AlfredModule" /> class.
        /// </summary>
        /// <param name="collectionProvider">The collection provider.</param>
        public AlfredTimeModule([NotNull] ICollectionProvider collectionProvider) : base(collectionProvider)
        {
            CurrentTimeWidget = new TextWidget();
        }

        protected override void InitializeProtected()
        {
            RegisterWidget(CurrentTimeWidget);
        }

        /// <summary>
        /// Gets the name and version of the module.
        /// </summary>
        /// <value>The name and version.</value>
        [NotNull]
        public override string NameAndVersion => "Time 0.1 Alpha";

        /// <summary>
        /// Gets the current time user interface widget.
        /// </summary>
        /// <value>The current time user interface widget.</value>
        [NotNull]
        public TextWidget CurrentTimeWidget { get; }

        protected override void UpdateProtected()
        {
            CurrentTimeWidget.Text = $"The time is now {DateTime.Now.ToString("t")}";
        }

        protected override void ShutdownProtected()
        {
            CurrentTimeWidget.Text = null;
        }
    }
}