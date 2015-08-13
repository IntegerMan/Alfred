// ---------------------------------------------------------
// SubQuery.cs
// 
// Created on:      08/12/2015 at 10:35 PM
// Last Modified:   08/12/2015 at 11:59 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;

namespace MattEland.Ani.Alfred.Chat.Aiml.Utils
{
    /// <summary>
    /// Represents a SubQuery of a chat message.
    /// </summary>
    /// <remarks>
    /// TODO: Include more information on the trace path this took - especially after redirects
    /// </remarks>
    public class SubQuery
    {
        public string FullPath { get; }
        public string Template { get; set; } = string.Empty;
        public List<string> ThatStar { get; } = new List<string>();
        public List<string> TopicStar { get; } = new List<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="SubQuery"/> class.
        /// </summary>
        /// <param name="fullPath">The full path.</param>
        public SubQuery(string fullPath)
        {
            FullPath = fullPath;
        }

        public List<string> InputStar { get; set; } = new List<string>();
    }
}