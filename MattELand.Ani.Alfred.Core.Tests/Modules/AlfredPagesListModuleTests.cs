// ---------------------------------------------------------
// AlfredPagesListModuleTests.cs
// 
// Created on:      08/29/2015 at 10:15 PM
// Last Modified:   08/29/2015 at 10:15 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Linq;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Modules;
using MattEland.Ani.Alfred.Core.Pages;
using MattEland.Ani.Alfred.PresentationShared.Commands;
using MattEland.Common;

using NUnit.Framework;

using Shouldly;

namespace MattEland.Ani.Alfred.Tests.Modules
{
    /// <summary>
    ///     Tests <see cref="AlfredPagesListModule"/>
    /// </summary>
    [TestFixture]
    public class AlfredPagesListModuleTests : AlfredTestBase
    {
        /// <summary>
        /// Sets up the environment for each test.
        /// </summary>
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            StartAlfred();
        }

        /// <summary>
        ///     Tests that the pages list module should include all pages.
        /// </summary>
        /// <remarks>
        ///     This is from an actual issue where the <see cref="AlfredPagesListModule"/> would not
        ///     display all pages on its list - only those that had registered before the widget
        ///     registered its controls.
        ///     
        ///     See issue ALF-81.
        /// </remarks>
        [Test]
        public void PagesListModuleShouldListAllPages()
        {
            var module = GetPagesListModule();

            // We expect a widget for every page in the system
            module.Widgets.Count().ShouldBe(Alfred.RootPages.Count());
        }

        /// <summary>
        ///     Gets the pages list module.
        /// </summary>
        /// <returns>
        ///     The pages list module.
        /// </returns>
        [NotNull]
        private IAlfredModule GetPagesListModule()
        {
            var alfred = Container.Provide<IAlfred>();

            var subsystem = alfred.Subsystems.First(s => s.Id.Matches("Core"));
            subsystem.ShouldNotBeNull("Core Subsystem could not be found");

            var page = subsystem.Pages.First(p => p.Id.Matches("Core"));
            page.ShouldNotBeNull("Core Page could not be found");

            var listPage = page.ShouldBeOfType<AlfredModuleListPage>();
            listPage.ShouldNotBeNull("Could not find module list");

            var module = listPage.Modules.First(m => m is AlfredPagesListModule);
            module.ShouldNotBeNull("Could not find pages list module");

            return module;
        }
    }
}