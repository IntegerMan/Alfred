﻿using MattEland.Ani.Alfred.Core.Subsystems;
using System;
using System.Collections.Generic;
using System.Linq;
using MattEland.Common.Providers;
using MattEland.Ani.Alfred.Core.Definitions;
using JetBrains.Annotations;
using MattEland.Ani.Alfred.Search.GitHub;
using MattEland.Ani.Alfred.Search.Bing;

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

        /// <summary>
        ///     Initializes a new instance of the <see cref="GitHubSearchSubsystem"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <param name="container"> The container. </param>
        /// <param name="bingApiKey"> The Bing API key. </param>
        public SearchSubsystem([NotNull] IObjectContainer container, [NotNull] string bingApiKey)
            : base(container)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));

            _gitHubSearchProvider = new GitHubSearchProvider(container);
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
                yield return _bingSearchProvider;
            }
        }

    }
}
