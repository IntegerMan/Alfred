// ---------------------------------------------------------
// Node.cs
// 
// Created on:      08/22/2015 at 11:36 PM
// Last Modified:   08/25/2015 at 6:31 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

using MattEland.Common.Annotations;

using MattEland.Common;

namespace MattEland.Ani.Alfred.Chat.Aiml.Utils
{
    /// <summary>
    ///     Represents a node in the conversational graph
    /// </summary>
    /// <remarks>
    ///     TODO: It might be nice to have a Parent property
    /// </remarks>
    [Serializable]
    public sealed class Node
    {
        [CanBeNull]
        [NonSerialized]
        private NodeEvaluator _evaluator;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Node" /> class.
        /// </summary>
        internal Node() : this(null)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Node" /> class.
        /// </summary>
        /// <param name="word">The word this node is associated with.</param>
        internal Node([CanBeNull] string word)
        {
            Word = word.NonNull();
            Template = string.Empty;
            Children = new Dictionary<string, Node>();
        }

        /// <summary>
        ///     Gets the keys to child node mapping for this node.
        /// </summary>
        /// <value>The children.</value>
        [NotNull]
        [ItemNotNull]
        public IDictionary<string, Node> Children { get; }

        /// <summary>
        ///     Gets or sets the template.
        /// </summary>
        /// <value>The template.</value>
        [NotNull]
        public string Template { get; private set; }

        /// <summary>
        ///     Gets or sets the word this node is associated with.
        /// </summary>
        /// <value>The word.</value>
        [NotNull]
        public string Word { get; }

        /// <summary>
        ///     Gets the node evaluator associated with this node.
        /// </summary>
        /// <value>The evaluator.</value>
        [NotNull]
        internal NodeEvaluator Evaluator
        {
            get
            {
                // Lazily build evaluators as they're needed
                return _evaluator ?? (_evaluator = new NodeEvaluator(this));
            }
        }

        /// <summary>
        ///     Adds the template to the node following the node's path to reach the destination child node.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="template">The template.</param>
        /// <exception cref="ArgumentNullException">A category has an  empty template tag.</exception>
        internal void AddTemplate([CanBeNull] string path, [NotNull] string template)
        {
            //- Validate Input
            if (string.IsNullOrEmpty(template))
            {
                var message = string.Format(CultureInfo.CurrentCulture,
                                            Resources.AddCategoryErrorEmptyTemplateTag.NonNull(),
                                            path);

                throw new ArgumentNullException(template, message);
            }

            // Handle the case where we've reached a leaf (for now) node. Stick the template here.
            if (path.IsEmpty())
            {
                Template = template;
                return;
            }

            // This template is meant for another; pass it along to the appropriate child (building as needed)
            AddTemplateToChild(path.NonNull(), template);
        }

        /// <summary>
        /// Adds the template to the appropriate child node along the path, building nodes as needed.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="template">The template.</param>
        private void AddTemplateToChild([NotNull] string path, [NotNull] string template)
        {
            // Grab our key as the first word from the path
            var words = path.Trim().Split(" ".ToCharArray());
            var key = words[0];
            Debug.Assert(key != null);
            key = key.ToUpperInvariant();

            // Chop off the rest of the path string
            var restOfPath = path.Substring(key.Length, path.Length - key.Length).Trim();

            // Grab the node we'll be delegating the rest of this off to
            var node = GetOrCreateNode(key);

            // Call this method on the child node and give it the remainder of the path
            node.AddTemplate(restOfPath, template);
        }

        /// <summary>
        ///     Gets the node if it already exists, otherwise the node is created and stored under this key for
        ///     later retrieval.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The Node.</returns>
        [NotNull]
        private Node GetOrCreateNode([NotNull] string key)
        {
            Node node;

            if (Children.ContainsKey(key))
            {
                node = Children[key];
                Debug.Assert(node != null);
            }
            else
            {
                node = new Node(key);

                Children.Add(key, node);
            }

            return node;
        }
    }
}