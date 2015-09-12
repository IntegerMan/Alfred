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

using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Pages;
using MattEland.Ani.Alfred.Tests.Controls;

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
    public sealed class ModuleListPageTests : UserInterfaceTestBase
    {
        /// <summary>
        ///     The mocking behavior.
        /// </summary>
        private const MockBehavior MockingBehavior = MockBehavior.Strict;

        /// <summary>
        /// Sets up the environment for each test.
        /// </summary>
        public override void SetUp()
        {
            base.SetUp();

            Page = new ModuleListPage(Container, "Test Page", "TestPageId");
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
        ///     You should be able to add modules to the module list page
        /// </summary>
        [Test]
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

        /// <summary>
        ///     Builds a mock module.
        /// </summary>
        /// <param name="mockingBehavior"> The mocking behavior. </param>
        /// <returns>
        ///     A mock module
        /// </returns>
        protected Mock<IAlfredModule> BuildMockModule(MockBehavior mockingBehavior)
        {
            var mock = new Mock<IAlfredModule>(mockingBehavior);

            mock.Setup(m => m.OnRegistered(It.IsAny<IAlfred>()));

            return mock;
        }

    }
}