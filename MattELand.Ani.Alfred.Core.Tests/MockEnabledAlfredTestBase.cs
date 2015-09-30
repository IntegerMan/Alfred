// ---------------------------------------------------------
// MockEnabledAlfredTestBase.cs
// 
// Created on:      09/11/2015 at 10:32 PM
// Last Modified:   09/11/2015 at 10:48 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Console;
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
        /// <returns>A mock subsystem</returns>
        protected Mock<IAlfredSubsystem> BuildMockSubsystem()
        {
            // Build the Mock
            var mock = new Mock<IAlfredSubsystem>(MockingBehavior);

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
        /// <returns>
        ///     The mock page.
        /// </returns>
        protected Mock<IPage> BuildMockPage()
        {
            // Some tests will want strict control over mocking and others won't
            var mock = new Mock<IPage>(MockingBehavior);

            SetupMockComponent(mock);

            // Set up simple members we expect to be hit during startup
            mock.SetupGet(p => p.IsRootLevel).Returns(true);
            mock.SetupGet(p => p.Id).Returns("TestPage");
            mock.SetupGet(p => p.Name).Returns("Test Page");

            return mock;
        }

        /// <summary>
        ///     Sets up a <paramref name="mock" /> component with common component actions.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="mock">
        /// The Mock object that derives from <see cref="IAlfredComponent" /> .
        /// </param>
        private void SetupMockComponent<T>(Mock<T> mock) where T : class, IAlfredComponent
        {
            // Setup Simple properties
            mock.SetupGet(s => s.NameAndVersion).Returns("Test Component 1.0.0.0");
            mock.SetupGet(s => s.Status).Returns(AlfredStatus.Offline);
            mock.SetupGet(m => m.Container).Returns(Container);

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
        /// <returns>The mock search controller</returns>
        protected Mock<ISearchController> BuildMockSearchController()
        {
            var mock = new Mock<ISearchController>(MockingBehavior);

            SetupMockComponent(mock);

            mock.SetupGet(m => m.StatusMessage).Returns("No searches have been made yet.");
            mock.SetupGet(m => m.Results).Returns(Container.ProvideCollection<ISearchResult>());

            return mock;
        }

        /// <summary>
        ///     Builds mock search provider.
        /// </summary>
        /// <param name="resultOperation">The result operation.</param>
        /// <returns>A mock search provider.</returns>
        protected Mock<ISearchProvider> BuildMockSearchProvider(ISearchOperation resultOperation = null)
        {
            // Build a default operation
            resultOperation = resultOperation ?? BuildMockSearchOperation().Object;

            // Set up the search provider
            var searchProvider = new Mock<ISearchProvider>();

            // When searching, there should always be an operation
            searchProvider.Setup(p => p.PerformSearch(It.IsAny<string>())).Returns(resultOperation);

            // Return a unique identifier for the Id to prevent collisions
            searchProvider.SetupGet(p => p.Id).Returns(Guid.NewGuid().ToString);

            return searchProvider;
        }

        /// <summary>
        ///     Builds a mock module.
        /// </summary>
        /// <returns>A mock module</returns>
        protected Mock<IAlfredModule> BuildMockModule()
        {
            var mock = new Mock<IAlfredModule>(MockingBehavior);

            // Configure Common Properties
            SetupMockComponent(mock);

            // Setup Methods
            mock.Setup(m => m.OnRegistered(It.IsAny<IAlfred>()));
            mock.Setup(m => m.Register(It.IsAny<IWidget>()));
            mock.Setup(m => m.Register(It.IsAny<IEnumerable<IWidget>>()));

            // Build out collections
            mock.SetupGet(m => m.Widgets).Returns(Container.ProvideCollection<IWidget>());

            return mock;
        }

        /// <summary>
        ///     Builds a mock operation.
        /// </summary>
        /// <returns>A mock operation.</returns>
        protected Mock<ISearchOperation> BuildMockSearchOperation()
        {
            var mock = new Mock<ISearchOperation>(MockingBehavior);

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
        /// <returns>A mock search result</returns>
        protected Mock<ISearchResult> BuildMockSearchResult()
        {
            var mock = new Mock<ISearchResult>(MockingBehavior);

            mock.SetupGet(m => m.MoreDetailsAction).Returns(null);
            mock.SetupGet(m => m.MoreDetailsText).Returns(string.Empty);

            return mock;
        }

        /// <summary>
        ///     Builds a mock widget.
        /// </summary>
        /// <returns>
        ///     A mock widget.
        /// </returns>
        protected Mock<IWidget> BuildMockWidget()
        {
            var mock = new Mock<IWidget>(MockingBehavior);

            // By default widgets will be visible
            mock.SetupGet(m => m.IsVisible).Returns(true);

            return mock;
        }

        /// <summary>
        ///     Builds a mock Alfred instance.
        /// </summary>
        /// <returns>
        ///     A Mock Alfred instance.
        /// </returns>
        protected Mock<IAlfred> BuildMockAlfred()
        {
            var mock = new Mock<IAlfred>(MockingBehavior);

            mock.Setup(m => m.Initialize());

            mock.SetupGet(m => m.Container).Returns(Container);

            return mock;
        }

        /// <summary>
        ///     Builds a mock console.
        /// </summary>
        /// <returns>
        ///     A mock console;
        /// </returns>
        protected Mock<IConsole> BuildMockConsole()
        {
            var console = new Mock<IConsole>(MockingBehavior);

            console.SetupGet(c => c.Container).Returns(Container);

            console.Setup(c => c.Log(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<LogLevel>()));

            return console;
        }

        /// <summary>
        ///     The mocking behavior to use when creating Mock objects.
        /// </summary>
        protected MockBehavior MockingBehavior { get; set; } = Moq.MockBehavior.Strict;

        /// <summary>
        ///     Builds a mock chat provider.
        /// </summary>
        /// <returns>
        ///     A Mock chat provider
        /// </returns>
        [NotNull]
        protected Mock<IChatProvider> BuildMockChatProvider()
        {
            var mock = new Mock<IChatProvider>(MockingBehavior);

            return mock;
        }

        /// <summary>
        ///     Builds mock property item.
        /// </summary>
        /// <param name="displayName"> The node's display name. </param>
        /// <param name="displayValue"> The display value. </param>
        /// <returns>
        ///     A mock property item
        /// </returns>
        protected Mock<IPropertyItem> BuildMockPropertyItem(string displayName, string displayValue)
        {
            var mockProperty = new Mock<IPropertyItem>(MockingBehavior);

            mockProperty.SetupGet(p => p.DisplayName).Returns(displayName);
            mockProperty.SetupGet(p => p.DisplayValue).Returns(displayValue);

            return mockProperty;
        }

        /// <summary>
        ///     Builds a mock property provider.
        /// </summary>
        /// <param name="name"> The display name. </param>
        /// <param name="properties"> The properties for the node. </param>
        /// <param name="children"> The children for the node. </param>
        /// <returns>
        ///     A mock property provider.
        /// </returns>
        protected Mock<IPropertyProvider> BuildMockPropertyProvider(
            string name = "A Test Node",
            IEnumerable<IPropertyItem> properties = null,
            IEnumerable<IPropertyProvider> children = null)
        {
            // Create a mock for that
            var mock = new Mock<IPropertyProvider>(MockingBehavior);

            // Build Basic Properties
            mock.SetupGet(m => m.DisplayName).Returns(name);
            mock.SetupGet(m => m.ItemTypeName).Returns("Test Node");

            // Build out a collection of children if none were provided
            children = children ?? Container.ProvideCollection<IPropertyProvider>();
            mock.SetupGet(m => m.PropertyProviders).Returns(children);

            // Build out a collection of properties if none were provided
            properties = properties ?? Container.ProvideCollection<IPropertyItem>();
            mock.SetupGet(m => m.Properties).Returns(properties);

            return mock;
        }
    }
}