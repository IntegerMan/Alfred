// ---------------------------------------------------------
// PageTests.cs
// 
// Created on:      08/09/2015 at 12:28 AM
// Last Modified:   08/09/2015 at 12:42 AM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Tests.Mocks;

using NUnit.Framework;

namespace MattEland.Ani.Alfred.Core.Tests.Pages
{
    /// <summary>
    ///     Tests oriented around testing the page update pumps and related functions
    /// </summary>
    [TestFixture]
    [SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    public class PageTests
    {
        [SetUp]
        public void Setup()
        {
            var bootstrapper = new AlfredBootstrapper();
            _alfred = bootstrapper.Create();
            _subsystem = new TestSubsystem();
            _page = new TestPage();

            _subsystem.AddAutoRegisterPage(_page);
            _alfred.Register(_subsystem);
        }

        [NotNull]
        private AlfredApplication _alfred;

        [NotNull]
        private TestPage _page;

        [NotNull]
        private TestSubsystem _subsystem;

        [Test]
        public void InitializeCausesPagesToGoOnline()
        {
            _alfred.Initialize();

            Assert.AreEqual(AlfredStatus.Online, _page.Status);
        }

        [Test]
        public void InitializeCausesRegisteredPagesToInitialize()
        {
            _alfred.Initialize();

            Assert.IsTrue(_page.LastInitialized > DateTime.MinValue, "Page was not initialized");
            Assert.IsTrue(_page.LastInitializationCompleted > DateTime.MinValue,
                          "Page was not notified initialized completed");
        }

        [Test]
        public void ShutdownCausesRegisteredPagesToGoOffline()
        {
            _alfred.Initialize();
            _alfred.Update();
            _alfred.Shutdown();

            Assert.AreEqual(AlfredStatus.Offline, _page.Status);
        }

        [Test]
        public void ShutdownCausesRegisteredPagesToShutdown()
        {
            _alfred.Initialize();
            _alfred.Update();
            _alfred.Shutdown();

            Assert.IsTrue(_page.LastShutdown > DateTime.MinValue, "Page was not shut down");
            Assert.IsTrue(_page.LastShutdownCompleted > DateTime.MinValue,
                          "Page was not notified of shut down completion");
        }

        [Test]
        public void UpdateCausesRegisteredPagesToUpdate()
        {
            _alfred.Initialize();
            _alfred.Update();

            Assert.IsTrue(_page.LastUpdated > DateTime.MinValue, "Page was not updated");
        }
    }
}