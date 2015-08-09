// ---------------------------------------------------------
// TimeModuleTests.cs
// 
// Created on:      08/08/2015 at 6:19 PM
// Last Modified:   08/08/2015 at 6:21 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Modules;
using MattEland.Ani.Alfred.Core.Pages;
using MattEland.Ani.Alfred.Core.Tests.Mocks;

using NUnit.Framework;

namespace MattEland.Ani.Alfred.Core.Tests.Modules
{
    [TestFixture]
    [SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    public sealed class TimeModuleTests
    {
        [SetUp]
        public void OnStartup()
        {
            var bootstrapper = new AlfredBootstrapper();
            _alfred = bootstrapper.Create();

            _page = new AlfredModuleListPage(_alfred.PlatformProvider, "Test");
            _module = new AlfredTimeModule(_alfred.PlatformProvider);
            _page.Register(_module);
            _subsystem = new TestSubsystem();
            _subsystem.AddAutoRegisterPage(_page);

            _alfred.Register(_subsystem);
        }

        [NotNull]
        private AlfredProvider _alfred;

        [NotNull]
        private AlfredTimeModule _module;

        [NotNull]
        private TestSubsystem _subsystem;

        [NotNull]
        private AlfredModuleListPage _page;

        private string GetTimeText()
        {
            var widget = _module.CurrentTimeWidget;

            var displayed = widget.Text;
            return displayed;
        }

        [Test]
        public void DisplayTextIsCorrectBeforeUpdate()
        {
            _alfred.Initialize();

            var currentTimeString = DateTime.Now.ToShortTimeString();

            var displayed = GetTimeText();

            Assert.IsNotNull(displayed, "Displayed time was null");
            Assert.IsTrue(
                          displayed.Contains(currentTimeString),
                          $"The time is displaying {displayed} when current time is {currentTimeString}");
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
            Assert.IsTrue(
                          displayed.Contains(currentTimeString),
                          $"The time is displaying {displayed} when current time is {currentTimeString}");
        }

        [Test]
        public void TimeModuleCautionWidgetHasText()
        {
            _module.IsAlertEnabled = false;

            _alfred.Initialize();

            // Feed in a time in the morning for testability purposes
            var oneThirtyAm = new DateTime(1980, 9, 10, 1, 30, 0);
            _module.Update(oneThirtyAm);

            Assert.IsFalse(
                           string.IsNullOrWhiteSpace(_module.BedtimeAlertWidget.Text),
                           "Alert text is not set when alert is visible.");
        }

        [Test]
        public void TimeModuleCautionWidgetIsHiddenDuringTheAfternoon()
        {
            _alfred.Initialize();

            // Feed in a time in the afternoon for testability purposes
            var oneThirtyPm = new DateTime(1980, 9, 10, 13, 30, 0);
            _module.Update(oneThirtyPm);

            Assert.IsFalse(
                           _module.BedtimeAlertWidget.IsVisible,
                           "It's the afternoon but the module is alerting that we're near bedtime.");
        }

        [Test]
        public void TimeModuleCautionWidgetIsHiddenDuringTheMorning()
        {
            _alfred.Initialize();

            // Feed in a time in the morning for testability purposes
            var nineThirtyAm = new DateTime(1980, 9, 10, 9, 30, 0);
            _module.Update(nineThirtyAm);

            Assert.IsFalse(
                           _module.BedtimeAlertWidget.IsVisible,
                           "It's morning but the module is alerting that we're near bedtime.");
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

            Assert.IsFalse(
                           _module.BedtimeAlertWidget.IsVisible,
                           "It's late in the evening with a noon bedtime but the alert is still showing.");
        }

        [Test]
        public void TimeModuleCautionWidgetIsNotVisibleAtNoon()
        {
            _alfred.Initialize();

            // Feed in a time in the afternoon for testability purposes
            var noon = new DateTime(1980, 9, 10, 12, 0, 0);
            _module.Update(noon);

            Assert.IsFalse(
                           _module.BedtimeAlertWidget.IsVisible,
                           "It's noon but the module is alerting that we're near bedtime.");
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
        public void TimeModuleCautionWidgetIsVisibleAtMidnight()
        {
            _alfred.Initialize();

            // Feed in exactly midnight for testing purposes
            var midnight = new DateTime(1980, 9, 10, 0, 0, 0);
            _module.Update(midnight);

            Assert.IsTrue(
                          _module.BedtimeAlertWidget.IsVisible,
                          "It's midnight but the module isn't alerting that we need to be in bed.");
        }

        [Test]
        public void TimeModuleCautionWidgetIsVisibleDuringTheEarlyMorning()
        {
            _alfred.Initialize();

            // Feed in a time in the morning for testability purposes
            var oneThirtyAm = new DateTime(1980, 9, 10, 1, 30, 0);
            _module.Update(oneThirtyAm);

            Assert.IsTrue(
                          _module.BedtimeAlertWidget.IsVisible,
                          "It's early morning but the module isn't alerting that we need to be in bed.");
        }

        [Test]
        public void TimeModuleCautionWidgetIsVisibleDuringTheNight()
        {
            _alfred.Initialize();

            // Feed in a time in the late evening for testability purposes
            var elevenThirtyPm = new DateTime(1980, 9, 10, 23, 30, 0);
            _module.Update(elevenThirtyPm);

            Assert.IsTrue(
                          _module.BedtimeAlertWidget.IsVisible,
                          "It's nearly midnight but the module isn't alerting that we need to be in bed.");
        }

        [Test]
        public void TimeModuleCurrentDateIsVisible()
        {
            _alfred.Initialize();

            var evening = new DateTime(1980, 9, 10, 22, 0, 0);
            _module.Update(evening);

            Assert.IsTrue(_module.CurrentDateWidget.IsVisible, "The current date was not visible");
        }

        [Test]
        public void TimeModuleDoesNotLogWhenTheHourDoesNotChange()
        {
            var console = new SimpleConsole();
            _alfred.Console = console;
            _alfred.Initialize();

            // This is a bit of a testing hack - since initialize causes the module to update, it'll update based on the current time.
            // This is what it should do, but we want to force updates given specific times so we'll just clear out the
            // stored value from the initial run
            _module.ClearLastTimeRun();

            _module.Update(new DateTime(1980, 9, 10, 9, 0, 0));
            _module.Update(new DateTime(1980, 9, 10, 9, 0, 30));
            _module.Update(new DateTime(1980, 9, 10, 9, 1, 0));

            // This will error if 0 or > 1 events are logged
            Assert.IsFalse(
                           console.Events.Any(e => e.Title == AlfredTimeModule.HourAlertEventTitle),
                           "Updating to times not in a new hour should not have logged");
        }

        /// <summary>
        ///     Tests that the bedtime alert widget is in the object structure.
        ///     This doesn't determine whether or not it's visible.
        /// </summary>
        [Test]
        public void TimeModuleHasCautionWidget()
        {
            _alfred.Initialize();

            Assert.IsNotNull(_module.BedtimeAlertWidget, "The Bedtime alert widget was not present");
            Assert.Contains(
                            _module.BedtimeAlertWidget,
                            _module.Widgets as ICollection,
                            "The module did not contain a registered bedtime alert widget.");
        }

        [Test]
        public void TimeModuleHasNoWidgetsWhenOffline()
        {
            _alfred.Initialize();
            _alfred.Shutdown();

            Assert.IsTrue(!_module.Widgets.Any(), "Widgets were left over after shutdown");
        }

        /// <summary>
        ///     The time module must always be visible.
        /// </summary>
        [Test]
        public void TimeModuleHasTimeAlwaysVisible()
        {
            _alfred.Initialize();

            Assert.IsTrue(_module.CurrentTimeWidget.IsVisible, "The time widget was hidden.");
        }

        [Test]
        public void TimeModuleHasWidgets()
        {
            _alfred.Initialize();

            Assert.IsNotNull(_module.Widgets);
            Assert.Greater(_module.Widgets.Count(), 0, "The time module did not have any Widgets");
        }

        [Test]
        public void TimeModuleLogsOnHalfHourChanges()
        {
            var console = new SimpleConsole();
            _alfred.Console = console;
            _alfred.Initialize();

            _module.Update(new DateTime(1980, 9, 10, 9, 29, 0));
            _module.Update(new DateTime(1980, 9, 10, 9, 30, 0));

            // This will error if 0 or > 1 events are logged
            var consoleEvent = console.Events.Single(e => e.Title == AlfredTimeModule.HalfHourAlertEventTitle);

            //  We want to ensure it's an informational purposes
            Assert.AreEqual(LogLevel.Info, consoleEvent.Level);
        }

        [Test]
        public void TimeModuleLogsWhenTheHourChanges()
        {
            var console = new SimpleConsole();
            _alfred.Console = console;
            _alfred.Initialize();

            _module.Update(new DateTime(1980, 9, 10, 8, 59, 0));
            _module.Update(new DateTime(1980, 9, 10, 9, 0, 0));

            // This will error if 0 or > 1 events are logged
            var consoleEvent = console.Events.Single(e => e.Title == AlfredTimeModule.HourAlertEventTitle);

            //  We want to ensure it's an informational purposes
            Assert.AreEqual(LogLevel.Info, consoleEvent.Level);
        }

        [Test]
        public void TimeModuleShowsCurrentDate()
        {
            _alfred.Initialize();

            var evening = new DateTime(1980, 9, 10, 22, 0, 0);
            _module.Update(evening);

            const string Expected = "Wednesday, September 10, 1980";
            var currentDateString = _module.CurrentTimeWidget.Text;

            Assert.AreEqual(
                            Expected,
                            _module.CurrentDateWidget.Text,
                            $"Expected '{Expected}' but was '{currentDateString}'");
        }

        [Test]
        public void TimeModuleStartsOffline()
        {
            Assert.AreEqual(AlfredStatus.Offline, _module.Status);
        }
    }
}