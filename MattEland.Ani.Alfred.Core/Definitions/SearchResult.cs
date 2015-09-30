using System;

namespace MattEland.Ani.Alfred.Core.Definitions
{
    /// <summary>
    ///     A basic search result intended to provide simple search functionality.
    /// </summary>
    public class SearchResult : ISearchResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchResult"/> class.
        /// </summary>
        /// <param name="title">The title.</param>
        public SearchResult(string title)
        {
            Title = title;
        }

        /// <summary>
        ///     Gets the textual description of the search result.
        /// </summary>
        /// <value>
        ///     The description of the search result.
        /// </value>
        public virtual string Description
        {
            get;
            protected set;
        }

        /// <summary>
        ///     Gets a textual display of the location. Location can vary by the type of search and could
        ///     be a physical street address, web URL, file or network path, or even a page number or
        ///     other reference code.
        /// </summary>
        /// <value>
        ///     The location text.
        /// </value>
        public virtual string LocationText
        {
            get;
            protected set;
        }

        /// <summary>
        ///     Gets an action that will provide more details on the search result. What this does varies
        ///     by the type of search result.
        /// </summary>
        /// <value>
        ///     The get more details action.
        /// </value>
        public virtual Action MoreDetailsAction
        {
            get;
        }

        /// <summary>
        ///     Gets the more details link's text.
        /// </summary>
        /// <value>
        ///     The more details text.
        /// </value>
        public virtual string MoreDetailsText
        {
            get;
            protected set;
        }

        /// <summary>
        ///     Gets the title or heading to use when displaying the search result.
        /// </summary>
        /// <value>
        ///     The title of the search result.
        /// </value>
        public virtual string Title
        {
            get;
        }
    }
}