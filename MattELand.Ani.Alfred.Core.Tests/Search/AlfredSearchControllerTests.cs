// ---------------------------------------------------------
// AlfredSearchControllerTests.cs
// 
// Created on:      09/09/2015 at 11:45 AM
// Last Modified:   09/09/2015 at 11:45 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;

using MattEland.Ani.Alfred.Core;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Common.Providers;
using MattEland.Testing;

using Moq;

using NUnit.Framework;

using Shouldly;

namespace MattEland.Ani.Alfred.Tests.Search
{
    /// <summary>
    ///     A test class containing unit tests related to <see cref="AlfredSearchController"/>
    /// </summary>
    [UnitTestProvider]
    [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
    public sealed class AlfredSearchControllerTests : AlfredTestBase
    {
        /// <summary>
        ///     Tests that search providers can be registered and show up in the controller's Providers
        ///     collection.
        /// </summary>
        [Test]
        public void SearchProvidersCanBeRegistered()
        {
            //! Arrange
            var controller = new AlfredSearchController(Container);
            var mockProvider = BuildMockSearchProvider();

            //! Act
            controller.Register(mockProvider.Object);

            //! Assert
            controller.SearchProviders.ShouldNotBeNull();
            controller.SearchProviders.ShouldContain(mockProvider.Object);

        }

        /// <summary>
        ///     Tests that null search providers cannot be registered.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void NullSearchProvidersCannotBeRegistered()
        {
            //! Arrange
            var controller = new AlfredSearchController(Container);

            //! Act / Assert - Expected ArgumentNullException
            controller.Register(null);
        }

        /// <summary>
        ///     Tests that search providers can be registered and show up in the controller's Providers
        ///     collection.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidOperationException))]
        public void SearchProvidersCannotBeRegisteredWhenNotOffline()
        {
            //! Arrange
            var controller = new AlfredSearchController(Container);
            controller.RegisterAsProvidedInstance(typeof(ISearchController), Container);

            var provider = BuildMockSearchProvider();

            var alfred = BuildAlfredInstance();

            //! Act / Assert - Expected InvalidOperationException
            alfred.Initialize();
            controller.Register(provider.Object);
        }

        /// <summary>
        ///     Builds a mock search provider.
        /// </summary>
        /// <returns>
        ///     A mock search provider.
        /// </returns>
        private static Mock<ISearchProvider> BuildMockSearchProvider()
        {
            var mock = new Mock<ISearchProvider>(MockBehavior.Strict);

            return mock;
        }

    }
}