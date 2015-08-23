// ---------------------------------------------------------
// MindExplorerPage.cs
// 
// Created on:      08/22/2015 at 11:16 PM
// Last Modified:   08/22/2015 at 11:16 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;

namespace MattEland.Ani.Alfred.Core.Pages
{
    /// <summary>
    /// An explorer page intended for delving through the details of items in a hierarchical / detail format.
    /// </summary>
    public class ExplorerPage : AlfredPage
    {
        [NotNull]
        private readonly IPlatformProvider _provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExplorerPage" /> class.
        /// </summary>
        /// <param name="provider">The platform provider.</param>
        /// <param name="name">The name.</param>
        /// <param name="id">The ID</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="ArgumentNullException"><paramref name="name" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="id" /> is <see langword="null" />.</exception>
        public ExplorerPage([NotNull] IPlatformProvider provider, [NotNull] string name, [NotNull] string id) : base(name, id)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            _provider = provider;
        }

        /// <summary>
        /// Gets the children of this component. Depending on the type of component this is, the children will
        /// vary in their own types.
        /// </summary>
        /// <value>The children.</value>
        public override IEnumerable<IAlfredComponent> Children
        {
            get { yield break; }
        }

        /// <summary>
        ///     Gets whether or not the component is visible to the user interface.
        /// </summary>
        /// <value>Whether or not the component is visible.</value>
        public override bool IsVisible
        {
            get
            {
                // Explorer pages are always visible
                return true;
            }
        }
    }
}