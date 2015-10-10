// ---------------------------------------------------------
// SubQuery.cs
// 
// Created on:      08/12/2015 at 10:35 PM
// Last Modified:   08/15/2015 at 11:17 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;

using MattEland.Common.Annotations;
using System.Text;
using System;
namespace MattEland.Ani.Alfred.Chat.Aiml.Utils
{
    /// <summary>
    ///     Represents a SubQuery of a chat message.
    /// </summary>
    public sealed class SubQuery
    {
        /// <summary>
        /// Gets the input star collection.
        /// </summary>
        /// <value>The input star collection.</value>
        [NotNull, ItemNotNull]
        public IList<string> InputStar { get; } = new List<string>();

        /// <summary>
        ///     Gets a single string out of all entries in <see cref="InputStar"/>.
        /// </summary>
        /// <value>
        ///     The input star string.
        /// </value>
        public string InputStarString
        {
            get
            {
                return BuildStarString(InputStar);
            }
        }

        /// <summary>
        ///     Gets the input text for this subquery.
        /// </summary>
        /// <value>
        ///     The input text.
        /// </value>
        [NotNull]
        public string InputText { get; internal set; } = string.Empty;

        /// <summary>
        ///     Gets or sets the raw response to the query.
        /// </summary>
        /// <remarks>
        ///     This is set after the chat engine evaluates the template associated with the query and
        ///     can be retrieved later on in the user interface or via tests for diagnostic /
        ///     troubleshooting purposes.
        /// </remarks>
        /// <value>
        ///     The response.
        /// </value>
        [NotNull]
        public string Response { get; internal set; } = string.Empty;

        /// <summary>
        /// Gets or sets the template.
        /// </summary>
        /// <value>The template.</value>
        [NotNull]
        public string Template { get; internal set; } = string.Empty;

        /// <summary>
        /// Gets the that star collection.
        /// </summary>
        /// <value>The that star collection.</value>
        [NotNull, ItemNotNull]
        public IList<string> ThatStar { get; } = new List<string>();

        /// <summary>
        ///     Gets a single string out of all entries in <see cref="ThatStar"/>.
        /// </summary>
        /// <value>
        ///     The that star string.
        /// </value>
        public string ThatStarString
        {
            get
            {
                return BuildStarString(ThatStar);
            }
        }

        /// <summary>
        /// Gets the topic star collection.
        /// </summary>
        /// <value>The topic star collection.</value>
        [NotNull, ItemNotNull]
        public IList<string> TopicStar { get; } = new List<string>();

        /// <summary>
        ///     Gets a single string out of all entries in <see cref="TopicStar"/>.
        /// </summary>
        /// <value>
        ///     The topic star string.
        /// </value>
        public string TopicStarString
        {
            get
            {
                return BuildStarString(TopicStar);
            }
        }

        /// <summary>
        ///     Builds a string out of the items in <paramref name="input" />
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>A string.</returns>
        private static string BuildStarString([NotNull] IEnumerable<string> input)
        {
            var sb = new StringBuilder();

            foreach (var item in input) { sb.AppendFormat("{0} ", item); }

            return sb.ToString();
        }
    }
}