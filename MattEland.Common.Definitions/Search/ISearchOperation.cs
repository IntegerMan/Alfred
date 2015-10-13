// ---------------------------------------------------------
// ISearchOperation.cs
// 
// Created on:      09/02/2015 at 6:04 PM
// Last Modified:   09/02/2015 at 6:04 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;

using MattEland.Common.Annotations;

namespace MattEland.Common.Definitions.Search
{
    /// <summary>
    ///     Interface for an ongoing search operation
    /// </summary>
    public interface ISearchOperation
    {
        /// <summary>
        ///     Gets a value indicating whether the search has completed yet. This is useful for slow or
        ///     asynchronous search operations.
        /// </summary>
        /// <value>
        ///     <see langword="true" /> if the search is complete, otherwise <see langword="false" /> .
        /// </value>
        bool IsSearchComplete { get; }

        /// <summary>
        ///     Gets a value indicating whether the operation encountered an error.
        /// </summary>
        /// <value>
        ///     <see langword="true" /> if an error was encountered, <see langword="false" /> if not.
        /// </value>
        bool EncounteredError { get; }

        /// <summary>
        ///     Gets a user-facing message describing the error if an error occurred retrieving search
        ///     results.
        /// </summary>
        /// <value>
        ///     A message describing the error.
        /// </value>
        string ErrorMessage { get; }

        /// <summary>
        ///     Gets the results of the search operation.
        /// </summary>
        /// <value>
        ///     The results.
        /// </value>
        [NotNull, ItemNotNull]
        IEnumerable<ISearchResult> Results { get; }

        /// <summary>
        ///     Updates the search operation, adding results to the <see cref="Results"/> collection and
        ///     updating <see cref="IsSearchComplete"/> based on the state of the search operation.
        /// </summary>
        void Update();

        /// <summary>
        ///     Aborts the search.
        ///     This is called when the user cancels a search or when a search times out.
        /// </summary>
        void Abort();
    }

}