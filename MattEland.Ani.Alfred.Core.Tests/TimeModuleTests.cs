using System;
using System.Linq;

using MattEland.Ani.Alfred.Core.Modules;
using MattEland.Ani.Alfred.Core.Widgets;

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

            var displayed = GetTimeText();

            Assert.IsNotNull(displayed, "Displayed time was null");
            Assert.IsTrue(displayed.Contains(currentTimeString), $"The time is displaying {displayed} when current time is {currentTimeString}");
        }

        private string GetTimeText()
        {
            if (_module.Widgets == null)
            {
                return null;
            }

            var widget = (TextWidget)_module.Widgets.First();

            var displayed = widget.Text;
            return displayed;
        }

        [Test]
        public void DisplayTextIsNullBeforeUpdate()
        {
            _alfred.Initialize();

            Assert.IsNull(GetTimeText(), "UI text was not null prior to update");
        }

        [Test]
        public void DisplayTextIsNullAfterShutdown()
        {
            _alfred.Initialize();
            _alfred.Update();
            _alfred.Shutdown();

            Assert.IsNull(GetTimeText(), "UI text was not null after shutdown");
        }

        [Test]
        public void TimeModuleHasWidgets()
        {
            _alfred.Initialize();

            Assert.IsNotNull(_module.Widgets);
            Assert.Greater(_module.Widgets.Count, 0, "The time module did not have any Widgets");
        }

        [Test]
        public void TimeModuleHasNoWidgetsWhenOffline()
        {
            Assert.IsNull(_module.Widgets);

            _alfred.Initialize();
            _alfred.Shutdown();

            Assert.IsNull(_module.Widgets);
        }

    }
}