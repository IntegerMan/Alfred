// ---------------------------------------------------------
// ExplorerPage.cs
// 
// Created on:      08/22/2015 at 11:16 PM
// Last Modified:   08/24/2015 at 5:38 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;

namespace MattEland.Ani.Alfred.Core.Pages
{
    /// <summary>
    ///     An explorer page intended for delving through the details of items in a hierarchical / detail
    ///     format.
    /// </summary>
    public class ExplorerPage : AlfredPage
    {

        /// <summary>
        ///     Initializes a new instance of the <see cref="ExplorerPage" /> class.
        /// </summary>
        /// <param name="provider">The platform provider.</param>
        /// <param name="name">The name.</param>
        /// <param name="id">The ID</param>
        /// <param name="displayName">The display name.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentNullException"><paramref name="name" /> is <see langword="null" />.</exception>
        public ExplorerPage([NotNull] IPlatformProvider provider,
                            [NotNull] string name,
                            [NotNull] string id,
                            [CanBeNull] string displayName = "Name") : base(name, id)
        {
            //- Validation
            if (provider == null) { throw new ArgumentNullException(nameof(provider)); }

            // Set Display Name
            if (displayName == null) { displayName = "Name"; }
            DisplayNameProperty = displayName;

            // Set the root nodes collection
            RootNodes = provider.CreateCollection<IPropertyProvider>();
        }

        /// <summary>
        ///     Gets the children of this component. Depending on the type of component this is, the children
        ///     will
        ///     vary in their own types.
        /// </summary>
        /// <value>The children.</value>
        public override IEnumerable<IAlfredComponent> Children
        {
            get { return RootNodes.OfType<IAlfredComponent>(); }
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

        /// <summary>
        ///     Gets the root nodes.
        /// </summary>
        /// <value>The root nodes.</value>
        [NotNull]
        [ItemNotNull]
        public IEnumerable<IPropertyProvider> RootNodes { get; set; }

        /// <summary>
        /// Gets the property to use to represent the item in the explorer grid.
        /// </summary>
        /// <value>The display property to use.</value>
        [NotNull]
        public string DisplayNameProperty { get; }
    }
}