// ---------------------------------------------------------
// PageTests.cs
// 
// Created on:      08/19/2015 at 9:31 PM
// Last Modified:   09/05/2015 at 10:45 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Tests.Mocks;
using MattEland.Testing;

using Moq;

using NUnit.Framework;

using Shouldly;

namespace MattEland.Ani.Alfred.Tests.Pages
{
    /// <summary>
    /// Tests oriented around testing the page update pumps and related functions
    /// </summary>
    [UnitTestProvider]
    [SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    [SuppressMessage("ReSharper", "ExceptionNotDocumented")]
    public class PageTests : AlfredTestBase
    {

        [NotNull]
        private Mock<IAlfredPage> _pageMock;

        [NotNull]
        private TestSubsystem _subsystem;

        /// <summary>
        ///     Arranges the scenario so that a page mock is set inside of Alfred inside of a test subsystem.
        /// </summary>
        /// <param name="mockBehavior"> The mock behavior. </param>
        private void ArrangeScenario(MockBehavior mockBehavior)
        {
            // The class we're testing here is the main Alfred pump
            Alfred = new AlfredApplication(Container);

            // TODO: This should be a mock or simple non-test object
            _subsystem = new TestSubsystem(Container);

            _pageMock = BuildPageMock(mockBehavior);

            _subsystem.AddAutoRegisterPage(_pageMock.Object);
            Alfred.Register(_subsystem);
        }

        /// <summary>
        ///     Builds a page mock.
        /// </summary>
        /// <param name="mockBehavior"> The mocking behavior for the new mock. </param>
        /// <returns>
        ///     The mock page.
        /// </returns>
        private Mock<IAlfredPage> BuildPageMock(MockBehavior mockBehavior)
        {
            // Some tests will want strict control over mocking and others won't
            var mock = new Mock<IAlfredPage>(mockBehavior);

            // Set up simple members we expect to be hit during startup
            mock.SetupGet(p => p.IsRootLevel).Returns(true);
            mock.Setup(p => p.OnRegistered(It.Is<IAlfred>(a => a == Alfred)));
            mock.Setup(p => p.OnInitializationCompleted());
            mock.Setup(p => p.Update());
            mock.Setup(p => p.OnShutdownCompleted());

            // When initialize is hit, set Status to Online
            mock.Setup(p => p.Initialize(It.Is<IAlfred>(a => a == Alfred)))
                .Callback(() => mock.SetupGet(p => p.Status)
                    .Returns(AlfredStatus.Online));

            // When shutdown is hit, set Status to Offline
            mock.Setup(p => p.Shutdown())
                .Callback(() => mock.SetupGet(p => p.Status)
                .Returns(AlfredStatus.Offline));

            return mock;
        }

        /// <summary>
        ///     When Alfred is initialized, a message will be sent to all member pages to initialize.
        /// </summary>
        [Test]
        public void InitializeCausesPagesToGoOnline()
        {
            //! Arrange
            ArrangeScenario(MockBehavior.Loose);

            //! Act
            Alfred.Initialize();

            //! Assert
            _pageMock.Object.Status.ShouldBe(AlfredStatus.Online);
        }

        /// <summary>
        ///     When Alfred is initialized, we expect member pages to get calls to Initialize and
        ///     OnInitializationCompleted.
        /// </summary>
        [Test]
        public void InitializeCausesRegisteredPagesToInitialize()
        {
            //! Arrange
            ArrangeScenario(MockBehavior.Strict);

            //! Act
            Alfred.Initialize();

            //! Assert
            _pageMock.Verify(p => p.Initialize(It.Is<IAlfred>(a => a == Alfred)), Times.Once);
            _pageMock.Verify(p => p.OnInitializationCompleted(), Times.Once);
            _pageMock.Verify(p => p.Update(), Times.Never);
            _pageMock.Verify(p => p.Shutdown(), Times.Never);
            _pageMock.Verify(p => p.OnShutdownCompleted(), Times.Never);
        }

        /// <summary>
        ///     Shutdown causes registered pages to go offline.
        /// </summary>
        [Test]
        public void ShutdownCausesRegisteredPagesToGoOffline()
        {
            //! Arrange
            ArrangeScenario(MockBehavior.Loose);

            //! Act
            Alfred.Initialize();
            Alfred.Update();
            Alfred.Shutdown();

            //! Assert
            _pageMock.Object.Status.ShouldBe(AlfredStatus.Offline);
        }

        /// <summary>
        ///     Shutdown causes registered pages to receive shutdown and shutdown completed calls
        /// </summary>
        [Test]
        public void ShutdownCausesRegisteredPagesToShutdown()
        {
            //! Arrange
            ArrangeScenario(MockBehavior.Strict);

            //! Act
            Alfred.Initialize();
            Alfred.Update();
            Alfred.Shutdown();

            //! Assert
            _pageMock.Verify(p => p.Shutdown(), Times.Once);
            _pageMock.Verify(p => p.OnShutdownCompleted(), Times.Once);
        }

        /// <summary>
        ///     When Alfred updates, members will receive an update call.
        /// </summary>
        [Test]
        public void UpdateCausesRegisteredPagesToUpdate()
        {
            //! Arrange
            ArrangeScenario(MockBehavior.Loose);

            //! Act
            Alfred.Initialize();
            Alfred.Update();

            //! Assert
            _pageMock.Verify(p => p.Update(), Times.Once);
        }
    }
}