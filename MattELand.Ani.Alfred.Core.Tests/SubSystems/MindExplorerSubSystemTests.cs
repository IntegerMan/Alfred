// ---------------------------------------------------------
// MindExplorerSubSystemTests.cs
// 
// Created on:      08/22/2015 at 10:42 PM
// Last Modified:   08/22/2015 at 10:42 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.SubSystems;
using MattEland.Common;

using NUnit.Framework;

namespace MattEland.Ani.Alfred.Tests.SubSystems
{
    /// <summary>
    /// Contains tests related to the MindExplorerSubSystem.
    /// </summary>
    [TestFixture]
    public class MindExplorerSubSystemTests
    {
        /// <summary>
        /// Sets up the environment for each test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {

        }

        /// <summary>
        /// Ensures that the mind explorer subsystem can be instantiated
        /// </summary>
        [Test]
        public void MindExplorerCanBeInitialized()
        {
            var subsystem = new MindExplorerSubSystem(new SimplePlatformProvider());
            Assert.That(subsystem.Name.HasText());
        }

    }
}