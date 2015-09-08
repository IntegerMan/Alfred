// ---------------------------------------------------------
// AlfredPowerModuleTests.cs
// 
// Created on:      08/02/2015 at 5:22 PM
// Last Modified:   08/03/2015 at 1:56 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Modules;
using MattEland.Ani.Alfred.Core.Pages;
using MattEland.Ani.Alfred.Core.Subsystems;
using MattEland.Testing;

using NUnit.Framework;

namespace MattEland.Ani.Alfred.Tests.Modules
{
    /// <summary>
    ///     Tests for the
    ///     <see
    ///         cref="AlfredPowerModule" />
    ///     module.
    /// </summary>
    [UnitTestProvider]
    [SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    public sealed class AlfredPowerModuleTests : AlfredTestBase
    {
        /// <summary>
        ///     Setups the tests.
        /// </summary>
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            _alfred = new AlfredApplication(Container);
            _module = new AlfredPowerModule(Container);

            RegisterTestModule(_alfred, _module);
        }

        /// <summary>
        /// Registers the module to the Alfred instance using a test subsystem and simple page.
        /// </summary>
        /// <param name="alfred">The Alfred instance.</param>
        /// <param name="module">The module.</param>
        private void RegisterTestModule([NotNull] IAlfred alfred, [NotNull] IAlfredModule module)
        {
            var subsystem = BuildTestSubsystem();

            var page = new AlfredModuleListPage(Container, "Test Page", "Test");
            subsystem.PagesToRegister.Add(page);

            alfred.Register(subsystem);

            page.Register(module);

        }

        [NotNull]
        private AlfredApplication _alfred;

        [NotNull]
        private AlfredPowerModule _module;

        [Test]
        public void AfterShutdownSystemWidgetIsStillPresentInModule()
        {
            _alfred.Initialize();
            _alfred.Update();
            _alfred.Shutdown();

            Assert.IsNotNull(_module.Widgets, "Module removed all widgets after shutdown");
            Assert.Contains(_module.AlfredStatusWidget,
                            _module.Widgets as ICollection,
                            "The Status Widget was not present after shutdown");
        }

        [Test]
        public void AtInitialStateInitializeIsVisible()
        {
            // Doing this again here to illustrate creation / configuration order more clearly
            _alfred = new AlfredApplication(Container);
            _module = new AlfredPowerModule(Container);

            RegisterTestModule(_alfred, _module);

            Assert.IsNotNull(_module.AlfredInstance, "Alfred was not set");
            Assert.IsTrue(_module.InitializeButton.IsVisible, "Initialize button was not visible while offline.");
            Assert.IsNotNull(_module.Widgets, "_module.Widgets was null");
            Assert.IsTrue(_module.Widgets.Contains(_module.InitializeButton),
                          "The Initialize button was not part of the UI while offline");
        }

        [Test]
        public void CoreModuleHasCurrentStatusAfterInitializeAndUpdate()
        {
            _alfred.Initialize();

            _alfred.Update();

            var text = _module.AlfredStatusWidget.Text;
            Assert.IsNotNull(text, "Widget text was null");
            Assert.IsTrue(text.EndsWith("Online"),
                          $"Module did not indicate that Alfred was online after update. Instead indicated: {text}");
        }

        [Test]
        public void CoreModuleHasNoProviderText()
        {
            _alfred = new AlfredApplication(Container);
            _module = new AlfredPowerModule(Container);

            var text = _module.AlfredStatusWidget.Text;
            Assert.IsNotNull(text, "Widget text was null");
            Assert.IsTrue(text.EndsWith("has not been set"),
                          $"Component did not indicate that Alfred was not set. Instead indicated: {text}");
        }

        [Test]
        public void WhenOfflineTheInitializeButtonAppears()
        {
            _alfred.Initialize();
            _alfred.Update();
            _alfred.Shutdown();

            Assert.IsTrue(_module.InitializeButton.IsVisible, "Initialize button was not visible while offline.");
            Assert.IsNotNull(_module.Widgets, "_module.Widgets was null");
            Assert.IsTrue(
                          _module.Widgets.Contains(_module.InitializeButton),
                          "The Initialize button was not part of the UI while offline");
        }

        [Test]
        public void WhenOfflineTheShutdownButtonDoesNotAppear()
        {
            _alfred.Initialize();
            _alfred.Update();
            _alfred.Shutdown();

            Assert.IsNotNull(_module.Widgets, "_module.Widgets was null");
            Assert.IsFalse(
                           _module.Widgets.Contains(_module.ShutdownButton),
                           "The Shut Down button was part of the UI while offline");
        }

        [Test]
        public void WhenOnlineTheShutdownButtonAppears()
        {
            _alfred.Initialize();

            Assert.IsTrue(_module.ShutdownButton.IsVisible, "Shut down button was not visible while online.");
            Assert.IsNotNull(_module.Widgets, "_module.Widgets was null");
            Assert.IsTrue(
                          _module.Widgets.Contains(_module.ShutdownButton),
                          "The Shut Down button was not part of the UI while online");
        }

        [Test]
        public void WhenOnlineTheInitializeButtonDoesNotAppear()
        {
            _alfred.Initialize();

            Assert.IsNotNull(_module.Widgets, "_module.Widgets was null");
            Assert.IsFalse(
                           _module.Widgets.Contains(_module.InitializeButton),
                           "The Initialize button was part of the UI while online");
        }

        [Test]
        public void ClickingTheInitializeButtonInitializesAlfredWhileOffline()
        {
            _module.InitializeButton.Click();

            Assert.AreEqual(AlfredStatus.Online, _alfred.Status, "Alfred was not online after button click");
        }

        [Test]
        public void ClickingTheShutdownButtonShutsDownAlfredWhileOnline()
        {
            _alfred.Initialize();
            _alfred.Update();

            _module.ShutdownButton.Click();

            Assert.AreEqual(AlfredStatus.Offline, _alfred.Status, "Alfred was not offline after button click");
        }

        [Test]
        public void ClickingTheInitializeButtonReinitializesAlfredWhileOfflineAfterShutdown()
        {
            _alfred.Initialize();
            _alfred.Update();
            _alfred.Shutdown();

            _module.InitializeButton.Click();

            Assert.AreEqual(AlfredStatus.Online, _alfred.Status, "Alfred was not online after button click");
        }

        [Test]
        public void WhenOfflineShutdownCannotExecute()
        {
            Assert.IsNotNull(_module.ShutdownButton.ClickCommand, "_module.ShutdownButton.ClickCommand was null");
            Assert.IsFalse(_module.ShutdownButton.ClickCommand.CanExecute(null), "Could click shutdown while offline");
        }

        [Test]
        public void WhenOnlineInitializeCannotExecute()
        {
            _alfred.Initialize();
            _alfred.Update();

            Assert.IsNotNull(_module.InitializeButton.ClickCommand, "_module.InitializeButton.ClickCommand was null");
            Assert.IsFalse(_module.InitializeButton.ClickCommand.CanExecute(null), "Could click initialize while online");
        }
    }
}