// ---------------------------------------------------------
// SubSystemTests.cs
// 
// Created on:      08/09/2015 at 12:12 AM
// Last Modified:   08/09/2015 at 12:12 AM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Tests.Mocks;
using MattEland.Testing;

using NUnit.Framework;

namespace MattEland.Ani.Alfred.Tests.Subsystems
{
    /// <summary>
    /// Tests oriented around testing the subsystem update pumps and related functions
    /// </summary>
    [UnitTestProvider]
    [SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    public class SubsystemTests : AlfredTestBase
    {
        [NotNull]
        private AlfredApplication _alfred;

        [NotNull]
        private TestSubsystem _subsystem;

        /// <summary>
        /// Sets up the environment for each test.
        /// </summary>
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            _alfred = new AlfredApplication(Container);

            _subsystem = new TestSubsystem(Container);
        }


        [Test]
        public void InitializeCausesRegisteredSubSystemsToInitialize()
        {

            _alfred.RegistrationProvider.Register(_subsystem);

            _alfred.Initialize();

            Assert.IsTrue(_subsystem.LastInitialized > DateTime.MinValue, "Subsystem was not initialized");
            Assert.IsTrue(_subsystem.LastInitializationCompleted > DateTime.MinValue,
                          "Subsystem was not notified initialized completed");
        }

        [Test]
        public void InitializeCausesSubSystemsToGoOnline()
        {

            _alfred.RegistrationProvider.Register(_subsystem);

            _alfred.Initialize();

            Assert.AreEqual(AlfredStatus.Online, _subsystem.Status);
        }

        [Test]
        public void UpdateCausesRegisteredSubSystemsToUpdate()
        {

            _alfred.RegistrationProvider.Register(_subsystem);

            _alfred.Initialize();
            _alfred.Update();

            Assert.IsTrue(_subsystem.LastUpdated > DateTime.MinValue, "Subsystem was not updated");
        }

        [Test]
        public void ShutdownCausesRegisteredSubSystemsToShutdown()
        {

            _alfred.RegistrationProvider.Register(_subsystem);

            _alfred.Initialize();
            _alfred.Update();
            _alfred.Shutdown();

            Assert.IsTrue(_subsystem.LastShutdown > DateTime.MinValue, "Subsystem was not shut down");
            Assert.IsTrue(_subsystem.LastShutdownCompleted > DateTime.MinValue, "Subsystem was not notified of shut down completion");
        }

        [Test]
        public void ShutdownCausesRegisteredSubSystemsToGoOffline()
        {

            _alfred.RegistrationProvider.Register(_subsystem);

            _alfred.Initialize();
            _alfred.Update();
            _alfred.Shutdown();

            Assert.AreEqual(AlfredStatus.Offline, _subsystem.Status);
        }

    }
}