using System;
using System.Collections.Generic;

using MattEland.Common.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Common;
using MattEland.Common.Definitions.Search;

namespace MattEland.Ani.Alfred.Core.Widgets
{
    /// <summary>
    ///     A search result widget. This class cannot be inherited.
    /// </summary>
    public sealed class SearchResultWidget : WidgetBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="WidgetBase" /> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <param name="result"> The search result. </param>
        /// <param name="parameters"> The creation parameters. </param>
        public SearchResultWidget([NotNull] ISearchResult result, [NotNull] WidgetCreationParameters parameters) : base(parameters)
        {
            if (result == null) throw new ArgumentNullException(nameof(result));

            // Set the search result most properties feed off of
            SearchResult = result;

            // Update the link widget given the search result
            BuildLinkWidget();
        }

        /// <summary>
        ///     Gets a list of properties provided by this item.
        /// </summary>
        /// <returns>The properties</returns>
        public override IEnumerable<IPropertyItem> Properties
        {
            get
            {
                var properties = base.Properties;
                if (properties != null)
                {
                    foreach (var property in properties)
                    {
                        yield return property;
                    }
                }

                // Render out the properties of this item
                yield return new AlfredProperty("Title", SearchResult.Title);
                yield return new AlfredProperty("Description", SearchResult.Description);
                yield return new AlfredProperty("Location", SearchResult.LocationText);
            }
        }

        /// <summary>
        /// Builds the link widget.
        /// </summary>
        private void BuildLinkWidget()
        {
            if (SearchResult.MoreDetailsAction != null)
            {
                string controlName = "linkResult" + SearchResult.GetHashCode();
                var parameters = new WidgetCreationParameters(controlName, Container);

                var alfredContainer = Container as IAlfredContainer;

                LinkWidget = new LinkWidget(SearchResult.MoreDetailsText, SearchResult.Url, parameters);
                if (alfredContainer != null)
                {
                    LinkWidget.Command = alfredContainer.BuildCommand(SearchResult.MoreDetailsAction);
                }
            }
            else
            {
                LinkWidget = null;
            }
        }

        /// <summary>
        ///     Gets the search result.
        /// </summary>
        /// <value>
        ///     The search result.
        /// </value>
        [NotNull]
        public ISearchResult SearchResult { get; }

        /// <summary>
        /// Gets the link widget.
        /// </summary>
        /// <value>The link widget.</value>
        [CanBeNull]
        public LinkWidget LinkWidget { get; private set; }

        /// <summary>
        ///     Gets the name of the broad categorization or type that this item is.
        /// </summary>
        /// <example>
        ///     Some examples of ItemTypeName values might be "Folder", "Application", "User", etc.
        /// </example>
        /// <value>The item type's name.</value>
        public override string ItemTypeName { get { return "Search Result"; } }

        /// <summary>
        ///     Gets the title.
        /// </summary>
        /// <value>
        ///     The title.
        /// </value>
        public string Title { get { return SearchResult.Title; } }

        /// <summary>
        ///     Gets the description.
        /// </summary>
        /// <value>
        ///     The description.
        /// </value>
        public string Description { get { return SearchResult.Description; } }

        /// <summary>
        ///     Gets the location.
        /// </summary>
        /// <value>
        ///     The location.
        /// </value>
        public string Location
        {
            get
            {
                return SearchResult.LocationText.HasText() ? SearchResult.LocationText : "Unknown Location";
            }
        }
    }
}