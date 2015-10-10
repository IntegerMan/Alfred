// ---------------------------------------------------------
// NodeEvaluator.cs
// 
// Created on:      08/25/2015 at 6:01 PM
// Last Modified:   08/25/2015 at 6:14 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using MattEland.Common.Annotations;

using MattEland.Common;

namespace MattEland.Ani.Alfred.Chat.Aiml.Utils
{
    /// <summary>
    ///     A utility class for evaluating nodes in determining which conversational path to follow.
    /// </summary>
    /// <remarks>
    ///     TODO: These methods are monstrous and are screaming for a better design. This is the bad code
    ///     that murders other classes and leaves the bodies out in the sun to decay.
    /// </remarks>
    internal sealed class NodeEvaluator
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <param name="node"> The node. </param>
        internal NodeEvaluator([NotNull] Node node)
        {
            if (node == null) { throw new ArgumentNullException(nameof(node)); }

            Node = node;
        }

        /// <summary>
        ///     Gets the node.
        /// </summary>
        /// <value>The node.</value>
        [NotNull]
        private Node Node { get; }

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
        internal string Evaluate(
            string path,
            [NotNull] SubQuery query,
            [NotNull] Request request,
            MatchState matchstate,
            [NotNull] StringBuilder wildcard)
        {
            // TODO: This method has a high cyclomatic complexity and should be refactored

            //- Validate Inputs
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            if (wildcard == null) { throw new ArgumentNullException(nameof(wildcard)); }
            if (query == null) { throw new ArgumentNullException(nameof(query)); }

            // Ensure we haven't taken too long
            if (request.CheckForTimedOut()) { return string.Empty; }

            //- Ensure path is something we can manipulate
            path = path.Trim();

            // If no child nodes, this is it - this is our match
            if (Node.Children.Count == 0)
            {
                // Update the path
                if (path.HasText()) { AddWordToStringBuilder(path, wildcard); }

                // Bring back the template
                return Node.Template;
            }

            // If there's no path at all, just use the template
            if (string.IsNullOrEmpty(path)) { return Node.Template; }

            // Grab the first word from the path
            const string WordSeparators = " \r\n\t";
            var words = path.Split(WordSeparators.ToCharArray());
            Debug.Assert(words[0] != null);
            var key = words[0].ToUpperInvariant();

            // Grab the rest of the path
            var newPath = path.Substring(key.Length, path.Length - key.Length);

            if (Node.Children.ContainsKey("_"))
            {
                var underscorePath = EvaluateUnderscoreChild(query,
                                                             request,
                                                             matchstate,
                                                             words,
                                                             newPath);

                if (underscorePath.HasText()) { return underscorePath; }
            }

            if (Node.Children.ContainsKey(key))
            {
                var keyedPath = EvaluateKeyedChildNode(query, request, matchstate, key, newPath);
                if (keyedPath.HasText()) { return keyedPath; }
            }

            if (Node.Children.ContainsKey("*"))
            {
                var asterixPath = EvaluateAsterixChildNode(query,
                                                           request,
                                                           matchstate,
                                                           words,
                                                           newPath);
                if (asterixPath.HasText()) { return asterixPath; }
            }

            if (Node.Word == "_" || Node.Word == "*")
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
        private string EvaluateAsterixChildNode(
            [NotNull] SubQuery query,
            [NotNull] Request request,
            MatchState matchstate,
            [NotNull, ItemNotNull] IReadOnlyList<string> words,
            string newPath)
        {
            //- Validate
            if (query == null) { throw new ArgumentNullException(nameof(query)); }
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            if (words == null) { throw new ArgumentNullException(nameof(words)); }

            var sbWildcard = new StringBuilder();
            AddWordToStringBuilder(words.First(), sbWildcard);

            var node = Node.Children["*"];
            Debug.Assert(node != null);

            var path = node.Evaluator.Evaluate(newPath, query, request, matchstate, sbWildcard);
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
        private string EvaluateKeyedChildNode(
            [NotNull] SubQuery query,
            [NotNull] Request request,
            MatchState matchstate,
            [NotNull] string key,
            string newPath)
        {
            // TODO: This method has a high cyclomatic complexity and should be refactored

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

            var child = Node.Children[key];
            Debug.Assert(child != null);
            var path = child.Evaluator.Evaluate(newPath, query, request, matchstate1, wildcard1);

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
        private string EvaluateUnderscoreChild(
            [NotNull] SubQuery query,
            [NotNull] Request request,
            MatchState matchstate,
            [NotNull, ItemNotNull] IReadOnlyList<string> words,
            string newPath)
        {
            //- Validate
            if (query == null) { throw new ArgumentNullException(nameof(query)); }
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            if (words == null) { throw new ArgumentNullException(nameof(words)); }

            var sbWildcard = new StringBuilder();

            AddWordToStringBuilder(words.First(), sbWildcard);

            var node = Node.Children["_"];
            Debug.Assert(node != null);
            var path = node.Evaluator.Evaluate(newPath, query, request, matchstate, sbWildcard);

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
        private static void AddWordToStringBuilder(
            string word,
            [NotNull] StringBuilder stringBuilder)
        {
            // If it's not the first string in there, add a space.
            if (stringBuilder.Length > 0) { stringBuilder.Append(" "); }

            // Add our word
            stringBuilder.Append(word);
        }
    }
}