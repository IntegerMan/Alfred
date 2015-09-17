// ---------------------------------------------------------
// ExplorerSearchProviderTests.cs
// 
// Created on:      09/10/2015 at 9:46 PM
// Last Modified:   09/11/2015 at 10:43 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Linq;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Subsystems;
using MattEland.Testing;

using NUnit.Framework;

using Shouldly;

namespace MattEland.Ani.Alfred.Tests.Search
{
    /// <summary>
    ///     Mind Explorer search unit tests.
    /// </summary>
    [UnitTestProvider]
    [SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
    public sealed class ExplorerSearchProviderTests : AlfredTestBase
    {
        /// <summary>
        ///     The search text.
        /// </summary>
        private const string SearchText = "Oh where is my hairbrush?";

        /// <summary>
        ///     Gets or sets the Mind Explorer subsystem.
        /// </summary>
        /// <value>
        ///     The subsystem.
        /// </value>
        [NotNull]
        private MindExplorerSubsystem Subsystem { get; set; }

        /// <summary>
        ///     Gets or sets the Mind Explorer search provider.
        /// </summary>
        /// <value>
        ///     The search provider.
        /// </value>
        [NotNull]
        private ExplorerSearchProvider SearchProvider { get; set; }

        /// <summary>
        /// Sets up the environment for each test.
        /// </summary>
        public override void SetUp()
        {
            base.SetUp();

            Subsystem = new MindExplorerSubsystem(Container, true);

            SearchProvider = Subsystem.SearchProviders.FirstOrDefault() as ExplorerSearchProvider;
            SearchProvider.ShouldNotBeNull();

        }

        /// <summary>
        ///     Ensures that the mind explorer subsystem contains a search provider
        /// </summary>
        [Test]
        public void MindExplorerSubsystemContainsSearchProvider()
        {

            //! Arrange

            var subsystem = Subsystem;
            var provider = SearchProvider;

            //! Act

            var numProviders = subsystem.SearchProviders.Count();

            //! Assert

            numProviders.ShouldBeGreaterThan(0);
            subsystem.SearchProviders.ShouldContain(provider);

        }

        /// <summary>
        ///     Searches on the mind explorer search provider should return an explorer search operation.
        /// </summary>
        [Test]
        public void MindExplorerSearchResultsInExpectedOperationType()
        {

            //! Arrange

            var searchProvider = SearchProvider;

            //! Act

            var operation = searchProvider.PerformSearch(SearchText);

            //! Assert

            operation.ShouldNotBeNull();
            operation.ShouldBeOfType<ExplorerSearchOperation>();

        }

        [TestCase("Alfred")]
        public void SearchCompletesInstantlyAndReturnsData(string searchText)
        {

            //! Arrange

            // Set up Alfred to have the mind explorer subsystem (and its search provider)
            Alfred = BuildAlfredInstance();
            Alfred.Register(Subsystem);
            Alfred.Initialize();

            var searchController = Alfred.SearchController;

            //! Act

            // Execute a Search for Alfred. The root node at least should match this
            searchController.PerformSearch(searchText);

            // Let the search compute
            Alfred.Update();

            //! Assert

            searchController.ShouldSatisfyAllConditions(
                () => searchController.Results.Count().ShouldBeGreaterThan(0),
                () => searchController.IsSearching.ShouldBe(false));

        }
    }
}