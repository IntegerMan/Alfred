// ---------------------------------------------------------
// CoreModuleTests.cs
// 
// Created on:      08/02/2015 at 5:22 PM
// Last Modified:   08/02/2015 at 5:25 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System.Collections;
using System.Diagnostics;

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

        [Test]
        public void AfterShutdownSystemWidgetIsStillPresentInModule()
        {
            _module.AlfredProvider = _alfred;

            _alfred.Initialize();
            _alfred.Update();
            _alfred.Shutdown();

            Assert.IsNotNull(_module.Widgets, "Module removed all widgets after shutdown");
            Assert.Contains(_module.AlfredStatusWidget, _module.Widgets as ICollection, "The Status Widget was not present after shutdown");
        }

        [Test]
        public void WhenOfflineTheInitializeButtonAppears()
        {
            _module.AlfredProvider = _alfred;

            _alfred.Initialize();
            _alfred.Update();
            _alfred.Shutdown();

            Assert.IsTrue(_module.InitializeButton.IsVisible, "Initialize button was not visible while offline.");
            Assert.IsNotNull(_module.Widgets, "_module.Widgets was null");
            Assert.IsTrue(_module.Widgets.Contains(_module.InitializeButton), "The Initialize button was not part of the UI while offline");
        }

        [Test]
        public void WhenOnlineTheShutdownButtonAppears()
        {
            _module.AlfredProvider = _alfred;

            _alfred.Initialize();

            Assert.IsTrue(_module.ShutdownButton.IsVisible, "Shut down button was not visible while online.");
            Assert.IsNotNull(_module.Widgets, "_module.Widgets was null");
            Assert.IsTrue(_module.Widgets.Contains(_module.ShutdownButton), "The Shut Down button was not part of the UI while online");
        }

        [Test]
        public void WheOnlineTheInitializeButtonDoesNotAppear()
        {
            _module.AlfredProvider = _alfred;

            _alfred.Initialize();

            Assert.IsNotNull(_module.Widgets, "_module.Widgets was null");
            Assert.IsFalse(_module.Widgets.Contains(_module.InitializeButton), "The Initialize button was part of the UI while online");
        }

        [Test]
        public void WhenOfflineTheShutdownButtonDoesNotAppear()
        {
            _module.AlfredProvider = _alfred;

            _alfred.Initialize();
            _alfred.Update();
            _alfred.Shutdown();

            Assert.IsNotNull(_module.Widgets, "_module.Widgets was null");
            Assert.IsFalse(_module.Widgets.Contains(_module.ShutdownButton), "The Shut Down button was part of the UI while offline");
        }

        [Test]
        public void AtInitialStateInitializeIsVisibleSetAlfredProviderFirst()
        {
            // Doing this again here to illustrate creation / configuration order more clearly
            _alfred = new AlfredProvider();
            _module = new AlfredCoreModule(new SimpleCollectionProvider());

            // Set the provider first, then add it to Alfred's modules
            _module.AlfredProvider = _alfred;
            _alfred.AddModule(_module);

            Assert.IsTrue(_module.InitializeButton.IsVisible, "Initialize button was not visible while offline.");
            Assert.IsNotNull(_module.Widgets, "_module.Widgets was null");
            Assert.IsTrue(_module.Widgets.Contains(_module.InitializeButton), "The Initialize button was not part of the UI while offline");
        }

        [Test]
        public void AtInitialStateInitializeIsVisibleAddModuleFirst()
        {
            // Doing this again here to illustrate creation / configuration order more clearly
            _alfred = new AlfredProvider();
            _module = new AlfredCoreModule(new SimpleCollectionProvider());

            // Add the module first, then set the provider
            _alfred.AddModule(_module);
            _module.AlfredProvider = _alfred;

            Assert.IsTrue(_module.InitializeButton.IsVisible, "Initialize button was not visible while offline.");
            Assert.IsNotNull(_module.Widgets, "_module.Widgets was null");
            Assert.IsTrue(_module.Widgets.Contains(_module.InitializeButton), "The Initialize button was not part of the UI while offline");
        }
    }
}