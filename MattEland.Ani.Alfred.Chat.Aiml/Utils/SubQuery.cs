// ---------------------------------------------------------
// SubQuery.cs
// 
// Created on:      08/12/2015 at 10:35 PM
// Last Modified:   08/15/2015 at 11:17 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;

using JetBrains.Annotations;

namespace MattEland.Ani.Alfred.Chat.Aiml.Utils
{
    /// <summary>
    ///     Represents a SubQuery of a chat message.
    /// </summary>
    public class SubQuery
    {

        /// <summary>
        /// Gets or sets the template.
        /// </summary>
        /// <value>The template.</value>
        [NotNull]
        public string Template { get; set; } = string.Empty;

        /// <summary>
        /// Gets the that star collection.
        /// </summary>
        /// <value>The that star collection.</value>
        [NotNull, ItemNotNull]
        public IList<string> ThatStar { get; } = new List<string>();

        /// <summary>
        /// Gets the topic star collection.
        /// </summary>
        /// <value>The topic star collection.</value>
        [NotNull, ItemNotNull]
        public IList<string> TopicStar { get; } = new List<string>();

        /// <summary>
        /// Gets the input star collection.
        /// </summary>
        /// <value>The input star collection.</value>
        [NotNull, ItemNotNull]
        public IList<string> InputStar { get; } = new List<string>();
    }
}