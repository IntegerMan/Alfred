// ---------------------------------------------------------
// AlfredSearchController.cs
// 
// Created on:      09/09/2015 at 12:30 AM
// Last Modified:   09/09/2015 at 12:30 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Common.Providers;

namespace MattEland.Ani.Alfred.Core
{
    /// <summary>
    ///     The default <see cref="ISearchController"/> for Alfred applications.
    /// </summary>
    public sealed class AlfredSearchController : ComponentBase, ISearchController, IHasContainer
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="AlfredSearchController"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="container" /> is <see langword="null" />.</exception>
        public AlfredSearchController([NotNull] IObjectContainer container) : base(container)
        {
            // Default search timeouts to 30 seconds
            SearchTimeoutInMilliseconds = TimeSpan.FromSeconds(30).Milliseconds;

            // Create Collections
            OngoingOperations = container.ProvideCollection<ISearchOperation>();
            Results = container.ProvideCollection<ISearchResult>();
            _searchProviders = container.ProvideCollection<ISearchProvider>();
        }

        /// <summary>
        ///     Gets whether or not the component is visible to the user interface.
        /// </summary>
        /// <value>Whether or not the component is visible.</value>
        public override bool IsVisible
        {
            get { return true; }
        }

        /// <summary>
        ///     Gets the children of the component. Depending on the type of component this is, the children
        ///     will vary in their own types.
        /// </summary>
        /// <value>The children.</value>
        public override IEnumerable<IAlfredComponent> Children
        {
            get
            {
                // TODO: Return ISearchProviders

                yield break;
            }
        }

        /// <summary>
        ///     Gets the name of the broad categorization or type that this item is.
        /// </summary>
        /// <example>
        ///     Some examples of ItemTypeName values might be "Folder", "Application", "User", etc.
        /// </example>
        /// <value>The item type's name.</value>
        public override string ItemTypeName
        {
            get { return "Search Controller"; }
        }

        /// <summary>
        ///     Gets the name of the component.
        /// </summary>
        /// <value>The name of the component.</value>
        [NotNull]
        public override string Name
        {
            get { return "Search Controller"; }
        }

        /// <summary>
        ///     Performs the search action against all search providers.
        /// </summary>
        /// <param name="searchText"> The search text. </param>
        public void PerformSearch(string searchText) { }

        /// <summary>
        ///     Performs the search action against a specific search provider.
        /// </summary>
        /// <param name="searchText"> The search text. </param>
        /// <param name="providerId"> The provider's identifier. </param>
        public void PerformSearch(string searchText, string providerId)
        {
            // TODO: Log this

            // TODO: Tell ISearchProviders to search

            // TODO: Store current search operations

            // TODO: Update status to in search / prepare tracking metrics
        }

        /// <summary>
        ///     Aborts any active searches.
        /// </summary>
        public void Abort()
        {
            // TODO: Log this

            // TODO: Tell ISearchProviders to abort
        }

        private double _searchTimeoutInMilliseconds;

        /// <summary>
        ///     Gets or sets the search timeout duration in milliseconds. Searches that take longer than this will be aborted.
        /// </summary>
        /// <value>
        ///     The search timeout in milliseconds.
        /// </value>
        public double SearchTimeoutInMilliseconds
        {
            get { return _searchTimeoutInMilliseconds; }
            set
            {
                // TODO: Validate

                _searchTimeoutInMilliseconds = value;
            }
        }

        /// <summary>
        ///     Gets the time the last search started. This will be <see langword="null"/> if no searches
        ///     have been made.
        /// </summary>
        /// <value>
        ///     The last search start time.
        /// </value>
        public DateTime? LastSearchStart { get; }

        /// <summary>
        ///     Gets the time the last search ended. This will be <see langword="null" /> if no searches
        ///     have been made or a search is currently executing.
        /// </summary>
        /// <value>
        ///     The last search end time.
        /// </value>
        public DateTime? LastSearchEnd { get; }

        /// <summary>
        ///     Gets the duration of the last search. This will be <see langword="null"/> if no searches
        ///     have been made or a search is currently executing.
        /// </summary>
        /// <value>
        ///     The last search duration.
        /// </value>
        public TimeSpan? LastSearchDuration { get; }

        /// <summary>
        ///     The search providers collection. This is used internally to add new providers.
        /// </summary>
        [NotNull, ItemNotNull]
        private readonly ICollection<ISearchProvider> _searchProviders;

        /// <summary>
        ///     Gets the search providers.
        /// </summary>
        /// <value>
        ///     The search providers.
        /// </value>
        [NotNull]
        [ItemNotNull]
        public IEnumerable<ISearchProvider> SearchProviders
        {
            get { return _searchProviders; }
        }

        /// <summary>
        ///     Gets the results of the last search operation.
        /// </summary>
        /// <value>
        ///     The results.
        /// </value>
        [NotNull, ItemNotNull]
        public IEnumerable<ISearchResult> Results { get; }

        /// <summary>
        ///     Gets the search operations that are currently executing.
        /// </summary>
        /// <value>
        ///     The ongoing operations.
        /// </value>
        [NotNull, ItemNotNull]
        public IEnumerable<ISearchOperation> OngoingOperations { get; }

        /// <summary>
        ///     Gets a value indicating whether any providers are still searching.
        /// </summary>
        /// <value>
        ///     <see langword="true"/> if this instance is searching, <see langword="false"/> if not.
        /// </value>
        public bool IsSearching { get; }

        /// <summary>
        ///     Gets a user-facing message describing the status of the last search including the number
        ///     of results found and details on any errors encountered.
        /// </summary>
        /// <value>
        ///     A message describing the status of the controller.
        /// </value>
        public string StatusMessage { get; }

        /// <summary>
        ///     Registers the search provider.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="provider"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     Thrown if this was called when the component is not
        ///     <see cref="AlfredStatus.Offline" /> .
        /// </exception>
        /// <param name="provider"> The provider. </param>
        public void Register(ISearchProvider provider)
        {
            if (provider == null) { throw new ArgumentNullException(nameof(provider)); }

            if (Status != AlfredStatus.Offline)
            {
                throw new InvalidOperationException("Cannot register new providers unless the controller is offline");
            }

            _searchProviders.Add(provider);
        }
    }
}