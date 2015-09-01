using System;
using System.Collections.Generic;
using System.Diagnostics;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml;
using MattEland.Ani.Alfred.Chat.Aiml.Utils;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Common;
using MattEland.Common.Providers;

namespace MattEland.Ani.Alfred.Chat
{
    /// <summary>
    /// Represents an AIML <see cref="Node"/> in the <see cref="ChatEngine"/> and presents it in an explorer-friendly manner.
    /// </summary>
    public class AimlExplorerNode : IPropertyProvider
    {

        [NotNull]
        private readonly Node _node;

        [NotNull, ItemNotNull]
        private readonly ICollection<AimlExplorerNode> _children;

        [NotNull]
        private readonly IObjectContainer _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="AimlExplorerNode" /> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="node">The node.</param>
        /// <exception cref="System.ArgumentNullException">node</exception>
        internal AimlExplorerNode([NotNull] IObjectContainer container, [NotNull] Node node)
        {
            if (container == null) { throw new ArgumentNullException(nameof(container)); }
            if (node == null) { throw new ArgumentNullException(nameof(node)); }

            _node = node;
            _container = container;
            _children = container.ProvideCollection<AimlExplorerNode>();
        }

        /// <summary>
        ///     Gets the name of the item.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get
            {
                return _node.Word.HasText() ? _node.Word : "Root Node";
            }
        }

        /// <summary>
        /// Gets a list of properties provided by this item.
        /// </summary>
        /// <returns>The properties</returns>
        public IEnumerable<IPropertyItem> Properties
        {
            get
            {
                yield return new AlfredProperty("Word", _node.Word);
                yield return new AlfredProperty("Template", _node.Template);
            }
        }

        /// <summary>
        /// Gets the property providers.
        /// </summary>
        /// <value>The property providers.</value>
        public IEnumerable<IPropertyProvider> PropertyProviders
        {
            get
            {
                RebuildChildren();
                return _children;
            }
        }

        /// <summary>
        /// Rebuilds the child collection.
        /// </summary>
        private void RebuildChildren()
        {
            _children.Clear();
            foreach (KeyValuePair<string, Node> pair in _node.Children)
            {
                Debug.Assert(pair.Value != null);
                var child = new AimlExplorerNode(_container, pair.Value);
                _children.Add(child);
            }
        }

        /// <summary>
        /// Gets the display name for use in the user interface.
        /// </summary>
        /// <value>The display name.</value>
        public string DisplayName
        {
            get { return Name; }
        }

        /// <summary>
        /// Gets the name of the broad categorization or type that this item is.
        /// </summary>
        /// <example>
        /// Some examples of ItemTypeName values might be "Folder", "Application", "User", etc.
        /// </example>
        /// <value>The item type's name.</value>
        public string ItemTypeName
        {
            get { return "Conversation Node"; }
        }
    }

}