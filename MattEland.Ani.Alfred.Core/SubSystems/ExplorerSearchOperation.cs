using System.Collections.Generic;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Common.Providers;

namespace MattEland.Ani.Alfred.Core.Subsystems
{
    /// <summary>
    ///     A search operation aimed at the Mind Explorer.
    /// </summary>
    public sealed class ExplorerSearchOperation : ISearchOperation
    {
        /// <summary>
        ///     The mind explorer subsystem.
        /// </summary>
        [NotNull]
        private readonly MindExplorerSubsystem _mindExplorerSubsystem;

        /// <summary>
        ///     The search text.
        /// </summary>
        [NotNull]
        private readonly string _searchText;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ExplorerSearchOperation"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="mindExplorerSubsystem">The mind explorer subsystem.</param>
        /// <param name="searchText">The search text.</param>
        internal ExplorerSearchOperation(
            IObjectContainer container,
            MindExplorerSubsystem mindExplorerSubsystem,
            string searchText)
        {
            _results = container.ProvideCollection<ISearchResult>();

            _mindExplorerSubsystem = mindExplorerSubsystem;
            _searchText = searchText;
        }

        /// <summary>
        ///     Gets a value indicating whether the search has completed yet. This is useful for slow or
        ///     asynchronous search operations.
        /// </summary>
        /// <value>
        ///     <see langword="true" /> if the search is complete, otherwise <see langword="false" /> .
        /// </value>
        public bool IsSearchComplete { get; private set; }

        /// <summary>
        ///     Gets a value indicating whether the operation encountered an error.
        /// </summary>
        /// <value>
        ///     <see langword="true" /> if an error was encountered, <see langword="false" /> if not.
        /// </value>
        public bool EncounteredError { get; private set; }

        /// <summary>
        ///     Gets a user-facing message describing the error if an error occurred retrieving search
        ///     results.
        /// </summary>
        /// <value>
        ///     A message describing the error.
        /// </value>
        public string ErrorMessage { get; } = string.Empty;

        /// <summary>
        ///     The search results.
        /// </summary>
        [NotNull, ItemNotNull]
        private readonly ICollection<ISearchResult> _results;

        /// <summary>
        ///     Gets the results of the search operation.
        /// </summary>
        /// <value>
        ///     The results.
        /// </value>
        public IEnumerable<ISearchResult> Results
        {
            get { return _results; }
        }

        /// <summary>
        ///     Updates the search operation, adding results to the <see cref="ISearchOperation.Results"/> collection and
        ///     updating <see cref="ISearchOperation.IsSearchComplete"/> based on the state of the search operation.
        /// </summary>
        public void Update()
        {
            // TODO: Implement search
        }

        /// <summary>
        ///     Aborts the search.
        ///     This is called when the user cancels a search or when a search times out.
        /// </summary>
        public void Abort()
        {
            // TODO: Do something
        }
    }
}