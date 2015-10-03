using System;
using System.Collections.Generic;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Common.Providers;
using JetBrains.Annotations;
using MattEland.Common;
using StackExchange.StacMan;
using System.Threading.Tasks;
using StackExchange.StacMan.Questions;
using System.Diagnostics.Contracts;

namespace MattEland.Ani.Alfred.Search.StackOverflow
{
    /// <summary>
    ///     A stack overflow search operation. This class cannot be inherited.
    /// </summary>
    internal sealed class StackOverflowSearchOperation : ISearchOperation, IHasContainer<IAlfredContainer>
    {
        /// <summary>
        ///     The client.
        /// </summary>
        [NotNull]
        private readonly StacManClient _client;

        /// <summary>
        ///     The query.
        /// </summary>
        [CanBeNull]
        private Task<StacManResponse<Question>> _query;

        /// <summary>
        ///     The search text.
        /// </summary>
        [NotNull]
        private readonly string _searchText;

        /// <summary>
        ///     Initializes a new instance of the <see cref="StackOverflowSearchOperation"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <param name="container"> The container. </param>
        /// <param name="searchText"> The search text. </param>
        /// <param name="apiKey"> The API key. </param>
        public StackOverflowSearchOperation([NotNull] IAlfredContainer container, [NotNull] string searchText, [CanBeNull] string apiKey)
        {
            //- Validate
            Contract.Requires(container != null);
            Contract.Requires(searchText.HasText());
            Contract.Requires(apiKey.HasText(), "apiKey was not set");

            // Set properties
            Container = container;
            _searchText = searchText;
            _client = new StacManClient(apiKey);

            // Create nested collections
            _results = container.ProvideCollection<ISearchResult>();

        }

        /// <summary>
        /// Gets the container.
        /// </summary>
        /// <value>
        /// The container.
        /// </value>
        [NotNull]
        public IAlfredContainer Container
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether the operation encountered an error.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if an error was encountered, <see langword="false"/> if not.
        /// </value>
        public bool EncounteredError
        {
            get; private set;
        }

        /// <summary>
        /// Gets a user-facing message describing the error if an error occurred retrieving search
        /// results.
        /// </summary>
        /// <value>
        /// A message describing the error.
        /// </value>
        public string ErrorMessage
        {
            get; private set;
        }

        /// <summary>
        /// Gets a value indicating whether the search has completed yet. This is useful for slow or
        /// asynchronous search operations.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the search is complete, otherwise <see langword="false"/> .
        /// </value>
        public bool IsSearchComplete
        {
            get; private set;
        }

        /// <summary>
        ///     The results collection. This can be modified to add new results.
        /// </summary>
        [NotNull, ItemNotNull]
        private ICollection<ISearchResult> _results;

        /// <summary>
        /// Gets the results of the search operation.
        /// </summary>
        /// <value>
        /// The results.
        /// </value>
        [NotNull, ItemNotNull]
        public IEnumerable<ISearchResult> Results
        {
            get
            {
                return _results;
            }
        }

        /// <summary>
        /// Aborts the search.
        /// This is called when the user cancels a search or when a search times out.
        /// </summary>
        public void Abort()
        {
            IsSearchComplete = true;
        }

        /// <summary>
        ///     Handles the result of the query.
        /// </summary>
        private void HandleQueryResult()
        {
            // If we got here, the operation has ended or encountered an error
            IsSearchComplete = true;

            // Check for task error
            EncounteredError = _query.IsFaulted;
            ErrorMessage = _query.Exception?.Message;

            // If no response, we can't do much
            var result = _query.Result;
            if (result == null) return;

            Wrapper<Question> wrapper = result.Data;

            // Check for error in the wrapper
            if (wrapper.ErrorMessage.HasText())
            {
                EncounteredError = true;
                ErrorMessage = string.Format("{0}: {1} (Error ID: {2})", wrapper.ErrorName, wrapper.ErrorMessage, wrapper.ErrorId);

                return;
            }

            // Process the questions returned
            var items = wrapper.Items;
            if (items != null)
            {
                foreach (var question in items)
                {
                    var item = new StackOverflowQuestionResult(Container, question);
                    _results.Add(item);
                }
            }
        }

        /// <summary>
        ///     Starts the search operation.
        /// </summary>
        private void StartSearch()
        {
            const string SiteName = "stackoverflow";

            _query = _client.Search.GetMatches(SiteName,
                         intitle: _searchText,
                         filter: @"withbody",
                         page: 1,
                         pagesize: 10,
                         sort: SearchSort.Relevance);
        }
        /// <summary>
        /// Updates the search operation, adding results to the <see cref="Results"/> collection and
        /// updating <see cref="IsSearchComplete"/> based on the state of the search operation.
        /// </summary>
        public void Update()
        {
            if (IsSearchComplete) return;

            // Start query as needed
            if (_query == null)
            {
                StartSearch();
            }

            // Check to see if the query completed and produce results as needed
            if (_query.IsCompleted || _query.IsFaulted)
            {
                HandleQueryResult();
            }
        }
    }
}