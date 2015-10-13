// ---------------------------------------------------------
// ISearchProvider.cs
// 
// Created on:      09/02/2015 at 5:50 PM
// Last Modified:   09/02/2015 at 6:04 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;

using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Common.Annotations;

namespace MattEland.Common.Definitions.Search
{
    /// <summary>
    ///     Defines an <see langword="object"/> capable of providing search results
    /// </summary>
    public interface ISearchProvider : IHasIdentifier
    {
        /// <summary>
        ///     Gets the display name of the search provider.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        [NotNull]
        string Name { get; }

        /// <summary>
        ///     Executes a search operation and returns a result to track the search.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <returns>
        ///     An <see cref="ISearchOperation" /> representing the potentially ongoing search.
        /// </returns>
        [NotNull]
        ISearchOperation PerformSearch([NotNull] string searchText);
    }

}