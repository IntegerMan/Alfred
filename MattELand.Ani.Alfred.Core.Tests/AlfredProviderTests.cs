using MattEland.Ani.Alfred.Core;
using NUnit.Framework;

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

        /// <summary>
        /// Tests initialization of Alfred
        /// </summary>
        [Test]
        public void InitializeAlfred()
        {
            Assert.NotNull(_alfred, "Alfred was not initialized");
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
        public void LogToConsole()
        {
            var numEvents = _alfred.Console.Events.Count;

            _alfred.Console.Log("Alfred Test Framework", "Testing logging to Alfred");

            Assert.AreEqual(numEvents + 1, _alfred.Console.Events.Count, "Event count did not increase after logging.");
        }

        [Test]
        public void InitializeCreatesEventsInLog()
        {
            var numEvents = _alfred.Console.Events.Count;

            _alfred.Initialize();

            Assert.Greater(_alfred.Console.Events.Count, numEvents, "Initializing Alfred did not create any log entries.");
        }

        [Test]
        public void ShutdownCreatesEventsInLog()
        {
            var numEvents = _alfred.Console.Events.Count;

            _alfred.Shutdown();

            Assert.Greater(_alfred.Console.Events.Count, numEvents, "Shutting Alfred down did not create any log entries.");
        }

        [Test]
        public void InitializationWithoutConsoleWorks()
        {
            _alfred.Console = null;

            _alfred.Initialize();

            // Nothing to assert here - I'm looking for errors encountered
        }
    }
}
