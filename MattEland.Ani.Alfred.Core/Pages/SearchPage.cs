// ---------------------------------------------------------
// SearchPage.cs
// 
// Created on:      09/13/2015 at 12:04 PM
// Last Modified:   09/13/2015 at 3:57 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;

using MattEland.Common.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Modules;

using System.Linq;

namespace MattEland.Ani.Alfred.Core.Pages
{
    /// <summary>
    ///     A search page that allows the user to search many aspects of Alfred and view search
    ///     results.
    /// </summary>
    public sealed class SearchPage : ModulePageBase
    {
        /// <summary>
        ///     The search module.
        /// </summary>
        [CanBeNull]
        private SearchModule _searchModule;

        /// <summary>
        ///     The search results module.
        /// </summary>
        [NotNull]
        private SearchResultsModule _searchResultsModule;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AlfredPage" /> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public SearchPage([NotNull] IAlfredContainer container) : this(container, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AlfredPage" /> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="includeSearchModule">Whether or not to include the search module.</param>
        public SearchPage([NotNull] IAlfredContainer container, bool includeSearchModule)
            : base(container, "Search", "SearchResults")
        {
            if (includeSearchModule)
            {
                _searchModule = new SearchModule(container);
            }

            _searchResultsModule = new SearchResultsModule(container);
        }

        /// <summary>
        ///     Gets the modules.
        /// </summary>
        /// <value>
        /// The modules.
        /// </value>
        public override IEnumerable<IAlfredModule> Modules
        {
            get
            {
                // Only include search module if this instance supports it
                if (_searchModule != null) yield return _searchModule;

                yield return _searchResultsModule;
            }
        }
    }
}