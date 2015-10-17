using System;
using System.Collections.Generic;
using System.Linq;
using MattEland.Ani.Alfred.Core.Definitions;
using System.Diagnostics.Contracts;
using MattEland.Common.Annotations;
using MattEland.Ani.Alfred.Core.Widgets;
using MattEland.Common.Definitions.Search;
using MattEland.Presentation.Logical.Widgets;

namespace MattEland.Ani.Alfred.Core.Pages
{
    /// <summary>
    ///     The search history page. This class cannot be inherited.
    /// </summary>
    public sealed class SearchHistoryPage : AlfredPage
    {
        /// <summary>
        ///     The no items label widget.
        /// </summary>
        [NotNull]
        private readonly TextWidget _noItemsLabel;

        /// <summary>
        ///     Identifier for the page. Useful for testing purposes.
        /// </summary>
        public const string PageId = "SearchHistory";

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
            : base(container, "Search History", PageId)
        {
            //- Validate
            Contract.Requires(container != null, "container is null.");
            Contract.Requires(searchController != null, "searchController is null.");

            // Create History Collection
            _historyItems = container.ProvideCollection<IWidget>();
            _historyEntries = container.ProvideCollection<SearchHistoryEntry>();

            // Create widgets
            _noItemsLabel = new TextWidget("No Searches have been made", BuildWidgetParameters("lblNoItems"));
            _historyList = new Repeater(BuildWidgetParameters("listHistory"))
            {
                ItemsSource = _historyItems
            };

            // Set the search controller we'll act against
            SearchController = searchController;
        }

        /// <summary>
        /// Updates the component
        /// </summary>
        protected override void UpdateProtected()
        {
            base.UpdateProtected();

            // Add history entries to the collection
            foreach (var entry in SearchController.SearchHistory)
            {
                if (!_historyEntries.Contains(entry))
                {
                    _historyEntries.Add(entry);

                    // Build a widget for this entry
                    int index = _historyItems.Count + 1;
                    var parameters = BuildWidgetParameters(string.Format("searchHistory{0}", index));
                    DateTime entryTime = entry.SearchTimeUtc.ToLocalTime();
                    string text = string.Format("{0}: {1}", entryTime, entry.SearchText);

                    var widget = new TextWidget(text, parameters);

                    // Add the widget to our list of widgets
                    _historyItems.Add(widget);
                }
            }

            // Update the visibility of the no items label
            _noItemsLabel.IsVisible = !_historyItems.Any();
        }

        /// <summary>
        ///     List of search history entries.
        /// </summary>
        [NotNull]
        private readonly Repeater _historyList;

        /// <summary>
        ///     The search history entry collection.
        /// </summary>
        [NotNull, ItemNotNull]
        private readonly ICollection<SearchHistoryEntry> _historyEntries;

        /// <summary>
        ///     The history items.
        /// </summary>
        [NotNull, ItemNotNull]
        private readonly ICollection<IWidget> _historyItems;

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
                yield return _noItemsLabel;
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
