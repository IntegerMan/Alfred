using System;
using System.Linq;

using MattEland.Ani.Alfred.Chat;
using MattEland.Ani.Alfred.Core;
using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Common.Providers;
using MattEland.Testing;

using NUnit.Framework;

using Shouldly;

namespace MattEland.Ani.Alfred.Tests
{
    /// <summary>
    ///     Unit tests surrounding Alfred's statuses and status transitions
    /// </summary>
    [UnitTestProvider]
    public class AlfredStatusTests : AlfredTestBase
    {

        /// <summary>
        ///     Sets up the Alfred provider's tests.
        /// </summary>
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            var alfred = new AlfredApplication(Container);
            alfred.RegisterAsProvidedInstance(typeof(IAlfred), Container);
        }

        /// <summary>
        ///     Tests that after initialization Alfred is online.
        /// </summary>
        [Test]
        public void AfterInitializationAlfredIsOnline()
        {
            Alfred.Initialize();

            Alfred.Status.ShouldBe(AlfredStatus.Online);
        }

        /// <summary>
        ///    Test that Alfred starts offline.
        /// </summary>
        [Test]
        public void AlfredStartsOffline()
        {
            Alfred.Status.ShouldBe(AlfredStatus.Offline);
        }

        /// <summary>
        ///     Ensures shutdown creates events in log.
        /// </summary>
        [Test]
        public void ShutdownCreatesEventsInLog()
        {
            var container = Container;
            var al = Alfred;
            var console = Container.Provide<IConsole>();

            // We need to be online to shut down or else we'll get errors
            al.Initialize();
            al.Container.ShouldBeSameAs(container);

            var eventsBeforeShutdown = console.EventCount;

            al.Shutdown();

            var message = $"Shutting Alfred down did not create any log entries ({console.EventCount}) on container {container.Name} with collection of type {console.Events.GetType()}";
            console.EventCount.ShouldBeGreaterThan(eventsBeforeShutdown, message);
        }

        /// <summary>
        ///     Tests that starting Alfred creates log entries
        /// </summary>
        [Test]
        public void InitializeCreatesEventsInLog()
        {
            var container = Container;
            var al = Alfred;
            var console = Container.Provide<IConsole>();

            var eventsBeforeInitialize = console.EventCount;

            al.Initialize();
            al.Container.ShouldBeSameAs(container);

            var message = $"Initializing Alfred did not create any log entries ({console.EventCount}) on container {container.Name} with collection of type {console.Events.GetType()}";
            console.EventCount.ShouldBeGreaterThan(eventsBeforeInitialize, message);
        }

        /// <summary>
        ///     Tests initialization of Alfred
        /// </summary>
        [Test]
        public void InitializeAlfred()
        {
            Assert.NotNull(Alfred, "Alfred was not initialized");
        }

        /// <summary>
        ///     Tests that initialization followed by shutdown results in shutdown.
        /// </summary>
        [Test]
        public void InitializeAndShutdownResultsInShutdown()
        {
            Alfred.Initialize();
            Alfred.Shutdown();

            Alfred.Status.ShouldBe(AlfredStatus.Offline);
        }

        /// <summary>
        ///     Ensures shutdown while offline errors.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidOperationException))]
        public void ShutdownWhileOfflineErrors()
        {
            // TODO: Introduce IHasStatus and ShouldBeOffline / ShouldBeOnline extension methods
            Alfred.Status.ShouldBe(AlfredStatus.Offline);
            Alfred.Shutdown();
        }

        /// <summary>
        ///     Tests that initializing while online causes issues
        /// </summary>
        [Test, ExpectedException(typeof(InvalidOperationException))]
        public void InitializeWhileOnlineErrors()
        {
            Alfred.Initialize();
            Alfred.Initialize();
        }

        /// <summary>
        ///     Tests that Shutdown causes Alfred to say goodbye.
        /// </summary>
        [Test]
        public void ShutdownCausesAlfredToSayGoodbye()
        {
            // Alfred will need a chat subsystem for this test
            var chatSubsystem = new ChatSubsystem(Container, "Test Monkey");
            Alfred.Register(chatSubsystem);
            Alfred.Subsystems.ShouldContain(chatSubsystem);

            // Turn Alfred on so we can move to a point where we can deactivate him
            Alfred.Initialize();
            Alfred.ChatProvider.ShouldNotBeNull();

            // Ensure no prior chat events pollute our test later
            var console = GetConsole();
            console.Clear();
            console.EventCount.ShouldBe(0);

            // Shutdown and get Alfred's reaction
            Alfred.Shutdown();
            var numChatMessagesInLog = console.Events.Count(e => e.Level == LogLevel.ChatResponse);

            numChatMessagesInLog.ShouldBeGreaterThan(0, "No chat messages were present in the log. Alfred didn't say goodbye.");
        }

    }
}