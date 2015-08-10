// ---------------------------------------------------------
// AlfredWidgetListPage.cs
// 
// Created on:      08/08/2015 at 7:17 PM
// Last Modified:   08/09/2015 at 10:03 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Widgets;

namespace MattEland.Ani.Alfred.Core.Pages
{
    /// <summary>
    ///     Represents a page grouping multiple widgets together without any module organization.
    /// </summary>
    public sealed class AlfredWidgetListPage : AlfredPage
    {
        [NotNull]
        [ItemNotNull]
        private readonly ICollection<AlfredWidget> _widgets;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AlfredWidgetListPage" /> class.
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
        ///     Gets the children of this component. Depending on the type of component this is, the children will
        ///     vary in their own types.
        /// </summary>
        /// <value>The children.</value>
        public override IEnumerable<IAlfredComponent> Children
        {
            get { yield break; }
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
    }
}