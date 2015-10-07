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
using MattEland.Ani.Alfred.Core.Pages;

using MattEland.Ani.Alfred.Tests.Controls;
using MattEland.Testing;

using NUnit.Framework;

using Shouldly;
using MattEland.Common;
using MattEland.Ani.Alfred.Core.Widgets;

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
    public sealed class SearchHistoryPageTests : UserInterfaceTestBase
    {

        /// <summary>
        ///     The search page should exist as a root page
        /// </summary>
        [Test]
        public void SearchHistoryPageIsARootPage()
        {
            //! Arrange
            var app = BuildApplicationInstance();
            Alfred = app.Alfred;

            //! Act
            var page = GetPageById<SearchHistoryPage>(SearchHistoryPage.PageId);

            //! Assert
            page.ShouldNotBeNull();
            page.IsRootLevel.ShouldBeTrue();
        }

        /// <summary>
        ///     The search history page should use a vertical layout
        /// </summary>
        [Test]
        [Category("Layout")]
        public void SearchHistoryPageShouldHaveVerticalLayout()
        {
            //! Arrange
            var searchController = BuildMockSearchController();
            var page = new SearchHistoryPage(AlfredContainer, searchController.Object);

            //! Act
            var layout = page.LayoutType;

            //! Assert
            layout.ShouldBe(LayoutType.VerticalStackPanel);
        }

        /// <summary>
        ///     The search history page should have a repeater
        /// </summary>
        [Test]
        public void SearchHistoryPageContainsRepeaterModule()
        {
            //! Arrange
            var searchController = BuildMockSearchController();
            var page = new SearchHistoryPage(AlfredContainer, searchController.Object);

            //! Act
            var repeater = page.Widgets.FirstOrDefault(w => w.Name.Matches("listHistory"));

            //! Assert
            repeater.ShouldNotBeNull();
            repeater.ShouldBe<Repeater>();
        }

        /// <summary>
        ///     Search text should appear in search history.
        /// </summary>
        [Test]
        public void SearchTextAppearsInSearchHistory()
        {
            //! Arrange

            // We'll want the full Alfred instance to test the controller and page interacting
            var app = BuildApplicationInstance();
            Alfred = app.Alfred;

            string searchText = Some.SearchText;

            //! Act

            // Bring Alfred Online
            Alfred.Initialize();

            // SearchPageTests
            Alfred.SearchController.PerformSearch(searchText);
            Alfred.Update();

            // Get the page
            var page = GetPageById<SearchHistoryPage>(SearchHistoryPage.PageId);

            var noCountWidget = page.Widgets.FirstOrDefault(w => w.Name.Matches("lblNoItems"));
            var listWidget = page.Widgets.FirstOrDefault(w => w.Name.Matches("listHistory"));

            //! Assert

            // Check the item count
            listWidget.ShouldNotBeNull();
            var repeater = listWidget.ShouldBe<Repeater>();
            repeater.Items.Count().ShouldBe(1);

            // Grab the widget represented by the search
            var firstItem = repeater.Items.First();
            var textControl = firstItem.ShouldBe<TextWidget>();

            // Verify the widget has the text we searched for
            textControl.Text.EndsWith(searchText);

            // Check that the no items widget is hidden
            noCountWidget.ShouldNotBeNull();
            noCountWidget.IsVisible.ShouldBeFalse();

        }

        /// <summary>
        ///     Search history should be empty without searches.
        /// </summary>
        [Test]
        public void SearchHistoryIsEmptyWithNoSearches()
        {
            //! Arrange

            // We'll want the full Alfred instance to test the controller and page interacting
            var app = BuildApplicationInstance();
            Alfred = app.Alfred;

            //! Act

            // Bring Alfred Online
            Alfred.Initialize();
            Alfred.Update();

            // Get the page
            var page = GetPageById<SearchHistoryPage>(SearchHistoryPage.PageId);

            var noCountWidget = page.Widgets.FirstOrDefault(w => w.Name.Matches("lblNoItems"));
            var listWidget = page.Widgets.FirstOrDefault(w => w.Name.Matches("listHistory"));

            //! Assert

            // The no count widget should be visible
            noCountWidget.ShouldNotBeNull();
            noCountWidget.IsVisible.ShouldBeTrue();

            // Check the item count
            var repeater = listWidget.ShouldBe<Repeater>();
            repeater.Items.Count().ShouldBe(0);

        }
    }

}