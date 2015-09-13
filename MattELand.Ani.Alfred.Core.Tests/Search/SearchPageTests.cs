// ---------------------------------------------------------
// SearchPageTests.cs
// 
// Created on:      09/13/2015 at 12:01 PM
// Last Modified:   09/13/2015 at 12:01 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Linq;

using MattEland.Ani.Alfred.Core.Pages;
using MattEland.Ani.Alfred.PresentationShared.Commands;
using MattEland.Ani.Alfred.Tests.Controls;
using MattEland.Testing;

using NUnit.Framework;

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
            var page = Alfred.RootPages.FirstOrDefault(p => p.GetType() == typeof(SearchPage));

            //! Assert
            page.ShouldNotBeNull();
            page.IsRootLevel.ShouldBeTrue();
        }
    }
}