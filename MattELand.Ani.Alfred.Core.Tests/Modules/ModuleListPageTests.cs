// ---------------------------------------------------------
// ModuleListPageTests.cs
// 
// Created on:      09/11/2015 at 9:31 PM
// Last Modified:   09/11/2015 at 9:31 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Linq;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core;
using MattEland.Ani.Alfred.Core.Pages;
using MattEland.Ani.Alfred.Core.Subsystems;
using MattEland.Ani.Alfred.Tests.Controls;
using MattEland.Common;
using MattEland.Testing;

using Moq;

using NUnit.Framework;

using Shouldly;

namespace MattEland.Ani.Alfred.Tests.Modules
{
    /// <summary>
    ///     A class containing tests for the <see cref="ModuleListPage"/> class.
    /// </summary>
    [SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    [SuppressMessage("ReSharper", "ExceptionNotDocumentedOptional")]
    [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    [Category("Modules")]
    [Category("Pages")]
    public sealed class ModuleListPageTests : UserInterfaceTestBase
    {
        /// <summary>
        ///     The mocking behavior.
        /// </summary>
        private const MockBehavior MockingBehavior = MockBehavior.Strict;

        /// <summary>
        ///     Sets up the environment for each test by creating a <see cref="ModuleListPage"/>, a
        ///     Subsystem to contain it, and registering all of these into an Alfred instance.
        /// </summary>
        public override void SetUp()
        {
            base.SetUp();

            // Create a subsystem to hold the page
            var subsystem = new SimpleSubsystem(Container, "Test Subsystem", "TestSubsystemId");

            // Build the Page and add it to the subsystem
            Page = new ModuleListPage(Container, "Test Page", "TestPageId");
            subsystem.PagesToRegister.Add(Page);

            // Build out Alfred and give it our page
            Alfred = BuildAlfredInstance();
            Alfred.Register(subsystem);
        }

        /// <summary>
        ///     Gets or sets the page.
        /// </summary>
        /// <value>
        ///     The page.
        /// </value>
        [NotNull]
        private ModuleListPage Page { get; set; }

        /// <summary>
        ///     When a page has a module with widgets in it, the page should be visible
        /// </summary>
        [Test]
        [Category("Visibility")]
        public void ModuleListPageIsVisibleWhenVisibleWidgetsArePresent()
        {
            //! Arrange

            // Set up a widget to be visible
            var widget = BuildMockWidget(MockingBehavior);
            widget.SetupGet(w => w.IsVisible)
                .Returns(true);

            // Add a module to contain things
            var module = BuildMockModule(MockingBehavior);
            module.SetupGet(m => m.Widgets)
                .Returns(widget.Object.ToCollection(Container));

            //! Act
            Page.Register(module.Object);
            Alfred.Initialize();

            //! Assert
            Page.IsVisible.ShouldBeTrue();
        }

        /// <summary>
        ///     When a page has no module with visible widgets, the page not should be visible
        /// </summary>
        [Test]
        [Category("Visibility")]
        public void ModuleListPageIsNotVisibleWhenNoVisibleWidgetsArePresent()
        {
            //! Arrange

            // Set up a widget to be hidden
            var widget = BuildMockWidget(MockingBehavior);
            widget.SetupGet(w => w.IsVisible)
                .Returns(false);

            // Add a module to contain things
            var module = BuildMockModule(MockingBehavior);
            module.SetupGet(m => m.Widgets)
                .Returns(widget.Object.ToCollection(Container));

            //! Act
            Page.Register(module.Object);
            Alfred.Initialize();

            //! Assert
            Page.IsVisible.ShouldBeFalse();
        }

        /// <summary>
        ///     You should be able to add modules to the module list page
        /// </summary>
        [Test]
        [Category("Visibility")]
        public void ModuleListPageIsNotVisibleWhenNoModulesArePresent()
        {
            //! Arrange - Object creation done in setup. Page is in Alfred with no Modules

            //! Act
            Alfred.Initialize();

            //! Assert
            Page.IsVisible.ShouldBeFalse();
        }

        /// <summary>
        ///     You should be able to add modules to the module list page
        /// </summary>
        [Test]
        [Category("Registration")]
        public void CanAddModulesToModuleListPage()
        {
            //! Arrange
            var module = BuildMockModule(MockingBehavior);

            //! Act
            Page.Register(module.Object);

            //! Assert
            Page.Modules.Count().ShouldBe(1);
            Page.Modules.ShouldContain(module.Object);
        }

        /// <summary>
        ///     You should be able to clear all modules on the module list page
        /// </summary>
        [Test]
        [Category("Registration")]
        public void CanClearModulesFromModuleListPage()
        {
            //! Arrange
            var module = BuildMockModule(MockingBehavior);

            //! Act
            Page.Register(module.Object);
            Page.ClearModules();

            //! Assert
            Page.Modules.Count().ShouldBe(0);
            Page.Modules.ShouldNotContain(module.Object);
        }

    }
}