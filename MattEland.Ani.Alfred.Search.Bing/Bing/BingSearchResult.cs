using MattEland.Ani.Alfred.Core.Definitions;
using System;
using MattEland.Common.Annotations;
using MattEland.Common.Providers;
using System.Diagnostics.Contracts;
namespace MattEland.Ani.Alfred.Search.Bing
{
    /// <summary>
    ///     A Bing search result.
    /// </summary>
    internal class BingSearchResult : SearchResult, IHasContainer<IAlfredContainer>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="BingSearchResult" /> class.
        /// </summary>
        /// <param name="container"> The container. </param>
        /// <param name="result"> The Bing search result. </param>
        public BingSearchResult([NotNull] IAlfredContainer container, [NotNull] WebResult result)
            : base(container, result.Title)
        {
            //- Validation
            Contract.Requires(container != null);
            Contract.Requires(result != null);

            // Set Basic Properties
            Description = result.Description;
            LocationText = result.DisplayUrl;
            Url = result.Url;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="BingSearchResult"/> class.
        /// </summary>
        /// <param name="container"> The container. </param>
        /// <param name="result"> The Bing search result. </param>
        public BingSearchResult([NotNull] IAlfredContainer container, [NotNull] NewsResult result)
            : base(container, result.Title)
        {
            //- Validation
            Contract.Requires(container != null);
            Contract.Requires(result != null);

            // Set Basic Properties
            Description = result.Description;
            LocationText = result.Source;
            Url = result.Url;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="BingSearchResult"/> class.
        /// </summary>
        /// <param name="container"> The container. </param>
        /// <param name="result"> The Bing search result. </param>
        public BingSearchResult([NotNull] IAlfredContainer container, [NotNull] ImageResult result)
            : base(container, result.Title)
        {
            //- Validation
            Contract.Requires(container != null);
            Contract.Requires(result != null);

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
                return "Visit Web Page";
            }
        }
    }

}