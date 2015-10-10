using System;
using System.Collections.Generic;

using MattEland.Common.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Common.Providers;
using MattEland.Common;
using System.Diagnostics.Contracts;

namespace MattEland.Ani.Alfred.Search.Bing
{
    /// <summary>
    ///     A search provider that looks for queries via Bing.
    /// </summary>
    [PublicAPI]
    public sealed class BingSearchProvider : ISearchProvider, IHasContainer<IAlfredContainer>
    {
        /// <summary>
        ///     The Bing API key.
        /// </summary>
        [NotNull]
        private readonly string _bingApiKey;

        /// <summary>
        ///     Initializes a new instance of the <see cref="BingSearchProvider"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="container"/> is null.
        /// </exception>
        /// <param name="container"> The container. </param>
        /// <param name="bingApiKey"> The Bing API key. </param>
        public BingSearchProvider([NotNull] IAlfredContainer container, [NotNull] string bingApiKey)
        {
            //- Validation
            Contract.Requires(container != null);
            Contract.Requires(bingApiKey.HasText());

            Container = container;

            _bingApiKey = bingApiKey;
        }

        /// <summary>
        ///     Gets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Id
        {
            get { return "BingSearch"; }
        }

        /// <summary>
        ///     Gets the display name of the search provider.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public string Name
        {
            get { return "Bing Search Provider"; }
        }

        /// <summary>
        ///     Executes a search operation and returns a result to track the search.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <returns>
        ///     An <see cref="ISearchOperation" /> representing the potentially ongoing search.
        /// </returns>
        public ISearchOperation PerformSearch(string searchText)
        {
            return new BingSearchOperation(Container, searchText, _bingApiKey);
        }

        /// <summary>
        ///     Gets the container.
        /// </summary>
        /// <value>
        ///     The container.
        /// </value>
        [NotNull]
        public IAlfredContainer Container { get; }
    }

}