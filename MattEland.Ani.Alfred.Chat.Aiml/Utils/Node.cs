// ---------------------------------------------------------
// Node.cs
// 
// Created on:      08/12/2015 at 10:27 PM
// Last Modified:   08/12/2015 at 11:59 PM
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
    [Serializable]
    public class Node
    {
        [NotNull]
        [ItemNotNull]
        private readonly Dictionary<string, Node> _children = new Dictionary<string, Node>();

        public string Filename { get; set; } = string.Empty;
        public string Template { get; set; } = string.Empty;
        public string Word { get; set; } = string.Empty;

        /// <summary>
        /// Gets the number of children this node has.
        /// </summary>
        /// <value>The children count.</value>
        public int ChildrenCount
        {
            get { return _children.Count; }
        }

        /// <summary>
        /// Adds the category to the node as a new child node.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="template">The template.</param>
        /// <param name="filename">The filename.</param>
        public void AddCategory([CanBeNull] string path, [NotNull] string template, [CanBeNull] string filename)
        {
            //- Validate Input
            if (string.IsNullOrEmpty(template))
            {
                string message = $"The category with a pattern: {path} found in file: {filename} has an empty template tag. ABORTING";
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

        public string evaluate(string path,
                               SubQuery query,
                               Request request,
                               MatchState matchstate,
                               StringBuilder wildcard)
        {
            if (request.StartedOn.AddMilliseconds(request.chatEngine.TimeOut) < DateTime.Now)
            {
                request.chatEngine.writeToLog("WARNING! Request timeout. User: " + request.user.UserID + " raw input: \"" +
                                       request.rawInput + "\"");
                request.hasTimedOut = true;
                return string.Empty;
            }
            path = path.Trim();
            if (_children.Count == 0)
            {
                if (path.Length > 0)
                {
                    storeWildCard(path, wildcard);
                }
                return Template;
            }
            if (path.Length == 0)
            {
                return Template;
            }
            var strArray = path.Split(" \r\n\t".ToCharArray());
            var key = UppercaseTextTransformer.TransformInput(strArray[0]);
            var path1 = path.Substring(key.Length, path.Length - key.Length);
            if (_children.ContainsKey("_"))
            {
                var node = _children["_"];
                var wildcard1 = new StringBuilder();
                storeWildCard(strArray[0], wildcard1);
                var str = node.evaluate(path1, query, request, matchstate, wildcard1);
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
                    return str;
                }
            }
            if (_children.ContainsKey(key))
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
                var str = node.evaluate(path1, query, request, matchstate1, wildcard1);
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
                    return str;
                }
            }
            if (_children.ContainsKey("*"))
            {
                var node = _children["*"];
                var wildcard1 = new StringBuilder();
                storeWildCard(strArray[0], wildcard1);
                var str = node.evaluate(path1, query, request, matchstate, wildcard1);
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
                    return str;
                }
            }
            if (Word == "_" || Word == "*")
            {
                storeWildCard(strArray[0], wildcard);
                return evaluate(path1, query, request, matchstate, wildcard);
            }
            wildcard = new StringBuilder();
            return string.Empty;
        }

        private void storeWildCard(string word, StringBuilder wildcard)
        {
            if (wildcard.Length > 0)
            {
                wildcard.Append(" ");
            }
            wildcard.Append(word);
        }
    }
}