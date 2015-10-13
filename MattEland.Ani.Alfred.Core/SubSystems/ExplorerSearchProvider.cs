// ---------------------------------------------------------
// ExplorerSearchProvider.cs
// 
// Created on:      09/10/2015 at 9:50 PM
// Last Modified:   09/10/2015 at 9:50 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------


using MattEland.Common.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Common.Definitions.Search;
using MattEland.Common.Providers;

namespace MattEland.Ani.Alfred.Core.Subsystems
{
    /// <summary>
    ///     A search provider for the mind explorer. This class cannot be inherited.
    /// </summary>
    public sealed class ExplorerSearchProvider : ISearchProvider, IHasContainer<IAlfredContainer>
    {
        /// <summary>
        ///     The mind explorer subsystem.
        /// </summary>
        [NotNull]
        private readonly MindExplorerSubsystem _mindExplorerSubsystem;

        /// <summary>
        ///     Initializes a new instance of the
        ///     ExplorerSearchProvider class.
        /// </summary>
        /// <param name="container"> The container. </param>
        /// <param name="mindExplorerSubsystem"> The mind explorer subsystem. </param>
        internal ExplorerSearchProvider(
            [NotNull] IAlfredContainer container,
            [NotNull] MindExplorerSubsystem mindExplorerSubsystem)
        {
            Container = container;
            _mindExplorerSubsystem = mindExplorerSubsystem;
        }

        /// <summary>
        ///     Gets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Id
        {
            get { return "ExplorerSearchProvider"; }
        }

        /// <summary>
        ///     Gets the display name of the search provider.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public string Name
        {
            get { return "Explorer Search Provider"; }
        }

        /// <summary>
        ///     Executes a search operation and returns a result to track the search.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <returns>
        ///     An <see cref="ISearchOperation" /> representing the potentially ongoing search.
        /// </returns>
        [NotNull]
        public ISearchOperation PerformSearch([NotNull] string searchText)
        {
            var operation = new ExplorerSearchOperation(Container, _mindExplorerSubsystem, searchText);

            return operation;
        }

        /// <summary>
        ///     Gets the container.
        /// </summary>
        /// <value>
        ///     The container.
        /// </value>
        [NotNull]
        public IAlfredContainer Container { get; }
    }

}