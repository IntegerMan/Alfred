// ---------------------------------------------------------
// AlfredSearchControllerTests.cs
// 
// Created on:      09/09/2015 at 11:45 AM
// Last Modified:   09/09/2015 at 11:45 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using MattEland.Ani.Alfred.Core;
using MattEland.Ani.Alfred.Core.Definitions;
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
    public sealed class AlfredSearchControllerTests : AlfredTestBase
    {
        /// <summary>
        ///     The mocking behavior used when creating Moq mocks.
        /// </summary>
        const MockBehavior MockingBehavior = MockBehavior.Strict;

        /// <summary>
        ///     A testing search string. The actual content doesn't matter.
        /// </summary>
        const string SearchString = "This is a test search string";

        /// <summary>
        ///     Tests that search providers can be registered and show up in the controller's Providers
        ///     collection.
        /// </summary>
        [Test]
        public void SearchProvidersCanBeRegistered()
        {
            //! Arrange
            var controller = new AlfredSearchController(Container);
            var mockProvider = BuildMockSearchProvider(MockBehavior.Strict);

            //! Act
            controller.Register(mockProvider.Object);

            //! Assert
            controller.SearchProviders.ShouldNotBeNull();
            controller.SearchProviders.ShouldContain(mockProvider.Object);
        }

        /// <summary>
        ///     Tests that <see langword="null"/> search providers cannot be registered.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void NullSearchProvidersCannotBeRegistered()
        {
            //! Arrange
            var controller = new AlfredSearchController(Container);

            //! Act / Assert - Expected ArgumentNullException
            controller.Register(null);
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

            var controller = new AlfredSearchController(Container);

            //! Act / Assert - Expected ArgumentNullException
            controller.Register(searchProvider1.Object);
            controller.Register(searchProvider2.Object);
        }

        /// <summary>
        ///     Tests that search providers can be registered and show up in the controller's Providers
        ///     collection.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidOperationException))]
        public void SearchProvidersCannotBeRegisteredWhenNotOffline()
        {
            //! Arrange
            var controller = new AlfredSearchController(Container);
            controller.RegisterAsProvidedInstance(typeof(ISearchController), Container);

            var provider = BuildMockSearchProvider(MockBehavior.Strict);

            var alfred = BuildAlfredInstance();

            //! Act / Assert - Expected InvalidOperationException
            alfred.Initialize();
            controller.Register(provider.Object);
        }

        /// <summary>
        ///     Undirected searches should start search operations.
        /// </summary>
        [Test]
        public void UndirectedSearchShouldStartSearchOperations()
        {
            //! Arrange
            var mockOperation = BuildMockOperation(MockingBehavior);

            var searchProvider = BuildMockSearchProvider(MockingBehavior, mockOperation.Object);

            var controller = new AlfredSearchController(Container);
            controller.Register(searchProvider.Object);

            //! Act
            controller.PerformSearch(SearchString);

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
            var mockOperation = BuildMockOperation(MockingBehavior);
            var searchProvider2 = BuildMockSearchProvider(MockingBehavior, mockOperation.Object);
            searchProvider2.SetupGet(s => s.Id).Returns(Provider2Id);


            var controller = new AlfredSearchController(Container);
            controller.Register(searchProvider1.Object);
            controller.Register(searchProvider2.Object);

            //! Act
            controller.PerformSearch(SearchString, Provider2Id);

            //! Assert
            searchProvider1.Verify(s => s.PerformSearch(SearchString), Times.Never);
            searchProvider2.Verify(s => s.PerformSearch(SearchString), Times.Once);
            controller.OngoingOperations.Count().ShouldBe(1);
            controller.OngoingOperations.ShouldContain(mockOperation.Object);
        }

        /// <summary>
        ///     Undirected searches should start search operations.
        /// </summary>
        [Test]
        public void SearchWithMultipleProvidersShouldPopulateOperationsCorrectly()
        {
            //! Arrange

            // Build out two search providers with mock operations
            var mockOperation1 = BuildMockOperation(MockingBehavior);
            var searchProvider1 = BuildMockSearchProvider(MockingBehavior, mockOperation1.Object);

            var mockOperation2 = BuildMockOperation(MockingBehavior);
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

            // Build out two search providers with mock operations
            var mockOperation = BuildMockOperation(MockingBehavior);
            var searchProvider = BuildMockSearchProvider(MockingBehavior, mockOperation.Object);

            // Add a controller with the two providers. We're testing this detached from Alfred
            var controller = new AlfredSearchController(Container);
            controller.Register(searchProvider.Object);

            //! Act
            controller.Initialize(BuildAlfredInstance());
            controller.PerformSearch(SearchString);
            controller.Update();

            //! Assert
            mockOperation.Verify(o => o.Update(), Times.Once);
        }

    }
}