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
        ///     Initializes a new instance of the <see cref="SubQuery" /> class.
        /// </summary>
        /// <param name="fullPath">The full path.</param>
        public SubQuery([NotNull] string fullPath)
        {
            FullPath = fullPath;
        }

        /// <summary>
        /// Gets the full path of the query.
        /// </summary>
        /// <value>The full path.</value>
        [NotNull]
        public string FullPath { get; }

        [NotNull]
        public string Template { get; set; } = string.Empty;

        [NotNull]
        public List<string> ThatStar { get; } = new List<string>();

        [NotNull]
        public List<string> TopicStar { get; } = new List<string>();

        [NotNull]
        public List<string> InputStar { get; } = new List<string>();
    }
}