using System;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace MattEland.Ani.Alfred.Core.Modules
{
    /// <summary>
    /// A module that keeps track of time and date information and presents it to the user.
    /// </summary>
    public class AlfredTimeModule : AlfredModule
    {
        private string _timeString;

        /// <summary>
        /// Gets the name and version of the module.
        /// </summary>
        /// <value>The name and version.</value>
        [NotNull]
        public override string NameAndVersion => "Time 0.1 Alpha";

        protected override void UpdateProtected()
        {
            _timeString = $"The time is now {DateTime.Now.ToString("t")}";
            OnPropertyChanged(nameof(UserInterfaceText));
        }

        /// <summary>
        /// Gets the user interface text.
        /// </summary>
        /// <value>The user interface text.</value>
        public override string UserInterfaceText => _timeString;

        protected override void ShutdownProtected()
        {
            _timeString = null;
            OnPropertyChanged(nameof(UserInterfaceText));
        }
    }
}