// ---------------------------------------------------------
// SearchPageTests.cs
// 
// Created on:      09/13/2015 at 12:01 PM
// Last Modified:   09/13/2015 at 12:01 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Modules;
using MattEland.Ani.Alfred.Core.Pages;
using MattEland.Ani.Alfred.PresentationShared.Commands;
using MattEland.Ani.Alfred.Tests.Controls;
using MattEland.Testing;

using NUnit.Framework;

using Shouldly;

namespace MattEland.Ani.Alfred.Tests.Search
{
    /// <summary>
    ///     Holds tests related to the search page
    /// </summary>
    [UnitTestProvider]
    public sealed class SearchPageTests : UserInterfaceTestBase
    {
        /// <summary>
        ///     The search page should exist as a root page
        /// </summary>
        [Test]
        public void SearchPageIsARootPage()
        {
            //! Arrange
            var app = new ApplicationManager(Container, BuildOptions());
            Alfred = app.Alfred;

            //! Act
            var page = Alfred.RootPages.FirstOrDefault(p => p is SearchPage);

            //! Assert
            page.ShouldNotBeNull();
            page.IsRootLevel.ShouldBeTrue();
        }

        /// <summary>
        ///     The search page should contain a search module
        /// </summary>
        [Test]
        public void SearchPageShouldContainSearchModule()
        {
            //! Arrange
            var page = new SearchPage(Container);

            //! Act
            var module = page.Children.FirstOrDefault(m => m is AlfredModule);

            //! Assert
            module.ShouldNotBeNull();
        }

        /// <summary>
        ///     The search page should use a vertical layout
        /// </summary>
        [Test]
        public void SearchPageShouldHaveVerticalLayout()
        {
            //! Arrange
            var page = new SearchPage(Container);

            //! Act
            var layout = page.LayoutType;

            //! Assert
            layout.ShouldBe(LayoutType.VerticalStackPanel);
        }
    }
}