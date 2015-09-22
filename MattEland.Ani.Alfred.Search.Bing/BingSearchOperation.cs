using System;
using System.Collections.Generic;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Common.Providers;

namespace MattEland.Ani.Alfred.Search.Bing
{
    /// <summary>
    ///     A bing search operation. This class cannot be inherited.
    /// </summary>
    public sealed class BingSearchOperation : ISearchOperation
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="BingSearchOperation" />
        ///     class.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="container"/> is null.
        /// </exception>
        /// <param name="container"> The container. </param>
        public BingSearchOperation([NotNull] IObjectContainer container)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));

            _results = container.ProvideCollection<ISearchResult>();

            IsSearchComplete = false;
            EncounteredError = false;
            ErrorMessage = null;
        }

        /// <summary>
        ///     Gets a value indicating whether the search has completed yet. This is useful for slow or
        ///     asynchronous search operations.
        /// </summary>
        /// <value>
        ///     <see langword="true" /> if the search is complete, otherwise <see langword="false" /> .
        /// </value>
        public bool IsSearchComplete { get; private set; }

        /// <summary>
        ///     Gets a value indicating whether the operation encountered an error.
        /// </summary>
        /// <value>
        ///     <see langword="true" /> if an error was encountered, <see langword="false" /> if not.
        /// </value>
        public bool EncounteredError { get; private set; }

        /// <summary>
        ///     Gets a user-facing message describing the error if an error occurred retrieving search
        ///     results.
        /// </summary>
        /// <value>
        ///     A message describing the error.
        /// </value>
        public string ErrorMessage { get; }

        /// <summary>
        ///     The search results.
        /// </summary>
        [NotNull, ItemNotNull]
        private readonly ICollection<ISearchResult> _results;

        /// <summary>
        ///     Gets the results of the search operation.
        /// </summary>
        /// <value>
        ///     The results.
        /// </value>
        public IEnumerable<ISearchResult> Results
        {
            get { return _results; }
        }

        /// <summary>
        ///     Updates the search operation, adding results to the
        ///     <see cref="ISearchOperation.Results"/> collection and updating
        ///     <see cref="ISearchOperation.IsSearchComplete"/> based on the state of the search
        ///     operation.
        /// </summary>
        public void Update()
        {
            // TODO: Update the search

            IsSearchComplete = true;
        }

        /// <summary>
        ///     Aborts the search.
        ///     This is called when the user cancels a search or when a search times out.
        /// </summary>
        public void Abort()
        {
            // TODO: Abort the search
        }
    }
}