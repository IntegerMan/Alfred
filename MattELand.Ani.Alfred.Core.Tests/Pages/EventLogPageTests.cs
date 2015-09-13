// ---------------------------------------------------------
// EventLogPageTests.cs
// 
// Created on:      08/30/2015 at 4:03 PM
// Last Modified:   08/30/2015 at 4:17 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Linq;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Pages;
using MattEland.Testing;

using NUnit.Framework;

using Shouldly;

namespace MattEland.Ani.Alfred.Tests.Pages
{
    /// <summary>
    /// Contains tests related to <see cref="EventLogPage" /> .
    /// </summary>
    [UnitTestProvider]
    [Category("Logging")]
    [Category("Pages")]
    public sealed class EventLogPageTests : AlfredTestBase
    {

        /// <summary>Gets the event log page.</summary>
        /// <returns>The event log page.</returns>
        [NotNull]
        private EventLogPage GetEventLogPage()
        {
            // Grab the core subsystem
            var core = GetSubsystem("Core");
            core.ShouldNotBeNull();

            // Get the event log page
            var page = core.Pages.Find("Event Log");
            page.ShouldNotBeNull();

            // Do a proper cast to the required type
            var eventLogPage = page.ShouldBeOfType<EventLogPage>();
            eventLogPage.ShouldNotBeNull();

            return eventLogPage;
        }

        /// <summary>
        ///     The event log page should list all events in its log.
        /// </summary>
        /// <remarks>
        ///     Test ALF-104 for sub-task ALF-58.
        /// </remarks>
        [Test]
        public void EventLogPageShouldListAllEvents()
        {
            // Alfred needs to be running
            StartAlfred();

            // Grab the console and ensure we have events (if we don't the test is inconclusive)
            var console = RequireConsole();
            var numConsoleEvents = console.Events.Count();
            if (numConsoleEvents <= 0) { Assert.Inconclusive("No Console Events Present"); }

            // Get our page
            var eventLogPage = GetEventLogPage();

            // Check our events
            var numPageEvents = eventLogPage.Events.Count();
            numPageEvents.ShouldBe(numConsoleEvents);
        }

        /// <summary>
        ///     Event log pages should use vertical stack layout.
        /// </summary>
        [Test, Category("Layout")]
        public void EventLogPageShouldUseVerticalStackLayout()
        {
            //! Arrange
            StartAlfred();

            //! Act
            var eventLogPage = GetEventLogPage();

            //! Assert
            eventLogPage.LayoutType.ShouldBe(LayoutType.VerticalStackPanel);
        }
    }
}