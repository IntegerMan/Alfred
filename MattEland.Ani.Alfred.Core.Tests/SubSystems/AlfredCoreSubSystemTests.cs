// ---------------------------------------------------------
// AlfredCoreSubSystemTests.cs
// 
// Created on:      08/08/2015 at 6:17 PM
// Last Modified:   08/08/2015 at 7:02 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Modules;

using NUnit.Framework;

namespace MattEland.Ani.Alfred.Core.Tests.SubSystems
{
    [TestFixture]
    [SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    public class AlfredCoreSubSystemTests
    {
        [SetUp]
        public void SetUp()
        {
            _subsystem = new AlfredControlSubSystem();
            _alfred = new AlfredProvider();
        }

        [NotNull]
        private AlfredControlSubSystem _subsystem;

        [NotNull]
        private AlfredProvider _alfred;

        [Test]
        public void AlfredContainsAPageAfterRegistration()
        {
            var pages = _alfred.RootPages.Count();

            _alfred.Register(_subsystem);
            _alfred.Initialize();

            Assert.Greater(_alfred.RootPages.Count(), pages);
        }

        [Test]
        public void ControlPageIsPresentInAlfredAfterInitialization()
        {
            _alfred.Register(_subsystem);
            _alfred.Initialize();
            _alfred.Update();

            Assert.IsTrue(_alfred.RootPages.Any(p => p.Name == AlfredControlSubSystem.ControlPageName),
                          "Control Page was not found");
        }

        [Test]
        public void EventLogPageIsPresentInAlfredAfterInitialization()
        {
            _alfred.Register(_subsystem);
            _alfred.Initialize();
            _alfred.Update();

            Assert.IsTrue(_alfred.RootPages.Any(p => p.Name == AlfredControlSubSystem.EventLogPageName),
                          "Event Log Page was not found");
        }

        [Test]
        public void SubsystemCanBeRegisteredInAlfred()
        {
            _alfred.Register(_subsystem);

            Assert.AreEqual(1, _alfred.SubSystems.Count(), "Subsystem was not registered");
            Assert.Contains(_subsystem,
                            _alfred.SubSystems as ICollection,
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

        [Test]
        public void SubsystemContainsModules()
        {
            Assert.IsTrue(_subsystem.Modules.Any(m => m is AlfredTimeModule), "Time Module not found");
            Assert.IsTrue(_subsystem.Modules.Any(m => m is AlfredPowerModule), "Power Module not found");
            Assert.IsTrue(_subsystem.Modules.Any(m => m is AlfredSubSystemListModule), "Subsystem List Module not found");
        }
    }
}