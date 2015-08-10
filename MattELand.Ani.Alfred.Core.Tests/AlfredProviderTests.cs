// ---------------------------------------------------------
// AlfredProviderTests.cs
// 
// Created on:      07/25/2015 at 11:43 PM
// Last Modified:   08/07/2015 at 11:12 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Modules;
using MattEland.Ani.Alfred.Core.Pages;
using MattEland.Ani.Alfred.Core.Tests.Mocks;
using MattEland.Ani.Alfred.Core.Widgets;

using NUnit.Framework;

namespace MattEland.Ani.Alfred.Core.Tests
{
    /// <summary>
    ///     Tests AlfredProvider
    /// </summary>
    [TestFixture]
    [SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    public sealed class AlfredProviderTests
    {
        /// <summary>
        ///     Sets up the alfred provider's tests.
        /// </summary>
        [SetUp]
        public void SetupAlfredProviderTests()
        {
            var bootstrapper = new AlfredBootstrapper();
            _alfred = bootstrapper.Create();
            _alfred.Console = new SimpleConsole();

            _subsystem = new TestSubsystem(_alfred.PlatformProvider);
            _page = new AlfredModuleListPage(_alfred.PlatformProvider, "Test Page");
        }

        [NotNull]
        private AlfredProvider _alfred;

        [NotNull]
        private TestSubsystem _subsystem;

        [NotNull]
        private AlfredModuleListPage _page;

        [Test]
        public void AddingStandardModulesAddsModules()
        {
            _alfred.Register(new AlfredControlSubsystem(_alfred.PlatformProvider));

            var numModules = 0;

            foreach (var subsystem in _alfred.Subsystems)
            {
                numModules += subsystem.Modules.Count();

                foreach (var page in subsystem.Pages)
                {
                    var modulePage = page as AlfredModuleListPage;

                    if (modulePage != null)
                    {
                        numModules += modulePage.Modules.Count();
                    }
                }
            }

            Assert.Greater(numModules,
                           0,
                           "Alfred did not have any modules after calling add standard modules.");
        }

        [Test]
        public void AfterInitializationAlfredIsOnline()
        {
            _alfred.Initialize();

            Assert.AreEqual(_alfred.Status, AlfredStatus.Online);
        }

        [Test]
        public void AlfredStartsOffline()
        {
            Assert.AreEqual(_alfred.Status, AlfredStatus.Offline);
        }

        [Test]
        public void AlfredStartsWithNoSubSystems()
        {
            Assert.AreEqual(0, _alfred.Subsystems.Count(), "Alfred started with subsystems when none were expected.");
        }

        [Test]
        public void InitializationWithoutConsoleWorks()
        {
            _alfred.Console = null;

            _alfred.Initialize();

            // Nothing to assert here - I'm looking for errors encountered
        }

        /// <summary>
        ///     Tests initialization of Alfred
        /// </summary>
        [Test]
        public void InitializeAlfred()
        {
            Assert.NotNull(_alfred, "Alfred was not initialized");
        }

        [Test]
        public void InitializeAndShutdownResultsInShutdown()
        {
            _alfred.Initialize();
            _alfred.Shutdown();

            Assert.AreEqual(_alfred.Status, AlfredStatus.Offline);
        }

        [Test]
        public void InitializeCreatesEventsInLog()
        {
            var console = _alfred.Console;
            Assert.NotNull(console, "Console was not present");

            var numEvents = console.Events.Count();

            _alfred.Initialize();

            Assert.Greater(console.Events.Count(), numEvents, "Initializing Alfred did not create any log entries.");
        }

        [Test]
        public void InitializeWhileOnlineErrors()
        {
            _alfred.Initialize();

            try
            {
                _alfred.Initialize();

                Assert.Fail("Expected an InvalidOperationException since Alfred was already initialized.");

            }
            catch (InvalidOperationException)
            {
                // No action
            }

            // Assert that we're still online.
            Assert.AreEqual(_alfred.Status, AlfredStatus.Online);
        }

        [Test]
        public void InitializingInitializesComponents()
        {
            _alfred.Register(new AlfredControlSubsystem(_alfred.PlatformProvider));

            _alfred.Initialize();

            foreach (var item in _alfred.Subsystems)
            {
                Assert.AreEqual(AlfredStatus.Online,
                                item.Status,
                                $"{item.NameAndVersion} was not initialized during initialization.");
            }
        }

        [Test]
        public void LogToConsole()
        {
            var console = _alfred.Console;
            Assert.NotNull(console, "Console was not present");

            var numEvents = console.Events.Count();

            console.Log("Alfred Test Framework", "Testing logging to Alfred", LogLevel.Verbose);

            Assert.AreEqual(numEvents + 1, console.Events.Count(), "Event count did not increase after logging.");
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ModulesCannotBeAddedWhileOnline()
        {
            _alfred.Initialize();
            _alfred.Register(new AlfredControlSubsystem(_alfred.PlatformProvider));
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ModulesCannotUpdateWhileOffline()
        {
            _alfred.Register(new AlfredControlSubsystem(_alfred.PlatformProvider));

            _alfred.Update();
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RegisteringAWidgetMultipleTimesThrowsAnException()
        {
            var testModule = new AlfredTestModule();

            var textWidget = new TextWidget();
            testModule.WidgetsToRegisterOnInitialize.Add(textWidget);
            testModule.WidgetsToRegisterOnInitialize.Add(textWidget);

            _alfred.Register(_subsystem);
            _subsystem.AddAutoRegisterPage(_page);
            _page.Register(testModule);

            _alfred.Initialize();
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void RegisteringNullSubsystemGeneratesNullRef()
        {
            AlfredSubsystem system = null;
            _alfred.Register(system);
        }

        [Test]
        public void RegisteringWidgetAtInitializeAndShutdownLeavesOneCopyInListAtReinitialize()
        {
            var testModule = new AlfredTestModule();

            var textWidget = new TextWidget();
            testModule.WidgetsToRegisterOnInitialize.Add(textWidget);
            testModule.WidgetsToRegisterOnShutdown.Add(textWidget);

            _alfred.Register(_subsystem);
            _subsystem.AddAutoRegisterPage(_page);
            _page.Register(testModule);

            _alfred.Initialize();
            _alfred.Update();
            _alfred.Shutdown();
            _alfred.Initialize();
            _alfred.Update();

            Assert.IsNotNull(testModule.Widgets, "testModule.Widgets was null");
            Assert.AreEqual(1,
                            testModule.Widgets.Count(),
                            "Widgets were not properly cleared from list after re-initialize");

        }

        [Test]
        public void RemoveAlfredConsole()
        {
            _alfred.Console = null;

            Assert.IsNull(_alfred.Console, "Could not remove Alfred's console");
        }

        [Test]
        public void SetConsole()
        {
            Assert.IsNotNull(_alfred.Console, "Alfred's console was null after creation");
        }

        [Test]
        public void ShutdownCreatesEventsInLog()
        {
            // We need to be online to shut down or else we'll get errors
            _alfred.Initialize();

            var console = _alfred.Console;
            Assert.NotNull(console, "Console was not present");

            var numEvents = console.Events.Count();

            _alfred.Shutdown();

            Assert.Greater(console.Events.Count(), numEvents, "Shutting Alfred down did not create any log entries.");
        }

        [Test]
        public void ShutdownWhileOfflineErrors()
        {
            try
            {
                _alfred.Shutdown();

                Assert.Fail("Expected an InvalidOperationException since Alfred was already offline.");

            }
            catch (InvalidOperationException)
            {
                // No action
            }

            // Assert that we're now offline.
            Assert.AreEqual(_alfred.Status, AlfredStatus.Offline);
        }

        [Test]
        public void ShuttingDownShutsDownComponents()
        {
            _alfred.Register(new AlfredControlSubsystem(_alfred.PlatformProvider));

            _alfred.Initialize();
            _alfred.Shutdown();

            foreach (var item in _alfred.Subsystems)
            {
                Assert.AreEqual(AlfredStatus.Offline,
                                item.Status,
                                $"{item.NameAndVersion} was not shut down during alfred shut down.");
            }
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void UpdateWithNoModulesWhileOfflineStillGeneratesError()
        {
            _alfred.Update();
        }
    }

}