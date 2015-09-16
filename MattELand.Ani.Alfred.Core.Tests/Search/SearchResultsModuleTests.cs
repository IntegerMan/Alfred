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
using MattEland.Common;
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
        ///     The status label should match the search controller's status during startup.
        /// </summary>
        [Test]
        public void SearchLabelShouldMatchSearchControllerStatusAfterUpdate()
        {
            //! Arrange

            // We're testing with an actual search controller and a dummy search provider
            // This lets us simulate searches while still testing the UI / controller interactions
            var searchController = BuildSearchControllerWithMockProvider();

            // Program Alfred to return this search controller
            MockAlfred.SetupGet(m => m.SearchController).Returns(searchController);
            Alfred = MockAlfred.Object;

            //! Act

            // Get the module settled in
            Module.Initialize(Alfred);
            Module.Update();

            // Send across a search for all items
            searchController.PerformSearch("Goat Cheese Tuxedos", "All");

            // Update the module. This should update the UI
            Module.Update();

            //! Assert

            const string ExpectedStatus = "Searching for \"Goat Cheese Tuxedos\". No results found so far...";

            searchController.StatusMessage.ShouldBe(ExpectedStatus);

            var label = GetResultsLabel();
            label.Text.ShouldBe(ExpectedStatus);

        }

        /// <summary>
        ///     Builds a search controller with a mock search provider.
        /// </summary>
        /// <returns>
        ///     The <see cref="AlfredSearchController"/> instance.
        /// </returns>
        private AlfredSearchController BuildSearchControllerWithMockProvider()
        {
            var searchController = new AlfredSearchController(Container);

            // Build up a search operation that is an ongoing operation
            var operation = BuildMockSearchOperation();
            operation.SetupGet(o => o.IsSearchComplete).Returns(false);

            // Give the search controller something to return
            var mockSearchProvider = BuildMockSearchProvider(operation.Object);
            searchController.Register(mockSearchProvider.Object);

            // Just do a bit of verification so this doesn't throw off all tests
            searchController.SearchProviders.Count().ShouldBe(1);

            return searchController;
        }

        /// <summary>
        ///     Simulates that a search just started.
        /// </summary>
        /// <param name="searchController"> </param>
        /// <param name="search"> The search. </param>
        /// <param name="numResults"> Number of results found so far. </param>
        /// <returns>
        ///     The returned value of StatusMessage.
        /// </returns>
        private static string ProgramSearch(Mock<ISearchController> searchController, string search, int numResults = 0)
        {
            // TODO: Delete this once the controller supports this logic.

            searchController.SetupGet(s => s.IsSearching).Returns(true);

            if (numResults <= 0)
            {
                search = $"Searching for \"{search}\". No results found so far...";
            }
            else
            {
                var results = numResults.Pluralize("result", "results");

                search = $"Searching for \"{search}\". {numResults} {results} found so far...";
            }

            searchController.SetupGet(s => s.StatusMessage).Returns(search);

            // Update Alfred's search controller with the updated Mock

            return search;
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