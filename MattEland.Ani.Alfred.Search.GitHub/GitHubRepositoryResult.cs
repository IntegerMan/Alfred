using IronGitHub.Entities;
using MattEland.Ani.Alfred.Core.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MattEland.Ani.Alfred.Search.GitHub
{
    /// <summary>
    /// Represents a GitHub repository found from a search result
    /// </summary>
    internal sealed class GitHubRepositoryResult : ISearchResult
    {
        private readonly Repository.RepositorySearchResults.RepositoryResult _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="GitHubRepositoryResult"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public GitHubRepositoryResult(Repository.RepositorySearchResults.RepositoryResult repository)
        {
            if (repository == null) throw new ArgumentNullException(nameof(repository));

            _repository = repository;
        }

        /// <summary>
        /// Gets the textual description of the search result.
        /// </summary>
        /// <value>
        /// The description of the search result.
        /// </value>
        public string Description
        {
            get
            {
                return _repository.Description;
            }
        }

        /// <summary>
        /// Gets a textual display of the location. Location can vary by the type of search and
        /// could be a physical street address, web URL, file or network path, or even a page
        /// number or other reference code.
        /// </summary>
        /// <value>
        /// The location text.
        /// </value>
        public string LocationText
        {
            get
            {
                return _repository.Url;
            }
        }

        /// <summary>
        /// Gets an action that will provide more details on the search result. What this does
        /// varies by the type of search result.
        /// </summary>
        /// <value>
        /// The get more details action.
        /// </value>
        public Action<ISearchResult> MoreDetailsAction
        {
            get
            {
                return null; // TODO: Allow drilling into these
            }
        }

        /// <summary>
        /// Gets the more details link's text.
        /// </summary>
        /// <value>
        /// The more details text.
        /// </value>
        public string MoreDetailsText
        {
            get
            {
                return "View Repository";
            }
        }

        /// <summary>
        /// Gets the title or heading to use when displaying the search result.
        /// </summary>
        /// <value>
        /// The title of the search result.
        /// </value>
        public string Title
        {
            get
            {
                return _repository.Name;
            }
        }
    }
}
