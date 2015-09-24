using System;
using System.Collections.Generic;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Common.Providers;
using MattEland.Common;
using System.Globalization;
using System.Data.Services.Client;
using System.Diagnostics.Contracts;

namespace MattEland.Ani.Alfred.Search.Bing
{
    /// <summary>
    ///     A bing search operation. This class cannot be inherited.
    /// </summary>
    [PublicAPI]
    public sealed class BingSearchOperation : ISearchOperation
    {

        /// <summary>
        ///     The search results.
        /// </summary>
        [NotNull, ItemNotNull]
        private readonly ICollection<ISearchResult> _results;

        /// <summary>
        ///     The search text.
        /// </summary>
        [NotNull]
        private string SearchText;
        private IAsyncResult _queryResult;
        private DataServiceQuery<WebResult> _query;

        /// <summary>
        ///     Initializes a new instance of the <see cref="BingSearchOperation" />
        ///     class.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="container"/> is null.
        /// </exception>
        /// <param name="container"> The container. </param>
        /// <param name="searchText"> The search text. </param>
        public BingSearchOperation([NotNull] IObjectContainer container, string searchText)
        {
            //- Validate
            Contract.Requires(container != null, "container was null");
            Contract.Requires(searchText.HasText(), "search text was empty");

            //- Set Values from Parameters
            SearchText = searchText;

            //- Build Results Collection
            _results = container.ProvideCollection<ISearchResult>();

            //- Set Defaults
            IsSearchComplete = false;
            EncounteredError = false;
            ErrorMessage = null;

        }

        private void StartSearch()
        {
            // Set up the Bing Search Container that will be used to make web service calls
            const string BingSearchPath = @"https://api.datamarket.azure.com/Bing/Search";

            var bingContainer = new BingSearchContainer(new Uri(BingSearchPath));

            // Various settings for the query
            const string ContentSafety = "Moderate";
            var culture = CultureInfo.CurrentCulture.Name;

            // Set up the query
            _query = bingContainer.Web(SearchText,
                null,
                null,
                culture,
                ContentSafety,
                null,
                null,
                null);

            _queryResult = _query.BeginExecute(null, _query);
        }

        /// <summary>
        ///     Gets or sets the search query.
        /// </summary>
        /// <value>
        ///     The search query.
        /// </value>
        private DataServiceQuery<WebResult> Query { get; set; }

        /// <summary>
        ///     Aborts the search.
        ///     This is called when the user cancels a search or when a search times out.
        /// </summary>
        public void Abort()
        {
            IsSearchComplete = true;
        }

        /// <summary>
        ///     Updates the search operation, adding results to the
        ///     <see cref="ISearchOperation.Results"/> collection and updating
        ///     <see cref="ISearchOperation.IsSearchComplete"/> based on the state of the search
        ///     operation.
        /// </summary>
        public void Update()
        {
            // This shouldn't happen, but guard anyway
            if (IsSearchComplete)
            {
                return;
            }

            // If this is the initial run, we'll need to boot up the search
            if (_query == null)
            {
                StartSearch();
            }

            // Check to see if the result succeeded
            if (_queryResult.CompletedSynchronously || _queryResult.IsCompleted)
            {
                OnQueryCompleted(_queryResult);
            }
        }

        /// <summary>
        ///     Occurs when a search query completes
        /// </summary>
        /// <param name="result"> The query result. </param>
        private void OnQueryCompleted(IAsyncResult result)
        {
            Contract.Requires(result != null, "result is null.");
            Contract.Assume(result.AsyncState != null, "AsyncState is null.");

            var query = result.AsyncState as DataServiceQuery<WebResult>;

            var results = query.EndExecute(result);

            // Translate the web search results into domain-specific results
            foreach (WebResult webResult in results)
            {
                var searchResult = new BingSearchResult(webResult);

                _results.Add(searchResult);
            }

            IsSearchComplete = true;
        }

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
        public string ErrorMessage { get; }

        /// <summary>
        ///     Gets a value indicating whether the search has completed yet. This is useful for slow or
        ///     asynchronous search operations.
        /// </summary>
        /// <value>
        ///     <see langword="true" /> if the search is complete, otherwise <see langword="false" /> .
        /// </value>
        public bool IsSearchComplete { get; private set; }

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
    }
}