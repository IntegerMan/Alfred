// ---------------------------------------------------------
// Repeater.cs
// 
// Created on:      09/18/2015 at 1:44 PM
// Last Modified:   09/18/2015 at 1:56 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;

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
        [NotNull]
        private IEnumerable<IWidget> _items;

        /// <summary>
        ///     Initializes a new instance of the Repeater class.
        /// </summary>
        /// <param name="parameters">Options for controlling the operation.</param>
        public Repeater(WidgetCreationParameters parameters) : base(parameters)
        {
            // Build out the default collection
            _items = parameters.Container.ProvideCollection<IWidget>();
        }

        /// <summary>
        ///     Gets or sets the items source.
        /// </summary>
        /// <value>
        /// The items source.
        /// </value>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value" /> is <see langword="null" /> .
        /// </exception>
        [NotNull]
        public IEnumerable<IWidget> Items
        {
            get { return _items; }
            set
            {
                // Validate
                if (value == null) throw new ArgumentNullException(nameof(value));

                _items = value;
            }
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
    }

}