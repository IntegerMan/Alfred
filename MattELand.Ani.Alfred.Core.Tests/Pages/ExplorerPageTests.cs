// ---------------------------------------------------------
// ExplorerPageTests.cs
// 
// Created on:      08/23/2015 at 3:40 PM
// Last Modified:   08/23/2015 at 3:40 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Pages;

using NUnit.Framework;

namespace MattEland.Ani.Alfred.Tests.Pages
{
    /// <summary>
    /// Contains tests related to <see cref="ExplorerPage"/>
    /// </summary>
    [TestFixture]
    [SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    public class ExplorerPageTests : AlfredTestBase
    {
        [NotNull]
        private ExplorerPage _page;

        /// <summary>
        /// Sets up the environment for each test.
        /// </summary>
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            _page = new ExplorerPage(new SimplePlatformProvider(), "Test Page", "TestExp");
        }

        /// <summary>
        /// Tests that the page has a collection of items after initialization
        /// </summary>
        [Test]
        public void PageHasRootNodesByDefault()
        {
            Assert.IsNotNull(_page.RootNodes);
            Assert.IsNotNull(_page.Children);
        }

        /// <summary>
        /// Tests that the page's root nodes can be set
        /// </summary>
        [Test]
        public void PageCanHaveNodes()
        {
            var children = GetSimpleCollection();

            _page.RootNodes = children;

            Assert.IsNotNull(_page.RootNodes);
            Assert.AreEqual(children, _page.RootNodes, "Page's root nodes were not the new collection");
        }

        /// <summary>
        /// Tests that the page's children contains all components in RootNodes
        /// </summary>
        [Test]
        public void ChildrenContainRootNodes()
        {
            var children = GetSimpleCollection();

            _page.RootNodes = children;

            Assert.AreEqual(_page.RootNodes, _page.Children);
        }

        /// <summary>
        /// Gets a simple collection of IPropertyProvider objects.
        /// </summary>
        /// <returns>The items</returns>
        [NotNull, ItemNotNull]
        private static IEnumerable<IPropertyProvider> GetSimpleCollection()
        {

            return new List<IPropertyProvider>
                   {
                       new AlfredEventLogPage(new SimplePlatformProvider(), new SimpleConsole(), "Event Test")
                   };
        }
    }
}