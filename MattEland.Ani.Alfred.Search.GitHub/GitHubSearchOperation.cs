using JetBrains.Annotations;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Common.Providers;
using Octokit;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace MattEland.Ani.Alfred.Search.GitHub
{
    /// <summary>
    /// A GitHub search operation. This class cannot be inherited. 
    /// </summary>
    public sealed class GitHubSearchOperation : ISearchOperation, IHasContainer
    {
        /// <summary>
        ///     The GitHub client.
        /// </summary>
        private readonly GitHubClient _client;
        /// <summary>
        ///     The results collection. New items can be added using this field.
        /// </summary>
        private readonly ICollection<ISearchResult> _results;

        /// <summary>
        /// Initializes a new instance of the GitHubSearchOperation class. 
        /// </summary>
        /// <param name="container"> The container. </param>
        public GitHubSearchOperation([NotNull] IObjectContainer container)
        {
            //- Validation
            Contract.Requires(container != null, "container is null.");

            //- Set properties from parameters
            Container = container;

            // Build out the results collection 
            _results = container.ProvideCollection<ISearchResult>();

            // Create an identifier to provide to the client library
            var assembly = GetType().Assembly;
            var productHeader = new ProductHeaderValue(assembly.FullName);

            _client = new GitHubClient(productHeader);
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
            get
            {
                throw new NotImplementedException();
            }
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
        /// Updates the search operation, adding results to the <see cref="Results"/> collection and
        /// updating <see cref="IsSearchComplete"/> based on the state of the search operation.
        /// </summary>
        public void Update()
        {
            // TODO: Actually search 

            IsSearchComplete = true;
        }
    }
}