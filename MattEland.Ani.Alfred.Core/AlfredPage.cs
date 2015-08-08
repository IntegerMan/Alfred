// ---------------------------------------------------------
// AlfredPage.cs
// 
// Created on:      08/07/2015 at 4:27 PM
// Last Modified:   08/07/2015 at 4:30 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Widgets;

namespace MattEland.Ani.Alfred.Core
{
    /// <summary>
    ///     A page that can be used in Alfred
    /// </summary>
    public abstract class AlfredPage : NotifyPropertyChangedBase
    {
        [NotNull]
        private string _name;

        /// <summary>
        ///     Gets or sets the name of the page.
        /// </summary>
        /// <value>The name.</value>
        [NotNull]
        public string Name
        {
            get { return _name; }
            set
            {
                if (value != _name)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        /// <summary>
        ///     Gets the widgets on the page.
        /// </summary>
        /// <value>The widgets.</value>
        public abstract IEnumerable<AlfredWidget> Widgets { get; }
    }
}