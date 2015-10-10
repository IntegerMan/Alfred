// ---------------------------------------------------------
// SubSystemTests.cs
// 
// Created on:      08/09/2015 at 12:12 AM
// Last Modified:   08/09/2015 at 12:12 AM
// Original author: Matt Eland
// ---------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

using MattEland.Common.Annotations;

using MattEland.Ani.Alfred.Core;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Common.Testing;

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
    public sealed class SubsystemTests : MockEnabledAlfredTestBase
    {

        [NotNull]
        private Mock<IAlfredSubsystem> _subsystem;

        /// <summary>
        /// Sets up the environment for each test.
        /// </summary>
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            Alfred = BuildAlfredInstance();

            _subsystem = BuildMockSubsystem();
        }

        /// <summary>
        ///     Initializing Alfred causes registered subsystems to get calls to Initialize and
        ///     OnInitializationCompleted methods.
        /// </summary>
        [Test]
        public void InitializeCausesRegisteredSubSystemsToInitialize()
        {
            //! Arrange

            Alfred.RegistrationProvider.Register(_subsystem.Object);

            //! Act

            Alfred.Initialize();

            //! Assert

            _subsystem.Verify(s => s.Initialize(It.Is<IAlfred>(a => a == Alfred)), Times.Once);
            _subsystem.Verify(s => s.OnInitializationCompleted(), Times.Once);
        }

        /// <summary>
        ///     Initializing Alfred causes registered subsystems to go online.
        /// </summary>
        [Test]
        public void InitializeCausesSubSystemsToGoOnline()
        {

            //! Arrange

            Alfred.RegistrationProvider.Register(_subsystem.Object);

            //! Act

            Alfred.Initialize();

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

            Alfred.RegistrationProvider.Register(_subsystem.Object);

            //! Act

            Alfred.Initialize();
            Alfred.Update();

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

            Alfred.RegistrationProvider.Register(_subsystem.Object);

            //! Act

            Alfred.Initialize();
            Alfred.Update();
            Alfred.Shutdown();

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

            Alfred.RegistrationProvider.Register(_subsystem.Object);

            //! Act

            Alfred.Initialize();
            Alfred.Update();
            Alfred.Shutdown();

            //! Assert

            _subsystem.Object.Status.ShouldBe(AlfredStatus.Offline);
        }

    }
}