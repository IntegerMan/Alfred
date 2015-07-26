using MattEland.Ani.Alfred.Core;
using NUnit.Framework;

namespace MattELand.Ani.Alfred.Core.Tests
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
                Console = new TestConsole()
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
            _alfred.Console.Log("Alfred Test Framework", "Testing logging to Alfred");
        }
    }
}
