// ---------------------------------------------------------
// AlfredSearchControllerTests.cs
// 
// Created on:      09/09/2015 at 11:45 AM
// Last Modified:   09/09/2015 at 11:45 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core;
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
    public sealed class AlfredSearchControllerTests : AlfredTestBase
    {

        /// <summary>
        ///     Gets the controller.
        /// </summary>
        /// <value>
        ///     The controller.
        /// </value>
        [NotNull]
        private AlfredSearchController Controller { get; set; }

        /// <summary>
        /// Sets up the environment for each test.
        /// </summary>
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            // Set up the Controller
            Controller = new AlfredSearchController(Container);
            Controller.RegisterAsProvidedInstance(typeof(ISearchController), Container);

            // Build Alfred - it should now grab the controller from Container
            Alfred = BuildAlfredInstance();
        }

        /// <summary>
        ///     The mocking behavior used when creating Moq mocks.
        /// </summary>
        const MockBehavior MockingBehavior = MockBehavior.Strict;

        /// <summary>
        ///     A testing search string. The actual content doesn't matter.
        /// </summary>
        const string SearchString = "This is a test search string";

        /// <summary>
        ///     A testing search string different than the first one. The actual content doesn't matter.
        /// </summary>
        const string AnotherSearchString = "This is another search string for testing";

        /// <summary>
        ///     Tests that search providers can be registered and show up in the controller's Providers
        ///     collection.
        /// </summary>
        [Test]
        public void SearchProvidersCanBeRegistered()
        {
            //! Arrange
            var mockProvider = BuildMockSearchProvider(MockBehavior.Strict);

            //! Act
            Controller.Register(mockProvider.Object);

            //! Assert
            Controller.SearchProviders.ShouldNotBeNull();
            Controller.SearchProviders.ShouldContain(mockProvider.Object);
        }

        /// <summary>
        ///     Tests that <see langword="null"/> search providers cannot be registered.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void NullSearchProvidersCannotBeRegistered()
        {
            //! Act / Assert - Expected ArgumentNullException
            Controller.Register(null);
        }

        /// <summary>
        ///     Tests that search providers with the same Id cannot be registered.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void SearchProvidersWithSameIdCannotBeRegistered()
        {
            //! Arrange
            const string SearchProviderId = "MyId";

            var searchProvider1 = BuildMockSearchProvider(MockingBehavior);
            searchProvider1.SetupGet(s => s.Id).Returns(SearchProviderId);

            var searchProvider2 = BuildMockSearchProvider(MockingBehavior);
            searchProvider2.SetupGet(s => s.Id).Returns(SearchProviderId);

            //! Act / Assert - Expected ArgumentNullException
            Controller.Register(searchProvider1.Object);
            Controller.Register(searchProvider2.Object);
        }

        /// <summary>
        ///     Tests that search providers cannot be registered when Alfred is offline
        /// </summary>
        [Test, ExpectedException(typeof(InvalidOperationException))]
        public void SearchProvidersCannotBeRegisteredWhenNotOffline()
        {
            //! Arrange
            var provider = BuildMockSearchProvider(MockBehavior.Strict);

            // Reconstruct Alfred - it should now grab the 
            Alfred = BuildAlfredInstance();

            //! Act / Assert - Expected InvalidOperationException
            Alfred.Initialize();
            Controller.Register(provider.Object);
        }

        /// <summary>
        ///     Undirected searches should start search operations.
        /// </summary>
        [Test]
        public void UndirectedSearchShouldStartSearchOperations()
        {
            //! Arrange
            var mockOperation = BuildMockSearchOperation(MockingBehavior);

            var searchProvider = BuildMockSearchProvider(MockingBehavior, mockOperation.Object);

            Controller.Register(searchProvider.Object);

            //! Act
            Controller.PerformSearch(SearchString);

            //! Assert
            searchProvider.Verify(s => s.PerformSearch(SearchString), Times.Once);
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
            var searchProvider1 = BuildMockSearchProvider(MockingBehavior);
            searchProvider1.SetupGet(s => s.Id).Returns(Provider1Id);

            // Build out the target search provider and its operation
            var mockOperation = BuildMockSearchOperation(MockingBehavior);
            var searchProvider2 = BuildMockSearchProvider(MockingBehavior, mockOperation.Object);
            searchProvider2.SetupGet(s => s.Id).Returns(Provider2Id);

            Controller.Register(searchProvider1.Object);
            Controller.Register(searchProvider2.Object);

            //! Act
            Controller.PerformSearch(SearchString, Provider2Id);

            //! Assert
            searchProvider1.Verify(s => s.PerformSearch(SearchString), Times.Never);
            searchProvider2.Verify(s => s.PerformSearch(SearchString), Times.Once);
            Controller.OngoingOperations.Count().ShouldBe(1);
            Controller.OngoingOperations.ShouldContain(mockOperation.Object);
        }

        /// <summary>
        ///     Undirected searches should start search operations.
        /// </summary>
        [Test]
        public void SearchWithMultipleProvidersShouldPopulateOperationsCorrectly()
        {
            //! Arrange

            // Build out two search providers with mock operations
            var mockOperation1 = BuildMockSearchOperation(MockingBehavior);
            var searchProvider1 = BuildMockSearchProvider(MockingBehavior, mockOperation1.Object);

            var mockOperation2 = BuildMockSearchOperation(MockingBehavior);
            var searchProvider2 = BuildMockSearchProvider(MockingBehavior, mockOperation2.Object);

            // Add a controller with the two providers. We're testing this detached from Alfred
            var controller = new AlfredSearchController(Container);
            controller.Register(searchProvider1.Object);
            controller.Register(searchProvider2.Object);

            //! Act
            controller.PerformSearch(SearchString);

            //! Assert
            controller.OngoingOperations.Count().ShouldBe(2);
            controller.OngoingOperations.ShouldContain(mockOperation1.Object);
            controller.OngoingOperations.ShouldContain(mockOperation2.Object);
        }

        /// <summary>
        ///     Undirected searches should start search operations.
        /// </summary>
        [Test]
        public void UpdateWithAnOngoingSearchCallsUpdatesOperation()
        {
            //! Arrange

            // Build out a search provider with a mock operations
            var mockOperation = BuildMockSearchOperation(MockingBehavior);
            var searchProvider = BuildMockSearchProvider(MockingBehavior, mockOperation.Object);

            // Add a controller with the provider
            Controller.Register(searchProvider.Object);

            //! Act
            Alfred.Initialize();
            Controller.PerformSearch(SearchString);
            Controller.Update();

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
            var mockOp = BuildMockSearchOperation(MockingBehavior);
            mockOp.Setup(o => o.Update())
                         .Callback(() => mockOp.SetupGet(o => o.IsSearchComplete).Returns(true));

            // Build out a search provider with the mock operations
            var searchProvider = BuildMockSearchProvider(MockingBehavior, mockOp.Object);

            // Add a controller with the provider
            Controller.Register(searchProvider.Object);

            //! Act
            Controller.Initialize(BuildAlfredInstance());
            Controller.PerformSearch(SearchString);
            Controller.Update();

            //! Assert
            Controller.OngoingOperations.ShouldNotContain(mockOp.Object);
            Controller.OngoingOperations.Count().ShouldBe(0);
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
            var mockResult1 = BuildMockSearchResult(MockingBehavior);
            var mockResult2 = BuildMockSearchResult(MockingBehavior);
            results.Add(mockResult1.Object);

            // Configure the operation to complete on update
            var mockOp = BuildMockSearchOperation(MockingBehavior);
            mockOp.SetupGet(o => o.Results).Returns(results);

            // Build out a search provider with the mock operations
            var searchProvider = BuildMockSearchProvider(MockingBehavior, mockOp.Object);

            // Add a controller with the provider. We're testing this detached from Alfred
            var controller = new AlfredSearchController(Container);
            controller.Register(searchProvider.Object);

            //! Act
            controller.Initialize(BuildAlfredInstance());
            controller.PerformSearch(SearchString);

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
            Controller.PerformSearch(SearchString);
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
        ///     Calling Abort on the search controller should in turn call abort on all ongoing searches.
        /// </summary>
        [Test]
        public void AbortShouldAbortOngoingSearches()
        {
            //! Arrange

            var provider = BuildMockSearchProvider(MockingBehavior);
            var operation = PrepareSearchProviderToReturnNewOperation(provider);

            Controller.Register(provider.Object);

            //! Act
            Controller.PerformSearch(SearchString);
            Controller.Abort();

            //! Assert
            operation.Verify(o => o.Abort(), Times.Once);
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
        private Mock<ISearchOperation> PrepareSearchProviderToReturnNewOperation(Mock<ISearchProvider> searchProvider)
        {
            var mockSearchOperation = BuildMockSearchOperation(MockingBehavior);

            searchProvider.Setup(s => s.PerformSearch(It.IsAny<string>()))
                          .Returns(mockSearchOperation.Object);

            return mockSearchOperation;
        }

        /// <summary>
        ///     Prepares the controller to have a search provider that returns a search result and
        ///     returns the new search provider.
        /// </summary>
        /// <param name="controller"> The controller. </param>
        /// <returns>
        ///     The search provider used to return results.
        /// </returns>
        private Mock<ISearchProvider> PrepareControllerToReturnOneSearchResult(AlfredSearchController controller)
        {

            // Build out a search provider with the mock operations
            var searchProvider = BuildMockSearchProvider(MockingBehavior);

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
        ///     Configure the mock operation to return a search result.
        /// </summary>
        /// <param name="mockOp"> The mock operation. </param>
        /// <returns>
        ///     The mock search result
        /// </returns>
        [SuppressMessage("ReSharper", "UnusedMethodReturnValue.Local")]
        private static Mock<ISearchResult> ConfigureOperationToReturnOneSearchResult(Mock<ISearchOperation> mockOp)
        {
            var result = BuildMockSearchResult(MockingBehavior);

            mockOp.SetupGet(o => o.Results).Returns(result.Object.ToCollection());

            return result;
        }

    }
}