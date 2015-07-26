using NUnit.Framework;
using System.Linq;

using MattEland.Ani.Alfred.Core.Console;

namespace MattEland.Ani.Alfred.Core.Tests
{
    /// <summary>
    /// Tests AlfredProvider
    /// </summary>
    [TestFixture]
    public class AlfredProviderTests
    {
        private AlfredProvider _alfred;

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

            console.Log("Alfred Test Framework", "Testing logging to Alfred");

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
            _alfred.AddStandardModules();

            Assert.Greater(_alfred.Modules.Count, 0, "Alfred did not have any modules after calling add standard modules.");
        }

        #endregion Modules

    }
}
