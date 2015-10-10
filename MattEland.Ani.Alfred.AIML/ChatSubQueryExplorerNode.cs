// ---------------------------------------------------------
// ChatSubQueryExplorerNode.cs
// 
// Created on:      09/02/2015 at 6:20 PM
// Last Modified:   09/03/2015 at 12:38 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

using MattEland.Common.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;
using MattEland.Ani.Alfred.Core.Definitions;
using System.Diagnostics.Contracts;
namespace MattEland.Ani.Alfred.Chat
{
    /// <summary>
    ///     A <see cref="MattEland.Ani.Alfred.Chat.ChatSubQueryExplorerNode.SubQuery" /> explorer
    ///     node for providing insight into chat input processing.
    /// </summary>
    public class ChatSubQueryExplorerNode : IPropertyProvider
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ChatSubQueryExplorerNode" /> class.
        /// </summary>
        /// <param name="subQuery"> The sub query. </param>
        internal ChatSubQueryExplorerNode([NotNull] SubQuery subQuery)
        {
            Contract.Requires(subQuery != null, "subQuery is null.");

            SubQuery = subQuery;
        }

        /// <summary>
        ///     Gets the sub query.
        /// </summary>
        /// <value>
        /// The sub query.
        /// </value>
        [NotNull]
        public SubQuery SubQuery { get; }

        /// <summary>
        ///     Gets the display name for use in the user interface.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        [NotNull]
        public string DisplayName
        {
            get { return Name; }
        }

        /// <summary>
        ///     Gets the name of the broad categorization or type that this item is.
        /// </summary>
        /// <value>
        /// The item type's name.
        /// </value>
        /// <example>
        ///     <para>
        ///         Some examples of
        ///         <see cref="MattEland.Ani.Alfred.Chat.ChatSubQueryExplorerNode.ItemTypeName" />
        ///     </para>
        ///     <para>values might be "Folder", "Application", "User", etc.</para>
        /// </example>
        [NotNull]
        public string ItemTypeName
        {
            get { return "Chat SubQuery"; }
        }

        /// <summary>
        ///     Gets the name of the item.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [NotNull]
        public string Name
        {
            get { return InputText; }
        }

        /// <summary>
        ///     Gets the input text.
        /// </summary>
        /// <value>
        /// The input text.
        /// </value>
        [NotNull]
        public string InputText
        {
            get { return SubQuery.InputText; }
        }

        /// <summary>
        ///     Gets a list of properties provided by this item.
        /// </summary>
        /// <returns>The properties</returns>
        public IEnumerable<IPropertyItem> Properties
        {
            get
            {
                yield return new AlfredProperty("Response", Response);
                yield return new AlfredProperty("Template", Template);
                yield return new AlfredProperty("Input Text", InputText);
                yield return new AlfredProperty("Input *", Input);
                yield return new AlfredProperty("Topic *", Topic);
                yield return new AlfredProperty("Subject *", Subject);
            }
        }

        /// <summary>
        ///     Gets the property providers.
        /// </summary>
        /// <value>
        /// The property providers.
        /// </value>
        [NotNull]
        [ItemNotNull]
        public IEnumerable<IPropertyProvider> PropertyProviders
        {
            get { yield break; }
        }

        /// <summary>
        ///     Gets the input.
        /// </summary>
        /// <value>
        /// The input.
        /// </value>
        [NotNull]
        public string Input
        {
            get { return SubQuery.InputStarString; }
        }

        /// <summary>
        ///     Gets the input.
        /// </summary>
        /// <value>
        /// The input.
        /// </value>
        [NotNull]
        public string Topic
        {
            get { return SubQuery.TopicStarString; }
        }

        /// <summary>
        ///     Gets the subject of conversation or, in AIML terms, the "That".
        /// </summary>
        /// <value>
        /// The subject.
        /// </value>
        [NotNull]
        public string Subject
        {
            get { return SubQuery.ThatStarString; }
        }

        /// <summary>
        ///     Gets the response.
        /// </summary>
        /// <value>
        /// The response.
        /// </value>
        [NotNull]
        public string Response
        {
            get { return SubQuery.Response; }
        }

        /// <summary>
        ///     Gets the template.
        /// </summary>
        /// <value>
        /// The template.
        /// </value>
        [NotNull]
        public string Template
        {
            get { return SubQuery.Template; }
        }
    }
}