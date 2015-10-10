using System;
using System.Diagnostics;

using MattEland.Common.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;
using System.Diagnostics.Contracts;
using MattEland.Common.Providers;

namespace MattEland.Ani.Alfred.Core.Subsystems
{

    /// <summary>
    ///     A search result containing information about an explorer node.
    /// </summary>
    internal sealed class ExplorerSearchResult : SearchResult
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ExplorerSearchResult" /> class.
        /// </summary>
        /// <param name="container"> The container. </param>
        /// <param name="node"> The node. </param>
        public ExplorerSearchResult([NotNull] IAlfredContainer container,
            [NotNull] IPropertyProvider node) : base(container, node.DisplayName)
        {
            //- Validate
            Contract.Requires(node != null, "node is null.");

            // Set Properties
            ExplorerNode = node;
            Description = ExplorerNode.ItemTypeName;
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

    }
}