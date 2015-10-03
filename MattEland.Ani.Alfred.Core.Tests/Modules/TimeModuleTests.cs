// ---------------------------------------------------------
// TimeModuleTests.cs
// 
// Created on:      08/08/2015 at 6:19 PM
// Last Modified:   08/10/2015 at 10:55 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Modules;
using MattEland.Common.Providers;
using MattEland.Testing;

using NUnit.Framework;

using Shouldly;

namespace MattEland.Ani.Alfred.Tests.Modules
{

    /// <summary>
    ///     Unit tests related to the <see cref="AlfredTimeModule"/>
    /// </summary>
    [UnitTestProvider]
    [SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
    public sealed class TimeModuleTests : TimeModuleTestsBase
    {

        private string GetTimeText()
        {
            var widget = Module.CurrentTimeWidget;

            return widget.Text;
        }

        [Test]
        public void DisplayTextIsCorrectBeforeUpdate()
        {
            Alfred.Initialize();

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
            Alfred.Initialize();
            Alfred.Update();
            Alfred.Shutdown();

            Assert.IsNull(GetTimeText(), "UI text was not null after shutdown");
        }

        [Test]
        public void ModuleIsOnlineAfterStartup()
        {
            Alfred.Initialize();

            Assert.AreEqual(AlfredStatus.Online, Module.Status);
        }

        [Test]
        public void ModuleReturnsCorrectTime()
        {
            Alfred.Initialize();

            var currentTimeString = DateTime.Now.ToShortTimeString();
            Alfred.Update();

            var displayed = GetTimeText();

            Assert.IsNotNull(displayed, "Displayed time was null");
            Assert.IsTrue(
                          displayed.Contains(currentTimeString),
                          $"The time is displaying {displayed} when current time is {currentTimeString}");
        }
        [Test]
        public void TimeModuleCurrentDateIsVisible()
        {
            Alfred.Initialize();

            var evening = new DateTime(1980, 9, 10, 22, 0, 0);
            Module.Update(evening);

            Assert.IsTrue(Module.CurrentDateWidget.IsVisible, "The current date was not visible");
        }

        [Test]
        public void TimeModuleDoesNotLogWhenTheHourDoesNotChange()
        {
            var console = new DiagnosticConsole(AlfredContainer);
            AlfredContainer.Console = console;

            Alfred.Initialize();

            // This is a bit of a testing hack - since initialize causes the module to update, it'll update based on the current time.
            // This is what it should do, but we want to force updates given specific times so we'll just clear out the
            // stored value from the initial run
            Module.ClearLastTimeRun();

            Module.Update(new DateTime(1980, 9, 10, 9, 0, 0));
            Module.Update(new DateTime(1980, 9, 10, 9, 0, 30));
            Module.Update(new DateTime(1980, 9, 10, 9, 1, 0));

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
            Alfred.Initialize();

            Assert.IsNotNull(Module.AlertWidget, "The Bedtime alert widget was not present");
            Assert.Contains(
                            Module.AlertWidget,
                            Module.Widgets as ICollection,
                            "The module did not contain a registered bedtime alert widget.");
        }

        [Test]
        public void TimeModuleHasName()
        {
            Assert.IsNotNullOrEmpty(Module.Name);
        }

        [Test]
        public void TimeModuleHasNoWidgetsWhenOffline()
        {
            Alfred.Initialize();
            Alfred.Shutdown();

            Assert.IsTrue(!Module.Widgets.Any(), "Widgets were left over after shutdown");
        }

        /// <summary>
        ///     The time module must always be visible.
        /// </summary>
        [Test]
        public void TimeModuleHasTimeAlwaysVisible()
        {
            Alfred.Initialize();

            Assert.IsTrue(Module.CurrentTimeWidget.IsVisible, "The time widget was hidden.");
        }

        [Test]
        public void TimeModuleHasWidgets()
        {
            Alfred.Initialize();

            Assert.IsNotNull(Module.Widgets);
            Assert.Greater(Module.Widgets.Count(), 0, "The time module did not have any Widgets");
        }

        /// <summary>
        ///     Tests that <see cref="AlfredTimeModule"/> logs a message every half hour.
        /// </summary>
        [Test]
        public void TimeModuleLogsOnHalfHourChanges()
        {
            // Have Alfred use the global console
            var console = Container.Provide<IConsole>();
            console.RegisterAsProvidedInstance(typeof(IConsole), Container);
            Alfred.Initialize();

            Module.Update(new DateTime(1980, 9, 10, 9, 29, 0));
            Module.Update(new DateTime(1980, 9, 10, 9, 30, 0));

            // This will error if 0 or > 1 events are logged
            var consoleEvent = console.Events.Single(e => e.Title == AlfredTimeModule.HalfHourAlertEventTitle);

            //  We want to ensure it has the chat notification status
            consoleEvent.ShouldNotBeNull();
            consoleEvent.Level.ShouldBe(LogLevel.ChatNotification);
        }

        /// <summary>
        ///     Tests that <see cref="AlfredTimeModule"/> logs a message every hour.
        /// </summary>
        [Test]
        public void TimeModuleLogsWhenTheHourChanges()
        {
            // Have Alfred use the global console
            var console = Container.Provide<IConsole>();
            console.RegisterAsProvidedInstance(typeof(IConsole), Container);

            Alfred.Initialize();

            Module.Update(new DateTime(1980, 9, 10, 8, 59, 0));
            Module.Update(new DateTime(1980, 9, 10, 9, 0, 0));

            // This will error if 0 or > 1 events are logged
            var consoleEvent = console.Events.Single(e => e.Title == AlfredTimeModule.HourAlertEventTitle);

            //  We want to ensure it has the chat notification status
            consoleEvent.ShouldNotBeNull();
            consoleEvent.Level.ShouldBe(LogLevel.ChatNotification);
        }

        [Test]
        public void TimeModuleShowsCurrentDate()
        {
            Alfred.Initialize();

            var evening = new DateTime(1980, 9, 10, 22, 0, 0);
            Module.Update(evening);

            const string Expected = "Wednesday, September 10, 1980";
            var currentDateString = Module.CurrentTimeWidget.Text;

            Assert.AreEqual(
                            Expected,
                            Module.CurrentDateWidget.Text,
                            $"Expected '{Expected}' but was '{currentDateString}'");
        }

        [Test]
        public void TimeModuleStartsOffline()
        {
            Assert.AreEqual(AlfredStatus.Offline, Module.Status);
        }
    }

}