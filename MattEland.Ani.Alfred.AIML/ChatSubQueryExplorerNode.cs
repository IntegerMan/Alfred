// ---------------------------------------------------------
// ChatSubQueryExplorerNode.cs
// 
// Created on:      09/01/2015 at 1:32 AM
// Last Modified:   09/01/2015 at 1:33 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;
using MattEland.Ani.Alfred.Core.Definitions;

namespace MattEland.Ani.Alfred.Chat
{
    /// <summary>
    ///     A <see cref="SubQuery" /> explorer node for providing insight into chat input processing.
    /// </summary>
    public class ChatSubQueryExplorerNode : IPropertyProvider
    {

        /// <summary>Initializes a new instance of the <see cref="ChatSubQueryExplorerNode" /> class.</summary>
        /// <param name="subQuery">The sub query.</param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are
        ///     <see langword="null" /> .
        /// </exception>
        internal ChatSubQueryExplorerNode([NotNull] SubQuery subQuery)
        {
            if (subQuery == null) { throw new ArgumentNullException(nameof(subQuery)); }

            SubQuery = subQuery;
        }

        /// <summary>Gets the sub query.</summary>
        /// <value>The sub query.</value>
        [NotNull]
        public SubQuery SubQuery { get; }

        /// <summary>Gets the display name for use in the user interface.</summary>
        /// <value>The display name.</value>
        [NotNull]
        public string DisplayName
        {
            get { return Name; }
        }

        /// <summary>Gets the name of the broad categorization or type that this item is.</summary>
        /// <value>The item type's name.</value>
        /// <example>Some examples of
        ///     <see cref="MattEland.Ani.Alfred.Chat.ChatSubQueryExplorerNode.ItemTypeName" />
        ///     values might be "Folder", "Application", "User", etc.</example>
        [NotNull]
        public string ItemTypeName
        {
            get { return "Chat SubQuery"; }
        }

        /// <summary>Gets the name of the item.</summary>
        /// <value>The name.</value>
        [NotNull]
        public string Name
        {
            get { return InputText; }
        }

        /// <summary>
        ///     Gets the input text.
        /// </summary>
        /// <value>
        ///     The input text.
        /// </value>
        [NotNull]
        public string InputText
        {
            get { return SubQuery.InputText; }
        }

        /// <summary>Gets a list of properties provided by this item.</summary>
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

        /// <summary>Gets the property providers.</summary>
        /// <value>The property providers.</value>
        [NotNull, ItemNotNull]
        public IEnumerable<IPropertyProvider> PropertyProviders
        {
            get { yield break; }
        }

        /// <summary>
        ///     Gets the input.
        /// </summary>
        /// <value>
        ///     The input.
        /// </value>
        [NotNull]
        public string Input
        {
            get
            {
                return BuildStarString(SubQuery.InputStar);
            }
        }

        /// <summary>
        ///     Gets the input.
        /// </summary>
        /// <value>
        ///     The input.
        /// </value>
        [NotNull]
        public string Topic
        {
            get
            {
                return BuildStarString(SubQuery.TopicStar);
            }
        }

        /// <summary>
        ///     Gets the subject of conversation or, in AIML terms, the "That".
        /// </summary>
        /// <value>
        ///     The subject.
        /// </value>
        [NotNull]
        public string Subject
        {
            get
            {
                return BuildStarString(SubQuery.ThatStar);
            }
        }

        /// <summary>
        ///     Gets the response.
        /// </summary>
        /// <value>
        ///     The response.
        /// </value>
        [NotNull]
        public string Response
        {
            get
            {
                return SubQuery.Response;
            }
        }

        /// <summary>
        ///     Gets the template.
        /// </summary>
        /// <value>
        ///     The template.
        /// </value>
        [NotNull]
        public string Template
        {
            get
            {
                return SubQuery.Template;
            }
        }



        /// <summary>Builds a string out of the items in <paramref name="input" />
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>A string.</returns>
        private string BuildStarString([NotNull] IEnumerable<string> input)
        {
            var sb = new StringBuilder();

            foreach (var item in input) { sb.AppendFormat("{0} ", item); }

            return sb.ToString();
        }
    }
}