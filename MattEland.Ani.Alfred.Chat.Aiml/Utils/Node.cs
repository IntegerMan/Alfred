// ---------------------------------------------------------
// Node.cs
// 
// Created on:      08/12/2015 at 10:27 PM
// Last Modified:   08/13/2015 at 1:17 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Xml;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml.Normalize;

namespace MattEland.Ani.Alfred.Chat.Aiml.Utils
{
    /// <summary>
    /// Represents a node in the conversational graph
    /// </summary>
    [Serializable]
    public class Node
    {
        [NotNull]
        [ItemNotNull]
        private readonly Dictionary<string, Node> _children = new Dictionary<string, Node>();

        /// <summary>
        ///     Gets or sets the filename.
        /// </summary>
        /// <value>The filename.</value>
        public string Filename { get; set; } = string.Empty;

        /// <summary>
        ///     Gets or sets the template.
        /// </summary>
        /// <value>The template.</value>
        public string Template { get; set; } = string.Empty;

        /// <summary>
        ///     Gets or sets the word.
        /// </summary>
        /// <value>The word.</value>
        public string Word { get; set; } = string.Empty;

        /// <summary>
        ///     Gets the number of children this node has.
        /// </summary>
        /// <value>The children count.</value>
        public int ChildrenCount
        {
            get { return _children.Count; }
        }

        /// <summary>
        ///     Adds the category to the node as a new child node.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="template">The template.</param>
        /// <param name="filename">The filename.</param>
        public void AddCategory([CanBeNull] string path, [NotNull] string template, [CanBeNull] string filename)
        {
            //- Validate Input
            if (string.IsNullOrEmpty(template))
            {
                string message =
                    $"The category with a pattern: {path} found in file: {filename} has an empty template tag. ABORTING";
                throw new XmlException(message);
            }

            if (string.IsNullOrWhiteSpace(path))
            {
                Template = template;
                Filename = filename;
            }
            else
            {
                // Grab our key as the first word from the path
                var words = path.Trim().Split(" ".ToCharArray());
                var key = words[0].ToUpperInvariant();

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

                node.AddCategory(restOfPath, template, filename);
            }
        }

        public string Evaluate(string path,
                                       SubQuery query,
                                       [NotNull] Request request,
                                       MatchState matchstate,
                                       [NotNull] StringBuilder wildcard)
        {
            //- Validate Inputs
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            if (wildcard == null)
            {
                throw new ArgumentNullException(nameof(wildcard));
            }

            // Ensure we haven't taken too long
            if (request.CheckForTimedOut())
            {
                return string.Empty;
            }

            //- Ensure path is something we can manipulate
            path = path?.Trim() ?? string.Empty;

            // If no child nodes, this is it - this is our match
            if (_children.Count == 0)
            {
                // Update the path
                if (!string.IsNullOrEmpty(path))
                {
                    AddWordToStringBuilder(path, wildcard);
                }

                // Bring back the template
                return Template;
            }

            // If there's no path at all, just use the template
            if (string.IsNullOrEmpty(path))
            {
                return Template;
            }

            // Grab the first word from the path
            const string WordSeparators = " \r\n\t";
            var words = path.Split(WordSeparators.ToCharArray());
            Debug.Assert(words[0] != null);
            var key = words[0].ToUpperInvariant();

            // Grab the rest of the path
            var newPath = path.Substring(key.Length, path.Length - key.Length);

            if (_children.ContainsKey("_"))
            {
                string str1;

                if (EvaluateUnderscoreChild(query, request, matchstate, words, newPath, out str1))
                {
                    return str1;
                }
            }

            if (_children.ContainsKey(key))
            {
                string s1;
                if (EvaluateKeyedChildNode(query, request, matchstate, key, newPath, out s1))
                {
                    return s1;
                }
            }

            if (_children.ContainsKey("*"))
            {
                string str2;
                if (EvaluateAsterixChildNode(query, request, matchstate, words, newPath, out str2))
                    return str2;
            }

            if (Word == "_" || Word == "*")
            {
                AddWordToStringBuilder(words[0], wildcard);
                return Evaluate(newPath, query, request, matchstate, wildcard);
            }

            return string.Empty;
        }

        private bool EvaluateAsterixChildNode(SubQuery query,
                                              Request request,
                                              MatchState matchstate,
                                              string[] words,
                                              string newPath,
                                              out string str2)
        {

            var node = _children["*"];
            var wildcard1 = new StringBuilder();
            AddWordToStringBuilder(words[0], wildcard1);
            var str = node.Evaluate(newPath, query, request, matchstate, wildcard1);
            if (str.Length > 0)
            {
                if (wildcard1.Length > 0)
                {
                    switch (matchstate)
                    {
                        case MatchState.UserInput:
                            query.InputStar.Add(wildcard1.ToString());
                            wildcard1.Remove(0, wildcard1.Length);
                            break;
                        case MatchState.That:
                            query.ThatStar.Add(wildcard1.ToString());
                            break;
                        case MatchState.Topic:
                            query.TopicStar.Add(wildcard1.ToString());
                            break;
                    }
                }
                {
                    str2 = str;
                    return true;
                }
            }
            str2 = string.Empty;
            return false;
        }

        private bool EvaluateKeyedChildNode(SubQuery query,
                                            Request request,
                                            MatchState matchstate,
                                            string key,
                                            string newPath,
                                            out string s1)
        {

            var matchstate1 = matchstate;
            if (key == "<THAT>")
            {
                matchstate1 = MatchState.That;
            }
            else if (key == "<TOPIC>")
            {
                matchstate1 = MatchState.Topic;
            }
            var node = _children[key];
            var wildcard1 = new StringBuilder();
            var str = node.Evaluate(newPath, query, request, matchstate1, wildcard1);
            if (str.Length > 0)
            {
                if (wildcard1.Length > 0)
                {
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
                }
                {
                    s1 = str;
                    return true;
                }
            }
            s1 = string.Empty;
            return false;
        }

        private bool EvaluateUnderscoreChild(SubQuery query,
                                             Request request,
                                             MatchState matchstate,
                                             string[] words,
                                             string newPath,
                                             out string str1)
        {

            var node = _children["_"];

            var wildcard1 = new StringBuilder();

            AddWordToStringBuilder(words[0], wildcard1);

            var str = node.Evaluate(newPath, query, request, matchstate, wildcard1);
            if (!string.IsNullOrEmpty(str))
            {
                if (wildcard1.Length > 0)
                {
                    switch (matchstate)
                    {
                        case MatchState.UserInput:
                            query.InputStar.Add(wildcard1.ToString());
                            wildcard1.Remove(0, wildcard1.Length);
                            break;
                        case MatchState.That:
                            query.ThatStar.Add(wildcard1.ToString());
                            break;
                        case MatchState.Topic:
                            query.TopicStar.Add(wildcard1.ToString());
                            break;
                    }
                }
                {
                    str1 = str;
                    return true;
                }
            }
            str1 = string.Empty;
            return false;
        }

        /// <summary>
        /// Adds a word to the string builder ensuring that there is space before the word if the
        /// StringBuilder has already had items added to it.
        /// </summary>
        /// <param name="word">The word.</param>
        /// <param name="stringBuilder">The string builder.</param>
        private static void AddWordToStringBuilder(string word, [NotNull] StringBuilder stringBuilder)
        {
            // If it's not the first string in there, add a space.
            if (stringBuilder.Length > 0)
            {
                stringBuilder.Append(" ");
            }

            // Add our word
            stringBuilder.Append(word);
        }
    }
}