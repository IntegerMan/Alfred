// ---------------------------------------------------------
// AlfredPagesListModuleTests.cs
// 
// Created on:      08/29/2015 at 10:15 PM
// Last Modified:   08/29/2015 at 10:15 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Linq;

using MattEland.Ani.Alfred.Core;
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
            var app = new ApplicationManager(Container);

            var alfred = app.Alfred;
            alfred.ShouldNotBeNull();
            alfred.Initialize();

            var subsystem = alfred.Subsystems.First(s => s.Id.Matches("Core"));
            subsystem.ShouldNotBeNull();

            var page = subsystem.Pages.First(p => p.Id.Matches("Core"));
            page.ShouldNotBeNull();

            var listPage = page.ShouldBeOfType<AlfredModuleListPage>();
            listPage.ShouldNotBeNull();

            var module = listPage.Modules.First(m => m is AlfredPagesListModule);
            module.ShouldNotBeNull();

            module.Widgets.Count().ShouldBe(alfred.RootPages.Count());
        }
    }
}