using MattEland.Ani.Alfred.Core.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using MattEland.Common.Providers;
using System.Diagnostics.Contracts;

namespace MattEland.Ani.Alfred.Search.StackOverflow
{
    /// <summary>
    ///     A stack overflow search provider. This class cannot be inherited.
    /// </summary>
    public sealed class StackOverflowSearchProvider : ISearchProvider, IHasContainer<IAlfredContainer>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="StackOverflowSearchProvider" /> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <param name="container"> The container. </param>
        public StackOverflowSearchProvider([NotNull] IAlfredContainer container, [CanBeNull] string apiKey)
        {
            //- Validate
            Contract.Requires(container != null);

            Container = container;

            ApiKey = apiKey;
        }

        /// <summary>
        ///     Gets the API key.
        /// </summary>
        /// <value>
        ///     The API key.
        /// </value>
        [CanBeNull]
        public string ApiKey { get; }

        /// <summary>
        ///     Gets the container.
        /// </summary>
        /// <value>
        ///     The container.
        /// </value>
        public IAlfredContainer Container
        {
            get;
        }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Id
        {
            get
            {
                return "StackOverflowSearch";
            }
        }

        /// <summary>
        /// Gets the display name of the search provider.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            get
            {
                return "StackOverflow Search Provider";
            }
        }

        /// <summary>
        /// Executes a search operation and returns a result to track the search.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <returns>
        /// An <see cref="ISearchOperation"/> representing the potentially ongoing search.
        /// </returns>
        public ISearchOperation PerformSearch([NotNull] string searchText)
        {
            return new StackOverflowSearchOperation(Container, searchText, ApiKey);
        }
    }
}
