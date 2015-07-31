﻿using System;
using System.Collections;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Modules;

using NUnit.Framework;

namespace MattEland.Ani.Alfred.Core.Tests
{
    [TestFixture]
    public sealed class TimeModuleTests
    {
        [NotNull]
        private AlfredProvider _alfred;

        [NotNull]
        private AlfredTimeModule _module;

        [SetUp]
        public void OnStartup()
        {
            _alfred = new AlfredProvider();
            _module = new AlfredTimeModule(_alfred.CollectionProvider);
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

            var widget = _module.CurrentTimeWidget;

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

        /// <summary>
        /// Tests that the bedtime alert widget is in the object structure.
        /// This doesn't determine whether or not it's visible.
        /// </summary>
        [Test]
        public void TimeModuleHasCautionWidget()
        {
            _alfred.Initialize();

            Assert.IsNotNull(_module.BedtimeAlertWidget, "The Bedtime alert widget was not present");
            Assert.Contains(_module.BedtimeAlertWidget, _module.Widgets as ICollection, "The module did not contain a registered bedtime alert widget.");
        }


        /// <summary>
        /// The time module must always be visible.
        /// </summary>
        [Test]
        public void TimeModuleHasTimeAlwaysVisible()
        {
            _alfred.Initialize();

            Assert.IsTrue(_module.CurrentTimeWidget.IsVisible, "The time widget was hidden.");
        }

        [Test]
        public void TimeModuleCautionWidgetIsHiddenDuringTheDay()
        {
            Assert.Inconclusive("Not implemented yet");
        }

    }
}