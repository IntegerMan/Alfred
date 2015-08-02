// ---------------------------------------------------------
// CoreModuleTests.cs
// 
// Created on:      08/02/2015 at 5:22 PM
// Last Modified:   08/02/2015 at 5:25 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Modules;

using NUnit.Framework;

namespace MattEland.Ani.Alfred.Core.Tests
{
    /// <summary>
    ///     Tests for the
    ///     <see
    ///         cref="AlfredCoreModule" />
    ///     module.
    /// </summary>
    [TestFixture]
    public class CoreModuleTests
    {
        [NotNull]
        private AlfredProvider _alfred = new AlfredProvider();

        [NotNull]
        private AlfredCoreModule _module;

        /// <summary>
        /// Setups the tests.
        /// </summary>
        [SetUp]
        public void SetupTests()
        {
            _module = new AlfredCoreModule(new SimpleCollectionProvider());

            _alfred.AddModule(_module);
        }

        [Test]
        public void CoreModuleDisplaysMessageWhenNoAlfredIsSet()
        {
            Assert.AreEqual(AlfredCoreModule.NoAlfredProviderMessage, _module.AlfredStatusWidget.Text, "Module did not acknowledge that Alfred Provider wasn't set");
        }

        [Test]
        public void CoreModuleHasCurrentStatus()
        {
            _alfred.Initialize();

            Assert.IsTrue(_module.AlfredStatusWidget.Text.EndsWith(": Online"), "Module did not indicate that Alfred was online after initialization");
        }
    }
}