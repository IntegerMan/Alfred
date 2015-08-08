using System;
using System.Diagnostics;

using NUnit.Framework;
using System.Linq;
using System.Runtime.CompilerServices;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.Core.Modules;
using MattEland.Ani.Alfred.Core.Tests.Mocks;
using MattEland.Ani.Alfred.Core.Widgets;

namespace MattEland.Ani.Alfred.Core.Tests
{
    /// <summary>
    /// Tests AlfredProvider
    /// </summary>
    [TestFixture]
    public sealed class AlfredProviderTests
    {
        [NotNull]
        private AlfredProvider _alfred;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public AlfredProviderTests()
        {
            _alfred = new AlfredProvider();
        }

        /// <summary>
        /// Sets up the alfred provider's tests.
        /// </summary>
        [SetUp]
        public void SetupAlfredProviderTests()
        {
            _alfred = new AlfredProvider
            {
                Console = new SimpleConsole()
            };
        }


        #region Initialization / Statuses

        /// <summary>
        /// Tests initialization of Alfred
        /// </summary>
        [Test]
        public void InitializeAlfred()
        {
            Assert.NotNull(_alfred, "Alfred was not initialized");
        }

        [Test]
        public void AlfredStartsOffline()
        {
            Assert.AreEqual(_alfred.Status, AlfredStatus.Offline);
        }

        [Test]
        public void AfterInitializationAlfredIsOnline()
        {
            _alfred.Initialize();

            Assert.AreEqual(_alfred.Status, AlfredStatus.Online);
        }

        [Test]
        public void InitializeAndShutdownResultsInShutdown()
        {
            _alfred.Initialize();
            _alfred.Shutdown();

            Assert.AreEqual(_alfred.Status, AlfredStatus.Offline);
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

        #endregion

        #region Console

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
        public void LogToConsole()
        {
            var console = _alfred.Console;
            Assert.NotNull(console, "Console was not present");

            var numEvents = console.Events.Count();

            console.Log("Alfred Test Framework", "Testing logging to Alfred", LogLevel.Verbose);

            Assert.AreEqual(numEvents + 1, console.Events.Count(), "Event count did not increase after logging.");
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
        public void InitializationWithoutConsoleWorks()
        {
            _alfred.Console = null;

            _alfred.Initialize();

            // Nothing to assert here - I'm looking for errors encountered
        }

        #endregion Console

        #region Modules

        [Test]
        public void AlfredStartsWithNoModules()
        {
            Assert.AreEqual(0, _alfred.Modules.Count, "Alfred started with modules when none were expected.");
        }

        [Test]
        public void AddingStandardModulesAddsModules()
        {
            StandardModuleProvider.AddStandardModules(_alfred);

            Assert.Greater(_alfred.Modules.Count, 0, "Alfred did not have any modules after calling add standard modules.");
        }

        [Test]
        public void InitializingInitializesModules()
        {
            StandardModuleProvider.AddStandardModules(_alfred);

            _alfred.Initialize();

            foreach (var module in _alfred.Modules)
            {
                Assert.AreEqual(AlfredStatus.Online, module.Status, $"Module {module.NameAndVersion} was not initialized during initialization.");
            }
        }

        [Test]
        public void ShuttingDownShutsDownModules()
        {
            StandardModuleProvider.AddStandardModules(_alfred);

            _alfred.Initialize();
            _alfred.Shutdown();

            foreach (var module in _alfred.Modules)
            {
                Assert.AreEqual(AlfredStatus.Offline, module.Status, $"Module {module.NameAndVersion} was not shut down during alfred shut down.");
            }
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
        public void ModulesCannotBeAddedWhileOnline()
        {
            _alfred.Initialize();
            StandardModuleProvider.AddStandardModules(_alfred);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void UpdateWithNoModulesWhileOfflineStillGeneratesError()
        {
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
            Assert.AreEqual(1, testModule.Widgets.Count, "Widgets were not properly cleared from list after re-initialize");

        }

        #endregion Modules

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RegisteringNullSubsystemGeneratesNullRef()
        {
            _alfred.RegisterSubSystem(null);
        }

    }
}
