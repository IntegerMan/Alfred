// ---------------------------------------------------------
// TimeModuleAlertTests.cs
// 
// Created on:      09/06/2015 at 10:37 PM
// Last Modified:   09/06/2015 at 10:37 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;

using MattEland.Ani.Alfred.Core.Modules;

using NUnit.Framework;

namespace MattEland.Ani.Alfred.Tests.Modules
{
    /// <summary>
    ///     A class for tests targeting <see cref="AlfredTimeModule" /> and its calculations related
    ///     to alert visibility.
    /// </summary>
    public class TimeModuleAlertTests : TimeModuleTestsBase
    {

        [Test]
        public void TimeModuleCautionWidgetHasText()
        {
            Module.IsAlertEnabled = false;

            Alfred.Initialize();

            // Feed in a time in the morning for testability purposes
            var oneThirtyAm = new DateTime(1980, 9, 10, 1, 30, 0);
            Module.Update(oneThirtyAm);

            Assert.IsFalse(string.IsNullOrWhiteSpace(Module.AlertWidget.Text),
                           "Alert text is not set when alert is visible.");
        }

        [Test]
        public void TimeModuleCautionWidgetIsHiddenDuringTheAfternoon()
        {
            Alfred.Initialize();

            // Feed in a time in the afternoon for testability purposes
            var oneThirtyPm = new DateTime(1980, 9, 10, 13, 30, 0);
            Module.Update(oneThirtyPm);

            Assert.IsFalse(Module.AlertWidget.IsVisible,
                           "It's the afternoon but the module is alerting that we're near bedtime.");
        }

        [Test]
        public void TimeModuleCautionWidgetIsHiddenDuringTheMorning()
        {
            Alfred.Initialize();

            // Feed in a time in the morning for testability purposes
            var nineThirtyAm = new DateTime(1980, 9, 10, 9, 30, 0);
            Module.Update(nineThirtyAm);

            Assert.IsFalse(Module.AlertWidget.IsVisible,
                           "It's morning but the module is alerting that we're near bedtime.");
        }

        [Test]
        public void TimeModuleCautionWidgetIsHiddenLateAtNightForNoonBedtimes()
        {
            Module.AlertHour = 12;
            Module.AlertMinute = 0;
            Module.AlertDurationInHours = 2;

            Alfred.Initialize();

            var evening = new DateTime(1980, 9, 10, 22, 0, 0);
            Module.Update(evening);

            Assert.IsFalse(Module.AlertWidget.IsVisible,
                           "It's late in the evening with a noon bedtime but the alert is still showing.");
        }

        [Test]
        public void TimeModuleCautionWidgetIsNotVisibleAtNoon()
        {
            Alfred.Initialize();

            // Feed in a time in the afternoon for testability purposes
            var noon = new DateTime(1980, 9, 10, 12, 0, 0);
            Module.Update(noon);

            Assert.IsFalse(Module.AlertWidget.IsVisible,
                           "It's noon but the module is alerting that we're near bedtime.");
        }

        [Test]
        public void TimeModuleCautionWidgetIsNotVisibleWhenDisabled()
        {
            Module.IsAlertEnabled = false;

            Alfred.Initialize();

            // Feed in a time in the morning for testability purposes
            var oneThirtyAm = new DateTime(1980, 9, 10, 1, 30, 0);
            Module.Update(oneThirtyAm);

            Assert.IsFalse(Module.AlertWidget.IsVisible,
                           "Alerts are disabled but the alert is still visible.");
        }

        [Test]
        public void TimeModuleCautionWidgetIsVisibleAfterAlarm()
        {
            Module.AlertMinute = 30;
            Alfred.Initialize();

            // This is just a minute after the alarm should start
            var alarmTime = new DateTime(1980, 9, 10, Module.AlertHour, Module.AlertMinute + 1, 0);
            Module.Update(alarmTime);

            Assert.IsTrue(Module.AlertWidget.IsVisible,
                          "It's right after the alarm but the alarm is off.");
        }

        [Test]
        public void TimeModuleCautionWidgetIsVisibleAtMidnight()
        {
            Alfred.Initialize();

            // Feed in exactly midnight for testing purposes
            var midnight = new DateTime(1980, 9, 10, 0, 0, 0);
            Module.Update(midnight);

            Assert.IsTrue(Module.AlertWidget.IsVisible,
                          "It's midnight but the module isn't alerting that we need to be in bed.");
        }

        [Test]
        public void TimeModuleCautionWidgetIsVisibleBeforeEndOfAlarm()
        {
            Module.AlertHour = 12;
            Module.AlertDurationInHours = 1;
            Module.AlertMinute = 30;
            Alfred.Initialize();

            // Feed in a time right before the alarm should end
            var endAlarmHour = new DateTime(1980, 9, 10, 13, 0, 0);
            Module.Update(endAlarmHour);

            Assert.IsTrue(Module.AlertWidget.IsVisible,
                          "It's right before the end of the alarm but the alarm is off.");
        }

        [Test]
        public void TimeModuleCautionWidgetIsVisibleDuringTheEarlyMorning()
        {
            Alfred.Initialize();

            // Feed in a time in the morning for testability purposes
            var oneThirtyAm = new DateTime(1980, 9, 10, 1, 30, 0);
            Module.Update(oneThirtyAm);

            Assert.IsTrue(Module.AlertWidget.IsVisible,
                          "It's early morning but the module isn't alerting that we need to be in bed.");
        }

        [Test]
        public void TimeModuleCautionWidgetIsVisibleDuringTheNight()
        {
            Alfred.Initialize();

            // Feed in a time in the late evening for testability purposes
            var elevenThirtyPm = new DateTime(1980, 9, 10, 23, 30, 0);
            Module.Update(elevenThirtyPm);

            Assert.IsTrue(Module.AlertWidget.IsVisible,
                          "It's nearly midnight but the module isn't alerting that we need to be in bed.");
        }
    }
}