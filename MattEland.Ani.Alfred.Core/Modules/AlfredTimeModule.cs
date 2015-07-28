using System;
using System.Collections.Generic;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Widgets;

namespace MattEland.Ani.Alfred.Core.Modules
{
    /// <summary>
    /// A module that keeps track of time and date information and presents it to the user.
    /// </summary>
    public class AlfredTimeModule : AlfredModule
    {

        [NotNull]
        private readonly TextWidget _timeWidget;

        public AlfredTimeModule()
        {
            _timeWidget = new TextWidget();
        }

        protected override void InitializeProtected()
        {
            RegisterWidget(_timeWidget);
        }

        /// <summary>
        /// Gets the name and version of the module.
        /// </summary>
        /// <value>The name and version.</value>
        [NotNull]
        public override string NameAndVersion => "Time 0.1 Alpha";

        protected override void UpdateProtected()
        {
            _timeWidget.Text = $"The time is now {DateTime.Now.ToString("t")}";
            OnPropertyChanged(nameof(UserInterfaceText));
        }

        /// <summary>
        /// Gets the user interface text.
        /// </summary>
        /// <value>The user interface text.</value>
        public override string UserInterfaceText => _timeWidget.Text;

        protected override void ShutdownProtected()
        {

            _timeWidget.Text = null;

            OnPropertyChanged(nameof(UserInterfaceText));

        }
    }
}