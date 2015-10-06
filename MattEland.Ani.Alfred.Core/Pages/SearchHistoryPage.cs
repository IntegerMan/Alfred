/// <param name="searchController"></param>
using System;
using System.Collections.Generic;
using System.Linq;
using MattEland.Ani.Alfred.Core.Definitions;
using System.Diagnostics.Contracts;
using JetBrains.Annotations;
using MattEland.Ani.Alfred.Core.Widgets;

namespace MattEland.Ani.Alfred.Core.Pages
{
    /// <summary>
    ///     The search history page. This class cannot be inherited.
    /// </summary>
    public sealed class SearchHistoryPage : AlfredPage
    {
        /// <summary>
        ///     Gets the search controller.
        /// </summary>
        /// <value>
        ///     The search controller.
        /// </value>
        [NotNull]
        public ISearchController SearchController { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComponentBase" /> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public SearchHistoryPage([NotNull] IAlfredContainer container)
            : this(container, container.SearchController)
        {

        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SearchHistoryPage"/> class.
        /// </summary>
        /// <param name="container"> The container. </param>
        /// <param name="searchController"> The search controller. </param>
        public SearchHistoryPage([NotNull] IAlfredContainer container, [NotNull] ISearchController searchController)
            : base(container, "Search History", "SearchHistory")
        {
            //- Validate
            Contract.Requires(container != null, "container is null.");
            Contract.Requires(searchController != null, "searchController is null.");

            // Create widgets
            _historyList = new Repeater(BuildWidgetParameters("listHistory"));

            // Set the search controller we'll act against
            SearchController = searchController;
        }



        /// <summary>
        ///     List of search history entries.
        /// </summary>
        [NotNull]
        private readonly Repeater _historyList;

        /// <summary>
        /// Gets the widgets collection.
        /// </summary>
        /// <value>
        /// The widgets.
        /// </value>
        [NotNull]
        [ItemNotNull]
        public IEnumerable<IWidget> Widgets
        {
            get
            {
                yield return _historyList;
            }
        }

        /// <summary>
        /// Gets the children of the component. Depending on the type of component this is, the children
        /// will vary in their own types.
        /// </summary>
        /// <value>The children.</value>
        public override IEnumerable<IAlfredComponent> Children
        {
            get
            {
                yield break;
            }
        }
    }
}
