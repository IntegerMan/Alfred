using IronGitHub;
using IronGitHub.Entities;
using JetBrains.Annotations;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Common;
using MattEland.Common.Providers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MattEland.Ani.Alfred.Search.GitHub
{
    /// <summary>
    /// A GitHub search operation. This class cannot be inherited. 
    /// </summary>
    public sealed class GitHubSearchOperation : ISearchOperation, IHasContainer, IDisposable
    {
        /// <summary>
        /// The GitHub API
        /// </summary>
        private readonly GitHubApi _api = new GitHubApi();

        /// <summary>
        ///     The results collection. New items can be added using this field.
        /// </summary>
        private readonly ICollection<ISearchResult> _results;

        private Task<Repository.RepositorySearchResults> _search;

        private readonly string _searchText;

        /// <summary>
        /// Initializes a new instance of the GitHubSearchOperation class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="searchText">The search text.</param>
        public GitHubSearchOperation([NotNull] IObjectContainer container, string searchText)
        {
            //- Validation
            if (container == null) throw new ArgumentNullException(nameof(searchText));
            if (searchText.IsEmpty()) throw new ArgumentNullException(nameof(searchText));

            //- Set properties from parameters
            Container = container;
            _searchText = searchText;

            // Build out the results collection 
            _results = container.ProvideCollection<ISearchResult>();
        }

        /// <summary>
        /// Gets the container. 
        /// </summary>
        /// <value> The container. </value>
        public IObjectContainer Container
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
        /// Gets a user-facing message describing the error if an error occurred retrieving search results. 
        /// </summary>
        /// <value> A message describing the error. </value>
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
        /// Gets the results of the search operation. 
        /// </summary>
        /// <value> The results. </value>
        public IEnumerable<ISearchResult> Results
        {
            get
            {
                return _results;
            }
        }

        /// <summary>
        /// Aborts the search. This is called when the user cancels a search or when a search times out.
        /// </summary>
        public void Abort()
        {
            IsSearchComplete = true;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _search.TryDispose();
        }

        /// <summary>
        /// Updates the search operation, adding results to the <see cref="Results"/> collection and
        /// updating <see cref="IsSearchComplete"/> based on the state of the search operation.
        /// </summary>
        public void Update()
        {
            // If we're already done, move on
            if (IsSearchComplete) return;

            // If we need to start the search, do that now
            if (_search == null)
            {
                _search = _api.Search.Repositories(_searchText);
            }

            // Check if the search has completed since last update
            if (_search.IsCompleted)
            {
                var result = _search.Result;
                foreach (var repository in result.Repositories)
                {
                    var item = new GitHubRepositoryResult(repository);

                    _results.Add(item);
                }
            }

            // Update properties
            EncounteredError = _search.IsFaulted;
            IsSearchComplete = _search.IsCompleted || _search.IsCanceled;
        }
    }
}