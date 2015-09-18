// ---------------------------------------------------------
// ExplorerSearchProviderTests.cs
// 
// Created on:      09/10/2015 at 9:46 PM
// Last Modified:   09/17/2015 at 11:23 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Subsystems;
using MattEland.Common;
using MattEland.Testing;

using Moq;

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
    [SuppressMessage("ReSharper", "ExceptionNotDocumentedOptional")]
    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    public sealed class ExplorerSearchProviderTests : MockEnabledAlfredTestBase
    {
        /// <summary>
        ///     The search text.
        /// </summary>
        private const string IrrelevantSearchString = "Some irrelevant search string";

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
            SearchProvider.ShouldNotBeNull();
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

            var operation = searchProvider.PerformSearch(IrrelevantSearchString);

            //! Assert

            operation.ShouldNotBeNull();
            operation.ShouldBeOfType<ExplorerSearchOperation>();
        }

        /// <summary>
        ///     Checks that searches will complete and return the expected amount of data
        /// </summary>
        /// <param name="searchText"> The search text. </param>
        [TestCase("Alfred")]
        public void SearchCompletesInstantlyAndReturnsData(string searchText)
        {
            //! Arrange

            // Set up Alfred to have the mind explorer subsystem (and its search provider)
            Alfred = BuildAlfredInstance();
            Alfred.Register(Subsystem);
            Alfred.Initialize();

            var searchController = Alfred.SearchController;

            //! Act

            // Execute a Search for Alfred. The root node at least should match this
            searchController.PerformSearch(searchText);

            // Let the search compute
            Alfred.Update();

            //! Assert

            searchController.ShouldSatisfyAllConditions(
                                                        () =>
                                                        searchController.Results.Count()
                                                                        .ShouldBeGreaterThan(0),
                                                        () =>
                                                        searchController.IsSearching.ShouldBe(false));
        }

        /// <summary>
        ///     After search and one update, search operations are complete.
        /// </summary>
        [Test]
        public void AfterSearchAndOneUpdateSearchOperationsAreComplete()
        {
            //! Arrange

            var provider = SearchProvider;

            //! Act

            var operation = provider.PerformSearch(IrrelevantSearchString);
            operation.Update();

            //! Assert

            operation.ShouldSatisfyAllConditions(
                                                 () => operation.ShouldBe<ExplorerSearchOperation>(),
                                                 () => operation.IsSearchComplete.ShouldBeTrue());
        }

        /// <summary>
        ///     Testing matching nodes in an explorer search by their display name alone.
        /// </summary>
        /// <param name="displayName"> The node's display name. </param>
        /// <param name="searchText"> The search text. </param>
        /// <param name="expectedMatch"> <see langword="true"/> if a match is expected. </param>
        [TestCase(@"Robin", @"Robin", true)]
        [TestCase(@"Dead Robin", @"Robin", true)]
        [TestCase(@"Robin", @"Dead Robin", false)]
        [TestCase(@"Apples", @"Oranges", false)]
        [TestCase(@"Apples", @"App", true)]
        [TestCase(@"Apples", @"ples", true)]
        [TestCase(@"Apples", @"les", true)]
        public void ExplorerNodeMatchSearchByDisplayName(string displayName,
                                                         string searchText,
                                                         bool expectedMatch)
        {
            //! Arrange

            // Build out a node to test, yielding the correct display name
            var mock = BuildMockPropertyProvider();
            mock.SetupGet(m => m.DisplayName).Returns(displayName);

            // The IsNodeSearchMatch method expects values in uppercase invariant already for performance.
            searchText = searchText.ToUpperInvariant();

            //! Act

            var isMatch = ExplorerSearchOperation.IsNodeSearchMatch(mock.Object, searchText);

            //! Assert

            var message = $"Searched '{displayName}' for '{searchText}' expecting {expectedMatch}";
            isMatch.ShouldBe(expectedMatch, message);
        }

        /// <summary>
        ///     Tests that explorer nodes are matched by property values even if their display names
        ///     don't cause the match.
        /// </summary>
        /// <param name="propertyValue"> The property value. </param>
        /// <param name="searchText"> The search text. </param>
        /// <param name="expectedMatch"> <see langword="true"/> if a match is expected. </param>
        [TestCase(@"Robin", @"Robin", true)]
        [TestCase(@"Dead Robin", @"Robin", true)]
        [TestCase(@"Robin", @"Dead Robin", false)]
        [TestCase(@"Apples", @"Oranges", false)]
        [TestCase(@"Apples", @"App", true)]
        [TestCase(@"Apples", @"ples", true)]
        [TestCase(@"Apples", @"les", true)]
        public void ExplorerNodeMatchSearchByPropertyValues(string propertyValue, string searchText, bool expectedMatch)
        {
            //! Arrange

            // Build out a property containing the name and value we desire
            var mockProperty = BuildMockPropertyItem("My Property", propertyValue);

            var mock = BuildMockPropertyProvider(mockProperty.Object.ToCollection());

            // The IsNodeSearchMatch method expects values in uppercase invariant already for performance.
            searchText = searchText.ToUpperInvariant();

            //! Act

            var isMatch = ExplorerSearchOperation.IsNodeSearchMatch(mock.Object, searchText);

            //! Assert

            var message = $"Searched node with property value {propertyValue} for '{searchText}' expecting {expectedMatch}";
            isMatch.ShouldBe(expectedMatch, message);
        }

        /// <summary>
        ///     Tests that explorer nodes are matched by property values even if their display names
        ///     don't cause the match.
        /// </summary>
        /// <param name="propertyName"> The property name. </param>
        /// <param name="searchText"> The search text. </param>
        /// <param name="expectedMatch"> <see langword="true"/> if a match is expected. </param>
        [TestCase(@"Robin", @"Robin", true)]
        [TestCase(@"Dead Robin", @"Robin", true)]
        [TestCase(@"Robin", @"Dead Robin", false)]
        [TestCase(@"Apples", @"Oranges", false)]
        [TestCase(@"Apples", @"App", true)]
        [TestCase(@"Apples", @"ples", true)]
        [TestCase(@"Apples", @"les", true)]
        public void ExplorerNodeMatchSearchByPropertyNames(string propertyName, string searchText, bool expectedMatch)
        {
            //! Arrange

            // Build out a property containing the name and value we desire
            var mockProperty = BuildMockPropertyItem(propertyName, "42");

            var mock = BuildMockPropertyProvider(mockProperty.Object.ToCollection());

            // The IsNodeSearchMatch method expects values in uppercase invariant already for performance.
            searchText = searchText.ToUpperInvariant();

            //! Act

            var isMatch = ExplorerSearchOperation.IsNodeSearchMatch(mock.Object, searchText);

            //! Assert

            var message = $"Searched node with property name {propertyName} for '{searchText}' expecting {expectedMatch}";
            isMatch.ShouldBe(expectedMatch, message);
        }

    }
}