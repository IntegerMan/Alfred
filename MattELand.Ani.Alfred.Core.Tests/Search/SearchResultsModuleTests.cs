// ---------------------------------------------------------
// SearchResultsModuleTests.cs
// 
// Created on:      09/15/2015 at 12:57 AM
// Last Modified:   09/15/2015 at 4:23 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Modules;
using MattEland.Ani.Alfred.Core.Widgets;
using MattEland.Ani.Alfred.Tests.Controls;
using MattEland.Testing;

using Moq;

using NUnit.Framework;

using Shouldly;

namespace MattEland.Ani.Alfred.Tests.Search
{
    /// <summary>
    ///     Tests for the Search Results Module. This class cannot be inherited.
    /// </summary>
    [UnitTestProvider]
    [Category("Search")]
    [Category("Modules")]
    [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
    [SuppressMessage("ReSharper", "ExceptionNotDocumentedOptional")]
    [SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    [SuppressMessage("ReSharper", "ExceptionNotDocumented")]
    [SuppressMessage("ReSharper", "RedundantArgumentName")]
    public sealed class SearchResultsModuleTests : UserInterfaceTestBase
    {
        /// <summary>
        ///     Gets the mock Alfred instance.
        /// </summary>
        /// <value>
        /// The mock Alfred instance.
        /// </value>
        private Mock<IAlfred> MockAlfred { get; set; }

        /// <summary>
        ///     Gets or sets the mock search controller.
        /// </summary>
        /// <value>
        /// The mock search controller.
        /// </value>
        private Mock<ISearchController> SearchController { get; set; }

        /// <summary>
        ///     Gets or sets the module.
        /// </summary>
        /// <value>
        /// The module.
        /// </value>
        [NotNull]
        private SearchResultsModule Module { get; set; }

        /// <summary>
        ///     Sets up the environment for each test.
        /// </summary>
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            // Build a Mock search controller to simulate searches
            SearchController = BuildMockSearchController();

            // Build a Mock Alfred that returns the search controller
            MockAlfred = BuildMockAlfred();
            MockAlfred.SetupGet(a => a.SearchController).Returns(SearchController.Object);

            // Use our Mock as the Alfred Instance
            Alfred = MockAlfred.Object;

            Module = new SearchResultsModule(Container);
        }

        /// <summary>
        ///     The status label should match the search controller's status during startup.
        /// </summary>
        [Test]
        public void SearchLabelShouldMatchSearchControllerStatusOnStartup()
        {
            //! Arrange

            var alfred = Alfred;

            //! Act

            Module.Initialize(alfred);
            Module.Update();

            //! Assert

            var label = GetResultsLabel();
            label.Text.ShouldBe(SearchController.Object.StatusMessage);
        }

        /// <summary>
        ///     The status label should provide an accurate status of search results for an ongoing
        ///     search with a variable number of results returned so far.
        /// </summary>
        /// <param name="numResults"> The number of results to return. </param>
        /// <param name="expectedFound"> The expected results found substring. </param>
        [TestCase(0, "No results")]
        [TestCase(1, "1 result")]
        [TestCase(2, "2 results")]
        public void SearchLabelShouldMatchStatusForInProgressSearches(int numResults, string expectedFound)
        {
            //! Arrange

            // We're testing with an actual search controller and a dummy search provider
            // This lets us simulate searches while still testing the UI / controller interactions
            const bool IsSearchComplete = false;
            var searchController = BuildSearchControllerWithProvider(IsSearchComplete, numResults);

            // Program Alfred to return this search controller
            MockAlfred.SetupGet(m => m.SearchController).Returns(searchController);
            Alfred = MockAlfred.Object;

            //! Act

            // Get everything started - we're bypassing subsystems and pages for this test
            searchController.Initialize(Alfred);
            Module.Initialize(Alfred);

            // Send across a search for all items
            searchController.PerformSearch("Goat Cheese Tuxedos", "All");

            // Simulate an update pulse
            searchController.Update();
            Module.Update();

            //! Assert

            var expectedStatus = $"Searching for \"Goat Cheese Tuxedos\". {expectedFound} found so far...";

            searchController.StatusMessage.ShouldBe(expectedStatus);

            var label = GetResultsLabel();
            label.Text.ShouldBe(expectedStatus);

        }

        /// <summary>
        ///     The status label should provide an accurate status of search results for completed
        ///     searches with a variable number of results found.
        /// </summary>
        /// <param name="numResults"> The number of results to return. </param>
        /// <param name="expectedFound"> The expected results found substring. </param>
        [TestCase(0, "No results")]
        [TestCase(1, "1 result")]
        [TestCase(2, "2 results")]
        public void SearchLabelShouldMatchStatusForCompletedSearches(int numResults, string expectedFound)
        {
            //! Arrange

            // We're testing with an actual search controller and a dummy search provider
            // This lets us simulate searches while still testing the UI / controller interactions
            const bool IsSearchComplete = true;
            var searchController = BuildSearchControllerWithProvider(IsSearchComplete, numResults);

            // Program Alfred to return this search controller
            MockAlfred.SetupGet(m => m.SearchController).Returns(searchController);
            Alfred = MockAlfred.Object;

            //! Act

            // Get everything started - we're bypassing subsystems and pages for this test
            searchController.Initialize(Alfred);
            Module.Initialize(Alfred);

            // Send across a search for all items
            searchController.PerformSearch("Squirrel Lice Tacos");

            // Simulate an update pulse
            searchController.Update();
            Module.Update();

            //! Assert

            var expectedStatus = $"Search complete. {expectedFound} found for the search: \"Squirrel Lice Tacos\".";

            searchController.StatusMessage.ShouldBe(expectedStatus);

            var label = GetResultsLabel();
            label.Text.ShouldBe(expectedStatus);

        }

        /// <summary>
        ///     Builds a search controller with a mock search provider.
        /// </summary>
        /// <param name="isSearchComplete">
        ///     <see langword="true"/> if the operation should indicate the search is complete.
        /// </param>
        /// <param name="numResults"> The number of results to yield. </param>
        /// <returns>
        ///     The <see cref="AlfredSearchController"/> instance.
        /// </returns>
        private AlfredSearchController BuildSearchControllerWithProvider(bool isSearchComplete, int numResults = 0)
        {
            var searchController = new AlfredSearchController(Container);

            // Build up a search operation that is an ongoing operation
            var operation = BuildMockSearchOperation();
            operation.SetupGet(o => o.IsSearchComplete).Returns(isSearchComplete);

            ProgramOperationToReturnResults(operation, numResults);

            // Give the search controller something to return
            var mockSearchProvider = BuildMockSearchProvider(operation.Object);
            searchController.Register(mockSearchProvider.Object);

            // Just do a bit of verification so this doesn't throw off all tests
            searchController.SearchProviders.Count().ShouldBe(1);

            return searchController;
        }

        /// <summary>
        ///     Programs an operation to return search results.
        /// </summary>
        /// <param name="operation"> The operation. </param>
        /// <param name="numResults"> The number of items to return. </param>
        private void ProgramOperationToReturnResults(Mock<ISearchOperation> operation, int numResults)
        {
            var results = Container.ProvideCollection<ISearchResult>();

            for (var i = 1; i <= numResults; i++)
            {
                var mockResult = BuildMockSearchResult();
                mockResult.SetupGet(r => r.Title).Returns($"Result {i}");

                results.Add(mockResult.Object);
            }

            // Sanity check
            results.Count.ShouldBe(numResults);

            operation.SetupGet(o => o.Results).Returns(results);
        }

        /// <summary>
        ///     Gets results label from the search results module.
        /// </summary>
        /// <returns>The results label.</returns>
        private TextWidget GetResultsLabel()
        {
            return FindWidgetOfTypeByName<TextWidget>(Module, @"lblResults");
        }

        /// <summary>
        ///     The <see cref="IStatusController" /> instance used by the search results module
        ///     should be that provided by its Alfred instance.
        /// </summary>
        [Test]
        public void SearchResultsModuleShouldUseAlfredSearchController()
        {
            //! Arrange

            var alfred = Alfred;

            //! Act

            Module.Initialize(alfred);

            var searchController = Module.SearchController;

            //! Assert

            searchController.ShouldNotBeNull();
            searchController.ShouldBe(SearchController.Object);
            MockAlfred.VerifyGet(a => a.SearchController, Times.AtLeastOnce);
        }

        /// <summary>
        ///     The Search Results module should use vertical layout
        /// </summary>
        [Test]
        public void SearchResultsShouldHaveVerticalLayout()
        {
            //! Assert

            Module.LayoutType.ShouldBe(LayoutType.VerticalStackPanel);
        }
    }
}