// ---------------------------------------------------------
// SystemSubSystemTests.cs
// 
// Created on:      08/07/2015 at 10:01 PM
// Last Modified:   08/07/2015 at 10:01 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Linq;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core;
using MattEland.Ani.Alfred.Core.Modules.SysMonitor;
using MattEland.Ani.Alfred.Core.Pages;
using MattEland.Testing;

using NUnit.Framework;

using Shouldly;

namespace MattEland.Ani.Alfred.Tests.Subsystems
{
    /// <summary>
    /// A series of tests related to the System Monitoring SubSystem
    /// </summary>
    [UnitTestProvider]
    [SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    public sealed class SystemSubsystemTests : AlfredTestBase
    {
        [NotNull]
        private SystemMonitoringSubsystem _subsystem;

        [NotNull]
        private AlfredApplication _alfred;

        private ValueMetricProviderFactory _metricProviderFactory;

        /// <summary>
        /// Sets up the test environment
        /// </summary>
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            _metricProviderFactory = new ValueMetricProviderFactory();
            _subsystem = new SystemMonitoringSubsystem(Container);

            _alfred = new AlfredApplication(Container);
        }

        [Test]
        public void SystemMonitoringSubsystemCanBeRegisteredInAlfred()
        {
            _alfred.RegistrationProvider.Register(_subsystem);

            _alfred.Subsystems.Count().ShouldBe(1);
            _alfred.Subsystems.ShouldContain(_subsystem);
        }

        [Test]
        public void SystemMonitoringSubsystemContainsModules()
        {
            _alfred.RegistrationProvider.Register(_subsystem);

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

            _alfred.RegistrationProvider.Register(_subsystem);
            _alfred.Initialize();

            Assert.AreEqual(1, _subsystem.Pages.Count());
        }

        [Test]
        public void AlfredContainsAPageAfterRegistration()
        {
            var pages = _alfred.RootPages.Count();

            _alfred.RegistrationProvider.Register(_subsystem);
            _alfred.Initialize();

            Assert.AreEqual(pages + _subsystem.RootPages.Count(), _alfred.RootPages.Count());
        }
    }
}