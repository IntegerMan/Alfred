// ---------------------------------------------------------
// ChatHandlersProvider.cs
// 
// Created on:      08/25/2015 at 11:26 AM
// Last Modified:   08/25/2015 at 11:26 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml;
using MattEland.Ani.Alfred.Chat.Aiml.Utils;
using MattEland.Ani.Alfred.Core.Definitions;

namespace MattEland.Ani.Alfred.Chat
{
    /// <summary>
    ///     An <see cref="IPropertyProvider" /> that provides explorer nodes for nodes inside of the
    ///     <see cref="ChatEngine" />'s AIML tree.
    /// </summary>
    public class ChatHandlersProvider : IPropertyProvider
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        /// <param name="provider">The platform provider.</param>
        /// <param name="engine">The chat engine.</param>
        /// <exception cref="System.ArgumentNullException">engine, provider
        /// </exception>
        internal ChatHandlersProvider(
            [NotNull] IPlatformProvider provider, [NotNull] ChatEngine engine)
        {
            if (provider == null) { throw new ArgumentNullException(nameof(provider)); }
            if (engine == null) { throw new ArgumentNullException(nameof(engine)); }

            RootNode = new AimlExplorerNode(provider, engine.RootNode);
        }

        /// <summary>
        /// Gets the root node.
        /// </summary>
        /// <value>The root node.</value>
        [NotNull]
        public AimlExplorerNode RootNode { get; }

        /// <summary>
        ///     The display name that will be used for each instance
        /// </summary>
        public const string InstanceDisplayName = "Chat Handlers";

        /// <summary>
        ///     Gets the display name for use in the user interface.
        /// </summary>
        /// <value>The display name.</value>
        public string DisplayName
        {
            get { return Name; }
        }

        /// <summary>
        ///     Gets the name of the broad categorization or type that this item is.
        /// </summary>
        /// <example>
        ///     Some examples of ItemTypeName values might be "Folder", "Application", "User", etc.
        /// </example>
        /// <value>The item type's name.</value>
        public string ItemTypeName
        {
            get { return "Container"; }
        }

        /// <summary>
        ///     Gets the name of the item.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return InstanceDisplayName; }
        }

        /// <summary>
        ///     Gets a list of properties provided by this item.
        /// </summary>
        /// <returns>The properties</returns>
        public IEnumerable<IPropertyItem> Properties
        {
            get { yield return new AlfredProperty("Name", DisplayName); }
        }

        /// <summary>
        ///     Gets the property providers.
        /// </summary>
        /// <value>The property providers.</value>
        public IEnumerable<IPropertyProvider> PropertyProviders
        {
            get { yield return RootNode; }
        }
    }

    /// <summary>
    /// Represents an Aiml <see cref="Node"/> in the <see cref="ChatEngine"/> and presents it in an explorer-friendly manner.
    /// </summary>
    public class AimlExplorerNode : IPropertyProvider
    {
        [NotNull]
        private readonly IPlatformProvider _provider;

        [NotNull]
        private readonly Node _node;

        [NotNull, ItemNotNull]
        private readonly ICollection<AimlExplorerNode> _children;

        /// <summary>
        /// Initializes a new instance of the <see cref="AimlExplorerNode" /> class.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <exception cref="System.ArgumentNullException">node</exception>
        internal AimlExplorerNode([NotNull] IPlatformProvider provider, [NotNull] Node node)
        {
            if (node == null) { throw new ArgumentNullException(nameof(node)); }

            _provider = provider;
            _node = node;
            _children = _provider.CreateCollection<AimlExplorerNode>();
        }

        /// <summary>
        ///     Gets the name of the item.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return _node.Word ?? "Root Node"; }
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
                var child = new AimlExplorerNode(_provider, pair.Value);
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