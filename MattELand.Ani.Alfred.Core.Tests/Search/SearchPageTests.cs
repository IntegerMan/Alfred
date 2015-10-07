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

using MattEland.Ani.Alfred.Tests.Controls;
using MattEland.Testing;

using NUnit.Framework;

using Shouldly;
using MattEland.Common;

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
        /// The search module's name
        /// </summary>
        private const string SearchModuleName = "Search";

        /// <summary>
        /// The search results module's name
        /// </summary>
        private const string SearchResultsModuleName = "Search Results";

        /// <summary>
        ///     The search page should exist as a root page
        /// </summary>
        [Test]
        public void SearchPageIsARootPage()
        {
            //! Arrange
            var app = BuildApplicationInstance();
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
            var page = new SearchPage(AlfredContainer);

            //! Act
            var module = page.FindModuleByName(SearchModuleName);

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
            var page = new SearchPage(AlfredContainer);

            //! Act
            var module = page.FindModuleByName(SearchResultsModuleName);

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
            var page = new SearchPage(AlfredContainer);

            //! Act
            var layout = page.LayoutType;

            //! Assert
            layout.ShouldBe(LayoutType.VerticalStackPanel);
        }

        /// <summary>
        /// The search page should have an option to allow it not to include the search module
        /// </summary>
        [Test]
        public void SearchPageShouldBeConfigurableToNotIncludeSearchModule()
        {
            //! Arrange

            var page = new SearchPage(AlfredContainer, false);

            //! Act

            var module = page.FindModuleByName(SearchModuleName);

            //! Assert

            module.ShouldBeNull();
            page.Modules.Count().ShouldBe(1);
        }

    }

}