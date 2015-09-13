// ---------------------------------------------------------
// AlfredEventingTests.cs
// 
// Created on:      08/19/2015 at 9:31 PM
// Last Modified:   09/03/2015 at 12:18 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
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
using MattEland.Ani.Alfred.Core.Widgets;
using MattEland.Common.Providers;
using MattEland.Testing;

using Moq;

using NUnit.Framework;

using Shouldly;

namespace MattEland.Ani.Alfred.Tests
{
    /// <summary>
    ///     Tests <see cref="AlfredApplication" /> and its ability to relay status transitons to
    ///     other components and handle the component registration process.
    /// </summary>
    [UnitTestProvider]
    [SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
    [SuppressMessage("ReSharper", "ExceptionNotDocumentedOptional")]
    public sealed class AlfredEventingTests : AlfredTestBase
    {

        [NotNull]
        private ModuleListPage _page;

        [NotNull]
        private SimpleSubsystem _subsystem;

        /// <summary>
        ///     Sets up the Alfred provider's tests.
        /// </summary>
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            var alfred = new AlfredApplication(Container);
            alfred.RegisterAsProvidedInstance(typeof(IAlfred), Container);
            _subsystem = BuildTestSubsystem();
            _page = new ModuleListPage(Container, "Test AlfredPage", "Test");
        }

        /// <summary>
        ///     Builds widget parameters.
        /// </summary>
        /// <param name="name">The name of the widget.</param>
        /// <returns>The <see cref="WidgetCreationParameters" /> .</returns>
        [NotNull]
        private WidgetCreationParameters BuildWidgetParams(string name = "WidgetTest")
        {
            return new WidgetCreationParameters(name, Container);
        }

        /// <summary>
        ///     Tests that adding standard modules adds modules.
        /// </summary>
        [Test]
        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        public void AddingStandardModulesAddsModules()
        {
            Alfred.RegistrationProvider.Register(new AlfredCoreSubsystem(Container));

            var numModules =
                Alfred.Subsystems.SelectMany(subsystem => subsystem.Pages)
                      .OfType<ModuleListPage>()
                      .Sum(modulePage => modulePage.Modules.Count());

            const string FailMessage = "Alfred did not have any modules after calling add standard modules.";
            numModules.ShouldBeGreaterThan(0, FailMessage);
        }

        /// <summary>
        ///     Alfred should start with no sub systems.
        /// </summary>
        [Test]
        public void AlfredStartsWithNoSubSystems()
        {
            Alfred.Subsystems.Count()
                  .ShouldBe(0, "Alfred started with subsystems when none were expected.");
        }

        /// <summary>
        ///     Tests that initializing Alfred causes components to initialize
        /// </summary>
        [Test]
        public void InitializingInitializesComponents()
        {
            Alfred.RegistrationProvider.Register(new AlfredCoreSubsystem(Container));

            Alfred.Initialize();

            foreach (var item in Alfred.Subsystems)
            {
                item.Status.ShouldBe(AlfredStatus.Online,
                                     $"{item.NameAndVersion} was not initialized during initialization.");
            }
        }

        /// <summary>
        ///     Tests that logging causes events to be logged
        /// </summary>
        [Test]
        public void LogToConsole()
        {
            var console = Container.Provide<IConsole>();
            console.ShouldNotBeNull("Console was not present");

            var numEvents = console.Events.Count();

            console.Log("Alfred Test Framework", "Testing logging to Alfred", LogLevel.Verbose);

            console.Events.Count()
                   .ShouldBe(numEvents + 1, "Event count did not increase after logging.");
        }

        /// <summary>
        ///     Ensures Modules cannot be added while online.
        /// </summary>
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ModulesCannotBeAddedWhileOnline()
        {
            Alfred.Initialize();

            var subsystem = new AlfredCoreSubsystem(Container);

            Alfred.RegistrationProvider.Register(subsystem);
        }

        /// <summary>
        ///     Ensures Modules cannot update while offline.
        /// </summary>
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ModulesCannotUpdateWhileOffline()
        {
            Alfred.RegistrationProvider.Register(new AlfredCoreSubsystem(Container));

            Alfred.Update();
        }

        /// <summary>
        ///     Ensures that registering a widget multiple times throws an exception.
        /// </summary>
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RegisteringAWidgetMultipleTimesThrowsAnException()
        {
            //! Arrange

            // Build a duplicate list of widgets
            var textWidget = new TextWidget(BuildWidgetParams());
            var duplicateWidgets = new List<IWidget> { textWidget, textWidget };

            // Build a mock object so that it will register the duplicates during its initialize
            var mockModule = new Mock<AlfredModule>(MockBehavior.Strict, Container);
            mockModule.Setup(m => m.OnRegistered(It.IsAny<IAlfred>()));
            mockModule.Setup(m => m.Initialize(It.Is<IAlfred>(a => a == Alfred)))
                .Callback(() => mockModule.Object.Register(duplicateWidgets));

            // Register all of the things
            _page.Register(mockModule.Object);
            _subsystem.PagesToRegister.Add(_page);
            Alfred.Register(_subsystem);

            //! Act / Assert (exception expected)

            // Start up Alfred and watch the world burn
            Alfred.Initialize();
        }

        /// <summary>
        ///     Registering <see langword="null" /> subsystem generates a
        ///     <see cref="NullReferenceException" />
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void RegisteringNullSubsystemGeneratesNullRef()
        {
            AlfredSubsystem system = null;
            Alfred.RegistrationProvider.Register(system);
        }

        /// <summary>
        ///     Ensures registering widget at initialize and shutdown leaves one copy in list at
        ///     reinitialize.
        /// </summary>
        [Test]
        public void RegisteringWidgetAtInitializeAndShutdownLeavesOneCopyInListAtReinitialize()
        {
            //! Arrange 

            var textWidget = new TextWidget(BuildWidgetParams());

            // This module should register its control both at initialize and shutdown
            var testModule = new SimpleModule(Container, "Test Module");
            testModule.WidgetsToRegisterOnInitialize.Add(textWidget);
            testModule.WidgetsToRegisterOnShutdown.Add(textWidget);

            // Set up our systems
            Alfred.Register(_subsystem);
            _subsystem.PagesToRegister.Add(_page);
            _page.Register(testModule);

            //! Act

            Alfred.Initialize();
            Alfred.Shutdown();
            Alfred.Initialize();

            //! Assert

            testModule.Widgets.ShouldNotBeNull("testModule.Widgets was null");
            testModule.Widgets.Count().ShouldBe(1,
                            "Widgets were not properly cleared from list after re-initialize");
        }

        /// <summary>
        ///     Shutting down shuts down components.
        /// </summary>
        [Test]
        public void ShuttingDownShutsDownComponents()
        {
            Alfred.RegistrationProvider.Register(new AlfredCoreSubsystem(Container));

            Alfred.Initialize();
            Alfred.Shutdown();

            foreach (var item in Alfred.Subsystems)
            {
                Assert.AreEqual(AlfredStatus.Offline,
                                item.Status,
                                $"{item.NameAndVersion} was not shut down during alfred shut down.");
            }
        }

        /// <summary>
        ///     Updates the with no modules while offline still generates errors.
        /// </summary>
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void UpdateWithNoModulesWhileOfflineStillGeneratesError()
        {
            Alfred.Update();
        }
    }

}