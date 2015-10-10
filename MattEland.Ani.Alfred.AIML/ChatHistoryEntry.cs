// ---------------------------------------------------------
// ChatHistoryEntry.cs
// 
// Created on:      08/25/2015 at 3:16 PM
// Last Modified:   09/01/2015 at 1:18 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;

using MattEland.Common.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml;
using MattEland.Ani.Alfred.Chat.Aiml.Utils;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Common;
using MattEland.Common.Providers;

namespace MattEland.Ani.Alfred.Chat
{
    /// <summary>Represents a chat history entry</summary>
    public class ChatHistoryEntry : IPropertyProvider
    {
        /// <summary>Initializes a new instance of the <see cref="ChatHistoryEntry" /> class.</summary>
        /// <param name="container">The container.</param>
        /// <param name="user">The <see cref="MattEland.Ani.Alfred.Chat.ChatHistoryEntry.User" /> .</param>
        /// <param name="message">The message.</param>
        /// <param name="chatResult">The result.</param>
        /// <exception cref="ArgumentNullException">Thrown when one or more required arguments are null.</exception>
        internal ChatHistoryEntry(
            [NotNull] IAlfredContainer container,
            [NotNull] User user,
            [NotNull] string message,
            [CanBeNull] ChatResult chatResult = null)
        {
            //- Validate
            if (user == null) { throw new ArgumentNullException(nameof(user)); }
            if (container == null) { throw new ArgumentNullException(nameof(container)); }
            if (message == null || message.IsEmpty())
            {
                throw new ArgumentNullException(nameof(message));
            }

            //- Set Properties
            User = user;
            Message = message;
            ChatResult = chatResult;
            DateTimeUtc = DateTime.UtcNow; //- We'll likely need this as a parameter someday.

            // Build out a collection of sub-queries
            SubQueries = container.ProvideCollection<ChatSubQueryExplorerNode>();

            if (chatResult?.SubQueries != null)
            {
                foreach (var subQuery in chatResult.SubQueries)
                {
                    SubQueries.Add(new ChatSubQueryExplorerNode(subQuery));
                }
            }
        }

        /// <summary>Gets the processing <see cref="SubQuery" /> objects related to this entry.</summary>
        /// <value>The sub queries.</value>
        [NotNull]
        [ItemNotNull]
        public ICollection<ChatSubQueryExplorerNode> SubQueries { get; }

        /// <summary>Gets the chat result when this entry represents a message from the
        ///     <see cref="ChatEngine" /> .</summary>
        /// <value>The chat result.</value>
        [CanBeNull]
        public ChatResult ChatResult { get; }

        /// <summary>Gets the date time the statement was made in UTC time.</summary>
        /// <value>The date time in UTC time.</value>
        public DateTime DateTimeUtc { get; }

        /// <summary>Gets the date time the statement was made in local time.</summary>
        /// <value>The date time in local time.</value>
        public DateTime DateTime
        {
            get { return DateTimeUtc.ToLocalTime(); }
        }

        /// <summary>
        ///     Gets the <see cref="MattEland.Ani.Alfred.Chat.ChatHistoryEntry.User" /> who made the
        ///     statement.
        /// </summary>
        /// <value>The user.</value>
        [NotNull]
        public User User { get; }

        /// <summary>Gets the message.</summary>
        /// <value>The message.</value>
        [NotNull]
        public string Message { get; }

        /// <summary>Gets a value indicating whether this instance is from the chat engine.</summary>
        /// <value>
        ///     <see langword="true" /> if this instance is from chat engine, <see langword="false" />
        ///     if it was user input.
        /// </value>
        public bool IsFromChatEngine
        {
            get { return ChatResult != null; }
        }

        /// <summary>Gets the display name for use in the user interface.</summary>
        /// <value>The display name.</value>
        public string DisplayName
        {
            get { return Name; }
        }

        /// <summary>Gets the name of the broad categorization or type that this item is.</summary>
        /// <value>The item type's name.</value>
        /// <example>
        ///     Some examples of <see cref="ItemTypeName" /> values might be "Folder", "Application",
        ///     "User", etc.
        /// </example>
        public string ItemTypeName
        {
            get { return "Chat Message"; }
        }

        /// <summary>Gets the name of the item.</summary>
        /// <value>The name.</value>
        [NotNull]
        public string Name
        {
            get
            {
                return string.Format(CultureInfo.InvariantCulture, "{0}: {1}", User.Name, Message);
            }
        }

        /// <summary>Gets a list of properties provided by this item.</summary>
        /// <returns>The properties</returns>
        public virtual IEnumerable<IPropertyItem> Properties
        {
            get
            {
                yield return new AlfredProperty("Source", User.Name);
                yield return new AlfredProperty("Message", Message);
                yield return new AlfredProperty("Time", DateTime);
            }
        }

        /// <summary>Gets the property providers.</summary>
        /// <value>The property providers.</value>
        public IEnumerable<IPropertyProvider> PropertyProviders
        {
            get { return SubQueries; }
        }
    }

}