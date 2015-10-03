using IronGitHub.Entities;
using JetBrains.Annotations;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Common;
using MattEland.Common.Providers;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace MattEland.Ani.Alfred.Search.GitHub
{
    /// <summary>
    /// Represents a GitHub repository found from a search result
    /// </summary>
    internal sealed class GitHubRepositoryResult : SearchResult
    {
        private readonly Repository.RepositorySearchResults.RepositoryResult _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="GitHubRepositoryResult" /> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="repository">The repository.</param>
        /// <exception cref="System.ArgumentNullException">
        /// </exception>
        public GitHubRepositoryResult([NotNull] IAlfredContainer container,
            Repository.RepositorySearchResults.RepositoryResult repository) : base(container, repository.Name)
        {
            //- Validation
            Contract.Requires<ArgumentNullException>(container != null);
            Contract.Requires<ArgumentNullException>(repository != null);

            _repository = repository;

            Description = repository.Description;
            LocationText = repository.Url;
            Url = repository.Url;
        }

        /// <summary>
        /// Gets the more details link's text.
        /// </summary>
        /// <value>
        /// The more details text.
        /// </value>
        public override string MoreDetailsText
        {
            get
            {
                return "View Repository";
            }
        }

    }
}
