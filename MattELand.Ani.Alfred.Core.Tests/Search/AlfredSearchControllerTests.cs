// ---------------------------------------------------------
// AlfredSearchControllerTests.cs
// 
// Created on:      09/09/2015 at 11:45 AM
// Last Modified:   09/14/2015 at 1:44 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core;
using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Common;
using MattEland.Common.Providers;
using MattEland.Testing;

using Moq;

using NUnit.Framework;

using Shouldly;

namespace MattEland.Ani.Alfred.Tests.Search
{
    /// <summary>
    ///     A test class containing unit tests related to <see cref="AlfredSearchController"/>
    /// </summary>
    [UnitTestProvider]
    [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
    [SuppressMessage("ReSharper", "ExceptionNotDocumented")]
    [SuppressMessage("ReSharper", "ExceptionNotDocumentedOptional")]
    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    [SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    [Category("Search")]
    public sealed class AlfredSearchControllerTests : MockEnabledAlfredTestBase
    {

        /// <summary>
        ///     A testing search string different than the first one. The actual content doesn't matter.
        /// </summary>
        private const string AnotherSearchString = "This is another search string for testing";

        /// <summary>
        ///     Gets the controller.
        /// </summary>
        /// <value>
        ///     The controller.
        /// </value>
        [NotNull]
        private AlfredSearchController Controller { get; set; }

        /// <summary>
        ///     Calling Abort on the search controller should in turn call abort on all ongoing searches.
        /// </summary>
        [Test]
        public void AbortShouldAbortOngoingSearches()
        {
            //! Arrange

            var operation = PrepareSearchControllerWithSearchProviderYieldingOperation();

            //! Act

            Controller.PerformSearch(Some.SearchText);
            Controller.Abort();

            //! Assert

            operation.Verify(o => o.Abort(), Times.Once);
        }

        /// <summary>
        ///     Searches directed at a specific target should start search operations on that target only.
        /// </summary>
        [Test]
        public void DirectedSearchShouldStartSearchOnTargetOnly()
        {
            //! Arrange
            const string Provider1Id = "Provider1ID";
            const string Provider2Id = "Provider2ID";

            // Build out the first search provider. This one should not be called
            var searchProvider1 = BuildMockSearchProvider();
            searchProvider1.SetupGet(s => s.Id).Returns(Provider1Id);

            // Build out the target search provider and its operation
            var mockOperation = BuildMockSearchOperation();
            var searchProvider2 = BuildMockSearchProvider(mockOperation.Object);
            searchProvider2.SetupGet(s => s.Id).Returns(Provider2Id);

            Controller.Register(searchProvider1.Object);
            Controller.Register(searchProvider2.Object);

            //! Act
            string searchText = Some.SearchText;
            Controller.PerformSearch(searchText, Provider2Id);

            //! Assert
            searchProvider1.Verify(s => s.PerformSearch(searchText), Times.Never);
            searchProvider2.Verify(s => s.PerformSearch(searchText), Times.Once);
            Controller.OngoingOperations.Count().ShouldBe(1);
            Controller.OngoingOperations.ShouldContain(mockOperation.Object);
        }

        /// <summary>
        ///     Calling Abort on the search controller should in turn call abort on all ongoing searches.
        /// </summary>
        [Test]
        public void IsSearchingShouldBeTrueWhenSearching()
        {
            //! Arrange

            var operation = PrepareSearchControllerWithSearchProviderYieldingOperation();

            //! Act

            Controller.PerformSearch(Some.SearchText);

            //! Assert

            Controller.IsSearching.ShouldBeTrue();
            Controller.OngoingOperations.ShouldContain(operation.Object);
        }

        /// <summary>
        ///     Ensures that results from providers are added to the controller after updates.
        ///     This also checks that multiple updates don't result in duplicate entries in the list.
        /// </summary>
        [Test]
        public void NewSearchResultsAreAddedToControllerResults()
        {
            //! Arrange

            // We'll be simulating a progressive disclosure of items
            var results = Container.ProvideCollection<ISearchResult>();
            var mockResult1 = BuildMockSearchResult();
            var mockResult2 = BuildMockSearchResult();
            results.Add(mockResult1.Object);

            // Configure the operation to complete on update
            var mockOp = BuildMockSearchOperation();
            mockOp.SetupGet(o => o.Results).Returns(results);

            // Build out a search provider with the mock operations
            var searchProvider = BuildMockSearchProvider(mockOp.Object);

            // Add a controller with the provider. We're testing this detached from Alfred
            var controller = new AlfredSearchController(AlfredContainer);
            controller.Register(searchProvider.Object);

            //! Act
            controller.Initialize(BuildAlfredInstance());
            controller.PerformSearch(Some.SearchText);

            // The first update should return the initial list entry only
            controller.Update();

            // Simulate a new item coming back for the next update
            results.Add(mockResult2.Object);
            controller.Update();

            // Simulate no new items next update
            controller.Update();

            //! Assert

            controller.Results.Count().ShouldBe(2); // Test against duplicated items
            controller.Results.ShouldContain(mockResult1.Object);
            controller.Results.ShouldContain(mockResult2.Object);
        }

        /// <summary>
        ///     Tests that <see langword="null"/> search providers cannot be registered.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        [Category("Registration")]
        [Category("Validation")]
        public void NullSearchProvidersCannotBeRegistered()
        {
            //! Act / Assert - Expected ArgumentNullException
            Controller.Register(null);
        }

        /// <summary>
        ///     Ensures that results from providers are added to the controller after updates.
        ///     This also checks that multiple updates don't result in duplicate entries in the list.
        /// </summary>
        [Test]
        public void SearchesClearOutOldResults()
        {
            //! Arrange

            // Get the controller online with some sample data
            var searchProvider = PrepareControllerToReturnOneSearchResult(Controller);

            //! Act

            // Do a search to generate some preliminary results
            Controller.PerformSearch(Some.SearchText);
            Controller.Update();

            // Next time we search, we shouldn't return any results - just a dummy operation
            PrepareSearchProviderToReturnNewOperation(searchProvider);

            // Do another search. We're testing that this clears out the first search
            Controller.PerformSearch(AnotherSearchString);

            // Note: we're not updating this time around - we just want to see if it cleared the results

            //! Assert

            Controller.Results.Count().ShouldBe(0);
            searchProvider.Verify(p => p.PerformSearch(It.IsAny<string>()), Times.Exactly(2));
        }

        /// <summary>
        ///     Tests that search providers can be registered and show up in the controller's Providers
        ///     collection.
        /// </summary>
        [Test]
        [Category("Registration")]
        public void SearchProvidersCanBeRegistered()
        {
            //! Arrange
            var mockProvider = BuildMockSearchProvider();

            //! Act
            Controller.Register(mockProvider.Object);

            //! Assert
            Controller.SearchProviders.ShouldNotBeNull();
            Controller.SearchProviders.ShouldContain(mockProvider.Object);
        }

        /// <summary>
        ///     Tests that search providers cannot be registered when Alfred is offline
        /// </summary>
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        [Category("Registration")]
        [Category("Validation")]
        public void SearchProvidersCannotBeRegisteredWhenNotOffline()
        {
            //! Arrange
            var provider = BuildMockSearchProvider();

            // Reconstruct Alfred - it should now grab the 
            Alfred = BuildAlfredInstance();

            //! Act / Assert - Expected InvalidOperationException
            Alfred.Initialize();
            Controller.Register(provider.Object);
        }

        /// <summary>
        ///     Tests that search providers with the same Id cannot be registered.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        [Category("Registration")]
        [Category("Validation")]
        public void SearchProvidersWithSameIdCannotBeRegistered()
        {
            //! Arrange
            const string SearchProviderId = "MyId";

            var searchProvider1 = BuildMockSearchProvider();
            searchProvider1.SetupGet(s => s.Id).Returns(SearchProviderId);

            var searchProvider2 = BuildMockSearchProvider();
            searchProvider2.SetupGet(s => s.Id).Returns(SearchProviderId);

            //! Act / Assert - Expected ArgumentNullException
            Controller.Register(searchProvider1.Object);
            Controller.Register(searchProvider2.Object);
        }

        /// <summary>
        ///     When searches are executed, a log entry should be created.
        /// </summary>
        [Test]
        [Category("Logging")]
        public void SearchShouldBeLogged()
        {
            //! Arrange

            PrepareSearchControllerWithSearchProviderYieldingOperation();

            // Set up a console for verification
            var console = BuildMockConsole();
            console.Object.RegisterAsProvidedInstance(typeof(IConsole), Container);

            //! Act

            string searchText = Some.SearchText;
            Controller.PerformSearch(searchText);

            //! Assert

            var firstProviderId = Controller.SearchProviders.First().Id;

            const string Title = "Search Executed";
            var message = string.Format("Searching {0} for: '{1}'", firstProviderId, searchText);

            console.Verify(c => c.Log(Title, message, LogLevel.Info), Times.Once);
        }

        /// <summary>
        ///     When searches are executed, a log entry should be created.
        /// </summary>
        [Test]
        [Category("Logging")]
        public void SearchShouldAddToSearchHistory()
        {
            //! Arrange

            PrepareSearchControllerWithSearchProviderYieldingOperation();

            //! Act

            string searchText = Some.SearchText;
            Controller.PerformSearch(searchText);

            //! Assert

            var matches = Controller.SearchHistory.Count(h => h.SearchText.Matches(searchText));

            matches.ShouldBe(1);
        }

        /// <summary>
        ///     When searches complete, a notifying log entry should be created.
        /// </summary>
        [Test]
        [Category("Logging")]
        public void SearchShouldLogWhenCompleted()
        {
            //! Arrange

            // Get Alfred online. This will use the container to get the Controller and Console
            Alfred = BuildAlfredInstance();
            Alfred.Initialize();

            //! Act

            Alfred.SearchController.PerformSearch(Some.SearchText);
            Alfred.Update();

            //! Assert

            Alfred.SearchController.IsSearching.ShouldBe(false);

            // Check the log entry
            const string ExpectedTitle = "Search Complete";
            const string ExpectedMessage = "Search complete. No results found.";

            var console = Container.TryProvide<IConsole>();
            console.ShouldNotBeNull();

            var matchingEvents = console.Events.Where(e => e.Title.Matches(ExpectedTitle)).ToList();

            var lastEvent = console.Events.Last();
            matchingEvents.Count.ShouldBe(1, string.Format("Did not find Search Complete event. Last event was: {0}", lastEvent));

            var completedEvent = matchingEvents.First();

            completedEvent.Message.ShouldBe(ExpectedMessage);
            completedEvent.Level.ShouldBe(LogLevel.ChatNotification);
        }


        /// <summary>
        ///     Undirected searches should start search operations.
        /// </summary>
        [Test]
        public void SearchWithMultipleProvidersShouldPopulateOperationsCorrectly()
        {
            //! Arrange

            // Build out two search providers with mock operations
            var mockOperation1 = BuildMockSearchOperation();
            var searchProvider1 = BuildMockSearchProvider(mockOperation1.Object);

            var mockOperation2 = BuildMockSearchOperation();
            var searchProvider2 = BuildMockSearchProvider(mockOperation2.Object);

            // Add a controller with the two providers. We're testing this detached from Alfred
            var controller = new AlfredSearchController(AlfredContainer);
            controller.Register(searchProvider1.Object);
            controller.Register(searchProvider2.Object);

            //! Act
            controller.PerformSearch(Some.SearchText);

            //! Assert
            controller.OngoingOperations.Count().ShouldBe(2);
            controller.OngoingOperations.ShouldContain(mockOperation1.Object);
            controller.OngoingOperations.ShouldContain(mockOperation2.Object);
        }

        /// <summary>
        /// Sets up the environment for each test.
        /// </summary>
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            // Set up the Controller
            Controller = new AlfredSearchController(AlfredContainer);
            Controller.RegisterAsProvidedInstance(typeof(ISearchController), Container);

            // Build Alfred - it should now grab the controller from Container
            Alfred = BuildAlfredInstance();
        }
        /// <summary>
        ///     Undirected searches should start search operations.
        /// </summary>
        [Test]
        public void UndirectedSearchShouldStartSearchOperations()
        {
            //! Arrange
            var mockOperation = BuildMockSearchOperation();

            var searchProvider = BuildMockSearchProvider(mockOperation.Object);

            Controller.Register(searchProvider.Object);

            //! Act
            string searchText = Some.SearchText;
            Controller.PerformSearch(searchText);

            //! Assert
            searchProvider.Verify(s => s.PerformSearch(searchText), Times.Once);
        }
        /// <summary>
        ///     Undirected searches should start search operations.
        /// </summary>
        [Test]
        public void UpdateWithAnOngoingSearchCallsUpdatesOperation()
        {
            //! Arrange

            // Build out a search provider with a mock operations
            var mockOperation = BuildMockSearchOperation();
            var searchProvider = BuildMockSearchProvider(mockOperation.Object);

            // Add a controller with the provider
            Controller.Register(searchProvider.Object);

            //! Act
            Alfred.Initialize();
            Controller.PerformSearch(Some.SearchText);

            //! Assert
            mockOperation.Verify(o => o.Update(), Times.Once);
        }

        /// <summary>
        ///     Undirected searches should start search operations.
        /// </summary>
        [Test]
        public void WhenSearchesCompleteTheyAreRemovedFromOngoingOperations()
        {
            //! Arrange

            // Configure the operation to complete on update
            var mockOp = BuildMockSearchOperation();
            mockOp.Setup(o => o.Update())
                  .Callback(() => mockOp.SetupGet(o => o.IsSearchComplete).Returns(true));

            // Build out a search provider with the mock operations
            var searchProvider = BuildMockSearchProvider(mockOp.Object);

            // Add a controller with the provider
            Controller.Register(searchProvider.Object);

            //! Act
            Controller.Initialize(BuildAlfredInstance());
            Controller.PerformSearch(Some.SearchText);
            Controller.Update();

            //! Assert
            Controller.OngoingOperations.ShouldNotContain(mockOp.Object);
            Controller.OngoingOperations.Count().ShouldBe(0);
        }

        /// <summary>
        ///     Configure the mock operation to return a search result.
        /// </summary>
        /// <param name="mockOp"> The mock operation. </param>
        /// <returns>
        ///     The mock search result
        /// </returns>
        [SuppressMessage("ReSharper", "UnusedMethodReturnValue.Local")]
        private Mock<ISearchResult> ConfigureOperationToReturnOneSearchResult(
            Mock<ISearchOperation> mockOp)
        {
            var result = BuildMockSearchResult();

            mockOp.SetupGet(o => o.Results).Returns(result.Object.ToCollection(Container));

            return result;
        }

        /// <summary>
        ///     Prepares the <paramref name="controller"/> to have a search provider that returns a
        ///     search result and returns the new search provider.
        /// </summary>
        /// <param name="controller"> The controller. </param>
        /// <returns>
        ///     The search provider used to return results.
        /// </returns>
        private Mock<ISearchProvider> PrepareControllerToReturnOneSearchResult(
            AlfredSearchController controller)
        {
            // Build out a search provider with the mock operations
            var searchProvider = BuildMockSearchProvider();

            // Simulate items in the list to begin with
            var mockOp = PrepareSearchProviderToReturnNewOperation(searchProvider);

            // Ensure that this operation returns a valid result
            ConfigureOperationToReturnOneSearchResult(mockOp);

            // Add a controller with the provider. We're testing this detached from Alfred
            controller.Register(searchProvider.Object);

            controller.Initialize(Alfred);

            return searchProvider;
        }

        /// <summary>
        ///     Prepares the search controller with a simple Mock search provider that yields a Mock operation when a search is made.
        /// </summary>
        /// <returns>
        ///     The Mock operation that will be yielded
        /// </returns>
        private Mock<ISearchOperation> PrepareSearchControllerWithSearchProviderYieldingOperation()
        {
            var provider = BuildMockSearchProvider();
            var operation = PrepareSearchProviderToReturnNewOperation(provider);

            Controller.Register(provider.Object);

            return operation;
        }

        /// <summary>
        ///     Prepare a search provider to return a new operation. This is useful in scenarios where
        ///     more than one search operation is being conducted.
        /// </summary>
        /// <param name="searchProvider"> The search provider. </param>
        /// <returns>
        ///     The mock search operation
        /// </returns>
        [SuppressMessage("ReSharper", "UnusedMethodReturnValue.Local")]
        [NotNull]
        private Mock<ISearchOperation> PrepareSearchProviderToReturnNewOperation(
            Mock<ISearchProvider> searchProvider)
        {
            var mockSearchOperation = BuildMockSearchOperation();

            searchProvider.Setup(s => s.PerformSearch(It.IsAny<string>()))
                          .Returns(mockSearchOperation.Object);

            return mockSearchOperation;
        }
    }
}