// ---------------------------------------------------------
// AlfredSearchTests.cs
// 
// Created on:      09/09/2015 at 12:10 AM
// Last Modified:   09/09/2015 at 12:10 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using MattEland.Ani.Alfred.Core;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Testing;

using NUnit.Framework;

using Shouldly;

namespace MattEland.Ani.Alfred.Tests.Search
{
    /// <summary>
    ///     Search tests related to how the search feature and the <see cref="AlfredApplication"/> interact.
    /// </summary>
    [UnitTestProvider]
    public sealed class AlfredSearchTests : AlfredTestBase
    {

        /// <summary>
        /// Sets up the environment for each test.
        /// </summary>
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
        }

        /// <summary>
        ///     Tests that new Alfred instances start with a non-null SearchController
        /// </summary>
        [Test]
        public void AlfredHasNonNullSearchController()
        {

            //! Arrange
            var alfred = CreateAlfredInstance();

            //! Act - No action - testing initial state

            //! Assert
            alfred.SearchController.ShouldNotBeNull();
            alfred.SearchController.ShouldImplementInterface<ISearchController>();

        }

        /// <summary>
        ///     When Alfred initializes, the <see cref="ISearchController"/> should get Initialize
        ///     and OnInitializationCompleted method calls.
        /// </summary>
        [Test]
        public void SearchControllerInitializesWhenAlfredInitializes()
        {
            //! Arrange

            //! Act

            //! Assert

            TestIsNotImplemented();
        }

    }
}