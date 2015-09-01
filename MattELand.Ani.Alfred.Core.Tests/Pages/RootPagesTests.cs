// ---------------------------------------------------------
// RootPagesTests.cs
// 
// Created on:      08/30/2015 at 5:59 PM
// Last Modified:   08/30/2015 at 6:00 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Tests.Mocks;
using MattEland.Common;

using NUnit.Framework;

using Shouldly;

namespace MattEland.Ani.Alfred.Tests.Pages
{
    /// <summary>
    ///     Tests Alfred's root pages
    /// </summary>
    [TestFixture]
    public class RootPagesTests : AlfredTestBase
    {
        [NotNull, ItemNotNull]
        private readonly ICollection<IAlfredPage> _testPages = new List<IAlfredPage>();

        /// <summary>
        /// Sets up the environment for each test.
        /// </summary>
        public override void SetUp()
        {
            base.SetUp();

            _testPages.Clear();
        }

        /// <summary>
        ///    Tests that Alfred's root pages collection should not include non-root pages.
        /// </summary>
        /// <remarks>
        ///    Test for issue ALF-105
        /// </remarks>
        [Test]
        public void AlfredRootPagesShouldNotIncludeNonRootPages()
        {
            // Build out a pair of test pages - one root, the other not
            var nonRootPage = BuildTestPage(false);
            _testPages.Add(nonRootPage);

            var rootPage = BuildTestPage(true);
            _testPages.Add(rootPage);

            /* Start Alfred. PrepareTestSubsystem will be called for the 
               TestSubsystem and our pages will be included */

            StartAlfred();

            // Get the test subsystem. This is going to be a ringer for us as it has a non-root page.
            var testSubsystem = TestSubsystem;
            testSubsystem.ShouldNotBeNull();
            testSubsystem.Pages.Count().ShouldBeGreaterThan(0, "Test subsystem had no pages");
            testSubsystem.Pages.ShouldContain(p => !p.IsRootLevel, "Test subsystem did not contain a non-root level page");

            Alfred.Subsystems.ShouldContain(testSubsystem, "Test subsystem was not found in Alfred subsystems");
            Alfred.RootPages.ShouldContain(rootPage, "The root page was not present in Alfred's root pages collection");

            Alfred.RootPages.ShouldNotContain(nonRootPage, "The non-root page was present in Alfred's root pages collection");
            Alfred.RootPages.ShouldNotContain(p => !p.IsRootLevel, "Alfred should not include non-root pages but did");
        }

        /// <summary>
        ///     Non root pages are still updated when Alfred updates.
        /// </summary>
        [Test]
        public void NonRootPagesAreStillUpdated()
        {
            var nonRootPage = BuildTestPage(false);
            _testPages.Add(nonRootPage);

            StartAlfred();

            nonRootPage.LastUpdated.ShouldBe(DateTime.MinValue);

            Alfred.Update();

            nonRootPage.LastUpdated.ShouldBeGreaterThan(DateTime.MinValue);
        }

        /// <summary>
        ///     Prepare the test subsystem prior to registration and startup.
        /// </summary>
        /// <param name="testSubsystem"> The test subsystem. </param>
        protected override void PrepareTestSubsystem(TestSubsystem testSubsystem)
        {
            base.PrepareTestSubsystem(testSubsystem);

            // Add all pages we want t o register to the test
            foreach (var page in _testPages)
            {
                testSubsystem.RegisterPages.Add(page);
            }
        }

        /// <summary>
        ///    Tests that Alfred's root pages collection should not include non-root pages.
        /// </summary>
        /// <remarks>
        ///    Test for issue ALF-105
        /// </remarks>
        [Test]
        public void AlfredRootPagesShouldIncludeAtLeastOnePage()
        {
            StartAlfred();

            Alfred.RootPages.Count().ShouldBeGreaterThan(0);
        }
    }
}