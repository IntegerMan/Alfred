using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Subsystems;
using MattEland.Common.Providers;
using System.Diagnostics.CodeAnalysis;

namespace MattEland.Ani.Alfred.Search.Bing
{
    /// <summary>
    ///     A subsystem designed to provide Bing search capabilities.
    /// </summary>
    [SuppressMessage("CodeRush", "Can implement base type constructors")]
    public sealed class BingSearchSubsystem : AlfredSubsystem
    {
        /// <summary>
        ///     The Bing search provider.
        /// </summary>
        [NotNull]
        private BingSearchProvider _bingSearchProvider;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AlfredSubsystem" /> class.
        /// </summary>
        /// <param name="container"> The container. </param>
        /// <param name="bingApiKey"> The bing API key. </param>
        public BingSearchSubsystem([NotNull] IObjectContainer container, [NotNull] string bingApiKey)
            : base(container)
        {
            _bingSearchProvider = new BingSearchProvider(container, bingApiKey);
        }

        /// <summary>
        ///     Gets the search providers.
        /// </summary>
        /// <value>
        ///     The search providers.
        /// </value>
        public override IEnumerable<ISearchProvider> SearchProviders
        {
            get { yield return _bingSearchProvider; }
        }

        /// <summary>
        ///     Gets the name of the component.
        /// </summary>
        /// <value>The name of the component.</value>
        public override string Name { get { return "Bing Search Subsystem"; } }

        /// <summary>
        ///     Gets the identifier for the <see cref="IAlfredSubsystem"/> to be used in command routing.
        /// </summary>
        /// <value>
        ///     The identifier for the subsystem.
        /// </value>
        public override string Id
        {
            get { return "Bing"; }
        }
    }
}
