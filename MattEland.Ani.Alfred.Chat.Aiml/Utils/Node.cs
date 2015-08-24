// ---------------------------------------------------------
// Node.cs
// 
// Created on:      08/22/2015 at 11:36 PM
// Last Modified:   08/24/2015 at 1:46 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;

using JetBrains.Annotations;

using MattEland.Common;

namespace MattEland.Ani.Alfred.Chat.Aiml.Utils
{
    /// <summary>
    ///     Represents a node in the conversational graph
    /// </summary>
    /// <remarks>
    /// TODO: This class is in need of cleanup with long, nebulous methods
    /// </remarks>
    [Serializable]
    internal sealed class Node
    {
        [NotNull]
        [ItemNotNull]
        private readonly Dictionary<string, Node> _children = new Dictionary<string, Node>();

        /// <summary>
        ///     Gets or sets the template.
        /// </summary>
        /// <value>The template.</value>
        private string Template { get; set; } = string.Empty;

        /// <summary>
        ///     Gets or sets the word.
        /// </summary>
        /// <value>The word.</value>
        private string Word { get; set; } = string.Empty;

        /// <summary>
        ///     Gets the number of children this node has.
        /// </summary>
        /// <value>The children count.</value>
        internal int ChildrenCount
        {
            get { return _children.Count; }
        }

        /// <summary>
        ///     Adds the category to the node as a new child node.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="template">The template.</param>
        /// <exception cref="ArgumentNullException">A category has an  empty template tag.</exception>
        internal void AddCategory([CanBeNull] string path, [NotNull] string template)
        {
            //- Validate Input
            if (string.IsNullOrEmpty(template))
            {
                var message = string.Format(CultureInfo.CurrentCulture,
                                            Resources.AddCategoryErrorEmptyTemplateTag.NonNull(),
                                            path);
                throw new ArgumentNullException(template, message);
            }

            if (string.IsNullOrWhiteSpace(path))
            {
                Template = template;
                return;
            }

            // Grab our key as the first word from the path
            var words = path.Trim().Split(" ".ToCharArray());
            var key = words[0];
            Debug.Assert(key != null);
            key = key.ToUpperInvariant();

            // Chop off the rest of the path string
            var restOfPath = path.Substring(key.Length, path.Length - key.Length).Trim();

            Node node;
            if (_children.ContainsKey(key))
            {
                node = _children[key];
                Debug.Assert(node != null);
            }
            else
            {
                node = new Node { Word = key };

                _children.Add(key, node);
            }

            node.AddCategory(restOfPath, template);
        }

        /// <summary>
        ///     Evaluates the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="query">The query.</param>
        /// <param name="request">The request.</param>
        /// <param name="matchstate">The match state.</param>
        /// <param name="wildcard">The wildcard.</param>
        /// <returns>The path expression</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="request" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="wildcard" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="query" /> is <see langword="null" />.
        /// </exception>
        internal string Evaluate(string path,
                               [NotNull] SubQuery query,
                               [NotNull] Request request,
                               MatchState matchstate,
                               [NotNull] StringBuilder wildcard)
        {
            //- Validate Inputs
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            if (wildcard == null) { throw new ArgumentNullException(nameof(wildcard)); }
            if (query == null) { throw new ArgumentNullException(nameof(query)); }

            // Ensure we haven't taken too long
            if (request.CheckForTimedOut()) { return string.Empty; }

            //- Ensure path is something we can manipulate
            path = path?.Trim() ?? string.Empty;

            // If no child nodes, this is it - this is our match
            if (_children.Count == 0)
            {
                // Update the path
                if (path.HasText()) { AddWordToStringBuilder(path, wildcard); }

                // Bring back the template
                return Template;
            }

            // If there's no path at all, just use the template
            if (string.IsNullOrEmpty(path)) { return Template; }

            // Grab the first word from the path
            const string WordSeparators = " \r\n\t";
            var words = path.Split(WordSeparators.ToCharArray());
            Debug.Assert(words[0] != null);
            var key = words[0].ToUpperInvariant();

            // Grab the rest of the path
            var newPath = path.Substring(key.Length, path.Length - key.Length);

            if (_children.ContainsKey("_"))
            {
                var underscorePath = EvaluateUnderscoreChild(query,
                                                             request,
                                                             matchstate,
                                                             words,
                                                             newPath);

                if (underscorePath.HasText()) { return underscorePath; }
            }

            if (_children.ContainsKey(key))
            {
                var keyedPath = EvaluateKeyedChildNode(query, request, matchstate, key, newPath);
                if (keyedPath.HasText()) { return keyedPath; }
            }

            if (_children.ContainsKey("*"))
            {
                var asterixPath = EvaluateAsterixChildNode(query,
                                                           request,
                                                           matchstate,
                                                           words,
                                                           newPath);
                if (asterixPath.HasText()) { return asterixPath; }
            }

            if (Word == "_" || Word == "*")
            {
                AddWordToStringBuilder(words[0], wildcard);

                return Evaluate(newPath, query, request, matchstate, wildcard);
            }

            return string.Empty;
        }

