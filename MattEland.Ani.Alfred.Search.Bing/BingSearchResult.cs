using MattEland.Ani.Alfred.Core.Definitions;
using System;
using JetBrains.Annotations;
using MattEland.Common.Providers;

namespace MattEland.Ani.Alfred.Search.Bing
{
    /// <summary>
    /// A Bing search result
    /// </summary>
    internal class BingSearchResult : SearchResult, IHasContainer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BingSearchResult" /> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="result">The Bing search result.</param>
        public BingSearchResult([NotNull] IObjectContainer container, [NotNull] WebResult result)
            : base(container, result.Title)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));
            if (result == null) throw new ArgumentNullException(nameof(result));

            // Set Basic Properties
            Description = result.Description;
            LocationText = result.DisplayUrl;
            Url = result.Url;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="BingSearchResult"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="result"> The Bing search result. </param>
        public BingSearchResult([NotNull] IObjectContainer container, [NotNull] NewsResult result)
            : base(container, result.Title)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));
            if (result == null) throw new ArgumentNullException(nameof(result));

            // Set Basic Properties
            Description = result.Description;
            LocationText = result.Source;
            Url = result.Url;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="BingSearchResult"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="result"> The Bing search result. </param>
        public BingSearchResult([NotNull] IObjectContainer container, [NotNull] ImageResult result)
            : base(container, result.Title)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));
            if (result == null) throw new ArgumentNullException(nameof(result));

            // Set Basic Properties
            Description = result.ContentType;
            LocationText = result.DisplayUrl;
            Url = result.SourceUrl;
        }

        /// <summary>
        ///     Gets the more details text.
        /// </summary>
        /// <value>
        ///     The more details text.
        /// </value>
        public override string MoreDetailsText
        {
            get
            {
                return Url;
            }
        }
    }

}