// ---------------------------------------------------------
// AlfredSearchTests.cs
// 
// Created on:      09/09/2015 at 12:10 AM
// Last Modified:   09/09/2015 at 11:33 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

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
    ///     Search tests related to how the search feature and the <see cref="AlfredApplication" />
    ///     interact.
    /// </summary>
    [UnitTestProvider]
    [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    [SuppressMessage("ReSharper", "ExceptionNotDocumented")]
    [SuppressMessage("ReSharper", "ExceptionNotDocumentedOptional")]
    public sealed class AlfredSearchTests : AlfredTestBase
    {
        /// <summary>
        ///     A testing search string. The actual content doesn't matter.
        /// </summary>
        const string SearchString = "This is a test search string";

        /// <summary>
        ///     Sets up the environment for each test.
        /// </summary>
        [SetUp]
        public override void SetUp() { base.SetUp(); }

        /// <summary>
        ///     Tests that new Alfred instances start with a non-null SearchController
        /// </summary>
        [Test]
        public void AlfredHasNonNullSearchController()
        {
            //! Arrange
            var alfred = BuildAlfredInstance();

            //! Act - No action - testing initial state

            //! Assert
            alfred.SearchController.ShouldNotBeNull();
            alfred.SearchController.ShouldImplementInterface<ISearchController>();
        }

        /// <summary>
        ///     When Alfred initializes, the <see cref="ISearchController" /> should get Initialize
        ///     and OnInitializationCompleted method calls.
        /// </summary>
        [Test]
        public void SearchControllerInitializesWhenAlfredInitializes()
        {
            //! Arrange
            var mock = BuildMockSearchController(MockBehavior.Strict);
            mock.Object.RegisterAsProvidedInstance(typeof(ISearchController), Container);

            Alfred = BuildAlfredInstance();

            //! Act
            Alfred.Initialize();

            //! Assert
            mock.Verify(c => c.Initialize(It.Is<IAlfred>(a => a == Alfred)), Times.Once);
            mock.Verify(c => c.OnInitializationCompleted(), Times.Once);
        }

        /// <summary>
        ///     When Alfred shuts down, the <see cref="ISearchController" /> should get Shutdown
        ///     and OnShutdownCompleted method calls.
        /// </summary>
        [Test]
        public void SearchControllerShutsDownWhenAlfredShutsDown()
        {
            //! Arrange
            var mock = BuildMockSearchController(MockBehavior.Strict);
            mock.Object.RegisterAsProvidedInstance(typeof(ISearchController), Container);

            Alfred = BuildAlfredInstance();

            //! Act
            Alfred.Initialize();
            Alfred.Shutdown();

            //! Assert
            mock.Verify(c => c.Shutdown(), Times.Once);
            mock.Verify(c => c.OnShutdownCompleted(), Times.Once);
        }

        /// <summary>
        ///     When Alfred updates, the <see cref="ISearchController" /> should get an Update method call.
        /// </summary>
        [Test]
        public void SearchControllerUpdatesWhenAlfredUpdates()
        {
            //! Arrange
            var mock = BuildMockSearchController(MockBehavior.Strict);
            mock.Object.RegisterAsProvidedInstance(typeof(ISearchController), Container);

            Alfred = BuildAlfredInstance();

            //! Act
            Alfred.Initialize();
            Alfred.Update();

            //! Assert
            mock.Verify(c => c.Update(), Times.Once);
        }

        /// <summary>
        ///     Registering a subsystem with search providers in Alfred should register those providers
        ///     into the <see cref="ISearchController"/>.
        /// </summary>
        [Test]
        public void RegisteringSubsystemWithSearchProviderRegistersProvider()
        {
            //! Arrange

            const MockBehavior MockBehavior = MockBehavior.Strict;

            // Build the collection of search providers to return from the subsystem
            var searchProviders = Container.ProvideCollection<ISearchProvider>();
            var provider = new Mock<ISearchProvider>(MockBehavior);
            searchProviders.Add(provider.Object);

            // Build out a subsystem that returns the providers
            var subsystem = BuildMockSubsystem(MockBehavior);
            subsystem.SetupGet(s => s.SearchProviders)
                .Returns(searchProviders);

            // Build Alfred and count the initial search providers
            Alfred = BuildAlfredInstance();
            var numSearchProviders = Alfred.SearchController.SearchProviders.Count();

            //! Act
            Alfred.Register(subsystem.Object);

            //! Assert
            subsystem.VerifyGet(s => s.SearchProviders, Times.Once);
            Alfred.SearchController.SearchProviders.ShouldContain(provider.Object);
            Alfred.SearchController.SearchProviders.Count().ShouldBe(numSearchProviders + 1);
        }

        /// <summary>
        ///     Undirected searches should start search operations.
        /// </summary>
        [Test]
        public void UndirectedSearchShouldStartSearchOperations()
        {
            //! Arrange
            const MockBehavior MockingBehavior = MockBehavior.Strict;

            var mockOperation = BuildMockOperation(MockingBehavior);

            var searchProvider = BuildSearchProvider(MockingBehavior, mockOperation.Object);

            var controller = new AlfredSearchController(Container);
            controller.Register(searchProvider.Object);

            //! Act
            controller.PerformSearch(SearchString);

            //! Assert
            searchProvider.Verify(s => s.PerformSearch(SearchString), Times.Once);
        }

        /// <summary>
        ///     Builds mock search provider.
        /// </summary>
        /// <param name="mockBehavior"> The mocking behavior. </param>
        /// <param name="resultOperation"> The result operation. </param>
        /// <returns>
        ///     A mock search provider.
        /// </returns>
        private static Mock<ISearchProvider> BuildSearchProvider(MockBehavior mockBehavior,
            ISearchOperation resultOperation = null)
        {
            // Build a default operation
            resultOperation = resultOperation ?? BuildMockOperation(mockBehavior).Object;

            // Set up the search provider
            var searchProvider = new Mock<ISearchProvider>(mockBehavior);
            searchProvider.Setup(p => p.PerformSearch(It.IsAny<string>())).Returns(resultOperation);

            return searchProvider;
        }

        /// <summary>
        ///     Undirected searches should start search operations.
        /// </summary>
        [Test]
        public void SearchWithMultipleProvidersShouldPopulateOperationsCorrectly()
        {
            //! Arrange
            const MockBehavior MockingBehavior = MockBehavior.Strict;

            // Build out two search providers with mock operations
            var mockOperation1 = BuildMockOperation(MockingBehavior);
            var searchProvider1 = BuildSearchProvider(MockingBehavior, mockOperation1.Object);

            var mockOperation2 = BuildMockOperation(MockingBehavior);
            var searchProvider2 = BuildSearchProvider(MockingBehavior, mockOperation2.Object);

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
            const MockBehavior MockingBehavior = MockBehavior.Strict;

            // Build out two search providers with mock operations
            var mockOperation = BuildMockOperation(MockingBehavior);
            var searchProvider = BuildSearchProvider(MockingBehavior, mockOperation.Object);

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

        /// <summary>
        ///     Builds a mock operation.
        /// </summary>
        /// <param name="mockBehavior"> The mocking behavior. </param>
        /// <returns>
        ///     A mock operation
        /// </returns>
        private static Mock<ISearchOperation> BuildMockOperation(MockBehavior mockBehavior)
        {
            var mock = new Mock<ISearchOperation>(mockBehavior);

            mock.Setup(m => m.Update());

            return mock;
        }

    }
}