// ---------------------------------------------------------
// RootPagesTests.cs
// 
// Created on:      08/30/2015 at 5:59 PM
// Last Modified:   08/30/2015 at 6:00 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Subsystems;
using MattEland.Testing;

using Moq;

using NUnit.Framework;

using Shouldly;

namespace MattEland.Ani.Alfred.Tests.Pages
{
    /// <summary>
    ///     Tests Alfred's root pages
    /// </summary>
    [UnitTestProvider]
    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    [SuppressMessage("ReSharper", "ExceptionNotDocumented")]
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
            //! Arrange

            // Build out a pair of test pages - one root, the other not
            const MockBehavior Behavior = MockBehavior.Loose;
            var nonRootPage = BuildPageMock(Behavior);
            nonRootPage.SetupGet(p => p.IsRootLevel).Returns(false);

            var rootPage = BuildPageMock(Behavior);
            rootPage.SetupGet(p => p.IsRootLevel).Returns(true);

            _testPages.Add(nonRootPage.Object);
            _testPages.Add(rootPage.Object);

            //! Act

            /* Start Alfred. PrepareTestSubsystem will be called for the 
               TestSubsystem and our pages will be included */

            StartAlfred();

            //! Assert

            // Get the test subsystem. This is going to be a ringer for us as it has a non-root page.
            var testSubsystem = TestSubsystem;
            testSubsystem.ShouldNotBeNull();
            testSubsystem.Pages.Count().ShouldBeGreaterThan(0, "Test subsystem had no pages");
            testSubsystem.Pages.ShouldContain(p => !p.IsRootLevel, "Test subsystem did not contain a non-root level page");

            Alfred.Subsystems.ShouldContain(testSubsystem, "Test subsystem was not found in Alfred subsystems");
            Alfred.RootPages.ShouldContain(rootPage.Object, "The root page was not present in Alfred's root pages collection");

            Alfred.RootPages.ShouldNotContain(nonRootPage.Object, "The non-root page was present in Alfred's root pages collection");
            Alfred.RootPages.ShouldNotContain(p => !p.IsRootLevel, "Alfred should not include non-root pages but did");
        }

        /// <summary>
        ///     Non root pages are not updated at startup (root pages shouldn't be either).
        /// </summary>
        [Test]
        public void NonRootPagesAreNotUpdatedOnStart()
        {
            //! Arrange

            var nonRootPage = BuildPageMock(MockBehavior.Loose);
            nonRootPage.SetupGet(p => p.IsRootLevel).Returns(false);
            _testPages.Add(nonRootPage.Object);

            //! Act

            StartAlfred();

            //! Assert

            nonRootPage.Verify(p => p.Update(), Times.Never);
        }

        /// <summary>
        ///     Non root pages are still updated when Alfred updates (root pages should be as well).
        /// </summary>
        [Test]
        public void NonRootPagesAreUpdatedOnUpdate()
        {
            //! Arrange

            var nonRootPage = BuildPageMock(MockBehavior.Loose);
            nonRootPage.SetupGet(p => p.IsRootLevel).Returns(false);
            _testPages.Add(nonRootPage.Object);

            //! Act

            StartAlfred();
            Alfred.Update();

            //! Assert

            nonRootPage.Verify(p => p.Update(), Times.Once);
        }

        /// <summary>
        ///     Prepare the test subsystem prior to registration and startup.
        /// </summary>
        /// <param name="testSubsystem"> The test subsystem. </param>
        protected override void PrepareTestSubsystem(SimpleSubsystem testSubsystem)
        {
            base.PrepareTestSubsystem(testSubsystem);

            // Add all pages we want t o register to the test
            foreach (var page in _testPages)
            {
                testSubsystem.PagesToRegister.Add(page);
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