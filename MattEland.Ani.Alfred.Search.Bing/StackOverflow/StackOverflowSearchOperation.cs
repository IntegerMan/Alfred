using System;
using System.Collections.Generic;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Common.Providers;
using JetBrains.Annotations;
using MattEland.Common;

namespace MattEland.Ani.Alfred.Search.StackOverflow
{
    /// <summary>
    ///     A stack overflow search operation. This class cannot be inherited.
    /// </summary>
    internal sealed class StackOverflowSearchOperation : ISearchOperation, IHasContainer
    {
        /// <summary>
        ///     The search text.
        /// </summary>
        [NotNull]
        private readonly string _searchText;

        /// <summary>
        ///     Initializes a new instance of the <see cref="StackOverflowSearchOperation"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <param name="container"> The container. </param>
        /// <param name="searchText"> The search text. </param>
        public StackOverflowSearchOperation([NotNull] IObjectContainer container, [NotNull] string searchText)
        {
            //- Validate
            if (container == null) throw new ArgumentNullException(nameof(container));
            if (searchText.IsEmpty()) throw new ArgumentNullException(nameof(Search));

            // Set properties
            Container = container;
            _searchText = searchText;

            // Create nested collections
            _results = container.ProvideCollection<ISearchResult>();
        }

        /// <summary>
        /// Gets the container.
        /// </summary>
        /// <value>
        /// The container.
        /// </value>
        [NotNull]
        public IObjectContainer Container
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether the operation encountered an error.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if an error was encountered, <see langword="false"/> if not.
        /// </value>
        public bool EncounteredError
        {
            get; private set;
        }

        /// <summary>
        /// Gets a user-facing message describing the error if an error occurred retrieving search
        /// results.
        /// </summary>
        /// <value>
        /// A message describing the error.
        /// </value>
        public string ErrorMessage
        {
            get; private set;
        }

        /// <summary>
        /// Gets a value indicating whether the search has completed yet. This is useful for slow or
        /// asynchronous search operations.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the search is complete, otherwise <see langword="false"/> .
        /// </value>
        public bool IsSearchComplete
        {
            get; private set;
        }

        /// <summary>
        ///     The results collection. This can be modified to add new results.
        /// </summary>
        [NotNull, ItemNotNull]
        private ICollection<ISearchResult> _results;

        /// <summary>
        /// Gets the results of the search operation.
        /// </summary>
        /// <value>
        /// The results.
        /// </value>
        [NotNull, ItemNotNull]
        public IEnumerable<ISearchResult> Results
        {
            get
            {
                return _results;
            }
        }

        /// <summary>
        /// Aborts the search.
        /// This is called when the user cancels a search or when a search times out.
        /// </summary>
        public void Abort()
        {
            IsSearchComplete = true;
        }

        /// <summary>
        /// Updates the search operation, adding results to the <see cref="Results"/> collection and
        /// updating <see cref="IsSearchComplete"/> based on the state of the search operation.
        /// </summary>
        public void Update()
        {
            if (IsSearchComplete) return;

            // TODO: Complete this
            IsSearchComplete = true;
            EncounteredError = true;
            ErrorMessage = "StackOverflow searching is not implemented yet.";
        }
    }
}