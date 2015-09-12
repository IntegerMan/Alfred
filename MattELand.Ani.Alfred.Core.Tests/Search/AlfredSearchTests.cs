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
    public sealed class AlfredSearchTests : MockEnabledAlfredTestBase
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
            var provider = BuildMockSearchProvider(MockBehavior);
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


    }
}