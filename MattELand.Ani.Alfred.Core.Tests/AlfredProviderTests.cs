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

        /// <summary>
        /// Tests initialization of Alfred
        /// </summary>
        [Test]
        public void InitializeAlfred()
        {
            var alfred = new AlfredProvider();
        }

        [Test]
        public void RemoveAlfredConsole()
        {
            var alfred = new AlfredProvider();

            alfred.Console = null;
        }

}
}
