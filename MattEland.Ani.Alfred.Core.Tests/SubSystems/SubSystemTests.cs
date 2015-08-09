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

using MattEland.Ani.Alfred.Core.Tests.Mocks;

using NUnit.Framework;

namespace MattEland.Ani.Alfred.Core.Tests.SubSystems
{
    /// <summary>
    /// Tests oriented around testing the subsystem update pumps and related functions
    /// </summary>
    [TestFixture]
    [SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    public class SubSystemTests
    {
        [NotNull]
        private AlfredProvider _alfred;

        [NotNull]
        private TestSubsystem _subsystem;

        [SetUp]
        public void Setup()
        {
            _alfred = new AlfredProvider();
            _subsystem = new TestSubsystem();
        }


        [Test]
        public void InitializeCausesRegisteredSubSystemsToInitialize()
        {
            var testSubsystem = new TestSubsystem();

            _alfred.Register(testSubsystem);

            _alfred.Initialize();

            Assert.IsTrue(testSubsystem.LastInitialized > DateTime.MinValue, "Subsystem was not initialized");
            Assert.IsTrue(testSubsystem.LastInitializationCompleted > DateTime.MinValue,
                          "Subsystem was not notified initialized completed");
        }

        [Test]
        public void InitializeCausesSubSystemsToGoOnline()
        {
            var testSubsystem = new TestSubsystem();

            _alfred.Register(testSubsystem);

            _alfred.Initialize();

            Assert.AreEqual(AlfredStatus.Online, testSubsystem.Status);
        }

        [Test]
        public void UpdateCausesRegisteredSubSystemsToUpdate()
        {
            var testSubsystem = new TestSubsystem();

            _alfred.Register(testSubsystem);

            _alfred.Initialize();
            _alfred.Update();

            Assert.IsTrue(testSubsystem.LastUpdated > DateTime.MinValue, "Subsystem was not updated");
        }

        [Test]
        public void ShutdownCausesRegisteredSubSystemsToShutdown()
        {
            var testSubsystem = new TestSubsystem();

            _alfred.Register(testSubsystem);

            _alfred.Initialize();
            _alfred.Update();
            _alfred.Shutdown();

            Assert.IsTrue(testSubsystem.LastShutdown > DateTime.MinValue, "Subsystem was not shut down");
            Assert.IsTrue(testSubsystem.LastShutdownCompleted > DateTime.MinValue, "Subsystem was not notified of shut down completion");
        }

        [Test]
        public void ShutdownCausesRegisteredSubSystemsToGoOffline()
        {
            var testSubsystem = new TestSubsystem();

            _alfred.Register(testSubsystem);

            _alfred.Initialize();
            _alfred.Update();
            _alfred.Shutdown();

            Assert.AreEqual(AlfredStatus.Offline, testSubsystem.Status);
        }

    }
}