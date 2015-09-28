// ---------------------------------------------------------
// SearchPageTests.cs
// 
// Created on:      09/13/2015 at 12:01 PM
// Last Modified:   09/13/2015 at 12:01 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Linq;

using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Modules;
using MattEland.Ani.Alfred.Core.Pages;
using MattEland.Ani.Alfred.PresentationAvalon.Commands;
using MattEland.Ani.Alfred.Tests.Controls;
using MattEland.Common;
using MattEland.Testing;

using NUnit.Framework;

using Shouldly;

namespace MattEland.Ani.Alfred.Tests.Search
{
    /// <summary>
    ///     Holds tests related to the search page
    /// </summary>
    [UnitTestProvider]
    [Category("Search")]
    [Category("Pages")]
    [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
    [SuppressMessage("ReSharper", "ExceptionNotDocumentedOptional")]
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
        [Category("Modules")]
        public void SearchPageShouldContainSearchModule()
        {
            //! Arrange
            var page = new SearchPage(Container);

            //! Act
            var module = page.Children.FirstOrDefault(m => m.Name.Matches("Search"));

            //! Assert
            module.ShouldNotBeNull();
            module.ShouldBe<SearchModule>();
        }

        /// <summary>
        ///     The search page should contain a search results module
        /// </summary>
        [Test]
        [Category("Modules")]
        public void SearchPageShouldContainSearchResultsModule()
        {
            //! Arrange
            var page = new SearchPage(Container);

            //! Act
            var module = page.Children.FirstOrDefault(m => m.Name.Matches("Search Results"));

            //! Assert
            module.ShouldNotBeNull();
            module.ShouldBe<SearchResultsModule>();
        }

        /// <summary>
        ///     The search page should use a vertical layout
        /// </summary>
        [Test]
        [Category("Layout")]
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