// ---------------------------------------------------------
// BingSearchTests.cs
// 
// Created on:      09/10/2015 at 9:46 PM
// Last Modified:   09/10/2015 at 9:46 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Linq;

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
    public sealed class MindExplorerSearchTests : AlfredTestBase
    {
        /// <summary>
        ///     Ensures that the mind explorer subsystem contains a search provider
        /// </summary>
        [Test]
        public void MindExplorerSubsystemContainsSearchProvider()
        {
            //! Arrange
            var subsystem = new MindExplorerSubsystem(Container, true);

            //! Act
            var numProviders = subsystem.SearchProviders.Count();

            //! Assert
            numProviders.ShouldBeGreaterThan(0);
        }
    }
}