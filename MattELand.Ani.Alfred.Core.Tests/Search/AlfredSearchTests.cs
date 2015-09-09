// ---------------------------------------------------------
// AlfredSearchTests.cs
// 
// Created on:      09/09/2015 at 12:10 AM
// Last Modified:   09/09/2015 at 11:33 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

using MattEland.Ani.Alfred.Core;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Common.Providers;
using MattEland.Testing;

using Moq;

using NUnit.Framework;

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
    public sealed class AlfredSearchTests : AlfredTestBase
    {

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
            var alfred = CreateAlfredInstance();

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

            Alfred = CreateAlfredInstance();

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

            Alfred = CreateAlfredInstance();

            //! Act
            Alfred.Initialize();
            Alfred.Shutdown();

            //! Assert
            mock.Verify(c => c.Shutdown(), Times.Once);
            mock.Verify(c => c.OnShutdownCompleted(), Times.Once);
        }

    }
}