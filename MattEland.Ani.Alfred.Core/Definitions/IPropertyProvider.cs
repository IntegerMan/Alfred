// ---------------------------------------------------------
// IPropertyIetm.cs
// 
// Created on:      08/23/2015 at 1:17 AM
// Last Modified:   08/23/2015 at 1:17 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;

using JetBrains.Annotations;

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
        string Name { get; }

        /// <summary>
        /// Gets a list of properties provided by this item.
        /// </summary>
        /// <returns>The properties</returns>
        [NotNull, ItemNotNull]
        IEnumerable<IPropertyItem> Properties { get; }

        /// <summary>
        /// Gets the property providers.
        /// </summary>
        /// <value>The property providers.</value>
        [NotNull, ItemNotNull]
        IEnumerable<IPropertyProvider> PropertyProviders { get; }

        /// <summary>
        /// Gets the display name for use in the user interface.
        /// </summary>
        /// <value>The display name.</value>
        [NotNull]
        string DisplayName { get; }
    }

}