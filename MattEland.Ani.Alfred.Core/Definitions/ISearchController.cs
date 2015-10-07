using System;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace MattEland.Ani.Alfred.Core.Definitions
{
    /// <summary>
    ///     Interface for a search controller that manages Alfred search operations.
    /// </summary>
    public interface ISearchController : IAlfredComponent
    {
        /// <summary>
        /// Gets the search history entries.
        /// </summary>
        /// <value>
        /// The search history entries.
        /// </value>
        [NotNull, ItemNotNull]
        IEnumerable<SearchHistoryEntry> SearchHistory { get; }

        /// <summary>
        ///     Performs the search action against all search providers.
        /// </summary>
        /// <param name="searchText"> The search text. </param>
        void PerformSearch([NotNull] string searchText);

        /// <summary>
        ///     Performs the search action against a specific search provider.
        /// </summary>
        /// <param name="searchText"> The search text. </param>
        /// <param name="providerId"> The provider's identifier. </param>
        void PerformSearch([NotNull] string searchText, [NotNull] string providerId);

        /// <summary>
        ///     Aborts any active searches.
        /// </summary>
        void Abort();

        /// <summary>
        ///     Gets or sets the search timeout duration in milliseconds. Searches that take longer than this will be aborted.
        /// </summary>
        /// <value>
        ///     The search timeout in milliseconds.
        /// </value>
        double SearchTimeoutInMilliseconds { get; set; }

        /// <summary>
        ///     Gets the time the last search started. This will be <see langword="null"/> if no searches
        ///     have been made.
        /// </summary>
        /// <value>
        ///     The last search start time.
        /// </value>
        DateTime? LastSearchStart { get; }

        /// <summary>
        ///     Gets the time the last search ended. This will be <see langword="null" /> if no searches
        ///     have been made or a search is currently executing.
        /// </summary>
        /// <value>
        ///     The last search end time.
        /// </value>
        DateTime? LastSearchEnd { get; }

        /// <summary>
        ///     Gets the duration of the last search. This will be <see langword="null"/> if no searches
        ///     have been made or a search is currently executing.
        /// </summary>
        /// <value>
        ///     The last search duration.
        /// </value>
        TimeSpan? LastSearchDuration { get; }

        /// <summary>
        ///     Gets the search providers.
        /// </summary>
        /// <value>
        ///     The search providers.
        /// </value>
        IEnumerable<ISearchProvider> SearchProviders { get; }

        /// <summary>
        ///     Gets the results of the last search operation.
        /// </summary>
        /// <value>
        ///     The results.
        /// </value>
        [NotNull]
        IEnumerable<ISearchResult> Results { get; }

        /// <summary>
        ///     Gets the search operations that are currently executing.
        /// </summary>
        /// <value>
        ///     The ongoing operations.
        /// </value>
        IEnumerable<ISearchOperation> OngoingOperations { get; }

        /// <summary>
        ///     Gets a value indicating whether any providers are still searching.
        /// </summary>
        /// <value>
        ///     <see langword="true"/> if this instance is searching, <see langword="false"/> if not.
        /// </value>
        bool IsSearching { get; }

        /// <summary>
        ///     Gets a user-facing message describing the status of the last search including the number
        ///     of results found and details on any errors encountered.
        /// </summary>
        /// <value>
        ///     A message describing the status of the controller.
        /// </value>
        string StatusMessage { get; }

        /// <summary>
        ///     Registers the search provider.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        ///     Thrown if this was called when the component is not <see cref="AlfredStatus.Offline"/>.
        /// </exception>
        /// <param name="provider"> The provider. </param>
        void Register([NotNull] ISearchProvider provider);

        /// <summary>
        ///     Occurs when a new result is added.
        /// </summary>
        event EventHandler<SearchResultEventArgs> ResultAdded;

        /// <summary>
        ///     Occurs when all results are cleared.
        /// </summary>
        event EventHandler ResultsCleared;
    }
}