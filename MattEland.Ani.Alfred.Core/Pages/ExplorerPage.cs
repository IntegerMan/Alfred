﻿// ---------------------------------------------------------
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

using MattEland.Common.Annotations;

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
        ///     The root nodes.
        /// </summary>
        [NotNull]
        private readonly ICollection<IPropertyProvider> _rootNodes;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ExplorerPage" /> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"> . </exception>
        /// <param name="container"> The container. </param>
        /// <param name="name"> The name. </param>
        /// <param name="id"> The ID. </param>
        /// <param name="displayName"> The display name. </param>
        public ExplorerPage([NotNull] IAlfredContainer container,
                            [NotNull] string name,
                            [NotNull] string id,
                            [CanBeNull] string displayName = "Name") : base(container, name, id)
        {
            //- Validation
            if (container == null) { throw new ArgumentNullException(nameof(container)); }

            // Set Display Name
            DisplayNameProperty = displayName ?? "Name";

            // Set the root nodes collection
            _rootNodes = container.ProvideCollection<IPropertyProvider>();
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
        public IEnumerable<IPropertyProvider> RootNodes
        {
            get
            {
                return _rootNodes;
            }
        }

        /// <summary>
        ///     Adds a root node.
        /// </summary>
        /// <param name="node"> The node. </param>
        public void AddRootNode(IPropertyProvider node)
        {
            _rootNodes.Add(node);
        }

        /// <summary>
        /// Gets the property to use to represent the item in the explorer grid.
        /// </summary>
        /// <value>The display property to use.</value>
        [NotNull]
        public string DisplayNameProperty { get; }

        /// <summary>
        ///     Clears the nodes.
        /// </summary>
        public void ClearNodes()
        {
            _rootNodes.Clear();
        }
    }
}