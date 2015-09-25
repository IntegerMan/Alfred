using MattEland.Ani.Alfred.Core.Definitions;
using System.Diagnostics.Contracts;
using System;
using JetBrains.Annotations;

namespace MattEland.Ani.Alfred.Search.Bing
{
    internal class BingSearchResult : SearchResult
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="BingSearchResult"/> class.
        /// </summary>
        /// <param name="result"> The Bing search result. </param>
        public BingSearchResult([NotNull] WebResult result) : base(result.Title)
        {
            Contract.Requires(result != null, "result is null.");

            // Set Basic Properties
            Description = result.Description;
            LocationText = result.DisplayUrl;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="BingSearchResult"/> class.
        /// </summary>
        /// <param name="result"> The Bing search result. </param>
        public BingSearchResult([NotNull] NewsResult result) : base(result.Title)
        {
            Contract.Requires(result != null, "result is null.");

            // Set Basic Properties
            Description = result.Description;
            LocationText = result.Source; // TODO: Maybe Url?
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="BingSearchResult"/> class.
        /// </summary>
        /// <param name="result"> The Bing search result. </param>
        public BingSearchResult([NotNull] ImageResult result) : base(result.Title)
        {
            Contract.Requires(result != null, "result is null.");

            // Set Basic Properties
            Description = result.ContentType;
            LocationText = result.DisplayUrl;
        }

        /// <summary>
        ///     Gets the action that is executed when a user wants more information.
        /// </summary>
        /// <value>
        ///     The more details action.
        /// </value>
        public override Action<ISearchResult> MoreDetailsAction
        {
            get
            {
                // TODO: Actually do something

                return base.MoreDetailsAction;
            }
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
                return "Open Web Page";
            }
        }
    }
}