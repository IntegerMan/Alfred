// ---------------------------------------------------------
// SearchModuleTests.cs
// 
// Created on:      09/13/2015 at 4:35 PM
// Last Modified:   09/14/2015 at 1:01 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Linq;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Modules;
using MattEland.Ani.Alfred.Core.Widgets;
using MattEland.Ani.Alfred.Tests.Controls;
using MattEland.Common;
using MattEland.Testing;

using Moq;

using NUnit.Framework;

using Shouldly;

namespace MattEland.Ani.Alfred.Tests.Search
{
    /// <summary>
    ///     Search module tests. This class cannot be inherited.
    /// </summary>
    [UnitTestProvider]
    [Category("Search")]
    [Category("Modules")]
    [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
    [SuppressMessage("ReSharper", "ExceptionNotDocumentedOptional")]
    [SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    [SuppressMessage("ReSharper", "ExceptionNotDocumented")]
    public sealed class SearchModuleTests : UserInterfaceTestBase
    {

        /// <summary>
        ///     Gets or sets the module.
        /// </summary>
        /// <value>
        /// The module.
        /// </value>
        [NotNull]
        private SearchModule Module { get; set; }

        /// <summary>
        ///     Sets up the environment for each test.
        /// </summary>
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            Module = new SearchModule(Container);
        }

        /// <summary>
        ///     The search module should contain a valid search button
        /// </summary>
        [Test]
        [Category("Widgets")]
        public void SearchModuleContainsValidSearchButton()
        {
            //! Arrange

            Alfred = BuildAlfredInstance();
            var module = Module;

            //! Act

            module.Initialize(Alfred);
            var widget = module.Widgets.FirstOrDefault(w => w.Name.Matches(@"btnSearch"));

            //! Assert

            widget.ShouldNotBeNull();
            var button = widget.ShouldBe<ButtonWidget>();

            button.Text.ShouldBe("Search");
            button.IsVisible.ShouldBeTrue();
            button.ClickCommand.ShouldNotBeNull();
            button.ClickCommand.CanExecute(null).ShouldBeTrue();
        }

        /// <summary>
        ///     The search module should contain a valid search label
        /// </summary>
        [Test]
        [Category("Widgets")]
        public void SearchModuleContainsValidSearchLabel()
        {
            //! Arrange

            Alfred = BuildAlfredInstance();
            var module = Module;

            //! Act

            module.Initialize(Alfred);
            var widget = module.Widgets.FirstOrDefault(w => w.Name.Matches(@"lblSearch"));

            //! Assert

            widget.ShouldNotBeNull();
            var label = widget.ShouldBe<TextWidget>();

            label.Text.ShouldBe("Search:");
            label.IsVisible.ShouldBeTrue();
        }

        /// <summary>
        ///     The search module should contain a valid search text box
        /// </summary>
        [Test]
        [Category("Widgets")]
        public void SearchModuleContainsValidSearchTextBox()
        {
            //! Arrange

            Alfred = BuildAlfredInstance();
            var module = Module;

            //! Act

            module.Initialize(Alfred);
            var widget = module.Widgets.FirstOrDefault(w => w.Name.Matches(@"txtSearch"));

            //! Assert

            widget.ShouldNotBeNull();
            var textBox = widget.ShouldBe<TextBoxWidget>();

            textBox.Text.ShouldBeNullOrEmpty();
            textBox.IsVisible.ShouldBeTrue();
        }

        /// <summary>
        ///     The search module should arrange items horizontally
        /// </summary>
        [Test]
        [Category("Layout")]
        public void SearchModuleUsesHorizontalLayout()
        {
            //! Arrange

            var module = Module;

            //! Act

            var layout = module.LayoutType;

            //! Assert

            layout.ShouldBe(LayoutType.HorizontalStackPanel);
        }

        /// <summary>
        ///     The search module should arrange items horizontally
        /// </summary>
        [Test]
        [Category("Layout")]
        public void SearchModuleShouldNotLimitWidth()
        {
            //! Arrange

            var module = Module;

            //! Act

            var width = module.Width;

            //! Assert

            width.ShouldBe(double.NaN);
        }

        /// <summary>
        ///     Tests clicking the search button with search text actually searches
        /// </summary>
        [Test]
        public void SearchOccursWhenButtonIsClickedAndTextIsPresent()
        {
            //! Arrange

            // Set up a mock controller for verification of search requested
            var searchController = BuildMockSearchController();

            // Set up our Alfred to control where searches go
            var alfred = BuildMockAlfred();
            alfred.SetupGet(a => a.SearchController).Returns(searchController.Object);

            Alfred = alfred.Object;

            //! Act

            // Start up Alfred so we have a User Interface
            var module = Module;
            module.Initialize(Alfred);

            // Grab UI components from the module
            var textBox = FindWidgetOfTypeByName<TextBoxWidget>(module, @"txtSearch");
            var searchButton = FindWidgetOfTypeByName<ButtonWidget>(module, @"btnSearch");

            // Set the search and execute it
            const string SearchText = "Find something please";
            textBox.Text = SearchText;
            searchButton.Click();

            //! Assert

            const string FailMessage = "Clicking the search button with valid search text did not execute a search";
            searchController.Verify(sc => sc.PerformSearch(SearchText),
                                    Times.Once,
                                    FailMessage);
        }

        /// <summary>
        ///     An alert message box should be shown when attempting to search with bad search values.
        /// </summary>
        /// <param name="input"> The input. </param>
        [TestCase(null)]
        [TestCase("")]
        [TestCase("   ")]
        [Category("Validation")]
        public void MessageBoxIsShownWhenSearchIsAttemptedWithBadInput(string input)
        {
            //! Arrange

            var messageBox = new Mock<IMessageBoxProvider>(MockingBehavior);
            MessageBox = messageBox.Object;

            Alfred = BuildMockAlfred().Object;

            //! Act

            // Start up Alfred so we have a User Interface
            Module.Initialize(Alfred);

            // Grab UI components from the module
            var textBox = FindWidgetOfTypeByName<TextBoxWidget>(Module, @"txtSearch");
            var searchButton = FindWidgetOfTypeByName<ButtonWidget>(Module, @"btnSearch");

            // Set the search and execute it
            textBox.Text = input;
            searchButton.Click();

            //! Assert
            messageBox.Verify(m => m.ShowAlert(It.IsAny<string>(), "Cannot Search"), Times.Once);

        }
    }
}