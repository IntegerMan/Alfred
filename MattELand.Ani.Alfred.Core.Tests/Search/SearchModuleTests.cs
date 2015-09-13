using System.Diagnostics.CodeAnalysis;
using System.Linq;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Modules;
using MattEland.Ani.Alfred.Core.Widgets;
using MattEland.Ani.Alfred.Tests.Controls;
using MattEland.Common;
using MattEland.Testing;

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
    public sealed class SearchModuleTests : UserInterfaceTestBase
    {

        /// <summary>
        /// Sets up the environment for each test.
        /// </summary>
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            Module = new SearchModule(Container);
            Alfred = BuildAlfredInstance();
        }

        /// <summary>
        ///     Gets or sets the module.
        /// </summary>
        /// <value>
        ///     The module.
        /// </value>
        [NotNull]
        private SearchModule Module { get; set; }

        /// <summary>
        ///     The search module should contain a valid search button
        /// </summary>
        [Test]
        [Category("Widgets")]
        public void SearchModuleContainsValidSearchButton()
        {
            //! Arrange

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

            var width = Module.Width;

            //! Assert

            width.ShouldBe(double.NaN);
        }

    }
}