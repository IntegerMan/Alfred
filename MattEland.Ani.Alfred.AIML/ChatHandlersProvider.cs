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

using MattEland.Common.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Common.Providers;

namespace MattEland.Ani.Alfred.Chat
{
    /// <summary>
    ///     An <see cref="IPropertyProvider" /> that provides explorer nodes for nodes inside of the
    ///     <see cref="ChatEngine" /> AIML tree.
    /// </summary>
    public class ChatHandlersProvider : IPropertyProvider
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <param name="container"> The container. </param>
        /// <param name="engine"> The chat engine. </param>
        internal ChatHandlersProvider(
            [NotNull] IAlfredContainer container, [NotNull] ChatEngine engine)
        {
            if (container == null) { throw new ArgumentNullException(nameof(container)); }
            if (engine == null) { throw new ArgumentNullException(nameof(engine)); }

            RootNode = new AimlExplorerNode(container, engine.RootNode);
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

}