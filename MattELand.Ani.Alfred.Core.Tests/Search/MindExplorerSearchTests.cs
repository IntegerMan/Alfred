// ---------------------------------------------------------
// MindExplorerSearchTests.cs
// 
// Created on:      09/10/2015 at 9:46 PM
// Last Modified:   09/11/2015 at 10:43 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Linq;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Subsystems;
using MattEland.Testing;

using NUnit.Framework;

using Shouldly;

namespace MattEland.Ani.Alfred.Tests.Search
{
    /// <summary>
    ///     Mind Explorer search unit tests.
    /// </summary>
    [UnitTestProvider]
    [SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
    public sealed class MindExplorerSearchTests : AlfredTestBase
    {
        /// <summary>
        ///     The search text.
        /// </summary>
        private const string SearchText = "Oh where is my hairbrush?";

        /// <summary>
        ///     Gets or sets the Mind Explorer subsystem.
        /// </summary>
        /// <value>
        ///     The subsystem.
        /// </value>
        [NotNull]
        private MindExplorerSubsystem Subsystem { get; set; }

        /// <summary>
        ///     Gets or sets the Mind Explorer search provider.
        /// </summary>
        /// <value>
        ///     The search provider.
        /// </value>
        [NotNull]
        private ExplorerSearchProvider SearchProvider { get; set; }

        /// <summary>
        /// Sets up the environment for each test.
        /// </summary>
        public override void SetUp()
        {
            base.SetUp();

            Subsystem = new MindExplorerSubsystem(Container, true);
            SearchProvider = Subsystem.SearchProviders.FirstOrDefault() as ExplorerSearchProvider;
        }

        /// <summary>
        ///     Ensures that the mind explorer subsystem contains a search provider
        /// </summary>
        [Test]
        public void MindExplorerSubsystemContainsSearchProvider()
        {
            //! Arrange
            var subsystem = Subsystem;
            var provider = SearchProvider;

            //! Act
            var numProviders = subsystem.SearchProviders.Count();

            //! Assert
            numProviders.ShouldBeGreaterThan(0);
            provider.ShouldNotBeNull();
            subsystem.SearchProviders.ShouldContain(provider);
        }

        /// <summary>
        ///     Searches on the mind explorer search provider should return an explorer search operation.
        /// </summary>
        [Test]
        public void MindExplorerSearchResultsInExpectedOperationType()
        {
            //! Arrange
            var searchProvider = SearchProvider;

            //! Act
            var operation = searchProvider.PerformSearch(SearchText);

            //! Assert
            operation.ShouldNotBeNull();
            operation.ShouldBeOfType<ExplorerSearchOperation>();
        }
    }
}