using MattEland.Ani.Alfred.Core.Modules;

using NUnit.Framework;

namespace MattEland.Ani.Alfred.Core.Tests
{
    [TestFixture]
    public class TimeModuleTests
    {
        private AlfredProvider _alfred;
        private AlfredTimeModule _module;

        [SetUp]
        public void OnStartup()
        {
            _alfred = new AlfredProvider();
            _module = new AlfredTimeModule();
            _alfred.Modules.Add(_module);
        }

        [Test]
        public void TimeModuleStartsOffline()
        {
            Assert.AreEqual(AlfredStatus.Offline, _module.Status);
        }

        [Test]
        public void ModuleIsOnlineAfterStartup()
        {
            _alfred.Initialize();

            Assert.AreEqual(AlfredStatus.Online, _module.Status);
        }

    }
}