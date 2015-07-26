using System;

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

        [Test]
        public void ModuleReturnsCorrectTime()
        {
            _alfred.Initialize();

            var currentTimeString = DateTime.Now.ToShortTimeString();
            _alfred.Update();

            var displayed = _module.UserInterfaceText;
            Assert.IsNotNull(displayed, "Displayed time was null");
            Assert.IsTrue(displayed.Contains(currentTimeString), $"The time is displaying {displayed} when current time is {currentTimeString}");
        }

        [Test]
        public void DisplayTextIsNullBeforeUpdate()
        {
            _alfred.Initialize();

            Assert.IsNull(_module.UserInterfaceText, "UI text was not null prior to update");
        }

        [Test]
        public void DisplayTextIsNullAfterShutdown()
        {
            _alfred.Initialize();
            _alfred.Update();
            _alfred.Shutdown();

            Assert.IsNull(_module.UserInterfaceText, "UI text was not null after shutdown");
        }

    }
}