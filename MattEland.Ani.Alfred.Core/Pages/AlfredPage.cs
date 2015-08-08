// ---------------------------------------------------------
// AlfredPage.cs
// 
// Created on:      08/07/2015 at 4:27 PM
// Last Modified:   08/07/2015 at 4:30 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
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
        /// Gets a value indicating whether this is a root level page that should show on the navigator.
        /// </summary>
        /// <value><c>true</c> if this page is root level; otherwise, <c>false</c>.</value>
        public bool IsRootLevel
        {
            get { return true; }
        }

    }

}