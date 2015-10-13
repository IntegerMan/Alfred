using System;

using MattEland.Common.Annotations;
using MattEland.Common.Providers;

namespace MattEland.Common.Definitions.Search
{
    /// <summary>
    ///     A basic search result intended to provide simple search functionality.
    /// </summary>
    public abstract class SearchResult : ISearchResult
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="SearchResult"/> class.
        /// </summary>
        /// <param name="container"> The container. </param>
        /// <param name="title"> The title. </param>
        public SearchResult(IObjectContainer container, string title)
        {
            Container = container;
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
        /// <summary>
        /// Gets the URL.
        /// </summary>
        /// <value>The URL.</value>
        public virtual string Url
        {
            get; protected set;
        }

        /// <summary>
        /// Builds the more details action.
        /// </summary>
        /// <returns>An action</returns>
        [CanBeNull]
        protected abstract Action BuildMoreDetailsAction();

        /// <summary>
        /// The action to take on requesting more details
        /// </summary>
        [CanBeNull]
        protected Action Action = null;

        /// <summary>
        ///     Gets the action that is executed when a user wants more information.
        /// </summary>
        /// <value>
        ///     The more details action.
        /// </value>
        public virtual Action MoreDetailsAction
        {
            get
            {
                // Lazy load the action
                if (Action == null)
                {
                    Action = BuildMoreDetailsAction();
                }

                return Action;
            }
        }

        /// <summary>
        /// Gets the container.
        /// </summary>
        /// <value>The container.</value>
        public IObjectContainer Container { get; protected set; }
    }
}

