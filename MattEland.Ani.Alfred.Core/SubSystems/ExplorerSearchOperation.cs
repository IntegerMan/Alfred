﻿// ---------------------------------------------------------
// ExplorerSearchOperation.cs
// 
// Created on:      09/10/2015 at 10:01 PM
// Last Modified:   09/17/2015 at 11:50 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

using MattEland.Common.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Common.Definitions.Search;
using MattEland.Common.Providers;

namespace MattEland.Ani.Alfred.Core.Subsystems
{
    /// <summary>
    ///     A search operation aimed at the Mind Explorer.
    /// </summary>
    public sealed class ExplorerSearchOperation : ISearchOperation, IHasContainer<IAlfredContainer>
    {
        /// <summary>
        ///     The mind explorer subsystem.
        /// </summary>
        [NotNull]
        private readonly MindExplorerSubsystem _mindExplorerSubsystem;

        /// <summary>
        ///     The search results.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        private readonly ICollection<ISearchResult> _results;

        /// <summary>
        ///     The search text.
        /// </summary>
        [NotNull]
        private readonly string _searchText;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ExplorerSearchOperation" /> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="mindExplorerSubsystem">The mind explorer subsystem.</param>
        /// <param name="searchText">The search text.</param>
        internal ExplorerSearchOperation(IAlfredContainer container,
                                         MindExplorerSubsystem mindExplorerSubsystem,
                                         string searchText)
        {
            Container = container;

            _results = container.ProvideCollection<ISearchResult>();

            _mindExplorerSubsystem = mindExplorerSubsystem;
            _searchText = searchText;
        }

        /// <summary>
        ///     Gets a value indicating whether the search has completed yet. This is useful for
        ///     slow or asynchronous search operations.
        /// </summary>
        /// <value>
        /// <see langword="true" /> if the search is complete, otherwise <see langword="false" /> .
        /// </value>
        public bool IsSearchComplete { get; private set; }

        /// <summary>
        ///     Gets a value indicating whether the operation encountered an error.
        /// </summary>
        /// <value>
        /// <see langword="true" /> if an error was encountered, <see langword="false" /> if not.
        /// </value>
        public bool EncounteredError { get; private set; }

        /// <summary>
        ///     Gets a user-facing message describing the error if an error occurred retrieving
        ///     search results.
        /// </summary>
        /// <value>
        /// A message describing the error.
        /// </value>
        public string ErrorMessage { get; } = string.Empty;

        /// <summary>
        ///     Gets the results of the search operation.
        /// </summary>
        /// <value>
        /// The results.
        /// </value>
        public IEnumerable<ISearchResult> Results
        {
            get { return _results; }
        }

        /// <summary>
        ///     Gets the container.
        /// </summary>
        /// <value>
        ///     The container.
        /// </value>
        public IAlfredContainer Container
        {
            get;
        }

        /// <summary>
        /// Carries out the search operation and adds any results to the results collection
        /// </summary>
        public void Update()
        {
            // Start with Alfred as the Mind Explorer's root node
            IPropertyProvider root = _mindExplorerSubsystem.AlfredInstance;

            /* Since we're doing a complete search and not a partial, _results will be cleared
               in case this operation is ever re-used. This prevents persistent results. */

            _results.Clear();

            // Search the entire tree
            if (root != null)
            {
                // Search using case insensitivity
                var searchText = _searchText.ToUpperInvariant();

                var results = SearchPropertyProviderTreeNode(Container, root, searchText);

                /* Add to _results instead of setting to a new collection in case something is
                   observing the list object. It's not likely here, but this is a good pattern. */

                foreach (var result in results)
                {
                    _results.Add(result);
                }
            }

            // We've now navigated the tree and finished the search
            IsSearchComplete = true;
            EncounteredError = false;
        }

        /// <summary>
        ///     Searches the hierarchy of <paramref name="node" /> for matches to the search text.
        /// </summary>
        /// <param name="container"> The container. </param>
        /// <param name="node"> The node. </param>
        /// <param name="searchText">
        ///     The search text. This is expected to be in uppercase invariant already and will not be
        ///     checked for performance.
        /// </param>
        /// <returns>
        ///     An enumerator that allows foreach to be used to process the property provider tree nodes
        ///     in this collection.
        /// </returns>
        [NotNull, ItemNotNull]
        public static IEnumerable<ISearchResult> SearchPropertyProviderTreeNode(
            [NotNull] IAlfredContainer container,
            [NotNull] IPropertyProvider node,
            [NotNull] string searchText)
        {
            // If this node matches our search criteria, add it to the results
            if (IsNodeSearchMatch(node, searchText))
            {
                var searchResult = new ExplorerSearchResult(container, node);

                // Yield a direct result
                yield return searchResult;
            }

            // Early exit if no children
            if (node.PropertyProviders == null) yield break;

            // Recursively search all children
            foreach (var childNode in node.PropertyProviders.Where(childNode => childNode != null))
            {
                var results = SearchPropertyProviderTreeNode(container, childNode, searchText);

                // Yield all results
                foreach (var result in results)
                {
                    yield return result;
                }
            }
        }

        /// <summary>
        ///     Query if the <paramref name="node" /> is a search match based on its name and
        ///     property names and values.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="searchText">
        /// The search text. This should already be in uppercase (invariant) for a correct
        /// comparison.
        /// </param>
        /// <returns>
        ///     <see langword="true" /> if <paramref name="node" /> matches the search,
        ///     <see langword="false" /> if not.
        /// </returns>
        public static bool IsNodeSearchMatch([NotNull] IPropertyProvider node, string searchText)
        {
            // If the name in of itself matches, exit now
            if (node.DisplayName.ToUpperInvariant().Contains(searchText))
            {
                return true;
            }

            // Check properties for any match
            if (node.Properties != null)
            {
                foreach (var property in node.Properties)
                {
                    if (property == null) continue;

                    if (property.DisplayName.ToUpperInvariant().Contains(searchText)
                        || property.DisplayValue.ToUpperInvariant().Contains(searchText))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        ///     Aborts the search. This is called when the user cancels a search or when a search
        ///     times out.
        /// </summary>
        public void Abort()
        {
            // No actions to take since this search is not yet asynchronous.
        }
    }

}