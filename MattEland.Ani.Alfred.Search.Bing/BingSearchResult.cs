using MattEland.Ani.Alfred.Core.Definitions;
using System.Diagnostics.Contracts;
using System;

namespace MattEland.Ani.Alfred.Search.Bing
{
    internal class BingSearchResult : SearchResult
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="BingSearchResult"/> class.
        /// </summary>
        /// <param name="result"> The Bing search result. </param>
        public BingSearchResult(WebResult result) : base(result.Title)
        {
            Contract.Requires(result != null, "result is null.");

            // Set the core property everything else feeds from
            WebSearchResult = result;

            // Set Basic Properties
            Description = WebSearchResult.Description;
            LocationText = WebSearchResult.DisplayUrl;
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

        /// <summary>
        ///     Gets the web search result.
        /// </summary>
        /// <value>
        ///     The web search result.
        /// </value>
        public WebResult WebSearchResult
        {
            get;
        }
    }
}