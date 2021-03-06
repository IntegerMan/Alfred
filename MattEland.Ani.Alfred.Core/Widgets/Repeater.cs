﻿// ---------------------------------------------------------
// Repeater.cs
// 
// Created on:      09/18/2015 at 1:44 PM
// Last Modified:   09/18/2015 at 1:56 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using MattEland.Common.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;
using System;

using MattEland.Presentation.Logical.Widgets;

namespace MattEland.Ani.Alfred.Core.Widgets
{
    /// <summary>
    ///     A repeater that lists out items to the user interface.
    /// </summary>
    public sealed class Repeater : WidgetBase
    {

        /// <summary>
        ///     The items source.
        /// </summary>
        [CanBeNull]
        private object _itemsSource;

        /// <summary>
        ///     Initializes a new instance of the Repeater class.
        /// </summary>
        /// <param name="parameters">Options for controlling the operation.</param>
        [SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
        public Repeater(WidgetCreationParameters parameters) : base(parameters)
        {
            _itemsSource = null;
            LayoutType = LayoutType.VerticalStackPanel;

            // Build out the default collection
            Items = BuildEmptyItemsCollection();
        }

        /// <summary>
        ///     Builds an empty items collection and returns it
        /// </summary>
        /// <returns>
        ///     An empty items collection
        /// </returns>
        [NotNull]
        private IEnumerable<IWidget> BuildEmptyItemsCollection()
        {
            return Container.ProvideCollection<IWidget>();
        }

        /// <summary>
        ///     Gets or sets the items.
        /// </summary>
        /// <value>
        ///     The items.
        /// </value>
        [NotNull]
        public IEnumerable<IWidget> Items { get; private set; }

        /// <summary>
        ///     Gets or sets the items source that populates the <see cref="Items"/> property.
        /// </summary>
        /// <value>
        ///     The items source.
        /// </value>
        public object ItemsSource
        {
            get { return _itemsSource; }
            set
            {
                if (_itemsSource != value)
                {
                    // Update the source
                    _itemsSource = value;

                    // A new source means we need new items
                    PopulateItemsFromItemsSource();
                }
            }
        }

        /// <summary>
        ///     Populates the <see cref="Items"/> collection from the <see cref="ItemsSource"/>.
        /// </summary>
        private void PopulateItemsFromItemsSource()
        {
            var items = _itemsSource as IEnumerable<IWidget>;

            // Validate Input
            if (items == null)
            {
                throw new InvalidOperationException("ItemsSource must be an IEnumerable<IWidget>");
            }

            Items = items ?? BuildEmptyItemsCollection();
        }

        /// <summary>
        ///     Gets the name of the broad categorization or type that this item is.
        /// </summary>
        /// <value>
        /// The item type's name.
        /// </value>
        /// <example>
        ///     Some examples of
        ///     <see cref="MattEland.Ani.Alfred.Core.Widgets.Repeater.ItemTypeName" /> values might
        ///     be "Folder", "Application", "User", etc.
        /// </example>
        public override string ItemTypeName
        {
            get { return "Repeater"; }
        }

        /// <summary>
        ///     Gets or sets layout type used by the repeater.
        /// </summary>
        /// <value>
        ///     The type of the layout.
        /// </value>
        public LayoutType LayoutType { get; set; }
    }

}