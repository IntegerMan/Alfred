using System;
using System.Collections.Generic;
using System.Linq;

namespace MattEland.Common.Definitions.Search
{
    /// <summary>
    ///     A search history entry.
    /// </summary>
    public struct SearchHistoryEntry
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="SearchHistoryEntry" /> struct.
        /// </summary>
        /// <param name="text"> The search text. </param>
        public SearchHistoryEntry(string text)
        {
            SearchText = text;
            SearchTimeUtc = DateTime.UtcNow;
        }

        /// <summary>
        ///     Gets or sets the search text.
        /// </summary>
        /// <value>
        ///     The search text.
        /// </value>
        public string SearchText
        {
            get; set;
        }

        /// <summary>
        ///     Gets or sets the Date/Time the search occurred in UTC.
        /// </summary>
        /// <value>
        ///     The search time.
        /// </value>
        public DateTime SearchTimeUtc
        {
            get; set;
        }
    }
}

