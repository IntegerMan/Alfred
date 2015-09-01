// ---------------------------------------------------------
// AlfredCoreSubSystemTests.cs
// 
// Created on:      08/19/2015 at 9:31 PM
// Last Modified:   08/25/2015 at 3:25 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core;
using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Modules;
using MattEland.Ani.Alfred.Core.Pages;
using MattEland.Ani.Alfred.Core.Subsystems;
using MattEland.Common.Providers;
using MattEland.Testing;

using NUnit.Framework;

namespace MattEland.Ani.Alfred.Tests.Subsystems
{
    [UnitTest]
    [SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    public class AlfredCoreSubsystemTests : AlfredTestBase
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            _subsystem = new AlfredCoreSubsystem(Container);

            var bootstrapper = new AlfredBootstrapper(Container);
            _alfred = bootstrapper.Create();
        }

        [NotNull]
        private AlfredCoreSubsystem _subsystem;

        [NotNull]
        private AlfredApplication _alfred;

        private static void AssertExpectedModules([NotNull] IEnumerable<IAlfredModule> modules)
        {
            modules = modules.ToList();
            Assert.IsTrue(modules.Any(m => m is AlfredTimeModule), "Time Module not found");
            Assert.IsTrue(modules.Any(m => m is AlfredPowerModule), "Power Module not found");
            Assert.IsTrue(modules.Any(m => m is AlfredSubsystemListModule),
                          "Subsystem List Module not found");
            Assert.IsTrue(modules.Any(m => m is AlfredPagesListModule),
                          "Pages List Module not found");
        }

        /// <summary>
        ///     Finds the page with the specified name and casts it to the expected type.
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <param name="pageName">Name of the page.</param>
        /// <returns>The page</returns>
        [NotNull]
        private T FindPage<T>(string pageName) where T : AlfredPage
        {
            var page = (T)_alfred.RootPages.First(p => p.Name == pageName);
            Assert.NotNull(page);

            return page;
        }

        [Test]
        public void AlfredContainsAPageAfterRegistration()
        {
            var pages = _alfred.RootPages.Count();

            _alfred.Register(_subsystem);
            _alfred.Initialize();

            Assert.Greater(_alfred.RootPages.Count(), pages);
        }

        [Test]
        public void ControlPageContainsCorrectModules()
        {
            _alfred.Register(_subsystem);
            _alfred.Initialize();
            _alfred.Update();

            // Grab the Page
            var pageName = AlfredCoreSubsystem.ControlPageName;
            var page = FindPage<AlfredModuleListPage>(pageName);

            // Ensure our expected modules are there
            AssertExpectedModules(page.Modules);
        }

        [Test]
        public void ControlPageIsPresentInAlfredAfterInitialization()
        {
            _alfred.Register(_subsystem);
            _alfred.Initialize();
            _alfred.Update();

            Assert.IsTrue(
                          _alfred.RootPages.Any(p => p.Name == AlfredCoreSubsystem.ControlPageName),
                          "Control Page was not found");
        }

        [Test]
        public void EventLogPageIsNotPresentInAlfredAfterInitializationWhenNoConsoleIsProvided()
        {
            _alfred.Register(_subsystem);
            _alfred.Initialize();
            _alfred.Update();

            Assert.IsTrue(_alfred.RootPages.All(p => p.Name != AlfredCoreSubsystem.EventLogPageName),
                          "Event Log Page was present when no console was provided");
        }

        [Test]
        public void EventLogPageIsPresentInAlfredAfterInitializationWhenConsoleIsProvided()
        {
            var console = new SimpleConsole();
            _alfred.Console = console;

            _alfred.Register(_subsystem);
            _alfred.Initialize();
            _alfred.Update();

            Assert.IsTrue(_alfred.RootPages.Any(p => p.Name == AlfredCoreSubsystem.EventLogPageName),
                          "Event Log Page was not found");
        }

        [Test]
        public void SubsystemCanBeRegisteredInAlfred()
        {
            _alfred.Register(_subsystem);

            Assert.AreEqual(1, _alfred.Subsystems.Count(), "Subsystem was not registered");
            Assert.Contains(_subsystem,
                            _alfred.Subsystems as ICollection,
                            "The subsystem was not found in the collection");
        }

        [Test]
        public void SubSystemContainsAPageAfterRegistration()
        {
            Assert.AreEqual(0, _subsystem.Pages.Count());

            _alfred.Register(_subsystem);
            _alfred.Initialize();

            Assert.GreaterOrEqual(_subsystem.Pages.Count(), 1);
        }
    }
}