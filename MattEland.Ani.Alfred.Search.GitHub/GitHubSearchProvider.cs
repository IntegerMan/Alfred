using JetBrains.Annotations;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Common.Providers;
using System;
using System.Diagnostics.Contracts;
namespace MattEland.Ani.Alfred.Search.GitHub
{
    /// <summary>
    ///     A GitHub search provider.
    /// </summary>
    public class GitHubSearchProvider : ISearchProvider, IHasContainer
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="GitHubSearchProvider"/> class.
        /// </summary>
        /// <param name="container"> The container. </param>
        public GitHubSearchProvider([NotNull] IObjectContainer container)
        {
            //- Validation
            Contract.Requires(container != null, "container is null.");

            // Set Properties from parameters
            Container = container;
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
        ///     Gets the identifier.
        /// </summary>
        /// <value>
        ///     The identifier.
        /// </value>
        public string Id
        {
            get
            {
                return "GitHub Search Provider";
            }
        }

        /// <summary>
        ///     Gets the display name of the search provider.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public string Name
        {
            get
            {
                return "GitHub Search Provider";
            }
        }

        /// <summary>
        ///     Executes a search operation and returns a result to track the search.
        /// </summary>
        /// <param name="searchText"> The search text. </param>
        /// <returns>
        ///     An <see cref="ISearchOperation"/> representing the potentially ongoing search.
        /// </returns>
        public ISearchOperation PerformSearch([NotNull] string searchText)
        {
            return new GitHubSearchOperation(Container);
        }
    }
}