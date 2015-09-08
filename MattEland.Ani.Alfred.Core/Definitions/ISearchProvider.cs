// ---------------------------------------------------------
// ISearchProvider.cs
// 
// Created on:      09/02/2015 at 5:50 PM
// Last Modified:   09/02/2015 at 6:04 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;

using JetBrains.Annotations;

namespace MattEland.Ani.Alfred.Core.Definitions
{
    /// <summary>
    ///     Defines an <see langword="object"/> capable of providing search results
    /// </summary>
    public interface ISearchProvider
    {
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