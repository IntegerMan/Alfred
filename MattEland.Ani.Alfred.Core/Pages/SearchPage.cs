// ---------------------------------------------------------
// SearchPage.cs
// 
// Created on:      09/13/2015 at 12:04 PM
// Last Modified:   09/13/2015 at 3:57 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Modules;
using MattEland.Common.Providers;

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
        [NotNull]
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
        public SearchPage([NotNull] IObjectContainer container)
            : base(container, "Search", "SearchPage")
        {
            _searchModule = new SearchModule(container);
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
                yield return _searchModule;
                yield return _searchResultsModule;
            }
        }
    }
}