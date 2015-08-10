// ---------------------------------------------------------
// SystemSubSystemTests.cs
// 
// Created on:      08/07/2015 at 10:01 PM
// Last Modified:   08/07/2015 at 10:01 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Modules.SysMonitor;
using MattEland.Ani.Alfred.Core.Pages;

using NUnit.Framework;

namespace MattEland.Ani.Alfred.Core.Tests.SubSystems
{
    /// <summary>
    /// A series of tests related to the System Monitoring SubSystem
    /// </summary>
    [TestFixture]
    [SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    public class SystemSubSystemTests
    {
        [NotNull]
        private SystemMonitoringSubsystem _subsystem;

        [NotNull]
        private AlfredProvider _alfred;

        [SetUp]
        public void TestSetup()
        {
            _subsystem = new SystemMonitoringSubsystem();

            var bootstrapper = new AlfredBootstrapper();
            _alfred = bootstrapper.Create();
        }

        [Test]
        public void SystemMonitoringSubsystemCanBeRegisteredInAlfred()
        {
            _alfred.Register(_subsystem);

            Assert.AreEqual(1, _alfred.Subsystems.Count(), "Subsystem was not registered");
            Assert.Contains(_subsystem, _alfred.Subsystems as ICollection, "The subsystem was not found in the collection");
        }

        [Test]
        public void SystemMonitoringSubsystemContainsModules()
        {
            _alfred.Register(_subsystem);

            var page = _subsystem.Pages.First() as AlfredModuleListPage;
            Assert.NotNull(page);

            Assert.IsTrue(page.Modules.Any(m => m is CpuMonitorModule), "CPU Monitor not found");
            Assert.IsTrue(page.Modules.Any(m => m is MemoryMonitorModule), "Memory Monitor not found");
            Assert.IsTrue(page.Modules.Any(m => m is DiskMonitorModule), "Disk Monitor not found");
        }

        [Test]
        public void SubSystemContainsAPageAfterRegistration()
        {
            Assert.AreEqual(0, _subsystem.Pages.Count());

            _alfred.Register(_subsystem);
            _alfred.Initialize();

            Assert.AreEqual(1, _subsystem.Pages.Count());
        }

        [Test]
        public void AlfredContainsAPageAfterRegistration()
        {
            var pages = _alfred.RootPages.Count();

            _alfred.Register(_subsystem);
            _alfred.Initialize();

            Assert.AreEqual(pages + 1, _alfred.RootPages.Count());
        }
    }
}