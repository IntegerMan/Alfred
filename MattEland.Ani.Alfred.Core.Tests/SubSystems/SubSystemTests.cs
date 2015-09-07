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

using Moq;

using NUnit.Framework;

using Shouldly;

namespace MattEland.Ani.Alfred.Tests.Subsystems
{
    /// <summary>
    /// Tests oriented around testing the subsystem update pumps and related functions
    /// </summary>
    [UnitTestProvider]
    [SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
    [SuppressMessage("ReSharper", "ExceptionNotDocumented")]
    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    public class SubsystemTests : AlfredTestBase
    {
        [NotNull]
        private AlfredApplication _alfred;

        [NotNull]
        private Mock<IAlfredSubsystem> _subsystem;

        /// <summary>
        /// Sets up the environment for each test.
        /// </summary>
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            _alfred = new AlfredApplication(Container);

            _subsystem = BuildMockSubsystem(MockBehavior.Strict);
        }

        /// <summary>
        ///     Initializing Alfred causes registered subsystems to get calls to Initialize and
        ///     OnInitializationCompleted methods.
        /// </summary>
        [Test]
        public void InitializeCausesRegisteredSubSystemsToInitialize()
        {
            //! Arrange

            _alfred.RegistrationProvider.Register(_subsystem.Object);

            //! Act

            _alfred.Initialize();

            //! Assert

            _subsystem.Verify(s => s.Initialize(It.Is<IAlfred>(a => a == _alfred)), Times.Once);
            _subsystem.Verify(s => s.OnInitializationCompleted(), Times.Once);
        }

        /// <summary>
        ///     Initializing Alfred causes registered subsystems to go online.
        /// </summary>
        [Test]
        public void InitializeCausesSubSystemsToGoOnline()
        {

            //! Arrange

            _alfred.RegistrationProvider.Register(_subsystem.Object);

            //! Act

            _alfred.Initialize();

            //! Assert

            _subsystem.Object.Status.ShouldBe(AlfredStatus.Online);
        }

        /// <summary>
        ///     Updating Alfred causes registered subsystems to get Update calls.
        /// </summary>
        [Test]
        public void UpdateCausesRegisteredSubSystemsToUpdate()
        {

            //! Arrange

            _alfred.RegistrationProvider.Register(_subsystem.Object);

            //! Act

            _alfred.Initialize();
            _alfred.Update();

            //! Assert

            _subsystem.Verify(s => s.Update(), Times.Once);
        }

        /// <summary>
        ///     Shutdown on Alfred causes registered subsystems to get Shutdown and OnShutdownCompleted
        ///     calls.
        /// </summary>
        [Test]
        public void ShutdownCausesRegisteredSubSystemsToShutdown()
        {

            //! Arrange

            _alfred.RegistrationProvider.Register(_subsystem.Object);

            //! Act

            _alfred.Initialize();
            _alfred.Update();
            _alfred.Shutdown();

            //! Assert

            _subsystem.Verify(s => s.Shutdown(), Times.Once);
            _subsystem.Verify(s => s.OnShutdownCompleted(), Times.Once);
        }

        /// <summary>
        ///     Shutdown on Alfred causes registered sub systems to go to Offline status.
        /// </summary>
        [Test]
        public void ShutdownCausesRegisteredSubSystemsToGoOffline()
        {

            //! Arrange

            _alfred.RegistrationProvider.Register(_subsystem.Object);

            //! Act

            _alfred.Initialize();
            _alfred.Update();
            _alfred.Shutdown();

            //! Assert

            _subsystem.Object.Status.ShouldBe(AlfredStatus.Offline);
        }

    }
}