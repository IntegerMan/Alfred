using MattEland.Ani.Alfred.Core.Subsystems;
using System;
using System.Collections.Generic;
using System.Linq;
using MattEland.Common.Providers;
using MattEland.Ani.Alfred.Core.Definitions;
using JetBrains.Annotations;
using System.Diagnostics.Contracts;
namespace MattEland.Ani.Alfred.Search.GitHub
{
    /// <summary>
    ///     A GitHub search subsystem. This class cannot be inherited.
    /// </summary>
    public sealed class GitHubSearchSubsystem : AlfredSubsystem
    {
        /// <summary>
        ///     The search provider.
        /// </summary>
        [NotNull]
        private readonly GitHubSearchProvider _searchProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="GitHubSearchSubsystem"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public GitHubSearchSubsystem(IObjectContainer container)
            : base(container)
        {
            Contract.Requires(container != null, "container is null.");

            _searchProvider = new GitHubSearchProvider(container);
        }
        /// <summary>
        /// Gets the identifier for the <see cref="IAlfredSubsystem"/> to be used in command routing.
        /// </summary>
        /// <value>The identifier for the subsystem.</value>
        public override string Id
        {
            get
            {
                return "GitHubSearch";
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
                return "GitHub Search Subsystem";
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
                yield return _searchProvider;
            }
        }


    }
}
