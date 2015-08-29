// ---------------------------------------------------------
// ExplorerWidgetTests.cs
// 
// Created on:      08/22/2015 at 11:27 PM
// Last Modified:   08/22/2015 at 11:27 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Pages;

using NUnit.Framework;

namespace MattEland.Ani.Alfred.Tests.Widgets
{
    /// <summary>
    /// Contains tests related to the <see cref="ExplorerPage"/>
    /// </summary>
    [TestFixture]
    [SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
    [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
    public class ExplorerWidgetTests : AlfredTestBase
    {
        [NotNull]
        private SimplePlatformProvider _provider;
        private const string TestPageId = "TestExp";
        private const string TestPageName = "Explorer Page";

        /// <summary>
        /// Sets up the environment for each test
        /// </summary>
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            _provider = new SimplePlatformProvider();
        }

        /// <summary>
        /// Simple constructor test for <see cref="ExplorerPage"/>
        /// </summary>
        /// <remarks>
        /// ALF-15
        /// </remarks>
        [Test]
        public void ExplorerPageCanBeCreated()
        {
            var page = new ExplorerPage(_provider, TestPageName, TestPageId);
            Assert.AreEqual(TestPageId, page.Id);
            Assert.AreEqual(TestPageName, page.Name);
        }

        /// <summary>
        /// Simple constructor test for <see cref="ExplorerPage"/> checking parameter validation on provider
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void ExplorerPageThrowsOnNoProvider()
        {
            new ExplorerPage(null, TestPageName, TestPageId);
        }
        /// <summary>
        /// Simple constructor test for <see cref="ExplorerPage"/> checking parameter validation on name
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void ExplorerPageThrowsOnNoName()
        {
            new ExplorerPage(_provider, null, TestPageId);
        }
        /// <summary>
        /// Simple constructor test for <see cref="ExplorerPage"/> checking parameter validation on id
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void ExplorerPageThrowsOnNoId()
        {
            new ExplorerPage(_provider, TestPageName, null);
        }

        /// <summary>
        /// Ensures that Explorer pages are visible.
        /// </summary>
        [Test]
        public void ExplorerPageIsVisible()
        {
            var page = new ExplorerPage(_provider, TestPageName, TestPageId);

            Assert.That(page.IsVisible);
        }
    }
}