// ---------------------------------------------------------
// SystemSubSystemTests.cs
// 
// Created on:      08/07/2015 at 10:01 PM
// Last Modified:   08/07/2015 at 10:01 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections;
using System.Linq;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Modules.SysMonitor;

using NUnit.Framework;

namespace MattEland.Ani.Alfred.Core.Tests
{
    /// <summary>
    /// A series of tests related to the System Monitoring SubSystem
    /// </summary>
    [TestFixture]
    public class SystemSubSystemTests
    {
        [NotNull]
        private SystemMonitoringSystem _subsystem;

        [NotNull]
        private AlfredProvider _alfred;

        [SetUp]
        public void TestSetup()
        {
            _subsystem = new SystemMonitoringSystem();
            _alfred = new AlfredProvider();
        }

        [Test]
        public void SystemMonitoringSubsystemCanBeRegisteredInAlfred()
        {
            _alfred.RegisterSubSystem(_subsystem);

            Assert.AreEqual(1, _alfred.SubSystems.Count(), "Subsystem was not registered");
            Assert.Contains(_subsystem, _alfred.SubSystems as ICollection, "The subsystem was not found in the collection");
        }

    }
}