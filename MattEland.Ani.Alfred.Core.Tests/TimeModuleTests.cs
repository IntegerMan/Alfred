using System;
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
            _module = new AlfredTimeModule(_alfred.PlatformProvider);
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

            Assert.IsTrue(_module.Widgets == null || _module.Widgets.Count == 0, "Widgets were left over after shutdown");
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
        public void TimeModuleCautionWidgetIsNotVisibleAtNoon()
        {
            _alfred.Initialize();

            // Feed in a time in the afternoon for testability purposes
            var noon = new DateTime(1980, 9, 10, 12, 0, 0);
            _module.Update(noon);

            Assert.IsFalse(_module.BedtimeAlertWidget.IsVisible, "It's noon but the module is alerting that we're near bedtime.");
        }

        [Test]
        public void TimeModuleCautionWidgetIsHiddenDuringTheAfternoon()
        {
            _alfred.Initialize();

            // Feed in a time in the afternoon for testability purposes
            var oneThirtyPm = new DateTime(1980, 9, 10, 13, 30, 0);
            _module.Update(oneThirtyPm);

            Assert.IsFalse(_module.BedtimeAlertWidget.IsVisible, "It's the afternoon but the module is alerting that we're near bedtime.");
        }

        [Test]
        public void TimeModuleCautionWidgetIsHiddenDuringTheMorning()
        {
            _alfred.Initialize();

            // Feed in a time in the morning for testability purposes
            var nineThirtyAm = new DateTime(1980, 9, 10, 9, 30, 0);
            _module.Update(nineThirtyAm);

            Assert.IsFalse(_module.BedtimeAlertWidget.IsVisible, "It's morning but the module is alerting that we're near bedtime.");
        }

        [Test]
        public void TimeModuleCautionWidgetIsVisibleDuringTheEarlyMorning()
        {
            _alfred.Initialize();

            // Feed in a time in the morning for testability purposes
            var oneThirtyAm = new DateTime(1980, 9, 10, 1, 30, 0);
            _module.Update(oneThirtyAm);

            Assert.IsTrue(_module.BedtimeAlertWidget.IsVisible, "It's early morning but the module isn't alerting that we need to be in bed.");
        }

        [Test]
        public void TimeModuleCautionWidgetIsNotVisibleWhenDisabled()
        {
            _module.IsAlertEnabled = false;

            _alfred.Initialize();

            // Feed in a time in the morning for testability purposes
            var oneThirtyAm = new DateTime(1980, 9, 10, 1, 30, 0);
            _module.Update(oneThirtyAm);

            Assert.IsFalse(_module.BedtimeAlertWidget.IsVisible, "Alerts are disabled but the alert is still visible.");
        }

        [Test]
        public void TimeModuleCautionWidgetHasText()
        {
            _module.IsAlertEnabled = false;

            _alfred.Initialize();

            // Feed in a time in the morning for testability purposes
            var oneThirtyAm = new DateTime(1980, 9, 10, 1, 30, 0);
            _module.Update(oneThirtyAm);

            Assert.IsFalse(string.IsNullOrWhiteSpace(_module.BedtimeAlertWidget.Text), "Alert text is not set when alert is visible.");
        }

        [Test]
        public void TimeModuleCautionWidgetIsVisibleDuringTheNight()
        {
            _alfred.Initialize();

            // Feed in a time in the late evening for testability purposes
            var elevenThirtyPm = new DateTime(1980, 9, 10, 23, 30, 0);
            _module.Update(elevenThirtyPm);

            Assert.IsTrue(_module.BedtimeAlertWidget.IsVisible, "It's nearly midnight but the module isn't alerting that we need to be in bed.");
        }

        [Test]
        public void TimeModuleCautionWidgetIsVisibleAtMidnight()
        {
            _alfred.Initialize();

            // Feed in exactly midnight for testing purposes
            var midnight = new DateTime(1980, 9, 10, 0, 0, 0);
            _module.Update(midnight);

            Assert.IsTrue(_module.BedtimeAlertWidget.IsVisible, "It's midnight but the module isn't alerting that we need to be in bed.");
        }

        [Test]
        public void TimeModuleCautionWidgetIsHiddenLateAtNightForNoonBedtimes()
        {
            _module.BedtimeHour = 12;
            _module.BedtimeMinute = 0;
            _module.AlertDurationInHours = 2;

            _alfred.Initialize();

            var evening = new DateTime(1980, 9, 10, 22, 0, 0);
            _module.Update(evening);

            Assert.IsFalse(_module.BedtimeAlertWidget.IsVisible, "It's late in the evening with a noon bedtime but the alert is still showing.");
        }

        [Test]
        public void TimeModuleShowsCurrentDate()
        {
            _alfred.Initialize();

            var evening = new DateTime(1980, 9, 10, 22, 0, 0);
            _module.Update(evening);

            const string ExpectedDateString = "Wednesday, September 10, 1980";
            var currentDateString = _module.CurrentTimeWidget.Text;

            Assert.AreEqual(ExpectedDateString, _module.CurrentDateWidget.Text, $"Expected '{ExpectedDateString}' but was '{currentDateString}'");
        }

        [Test]
        public void TimeModuleurrentDateIsVisible()
        {
            _alfred.Initialize();

            var evening = new DateTime(1980, 9, 10, 22, 0, 0);
            _module.Update(evening);

            Assert.IsTrue(_module.CurrentDateWidget.IsVisible, "The current date was not visible");
        }

    }
}