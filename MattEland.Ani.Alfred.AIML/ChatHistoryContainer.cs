// ---------------------------------------------------------
// ChatHistoryContainer.cs
// 
// Created on:      08/25/2015 at 11:07 AM
// Last Modified:   08/25/2015 at 11:12 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;

using MattEland.Ani.Alfred.Core.Definitions;

namespace MattEland.Ani.Alfred.Chat
{
    /// <summary>
    ///     An <see cref="IPropertyProvider" /> that stores and retrieves chat history entries for inputs
    ///     and outputs involving the <see cref="IChatProvider" />.
    /// </summary>
    public class ChatHistoryContainer : IPropertyProvider
    {

        /// <summary>
        ///     The display name used for each instance's display name
        /// </summary>
        public const string InstanceDisplayName = "Chat History";

        /// <summary>
        ///     Gets the display name for use in the user interface.
        /// </summary>
        /// <value>The display name.</value>
        public string DisplayName
        {
            get { return Name; }
        }

        /// <summary>
        ///     Gets the name of the broad categorization or type that this item is.
        /// </summary>
        /// <example>
        ///     Some examples of ItemTypeName values might be "Folder", "Application", "User", etc.
        /// </example>
        /// <value>The item type's name.</value>
        public string ItemTypeName
        {
            get { return "Container"; }
        }

        /// <summary>
        ///     Gets the name of the item.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return InstanceDisplayName; }
        }

        /// <summary>
        ///     Gets a list of properties provided by this item.
        /// </summary>
        /// <returns>The properties</returns>
        public IEnumerable<IPropertyItem> Properties
        {
            get { yield return new AlfredProperty("Name", DisplayName); }
        }

        /// <summary>
        ///     Gets the property providers.
        /// </summary>
        /// <value>The property providers.</value>
        public IEnumerable<IPropertyProvider> PropertyProviders
        {
            get
            {
                // TODO: Chat history should go in here

                yield break;
            }
        }
    }
}