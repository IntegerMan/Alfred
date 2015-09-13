// ---------------------------------------------------------
// MockEnabledAlfredTestBase.cs
// 
// Created on:      09/11/2015 at 10:32 PM
// Last Modified:   09/11/2015 at 10:48 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;

using MattEland.Ani.Alfred.Core.Definitions;

using Moq;

namespace MattEland.Ani.Alfred.Tests
{
    /// <summary>
    ///     A mocking friendly version of <see cref="AlfredTestBase" /> that adds mocking methods
    ///     for various components.
    /// </summary>
    [SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    [SuppressMessage("ReSharper", "UnusedMethodReturnValue.Global")]
    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    public abstract class MockEnabledAlfredTestBase : AlfredTestBase
    {
        /// <summary>
        ///     Builds a mock <see cref="IAlfredSubsystem" /> .
        /// </summary>
        /// <param name="mockBehavior">The mocking behavior.</param>
        /// <returns>A mock subsystem</returns>
        protected Mock<IAlfredSubsystem> BuildMockSubsystem(MockBehavior mockBehavior)
        {
            // Build the Mock
            var mock = new Mock<IAlfredSubsystem>(mockBehavior);

            mock.SetupGet(s => s.Id).Returns("Test");
            mock.SetupGet(s => s.Pages).Returns(Container.ProvideCollection<IPage>());
            mock.SetupGet(s => s.SearchProviders)
                .Returns(Container.ProvideCollection<ISearchProvider>());

            SetupMockComponent(mock);

            return mock;
        }

        /// <summary>
        ///     Builds a page mock.
        /// </summary>
        /// <param name="mockBehavior"> The mocking behavior for the new mock. </param>
        /// <returns>
        ///     The mock page.
        /// </returns>
        protected Mock<IPage> BuildMockPage(MockBehavior mockBehavior)
        {
            // Some tests will want strict control over mocking and others won't
            var mock = new Mock<IPage>(mockBehavior);

            // Set up simple members we expect to be hit during startup
            mock.SetupGet(p => p.IsRootLevel).Returns(true);

            SetupMockComponent(mock);

            return mock;
        }

        /// <summary>
        ///     Sets up a <paramref name="mock" /> component with common component actions.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="mock">
        /// The Mock object that derives from <see cref="IAlfredComponent" /> .
        /// </param>
        private static void SetupMockComponent<T>(Mock<T> mock) where T : class, IAlfredComponent
        {
            // Setup Simple properties
            mock.SetupGet(s => s.NameAndVersion).Returns("Test Component 1.0.0.0");
            mock.SetupGet(s => s.Status).Returns(AlfredStatus.Offline);

            // Setup simple methods
            mock.Setup(s => s.Update());
            mock.Setup(s => s.OnRegistered(It.IsAny<IAlfred>()));
            mock.Setup(s => s.OnInitializationCompleted());
            mock.Setup(s => s.OnShutdownCompleted());

            // Initialize causes the subsystem to go online
            Action initializeAction = () =>
                                      {
                                          mock.SetupGet(s => s.Status).Returns(AlfredStatus.Online);
                                          mock.SetupGet(s => s.IsOnline).Returns(true);
                                      };
            mock.Setup(s => s.Initialize(It.IsAny<IAlfred>())).Callback(initializeAction);

            // Shutdown causes the subsystem to go offline
            Action shutdownAction = () =>
                                    {
                                        mock.SetupGet(s => s.Status).Returns(AlfredStatus.Offline);
                                        mock.SetupGet(s => s.IsOnline).Returns(false);
                                    };
            mock.Setup(s => s.Shutdown()).Callback(shutdownAction);
        }

        /// <summary>
        ///     Builds a mock search controller.
        /// </summary>
        /// <param name="mockBehavior">The mocking behavior.</param>
        /// <returns>The mock search controller</returns>
        protected static Mock<ISearchController> BuildMockSearchController(MockBehavior mockBehavior)
        {
            var mock = new Mock<ISearchController>(mockBehavior);

            SetupMockComponent(mock);

            return mock;
        }

        /// <summary>
        ///     Builds mock search provider.
        /// </summary>
        /// <param name="mockBehavior">The mocking behavior.</param>
        /// <param name="resultOperation">The result operation.</param>
        /// <returns>A mock search provider.</returns>
        protected Mock<ISearchProvider> BuildMockSearchProvider(MockBehavior mockBehavior,
                                                                ISearchOperation resultOperation =
                                                                    null)
        {
            // Build a default operation
            resultOperation = resultOperation ?? BuildMockSearchOperation(mockBehavior).Object;

            // Set up the search provider
            var searchProvider = new Mock<ISearchProvider>(mockBehavior);

            // When searching, there should always be an operation
            searchProvider.Setup(p => p.PerformSearch(It.IsAny<string>())).Returns(resultOperation);

            // Return a unique identifier for the Id to prevent collisions
            searchProvider.SetupGet(p => p.Id).Returns(Guid.NewGuid().ToString);

            return searchProvider;
        }

        /// <summary>
        ///     Builds a mock module.
        /// </summary>
        /// <param name="mockingBehavior">The mocking behavior.</param>
        /// <returns>A mock module</returns>
        protected Mock<IAlfredModule> BuildMockModule(MockBehavior mockingBehavior)
        {
            var mock = new Mock<IAlfredModule>(mockingBehavior);

            // Configure Common Properties
            SetupMockComponent(mock);

            // Setup Methods
            mock.Setup(m => m.OnRegistered(It.IsAny<IAlfred>()));

            // Build out collections
            mock.SetupGet(m => m.Widgets).Returns(Container.ProvideCollection<IWidget>());

            return mock;
        }

        /// <summary>
        ///     Builds a mock operation.
        /// </summary>
        /// <param name="mockBehavior">The mocking behavior.</param>
        /// <returns>A mock operation.</returns>
        protected Mock<ISearchOperation> BuildMockSearchOperation(MockBehavior mockBehavior)
        {
            var mock = new Mock<ISearchOperation>(mockBehavior);

            // Setup properties
            mock.SetupGet(m => m.EncounteredError).Returns(false);
            mock.SetupGet(m => m.ErrorMessage).Returns(string.Empty);
            mock.SetupGet(m => m.IsSearchComplete).Returns(false);
            mock.SetupGet(m => m.Results).Returns(Container.ProvideCollection<ISearchResult>());

            // Setup simple methods
            mock.Setup(m => m.Update());

            // Abort will set the operation to have completed with errors
            Action abortedCallback = () =>
                                     {
                                         mock.SetupGet(o => o.IsSearchComplete).Returns(true);
                                         mock.SetupGet(o => o.EncounteredError).Returns(true);
                                         mock.SetupGet(o => o.ErrorMessage)
                                             .Returns("The search was aborted");
                                     };

            mock.Setup(m => m.Abort()).Callback(abortedCallback);

            return mock;
        }

        /// <summary>
        ///     Builds a mock search result.
        /// </summary>
        /// <param name="mockingBehavior">The mocking behavior used when creating Moq mocks.</param>
        /// <returns>A mock search result</returns>
        protected static Mock<ISearchResult> BuildMockSearchResult(MockBehavior mockingBehavior)
        {
            var mock = new Mock<ISearchResult>(mockingBehavior);

            return mock;
        }

        /// <summary>
        ///     Builds a mock widget.
        /// </summary>
        /// <param name="mockingBehavior"> The mocking behavior. </param>
        /// <returns>
        ///     A mock widget.
        /// </returns>
        protected static Mock<IWidget> BuildMockWidget(MockBehavior mockingBehavior)
        {
            var mock = new Mock<IWidget>(mockingBehavior);

            // By default widgets will be visible
            mock.SetupGet(m => m.IsVisible).Returns(true);

            return mock;
        }
    }
}