using MattEland.Ani.Alfred.Core.Subsystems;
using System;
using System.Collections.Generic;
using System.Linq;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Common.Annotations;
using MattEland.Ani.Alfred.Search.GitHub;
using MattEland.Ani.Alfred.Search.Bing;
using MattEland.Ani.Alfred.Search.StackOverflow;
using System.Diagnostics.Contracts;

namespace MattEland.Ani.Alfred.Search
{
    /// <summary>
    ///     A search subsystem containing multiple search providers. This class cannot be inherited.
    /// </summary>
    public sealed class SearchSubsystem : AlfredSubsystem
    {
        /// <summary>
        ///     The GitHub search provider.
        /// </summary>
        [NotNull]
        private readonly GitHubSearchProvider _gitHubSearchProvider;

        /// <summary>
        ///     The Bing search provider.
        /// </summary>
        [NotNull]
        private readonly BingSearchProvider _bingSearchProvider;

        [NotNull]
        private readonly StackOverflowSearchProvider _stackOverflowSearchProvider;

        /// <summary>
        ///     Initializes a new instance of the <see cref="GitHubSearchSubsystem"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <param name="container"> The container. </param>
        /// <param name="bingApiKey"> The Bing API key. </param>
        /// <param name="stackOverflowApiKey"> The stack overflow API key. </param>
        public SearchSubsystem([NotNull] IAlfredContainer container,
            [NotNull] string bingApiKey,
            [CanBeNull] string stackOverflowApiKey)
            : base(container)
        {
            //- Validation
            Contract.Requires(container != null);

            _gitHubSearchProvider = new GitHubSearchProvider(container);
            _stackOverflowSearchProvider = new StackOverflowSearchProvider(container, stackOverflowApiKey);
            _bingSearchProvider = new BingSearchProvider(container, bingApiKey);
        }
        /// <summary>
        /// Gets the identifier for the <see cref="IAlfredSubsystem"/> to be used in command routing.
        /// </summary>
        /// <value>The identifier for the subsystem.</value>
        public override string Id
        {
            get
            {
                return "Search";
            }
        }

        /// <summary>
        /// Gets the name of the component.
        /// </summary>
        /// <value>The name of the component.</value>
        public override string Name
        {
            get
            {
                return "Search Subsystem";
            }
        }

        /// <summary>
        ///     Gets the search providers.
        /// </summary>
        /// <value>
        ///     The search providers.
        /// </value>
        [NotNull, ItemNotNull]
        public override IEnumerable<ISearchProvider> SearchProviders
        {
            get
            {
                yield return _gitHubSearchProvider;
                yield return _stackOverflowSearchProvider;
                yield return _bingSearchProvider;
            }
        }

    }
}
