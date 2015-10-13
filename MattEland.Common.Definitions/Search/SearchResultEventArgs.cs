using System;

namespace MattEland.Common.Definitions.Search
{
    /// <summary>
    ///     The search result event arguments
    /// </summary>
    public sealed class SearchResultEventArgs : EventArgs
    {
        /// <summary>
        ///     Gets or sets the search result.
        /// </summary>
        /// <value>
        ///     The search result.
        /// </value>
        public ISearchResult Result { get; set; }
    }
}