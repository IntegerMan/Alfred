// ---------------------------------------------------------
// AlfredProviderTests.cs
// 
// Created on:      07/25/2015 at 11:43 PM
// Last Modified:   08/07/2015 at 11:12 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.Linq;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.Core.Modules;
using MattEland.Ani.Alfred.Core.Tests.Mocks;
using MattEland.Ani.Alfred.Core.Widgets;

using NUnit.Framework;

namespace MattEland.Ani.Alfred.Core.Tests
{
    /// <summary>
    ///     Tests AlfredProvider
    /// </summary>
    [TestFixture]
    public sealed class AlfredProviderTests
    {
        /// <summary>
        ///     Sets up the alfred provider's tests.
        /// </summary>
        [SetUp]
        public void SetupAlfredProviderTests()
        {
            _alfred = new AlfredProvider
            {
                Console = new SimpleConsole()
            };
        }

        [NotNull]
        private AlfredProvider _alfred;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        public AlfredProviderTests()
        {
            _alfred = new AlfredProvider();
        }

        [Test]
        public void AddingStandardModulesAddsModules()
        {
            StandardModuleProvider.AddStandardModules(_alfred);

            Assert.Greater(_alfred.Modules.Count,
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
        public void AlfredStartsWithNoModules()
        {
            Assert.AreEqual(0, _alfred.Modules.Count, "Alfred started with modules when none were expected.");
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
        public void InitializeCausesRegisteredSubSystemsToInitialize()
        {
            var testSubsystem = new TestSubSystem();

            _alfred.RegisterSubSystem(testSubsystem);

            _alfred.Initialize();

            Assert.IsTrue(testSubsystem.LastInitialized > DateTime.MinValue, "Subsystem was not initialized");
            Assert.IsTrue(testSubsystem.LastInitializationCompleted > DateTime.MinValue,
                          "Subsystem was not notified initialized completed");
        }

        [Test]
        public void InitializeCausesSubSystemsToGoOnline()
        {
            var testSubsystem = new TestSubSystem();

            _alfred.RegisterSubSystem(testSubsystem);

            _alfred.Initialize();

            Assert.AreEqual(AlfredStatus.Online, testSubsystem.Status);
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
        public void InitializingInitializesModules()
        {
            StandardModuleProvider.AddStandardModules(_alfred);

            _alfred.Initialize();

            foreach (var module in _alfred.Modules)
            {
                Assert.AreEqual(AlfredStatus.Online,
                                module.Status,
                                $"Module {module.NameAndVersion} was not initialized during initialization.");
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
            StandardModuleProvider.AddStandardModules(_alfred);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ModulesCannotUpdateWhileOffline()
        {
            StandardModuleProvider.AddStandardModules(_alfred);

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

            _alfred.AddModule(testModule);

            _alfred.Initialize();
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RegisteringNullSubsystemGeneratesNullRef()
        {
            _alfred.RegisterSubSystem(null);
        }

        [Test]
        public void RegisteringWidgetAtInitializeAndShutdownLeavesOneCopyInListAtReinitialize()
        {
            var testModule = new AlfredTestModule();

            var textWidget = new TextWidget();
            testModule.WidgetsToRegisterOnInitialize.Add(textWidget);
            testModule.WidgetsToRegisterOnShutdown.Add(textWidget);

            _alfred.AddModule(testModule);

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
        public void ShuttingDownShutsDownModules()
        {
            StandardModuleProvider.AddStandardModules(_alfred);

            _alfred.Initialize();
            _alfred.Shutdown();

            foreach (var module in _alfred.Modules)
            {
                Assert.AreEqual(AlfredStatus.Offline,
                                module.Status,
                                $"Module {module.NameAndVersion} was not shut down during alfred shut down.");
            }
        }

        [Test]
        public void UpdateCausesRegisteredSubSystemsToUpdate()
        {
            var testSubsystem = new TestSubSystem();

            _alfred.RegisterSubSystem(testSubsystem);

            _alfred.Initialize();
            _alfred.Update();

            Assert.IsTrue(testSubsystem.LastUpdated > DateTime.MinValue, "Subsystem was not updated");
        }

        [Test]
        public void ShutdownCausesRegisteredSubSystemsToShutdown()
        {
            var testSubsystem = new TestSubSystem();

            _alfred.RegisterSubSystem(testSubsystem);

            _alfred.Initialize();
            _alfred.Update();
            _alfred.Shutdown();

            Assert.IsTrue(testSubsystem.LastShutdown > DateTime.MinValue, "Subsystem was not shut down");
            Assert.IsTrue(testSubsystem.LastShutdownCompleted > DateTime.MinValue, "Subsystem was not notified of shut down completion");
        }

        [Test]
        public void ShutdownCausesRegisteredSubSystemsToGoOffline()
        {
            var testSubsystem = new TestSubSystem();

            _alfred.RegisterSubSystem(testSubsystem);

            _alfred.Initialize();
            _alfred.Update();
            _alfred.Shutdown();

            Assert.AreEqual(AlfredStatus.Offline, testSubsystem.Status);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void UpdateWithNoModulesWhileOfflineStillGeneratesError()
        {
            _alfred.Update();
        }
    }

}