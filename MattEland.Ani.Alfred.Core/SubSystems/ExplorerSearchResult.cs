using System;
using System.Diagnostics;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;

namespace MattEland.Ani.Alfred.Core.Subsystems
{
    /// <summary>
    ///     A search result containing information about an explorer node.
    /// </summary>
    internal sealed class ExplorerSearchResult : ISearchResult
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ExplorerSearchResult" /> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if <paramref name="node"/> is null.
        /// </exception>
        /// <param name="node"> The node. </param>
        public ExplorerSearchResult([NotNull] IPropertyProvider node)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));

            ExplorerNode = node;
        }

        /// <summary>
        ///     The node in the explorer tree.
        /// </summary>
        [NotNull]
        public IPropertyProvider ExplorerNode
        {
            [DebuggerStepThrough]
            get;
        }

        /// <summary>
        ///     Gets the title or heading to use when displaying the search result.
        /// </summary>
        /// <value>
        ///     The title of the search result.
        /// </value>
        public string Title
        {
            get { return ExplorerNode.DisplayName; }
        }

        /// <summary>
        ///     Gets the textual description of the search result.
        /// </summary>
        /// <value>
        /// The description of the search result.
        /// </value>
        public string Description { get { return ExplorerNode.ItemTypeName; } }

        /// <summary>
        ///     Gets a textual display of the location. Location can vary by the type of search and
        ///     could be a physical street address, web URL, file or network path, or even a page
        ///     number or other reference code.
        /// </summary>
        /// <value>
        /// The location text.
        /// </value>
        public string LocationText
        {
            get
            {
                // TODO: Give the node context info on its parent 

                return string.Empty;
            }
        }

        /// <summary>
        ///     Gets an action that will provide more details on the search result. What this does
        ///     varies by the type of search result.
        /// </summary>
        /// <value>
        /// The get more details action.
        /// </value>
        public Action<ISearchResult> MoreDetailsAction
        {
            get
            {
                // TODO: It'd be nice to have an action to navigate to the item
                return null;
            }
        }

        /// <summary>
        ///     Gets the more details link's text.
        /// </summary>
        /// <value>
        ///     The more details text.
        /// </value>
        public string MoreDetailsText
        {
            get { return string.Empty; }
        }
    }
}