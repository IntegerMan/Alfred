// ---------------------------------------------------------
// ISearchResult.cs
// 
// Created on:      09/02/2015 at 6:02 PM
// Last Modified:   09/02/2015 at 6:03 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;

using MattEland.Common.Annotations;

namespace MattEland.Common.Definitions.Search
{
    /// <summary>
    ///     Interface for a search result.
    /// </summary>
    public interface ISearchResult
    {
        /// <summary>
        /// Gets the URL of the item, if any.
        /// </summary>
        /// <value>The URL.</value>
        string Url { get; }

        /// <summary>
        ///     Gets the title or heading to use when displaying the search result.
        /// </summary>
        /// <value>
        /// The title of the search result.
        /// </value>
        [NotNull]
        string Title { get; }

        /// <summary>
        ///     Gets the textual description of the search result.
        /// </summary>
        /// <value>
        /// The description of the search result.
        /// </value>
        [NotNull]
        string Description { get; }

        /// <summary>
        ///     Gets a textual display of the location. Location can vary by the type of search and
        ///     could be a physical street address, web URL, file or network path, or even a page
        ///     number or other reference code.
        /// </summary>
        /// <value>
        /// The location text.
        /// </value>
        [NotNull]
        string LocationText { get; }

        /// <summary>
        ///     Gets an action that will provide more details on the search result. What this does
        ///     varies by the type of search result.
        /// </summary>
        /// <value>
        /// The get more details action.
        /// </value>
        [CanBeNull]
        Action MoreDetailsAction { get; }

        /// <summary>
        ///     Gets the more details link's text.
        /// </summary>
        /// <value>
        ///     The more details text.
        /// </value>
        [CanBeNull]
        string MoreDetailsText { get; }
    }
}