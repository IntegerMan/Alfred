// ---------------------------------------------------------
// IPropertyIetm.cs
// 
// Created on:      08/23/2015 at 1:17 AM
// Last Modified:   08/23/2015 at 1:17 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;

using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.Core.Definitions
{
    /// <summary>
    /// An interface defining an item that can provide properties
    /// </summary>
    public interface IPropertyProvider
    {
        /// <summary>
        ///     Gets the name of the item.
        /// </summary>
        /// <value>The name.</value>
        [NotNull]
        string Name { get; }

        /// <summary>
        /// Gets a list of properties provided by this item.
        /// </summary>
        /// <returns>The properties</returns>
        [CanBeNull, ItemNotNull]
        IEnumerable<IPropertyItem> Properties { get; }

        /// <summary>
        /// Gets the property providers.
        /// </summary>
        /// <value>The property providers.</value>
        [CanBeNull, ItemNotNull]
        IEnumerable<IPropertyProvider> PropertyProviders { get; }

        /// <summary>
        /// Gets the display name for use in the user interface.
        /// </summary>
        /// <value>The display name.</value>
        [NotNull]
        string DisplayName { get; }

        /// <summary>
        /// Gets the name of the broad categorization or type that this item is.
        /// </summary>
        /// <example>
        /// Some examples of ItemTypeName values might be "Folder", "Application", "User", etc.
        /// </example>
        /// <value>The item type's name.</value>
        [NotNull]
        string ItemTypeName { get; }
    }

}