// ---------------------------------------------------------
// UserInterfacePage.cs
// 
// Created on:      08/07/2015 at 4:12 PM
// Last Modified:   08/07/2015 at 4:17 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Widgets;

namespace MattEland.Ani.Alfred.Core
{
    /// <summary>
    ///     Represents a user interface page grouping multiple widgets together in a cohesive manner.
    /// </summary>
    public class UserInterfacePage
    {
        [NotNull]
        [ItemNotNull]
        private readonly ICollection<AlfredWidget> _widgets;

        /// <summary>
        ///     Initializes a new instance of the <see cref="UserInterfacePage" /> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <exception cref="System.ArgumentNullException">provider</exception>
        public UserInterfacePage([NotNull] IPlatformProvider provider)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            _widgets = provider.CreateCollection<AlfredWidget>();
        }

        /// <summary>
        ///     Gets the widgets on the page.
        /// </summary>
        /// <value>The widgets.</value>
        [NotNull]
        [ItemNotNull]
        public IEnumerable<AlfredWidget> Widgets
        {
            [DebuggerStepThrough]
            get
            { return _widgets; }
        }

        /// <summary>
        ///     Adds the widget to the page.
        /// </summary>
        /// <param name="widget">The widget.</param>
        /// <exception cref="System.ArgumentNullException">widget</exception>
        public void AddWidget([NotNull] AlfredWidget widget)
        {
            if (widget == null)
            {
                throw new ArgumentNullException(nameof(widget));
            }

            _widgets.Add(widget);
        }

        /// <summary>
        ///     Clears the widgets from the collection.
        /// </summary>
        public void Clear()
        {
            _widgets.Clear();
        }
    }
}