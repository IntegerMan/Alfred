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
    ///     Tests for the Search Results Module. This class cannot be inherited.
    /// </summary>
    [UnitTestProvider]
    [Category("Search")]
    [Category("Modules")]
    [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
    [SuppressMessage("ReSharper", "ExceptionNotDocumentedOptional")]
    [SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    [SuppressMessage("ReSharper", "ExceptionNotDocumented")]
    public sealed class SearchResultsModuleTests : UserInterfaceTestBase
    {
        /// <summary>
        ///     Gets or sets the module.
        /// </summary>
        /// <value>
        ///     The module.
        /// </value>
        [NotNull]
        private SearchResultsModule Module { get; set; }

        /// <summary>
        ///     Sets up the environment for each test.
        /// </summary>
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            Module = new SearchResultsModule(Container);
        }

        /// <summary>
        ///     The "no searches yet" label should be shown on startup.
        /// </summary>
        [Test]
        public void NoSearchesLabelShouldBeShownOnStartup()
        {
            //! Arrange

            Alfred = BuildMockAlfred().Object;

            //! Act

            Module.Initialize(Alfred);

            //! Assert

            var label = FindWidgetOfTypeByName<TextWidget>(Module, @"lblResults");
            label.Text.ShouldBe(SearchResultsModule.NoSearchesMadeMessage);
        }

        /// <summary>
        ///     The Search Results module should use vertical layout
        /// </summary>
        [Test]
        public void SearchResultsShouldHaveVerticalLayout()
        {

            //! Assert

            Module.LayoutType.ShouldBe(LayoutType.VerticalStackPanel);

        }
    }
}