// ---------------------------------------------------------
// ChatHistoryProvider.cs
// 
// Created on:      08/25/2015 at 11:07 AM
// Last Modified:   08/25/2015 at 9:57 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Common.Providers;

namespace MattEland.Ani.Alfred.Chat
{
    /// <summary>
    ///     An <see cref="IPropertyProvider" /> that stores and retrieves chat history entries for inputs
    ///     and outputs involving the <see cref="IChatProvider" />.
    /// </summary>
    internal sealed class ChatHistoryProvider : IPropertyProvider
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ChatHistoryProvider" /> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="container"/> is <see langword="null" />.
        /// </exception>
        /// <param name="container"> The container. </param>
        internal ChatHistoryProvider([NotNull] IAlfredContainer container)
        {
            //- Validate
            if (container == null) { throw new ArgumentNullException(nameof(container)); }

            // Create the collection. We need it to be in an observable collection for XAML systems
            HistoryEntries = container.ProvideCollection<ChatHistoryEntry>();
        }

        /// <summary>
        ///     Gets the history entries.
        /// </summary>
        /// <value>The history entries.</value>
        [NotNull]
        [ItemNotNull]
        private ICollection<ChatHistoryEntry> HistoryEntries { get; }

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
            get { return "Chat History"; }
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
            get { return HistoryEntries; }
        }

        /// <summary>
        ///     Adds a statement with attribution to a specific <see cref="User" />.
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <exception cref="System.ArgumentNullException">entry</exception>
        internal void Add([NotNull] ChatHistoryEntry entry)
        {
            if (entry == null) { throw new ArgumentNullException(nameof(entry)); }

            HistoryEntries.Add(entry);
        }

        /// <summary>
        ///     Gets the last message from the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>The last <see cref="ChatHistoryEntry"/> for that user.</returns>
        [CanBeNull]
        internal ChatHistoryEntry GetLastMessageFromUser([NotNull] User user)
        {
            if (user == null) { throw new ArgumentNullException(nameof(user)); }

            var userEntries = HistoryEntries.Where(e => e.User == user);
            return userEntries.OrderBy(e => e.DateTimeUtc).FirstOrDefault();
        }
    }

}