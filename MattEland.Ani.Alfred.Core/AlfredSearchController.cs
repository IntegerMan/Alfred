﻿// ---------------------------------------------------------
// AlfredSearchController.cs
// 
// Created on:      09/09/2015 at 12:30 AM
// Last Modified:   09/09/2015 at 12:30 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Common;
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
            _ongoingOperations = container.ProvideCollection<ISearchOperation>();
            _results = container.ProvideCollection<ISearchResult>();
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
        public void PerformSearch([NotNull] string searchText)
        {
            // If there's stuff going on - cancel it
            if (OngoingOperations.Any()) { Abort(); }

            // Search all providers
            foreach (var provider in SearchProviders)
            {
                StartSearchOperation(searchText, provider);
            }
        }

        /// <summary>
        ///     Starts the search operation on the specified <paramref name="searchProvider"/>.
        /// </summary>
        /// <param name="searchText"> The search text. </param>
        /// <param name="searchProvider"> The search provider. </param>
        private void StartSearchOperation(
            [NotNull] string searchText, [NotNull] ISearchProvider searchProvider)
        {
            // TODO: Log this

            var operation = searchProvider.PerformSearch(searchText);

            _ongoingOperations.Add(operation);

            // TODO: Update status to in search / prepare tracking metrics
        }

        /// <summary>
        ///     Performs the search action against a specific search provider.
        /// </summary>
        /// <param name="searchText"> The search text. </param>
        /// <param name="providerId"> The provider's identifier. </param>
        public void PerformSearch([NotNull] string searchText, [NotNull] string providerId)
        {
            // When searching all providers, use the simpler method
            if (providerId.IsEmpty() || providerId.Matches("All"))
            {
                PerformSearch(searchText);
                return;
            }

            var target = SearchProviders.FirstOrDefault(s => s.Id.Matches(providerId));
            if (target != null)
            {
                StartSearchOperation(searchText, target);
            }
            else
            {
                // TODO: Log this
            }
        }

        /// <summary>
        ///     Aborts any active searches.
        /// </summary>
        public void Abort()
        {
            // TODO: Log this

            // Tell each search to abort
            foreach (var operation in OngoingOperations)
            {
                operation.Abort();
            }

            // Clear out all old operations and results
            _ongoingOperations.Clear();
            _results.Clear();
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
        ///     The results collection. This is used to append new items.
        /// </summary>
        [NotNull, ItemNotNull]
        private readonly ICollection<ISearchResult> _results;

        /// <summary>
        ///     Gets the results of the last search operation.
        /// </summary>
        /// <value>
        ///     The results.
        /// </value>
        [NotNull]
        [ItemNotNull]
        public IEnumerable<ISearchResult> Results
        {
            get { return _results; }
        }

        /// <summary>
        ///     The ongoing operations collection. This is used for adding new operations.
        /// </summary>
        [NotNull, ItemNotNull]
        private readonly ICollection<ISearchOperation> _ongoingOperations;

        /// <summary>
        ///     Gets the search operations that are currently executing.
        /// </summary>
        /// <value>
        ///     The ongoing operations.
        /// </value>
        [NotNull]
        [ItemNotNull]
        public IEnumerable<ISearchOperation> OngoingOperations
        {
            get { return _ongoingOperations; }
        }

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
        /// <exception cref="ArgumentException">
        ///     When <paramref name="provider"/>'s Id is the same as another provider that has already
        ///     been added.
        /// </exception>
        /// <param name="provider"> The provider. </param>
        public void Register(ISearchProvider provider)
        {
            if (provider == null) { throw new ArgumentNullException(nameof(provider)); }

            // Require Offline Status
            if (Status != AlfredStatus.Offline)
            {
                throw new InvalidOperationException("Cannot register new providers unless the controller is offline");
            }

            // Check for Duplicates
            var id = provider.Id;
            if (_searchProviders.Any(p => p.Id.Matches(id)))
            {
                throw new ArgumentException($"A provider with the Id of {id} has already been added", nameof(provider));
            }

            _searchProviders.Add(provider);
        }

        /// <summary>
        ///     Updates the component.
        /// </summary>
        protected override void UpdateProtected()
        {
            base.UpdateProtected();

            // Get ops .ToList() because we may be removing operations while iterating
            var ongoingOperations = _ongoingOperations.ToList();

            // Update all ongoing operations.
            foreach (var op in ongoingOperations)
            {
                Debug.Assert(op != null);

                // Update the operation
                op.Update();

                // Add new results as they come in
                foreach (var result in op.Results)
                {
                    // NOTE: This lookup operation may not scale well and a Set may be needed
                    if (!_results.Contains(result))
                    {
                        _results.Add(result);
                    }
                }

                // If the operation has completed, remove it from the list
                if (op.IsSearchComplete) { _ongoingOperations.Remove(op); }

                // TODO: Handle errors

            }

            // TODO: Update status text

        }
    }
}