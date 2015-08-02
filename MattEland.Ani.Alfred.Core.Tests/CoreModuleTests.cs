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
        private AlfredProvider _alfred;

        [NotNull]
        private AlfredCoreModule _module;

        /// <summary>
        /// Setups the tests.
        /// </summary>
        [SetUp]
        public void SetupTests()
        {
            _alfred = new AlfredProvider();

            _module = new AlfredCoreModule(new SimpleCollectionProvider());

            _alfred.AddModule(_module);
        }

        [Test]
        public void CoreModuleDisplaysMessageWhenNoAlfredIsSet()
        {
            Assert.AreEqual(AlfredCoreModule.NoAlfredProviderMessage, _module.AlfredStatusWidget.Text, "Module did not acknowledge that Alfred Provider wasn't set");
        }

        [Test]
        public void CoreModuleDefaultsToNoAlfred()
        {
            Assert.IsNull(_module.AlfredProvider, "Alfred Provider was unexpectedly set");
        }

        [Test]
        public void CoreModuleHasCurrentStatusAfterSettingProvider()
        {
            _module.AlfredProvider = _alfred;

            // Do not initialize or update yet - we should be offline

            var text = _module.AlfredStatusWidget.Text;
            Assert.IsNotNull(text, "Widget text was null");
            Assert.IsTrue(text.EndsWith("Offline"), $"Module did not indicate that Alfred was offline. Instead indicated: {text}");
        }

        [Test]
        public void CoreModuleHasCurrentStatusAfterInitializeAndUpdate()
        {
            _module.AlfredProvider = _alfred;

            _alfred.Initialize();

            _alfred.Update();

            var text = _module.AlfredStatusWidget.Text;
            Assert.IsNotNull(text, "Widget text was null");
            Assert.IsTrue(text.EndsWith("Online"), $"Module did not indicate that Alfred was online after update. Instead indicated: {text}");
        }
    }
}