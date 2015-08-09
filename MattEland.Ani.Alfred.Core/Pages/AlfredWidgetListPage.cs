// ---------------------------------------------------------
// AlfredWidgetPage.cs
// 
// Created on:      08/07/2015 at 4:12 PM
// Last Modified:   08/07/2015 at 4:28 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Interfaces;
using MattEland.Ani.Alfred.Core.Modules;
using MattEland.Ani.Alfred.Core.Widgets;

namespace MattEland.Ani.Alfred.Core.Pages
{
    /// <summary>
    ///     Represents a page grouping multiple widgets together without any module organization.
    /// </summary>
    public class AlfredWidgetListPage : AlfredPage
    {
        [NotNull]
        [ItemNotNull]
        private readonly ICollection<AlfredWidget> _widgets;

        /// <summary>
        /// Initializes a new instance of the <see cref="AlfredWidgetListPage" /> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="name">The name.</param>
        /// <exception cref="System.ArgumentNullException">provider</exception>
        public AlfredWidgetListPage([NotNull] IPlatformProvider provider, [NotNull] string name) : base(name)
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
        [UsedImplicitly]
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
        public void Register([NotNull] AlfredWidget widget)
        {
            _widgets.AddSafe(widget);
        }

        /// <summary>
        ///     Clears the widgets from the collection.
        /// </summary>
        public void Clear()
        {
            _widgets.Clear();
        }


        /// <summary>
        /// Gets the children of this component. Depending on the type of component this is, the children will
        /// vary in their own types.
        /// </summary>
        /// <value>The children.</value>
        public override IEnumerable<IAlfredComponent> Children
        {
            get
            {
                yield break;
            }
        }

    }
}