        /// <summary>
        ///     Evaluates an asterix child node.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="request">The request.</param>
        /// <param name="matchstate">The match state.</param>
        /// <param name="words">The words.</param>
        /// <param name="newPath">The new path.</param>
        /// <returns>The result</returns>
        private string EvaluateAsterixChildNode([NotNull] SubQuery query,
                                                [NotNull] Request request,
                                                MatchState matchstate,
                                                [NotNull] IReadOnlyList<string> words,
                                                string newPath)
        {
            //- Validate
            if (query == null) { throw new ArgumentNullException(nameof(query)); }
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            if (words == null) { throw new ArgumentNullException(nameof(words)); }

            var sbWildcard = new StringBuilder();
            AddWordToStringBuilder(words[0], sbWildcard);

            var node = _children["*"];
            Debug.Assert(node != null);

            var path = node.Evaluate(newPath, query, request, matchstate, sbWildcard);
            if (!path.HasText()) { return string.Empty; }

            if (sbWildcard.Length <= 0) { return path; }

            switch (matchstate)
            {
                case MatchState.UserInput:
                    query.InputStar.Add(sbWildcard.ToString());
                    sbWildcard.Remove(0, sbWildcard.Length);
                    break;

                case MatchState.That:
                    query.ThatStar.Add(sbWildcard.ToString());
                    break;

                case MatchState.Topic:
                    query.TopicStar.Add(sbWildcard.ToString());
                    break;
            }

            return path;
        }

        /// <summary>
        ///     Evaluates a keyed child node and returns its path result.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="request">The request.</param>
        /// <param name="matchstate">The match state.</param>
        /// <param name="key">The key.</param>
        /// <param name="newPath">The new path.</param>
        /// <returns>The path result</returns>
        private string EvaluateKeyedChildNode([NotNull] SubQuery query,
                                              [NotNull] Request request,
                                              MatchState matchstate,
                                              [NotNull] string key,
                                              string newPath)
        {
            //- Validate
            if (query == null) { throw new ArgumentNullException(nameof(query)); }
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            if (key == null) { throw new ArgumentNullException(nameof(key)); }

            var matchstate1 = matchstate;
            switch (key)
            {
                case "<THAT>":
                    matchstate1 = MatchState.That;
                    break;

                case "<TOPIC>":
                    matchstate1 = MatchState.Topic;
                    break;
            }

            var wildcard1 = new StringBuilder();

            var node = _children[key];
            Debug.Assert(node != null);
            var path = node.Evaluate(newPath, query, request, matchstate1, wildcard1);

            if (!path.HasText()) { return string.Empty; }

            if (wildcard1.Length <= 0) { return path; }

            switch (matchstate)
            {
                case MatchState.UserInput:
                    query.InputStar.Add(wildcard1.ToString());
                    wildcard1.Remove(0, wildcard1.Length);
                    break;

                case MatchState.That:
                    query.ThatStar.Add(wildcard1.ToString());
                    wildcard1.Remove(0, wildcard1.Length);
                    break;

                case MatchState.Topic:
                    query.TopicStar.Add(wildcard1.ToString());
                    wildcard1.Remove(0, wildcard1.Length);
                    break;
            }

            return path;
        }

        /// <summary>
        ///     Evaluates an underscore child.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="request">The request.</param>
        /// <param name="matchstate">The match state.</param>
        /// <param name="words">The words.</param>
        /// <param name="newPath">The new path.</param>
        /// <returns>The path result</returns>
        private string EvaluateUnderscoreChild([NotNull] SubQuery query,
                                               [NotNull] Request request,
                                               MatchState matchstate,
                                               [NotNull] IReadOnlyList<string> words,
                                               string newPath)
        {
            //- Validate
            if (query == null) { throw new ArgumentNullException(nameof(query)); }
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            if (words == null) { throw new ArgumentNullException(nameof(words)); }

            var sbWildcard = new StringBuilder();

            AddWordToStringBuilder(words[0], sbWildcard);

            var node = _children["_"];
            Debug.Assert(node != null);
            var path = node.Evaluate(newPath, query, request, matchstate, sbWildcard);

            if (!path.HasText()) { return string.Empty; }
            if (sbWildcard.Length <= 0) { return path; }

            switch (matchstate)
            {
                case MatchState.UserInput:
                    query.InputStar.Add(sbWildcard.ToString());
                    sbWildcard.Remove(0, sbWildcard.Length);
                    break;

                case MatchState.That:
                    query.ThatStar.Add(sbWildcard.ToString());
                    break;

                case MatchState.Topic:
                    query.TopicStar.Add(sbWildcard.ToString());
                    break;
            }

            return path;
        }

        /// <summary>
        ///     Adds a word to the string builder ensuring that there is space before the word if the
        ///     StringBuilder has already had items added to it.
        /// </summary>
        /// <param name="word">The word.</param>
        /// <param name="stringBuilder">The string builder.</param>
        private static void AddWordToStringBuilder(string word,
                                                   [NotNull] StringBuilder stringBuilder)
        {
            // If it's not the first string in there, add a space.
            if (stringBuilder.Length > 0) { stringBuilder.Append(" "); }

            // Add our word
            stringBuilder.Append(word);
        }
    }
}