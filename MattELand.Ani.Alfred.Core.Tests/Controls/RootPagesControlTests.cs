using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows.Controls;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.PresentationShared.Commands;
using MattEland.Ani.Alfred.PresentationShared.Controls;

using NUnit.Framework;

namespace MattEland.Ani.Alfred.Tests.Controls
{
    /// <summary>
    /// Tests related to <see cref="RootPagesControl"/>
    /// </summary>
    [TestFixture]
    [SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    [SuppressMessage("ReSharper", "IsExpressionAlwaysTrue")]
    public class RootPagesControlTests : UserInterfaceTestBase
    {
        [NotNull]
        private RootPagesControl _control;

        [NotNull]
        private ApplicationManager _app;

        /// <summary>
        /// Sets up the test environment for each test.
        /// </summary>
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            _app = new ApplicationManager(Container, null, null, false);
            _control = new RootPagesControl(_app);
            _app.Alfred.Initialize();

            // Get the control ready for interaction
            InitializeControl(_control);
        }

        /// <summary>
        /// Ensures that the RootPagesControl has a TabControl named TabPages
        /// </summary>
        [Test, STAThread]
        public void ControlHasTabPages() { AssertHasTabControl(_control.TabPages); }

        /// <summary>
        /// Checks that the control auto-selects the first tab.
        /// </summary>
        [Test, STAThread]
        public void ControlHasFirstItemSelectedInitially()
        {
            var selectedItem = _control.TabPages?.SelectedItem;
            Assert.IsNotNull(selectedItem);
        }

        /// <summary>
        /// Checks that the control reacts to unrealistic commands by not marking them as handled.
        /// </summary>
        [Test, STAThread]
        public void ControlDoesNotHandleBogusCommands()
        {
            var command = new ShellCommand("Nav", "Pages", "IamTheBatman");
            var result = _control.HandlePageNavigationCommand(command);
            Assert.IsFalse(result);
        }

        /// <summary>
        /// Checks that the control can successfully handle navigation commands.
        /// </summary>
        [Test, STAThread]
        public void ControlCanHandleNavigationCommands()
        {
            Assert.IsNotNull(_app.Alfred.RootPages);

            var lastPage = _app.Alfred.RootPages.LastOrDefault();
            Assert.IsNotNull(lastPage);

            var command = new ShellCommand("Nav", "Pages", lastPage.Id);

            var tab = _control.TabPages;

            Assert.IsNotNull(tab, "TabPages was null");
            Assert.IsTrue(tab.HasItems, "TabPages did not have items");
            var result = _control.HandlePageNavigationCommand(command);

            Assert.IsTrue(result, "Navigation Failed");

            var selectedItem = tab.SelectedItem;
            Assert.IsNotNull(selectedItem, "Selected tab was null after navigate");
            var selectedDomainItem = selectedItem as IAlfredPage;
            Assert.IsNotNull(selectedDomainItem);
            Assert.AreEqual(lastPage.Id, selectedDomainItem.Id, "Selected tab's ID did not match last tab's ID");
        }
    }
}
