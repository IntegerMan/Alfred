// --------------------------------------------------------- AlfredSearchController.cs
// 
// Created on: 09/09/2015 at 12:30 AM Last Modified: 09/16/2015 at 10:51 AM
// 
// Last Modified by: Matt Eland ---------------------------------------------------------

using JetBrains.Annotations;
using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Common;
using MattEland.Common.Providers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MattEland.Ani.Alfred.Core
{
    /// <summary>
    /// The default <see cref="ISearchController"/> for Alfred applications. 
    /// </summary>
    public sealed class AlfredSearchController : ComponentBase, ISearchController
    {
        /// <summary>
        /// The ongoing operations collection. This is used for adding new operations. 
        /// </summary>
        /// <remarks>
        /// TODO: At some point these should probably become Task objects for more flexibility
        /// </remarks>
        [NotNull]
        [ItemNotNull]
        private readonly ICollection<ISearchOperation> _ongoingOperations;

        /// <summary>
        /// The results collection. This is used to append new items. 
        /// </summary>
        [NotNull]
        [ItemNotNull]
        private readonly ICollection<ISearchResult> _results;

        /// <summary>
        /// The search providers collection. This is used internally to add new providers. 
        /// </summary>
        [NotNull]
        [ItemNotNull]
        private readonly ICollection<ISearchProvider> _searchProviders;

        /// <summary>
        /// The last search's textual input. 
        /// </summary>
        [CanBeNull]
        private string _lastSearch;

        private double _searchTimeoutInMilliseconds;

        /// <summary>
        /// Initializes a new instance of the <see cref="AlfredSearchController"/> class. 
        /// </summary>
        /// <param name="container"> The container. </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="container"/> is <see langword="null"/> .
        /// </exception>
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
        /// Occurs when a new result is added. 
        /// </summary>
        public event EventHandler<SearchResultEventArgs> ResultAdded;

        /// <summary>
        /// Occurs when all results are cleared. 
        /// </summary>
        public event EventHandler ResultsCleared;

        /// <summary>
        /// Gets the children of the component. Depending on the type of component this is, the
        /// children will vary in their own types.
        /// </summary>
        /// <value> The children. </value>
        public override IEnumerable<IAlfredComponent> Children
        {
            get { yield break; }
        }

        /// <summary>
        /// Gets a value indicating whether any providers are still searching. 
        /// </summary>
        /// <value>
        /// <see langword="true"/> if this instance is searching, <see langword="false"/> if not.
        /// </value>
        public bool IsSearching
        {
            get { return OngoingOperations.Any(); }
        }

        /// <summary>
        /// Gets whether or not the component is visible to the user interface. 
        /// </summary>
        /// <value> Whether or not the component is visible. </value>
        public override bool IsVisible
        {
            get { return true; }
        }

        /// <summary>
        /// Gets the name of the broad categorization or type that this item is. 
        /// </summary>
        /// <value> The item type's name. </value>
        /// <example>
        /// Some examples of
        /// <see cref="MattEland.Ani.Alfred.Core.AlfredSearchController.ItemTypeName"/> values might
        /// be "Folder", "Application", "User", etc.
        /// </example>
        public override string ItemTypeName
        {
            get { return "Search Controller"; }
        }

        /// <summary>
        /// Gets the duration of the last search. This will be <see langword="null"/> if no searches
        /// have been made or a search is currently executing.
        /// </summary>
        /// <value> The last search duration. </value>
        public TimeSpan? LastSearchDuration { get; }

        /// <summary>
        /// Gets the time the last search ended. This will be <see langword="null"/> if no searches
        /// have been made or a search is currently executing.
        /// </summary>
        /// <value> The last search end time. </value>
        public DateTime? LastSearchEnd { get; }

        /// <summary>
        /// Gets the time the last search started. This will be <see langword="null"/> if no
        /// searches have been made.
        /// </summary>
        /// <value> The last search start time. </value>
        public DateTime? LastSearchStart { get; }

        /// <summary>
        /// Gets the name of the component. 
        /// </summary>
        /// <value> The name of the component. </value>
        [NotNull]
        public override string Name
        {
            get { return "Search Controller"; }
        }

        /// <summary>
        /// Gets the search operations that are currently executing. 
        /// </summary>
        /// <value> The ongoing operations. </value>
        [NotNull]
        [ItemNotNull]
        public IEnumerable<ISearchOperation> OngoingOperations
        {
            get { return _ongoingOperations; }
        }

        /// <summary>
        /// Gets the results of the last search operation. 
        /// </summary>
        /// <value> The results. </value>
        [NotNull]
        [ItemNotNull]
        public IEnumerable<ISearchResult> Results
        {
            get { return _results; }
        }

        /// <summary>
        /// Gets the search providers. 
        /// </summary>
        /// <value> The search providers. </value>
        [NotNull]
        [ItemNotNull]
        public IEnumerable<ISearchProvider> SearchProviders
        {
            get { return _searchProviders; }
        }

        /// <summary>
        /// Gets or sets the search timeout duration in milliseconds. Searches that take longer than
        /// this will be aborted.
        /// </summary>
        /// <value> The search timeout in milliseconds. </value>
        public double SearchTimeoutInMilliseconds
        {
            get
            {
                return _searchTimeoutInMilliseconds;
            }
            set
            {
                // TODO: Validate 

                _searchTimeoutInMilliseconds = value;
            }
        }

        /// <summary>
        ///     Gets a pluralized results string for logging, display, or user notification.
        /// </summary>
        /// <returns>
        ///     The pluralized results string.
        /// </returns>
        private string GetPluralizedResultsString()
        {
            var numResults = _results.Count;

            if (numResults <= 0)
            {
                return "No results";
            }

            return numResults.Pluralize("# result", "# results");
        }
        /// <summary>
        /// Gets a user-facing message describing the status of the last search including the number
        /// of results found and details on any errors encountered.
        /// </summary>
        /// <value> A message describing the status of the search. </value>
        public string StatusMessage
        {
            get
            {
                // When starting up, no searches will be made, so just use static text 
                if (!_lastSearch.HasText())
                {
                    return "No searches have been made yet.";
                }

                // Build out a result string ("42 results" / "1 result" / "No results") 
                var results = GetPluralizedResultsString();

                // Format the results based on the search status (in process vs completed) 
                return IsSearching
                           ? $"Searching for \"{_lastSearch}\". {results} found so far..."
                           : $"Search complete. {results} found for the search: \"{_lastSearch}\".";
            }
        }

        /// <summary>
        /// Aborts any active searches. 
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
        }

        /// <summary>
        /// Performs the search action against all search providers. 
        /// </summary>
        /// <param name="searchText"> The search text. </param>
        public void PerformSearch([NotNull] string searchText)
        {
            // Cancel any ongoing searches 
            if (OngoingOperations.Any())
            {
                Abort();
            }

            // Start tracking this search 
            RecordSearchStart(searchText);

            // Search all providers 
            foreach (var provider in SearchProviders)
            {
                BuildSearchOperation(searchText, provider);
            }

            if (OngoingOperations.Any())
            {
                // Allow the searches to kick off 
                UpdateOngoingOperations();
            }
            else
            {
                // This didn't spawn any operations (perhaps no providers). Make sure its logged.
                LogSearchCompleted();
            }
        }

        /// <summary>
        /// Performs the search action against a specific search provider. 
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
                // Do a bit of record keeping for performance / timeout calculations 
                RecordSearchStart(searchText);

                // Create the search and add it to our collection 
                BuildSearchOperation(searchText, target);

                // Update the internal tracking mechanisms and start the search 
                UpdateOngoingOperations();
            }
            else
            {
                // TODO: Log this 
            }
        }

        /// <summary>
        ///     Registers the search provider.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="provider"/> is <see langword="null"/> .
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     Thrown if this was called when the component is not
        ///     <see cref="MattEland.Ani.Alfred.Core.Definitions.AlfredStatus.Offline"/> .
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     When <paramref name="provider"/> has an Id that is the same as another
        ///     <paramref name="provider"/> that has already been added.
        /// </exception>
        /// <param name="provider"> The provider. </param>
        public void Register(ISearchProvider provider)
        {
            if (provider == null) throw new ArgumentNullException(nameof(provider));

            // Require Offline Status 
            if (Status != AlfredStatus.Offline)
            {
                throw new InvalidOperationException(
                    "Cannot register new providers unless the controller is offline");
            }

            // Check for Duplicates 
            var id = provider.Id;
            if (_searchProviders.Any(p => p.Id.Matches(id)))
            {
                throw new ArgumentException(
                    $"A provider with the Id of {id} has already been added",
                    nameof(provider));
            }

            _searchProviders.Add(provider);
        }

        /// <summary>
        /// Updates the component. 
        /// </summary>
        protected override void UpdateProtected()
        {
            base.UpdateProtected();

            UpdateOngoingOperations();
        }

        /// <summary>
        /// Adds a result to the results collection and raises the <see cref="ResultAdded"/> event. 
        /// </summary>
        /// <param name="result"> The result. </param>
        private void AddResult([NotNull] ISearchResult result)
        {
            // Results can already exist in the collection, potentially, so guard against that 
            if (_results.Contains(result))
            {
                return;
            }

            _results.Add(result);

            var e = new SearchResultEventArgs { Result = result };

            ResultAdded?.Invoke(this, e);
        }

        /// <summary>
        /// Builds a search operation from the specified <paramref name="searchProvider"/> for the
        /// specified <paramref name="searchText"/>.
        /// </summary>
        /// <param name="searchText"> The search text. </param>
        /// <param name="searchProvider"> The search provider. </param>
        private void BuildSearchOperation([NotNull] string searchText,
                                          [NotNull] ISearchProvider searchProvider)
        {
            // Log this event 
            var logMessage = $"Searching {searchProvider.Id} for: '{searchText}'";
            logMessage.Log("Search Executed", LogLevel.Info, Container);

            var operation = searchProvider.PerformSearch(searchText);

            _ongoingOperations.Add(operation);
        }

        /// <summary>
        /// Records the start of a search. 
        /// </summary>
        /// <param name="searchText"> The search text. </param>
        private void RecordSearchStart(string searchText)
        {
            // Clear out all old search results
            _results.Clear();

            // Tell other parts of the UI that our result set is now different 
            ResultsCleared?.Invoke(this, EventArgs.Empty);

            // Set the new search text
            _lastSearch = searchText;

            // TODO: Prepare tracking metrics 
        }

        /// <summary>
        ///     Logs that a search completed in such a way that it notifies the user.
        /// </summary>
        private void LogSearchCompleted()
        {
            // Build a plural-friendly message
            var message = string.Format("Search complete. {0} found.",
                GetPluralizedResultsString());

            //- Log the message
            message.Log("Search Complete", LogLevel.ChatNotification, Container);
        }
        /// <summary>
        /// Updates the ongoing operations, allowing each one to check on its status and update the
        /// results collection.
        /// </summary>
        private void UpdateOngoingOperations()
        {
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
                    AddResult(result);
                }

                // If the operation has completed, remove it from the list 
                if (op.IsSearchComplete)
                {
                    _ongoingOperations.Remove(op);

                    if (_ongoingOperations.Count == 0)
                    {
                        LogSearchCompleted();
                    }
                }

                // TODO: Handle errors 
            }
        }
    }
}