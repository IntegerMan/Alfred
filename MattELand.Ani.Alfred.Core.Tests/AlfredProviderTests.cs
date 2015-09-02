// ---------------------------------------------------------
// AlfredProviderTests.cs
// 
// Created on:      08/19/2015 at 9:31 PM
// Last Modified:   08/24/2015 at 11:51 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core;
using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Pages;
using MattEland.Ani.Alfred.Core.Subsystems;
using MattEland.Ani.Alfred.Core.Widgets;
using MattEland.Ani.Alfred.Tests.Mocks;
using MattEland.Common.Providers;
using MattEland.Testing;

using NUnit.Framework;

using Shouldly;

namespace MattEland.Ani.Alfred.Tests
{
    /// <summary>
    ///     Tests <see cref="AlfredApplication"/>
    /// </summary>
    [UnitTest]
    [SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    public sealed class AlfredProviderTests : AlfredTestBase
    {
        /// <summary>
        ///     Sets up the Alfred provider's tests.
        /// </summary>
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            var alfred = new AlfredApplication(Container);
            alfred.RegisterAsProvidedInstance(typeof(IAlfred), Container);
            _subsystem = new TestSubsystem(Container);
            _page = new AlfredModuleListPage(Container, "Test Page", "Test");

        }

        [NotNull]
        private TestSubsystem _subsystem;

        [NotNull]
        private AlfredModuleListPage _page;

        /// <summary>
        ///     Builds widget parameters.
        /// </summary>
        /// <param name="name">The name of the widget.</param>
        /// <returns>The <see cref="WidgetCreationParameters"/>.</returns>
        [NotNull]
        private WidgetCreationParameters BuildWidgetParams(string name = "WidgetTest")
        {
            return new WidgetCreationParameters(name);
        }

        /// <summary>
        ///     Tests that adding standard modules adds modules.
        /// </summary>
        [Test]
        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        public void AddingStandardModulesAddsModules()
        {
            Alfred.Register(new AlfredCoreSubsystem(Container));

            var numModules =
                Alfred.Subsystems.SelectMany(subsystem => subsystem.Pages)
                       .OfType<AlfredModuleListPage>()
                       .Sum(modulePage => modulePage.Modules.Count());

            numModules.ShouldBeGreaterThan(0, "Alfred did not have any modules after calling add standard modules.");
        }

        /// <summary>
        ///     Tests that after initialization Alfred is online.
        /// </summary>
        [Test]
        public void AfterInitializationAlfredIsOnline()
        {
            Alfred.Initialize();

            Alfred.Status.ShouldBe(AlfredStatus.Online);
        }

        /// <summary>
        ///    Test that Alfred starts offline.
        /// </summary>
        [Test]
        public void AlfredStartsOffline()
        {
            Alfred.Status.ShouldBe(AlfredStatus.Offline);
        }

        /// <summary>
        ///     Alfred should start with no sub systems.
        /// </summary>
        [Test]
        public void AlfredStartsWithNoSubSystems()
        {
            Alfred.Subsystems.Count().ShouldBe(0,
                            "Alfred started with subsystems when none were expected.");
        }

        /// <summary>
        ///     Tests initialization of Alfred
        /// </summary>
        [Test]
        public void InitializeAlfred()
        {
            Assert.NotNull(Alfred, "Alfred was not initialized");
        }

        /// <summary>
        ///     Tests that initialization followed by shutdown results in shutdown.
        /// </summary>
        [Test]
        public void InitializeAndShutdownResultsInShutdown()
        {
            Alfred.Initialize();
            Alfred.Shutdown();

            Alfred.Status.ShouldBe(AlfredStatus.Offline);
        }

        /// <summary>
        ///     Tests that initializing while online causes issues
        /// </summary>
        [Test, ExpectedException(typeof(InvalidOperationException))]
        public void InitializeWhileOnlineErrors()
        {
            Alfred.Initialize();
            Alfred.Initialize();
        }

        /// <summary>
        ///     Tests that initializing Alfred causes components to initialize
        /// </summary>
        [Test]
        public void InitializingInitializesComponents()
        {
            Alfred.Register(new AlfredCoreSubsystem(Container));

            Alfred.Initialize();

            foreach (var item in Alfred.Subsystems)
            {
                item.Status.ShouldBe(AlfredStatus.Online,
                                $"{item.NameAndVersion} was not initialized during initialization.");
            }
        }

        /// <summary>
        ///     Tests that logging causes events to be logged
        /// </summary>
        [Test]
        public void LogToConsole()
        {
            var console = Container.Provide<IConsole>();
            console.ShouldNotBeNull("Console was not present");

            var numEvents = console.Events.Count();

            console.Log("Alfred Test Framework", "Testing logging to Alfred", LogLevel.Verbose);

            console.Events.Count().ShouldBe(numEvents + 1,
                "Event count did not increase after logging.");
        }

        /// <summary>
        ///     Ensures Modules cannot be added while online.
        /// </summary>
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ModulesCannotBeAddedWhileOnline()
        {
            Alfred.Initialize();
            Alfred.Register(new AlfredCoreSubsystem(Container));
        }

        /// <summary>
        ///     Ensures Modules cannot update while offline.
        /// </summary>
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ModulesCannotUpdateWhileOffline()
        {
            Alfred.Register(new AlfredCoreSubsystem(Container));

            Alfred.Update();
        }

        /// <summary>
        ///     Ensures that registering a widget multiple times throws an exception.
        /// </summary>
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RegisteringAWidgetMultipleTimesThrowsAnException()
        {
            var testModule = new AlfredTestModule(Container);

            var textWidget = new TextWidget(BuildWidgetParams());
            testModule.WidgetsToRegisterOnInitialize.Add(textWidget);
            testModule.WidgetsToRegisterOnInitialize.Add(textWidget);

            Alfred.Register(_subsystem);
            _subsystem.AddAutoRegisterPage(_page);
            _page.Register(testModule);

            Alfred.Initialize();
        }

        /// <summary>
        ///     Registering null subsystem generates null reference exceptions.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void RegisteringNullSubsystemGeneratesNullRef()
        {
            AlfredSubsystem system = null;
            Alfred.Register(system);
        }

        /// <summary>
        ///     Ensures registering widget at initialize and shutdown leaves one copy in list at reinitialize.
        /// </summary>
        [Test]
        public void RegisteringWidgetAtInitializeAndShutdownLeavesOneCopyInListAtReinitialize()
        {
            var testModule = new AlfredTestModule(Container);

            var textWidget = new TextWidget(BuildWidgetParams());
            testModule.WidgetsToRegisterOnInitialize.Add(textWidget);
            testModule.WidgetsToRegisterOnShutdown.Add(textWidget);

            Alfred.Register(_subsystem);
            _subsystem.AddAutoRegisterPage(_page);
            _page.Register(testModule);

            Alfred.Initialize();
            Alfred.Update();
            Alfred.Shutdown();
            Alfred.Initialize();
            Alfred.Update();

            Assert.IsNotNull(testModule.Widgets, "testModule.Widgets was null");
            Assert.AreEqual(1,
                            testModule.Widgets.Count(),
                            "Widgets were not properly cleared from list after re-initialize");
        }

        /// <summary>
        ///     Ensures shutdown creates events in log.
        /// </summary>
        [Test]
        public void ShutdownCreatesEventsInLog()
        {
            var container = Container;
            var al = Alfred;
            var console = Container.Provide<IConsole>();

            // We need to be online to shut down or else we'll get errors
            al.Initialize();
            al.Container.ShouldBeSameAs(container);

            var numEvents = console.EventCount;

            al.Shutdown();

            if (console.EventCount <= numEvents)
            {
                var message = $"Shutting Alfred down did not create any log entries ({console.EventCount}) on container {container.Name} with collection of type {console.Events.GetType()}";
                Assert.Fail(message);
            }
        }

        /// <summary>
        ///     Tests that starting Alfred creates log entries
        /// </summary>
        [Test]
        public void InitializeCreatesEventsInLog()
        {
            var container = Container;
            var al = Alfred;
            var console = Container.Provide<IConsole>();

            var numEvents = console.EventCount;

            al.Initialize();
            al.Container.ShouldBeSameAs(container);

            if (console.EventCount <= numEvents)
            {
                var message = $"Initializing Alfred did not create any log entries ({console.EventCount}) on container {container.Name} with collection of type {console.Events.GetType()}";
                Assert.Fail(message);
            }
        }

        /// <summary>
        ///     Ensures shutdown while offline errors.
        /// </summary>
        [Test]
        public void ShutdownWhileOfflineErrors()
        {
            try
            {
                Alfred.Shutdown();

                Assert.Fail("Expected an InvalidOperationException since Alfred was already offline.");
            }
            catch (InvalidOperationException)
            {
                // No action
            }

            // Assert that we're now offline.
            Assert.AreEqual(Alfred.Status, AlfredStatus.Offline);
        }

        /// <summary>
        ///     Shutting down shuts down components.
        /// </summary>
        [Test]
        public void ShuttingDownShutsDownComponents()
        {
            Alfred.Register(new AlfredCoreSubsystem(Container));

            Alfred.Initialize();
            Alfred.Shutdown();

            foreach (var item in Alfred.Subsystems)
            {
                Assert.AreEqual(AlfredStatus.Offline,
                                item.Status,
                                $"{item.NameAndVersion} was not shut down during alfred shut down.");
            }
        }

        /// <summary>
        ///     Updates the with no modules while offline still generates errors.
        /// </summary>
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void UpdateWithNoModulesWhileOfflineStillGeneratesError()
        {
            Alfred.Update();
        }
    }

}