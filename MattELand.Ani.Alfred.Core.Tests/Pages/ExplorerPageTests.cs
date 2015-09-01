// ---------------------------------------------------------
// ExplorerPageTests.cs
// 
// Created on:      08/23/2015 at 3:40 PM
// Last Modified:   08/30/2015 at 4:02 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Pages;
using MattEland.Common;
using MattEland.Testing;

using NUnit.Framework;

using Shouldly;

namespace MattEland.Ani.Alfred.Tests.Pages
{
    /// <summary>
    /// Contains tests related to <see cref="ExplorerPage" />
    /// </summary>
    [UnitTest]
    [SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    public class ExplorerPageTests : AlfredTestBase
    {
        /// <summary>
        ///     Sets up the environment for each test.
        /// </summary>
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            _page = new ExplorerPage(Container, "Test Page", "TestExp");
        }

        [NotNull]
        private ExplorerPage _page;

        /// <summary>
        ///     Gets a simple collection of <see cref="IPropertyProvider" /> objects.
        /// </summary>
        /// <returns>
        ///     The items.
        /// </returns>
        [NotNull]
        [ItemNotNull]
        private IEnumerable<IPropertyProvider> GetSimpleCollection()
        {
            return new List<IPropertyProvider> { new AlfredEventLogPage(Container, "Event Test") };
        }

        /// <summary>
        ///     Tests that the page's root nodes can be set.
        /// </summary>
        [Test]
        public void PageCanHaveNodes()
        {
            var children = GetSimpleCollection().ToList();

            foreach (var child in children) { _page.AddRootNode(child); }

            _page.RootNodes.ShouldNotBeNull();

            foreach (var child in children) { _page.RootNodes.ShouldContain(child); }
            children.Count().ShouldBe(_page.RootNodes.Count(), "Page's root nodes were not the new collection");
        }

        /// <summary>
        ///     Tests that the page has a collection of items after initialization.
        /// </summary>
        [Test]
        public void PageHasRootNodesByDefault()
        {
            Assert.IsNotNull(_page.RootNodes);
            Assert.IsNotNull(_page.Children);
        }
    }
}