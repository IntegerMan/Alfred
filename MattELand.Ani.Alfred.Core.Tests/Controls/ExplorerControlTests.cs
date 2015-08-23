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
    /// Tests related to <see cref="ExplorerControl"/>
    /// </summary>
    [TestFixture]
    [SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    [SuppressMessage("ReSharper", "IsExpressionAlwaysTrue")]
    public class ExplorerControlTests
    {
        [NotNull]
        private ExplorerControl _control;

        [NotNull]
        private ApplicationManager _app;

        /// <summary>
        /// Sets up the test environment for each test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _app = new ApplicationManager(enableSpeech: false);
            _control = new ExplorerControl(_app.RootNodes);
            _app.Alfred.Initialize();

            // Simulate a page load
            _control.SimulateLoadedEvent();
        }

        /// <summary>
        /// Ensures that the control has a TreeView named TreeNodes
        /// </summary>
        [Test, STAThread]
        public void ControlHasTreeView()
        {
            Assert.IsNotNull(_control.TreeHierarchy);
            Assert.That(_control.TreeHierarchy is TreeView);
        }


        /// <summary>
        /// Ensures that the control has a DataGrid named GridProperties
        /// </summary>
        [Test, STAThread]
        public void ControlHasDataGrid()
        {
            Assert.IsNotNull(_control.GridProperties);
            Assert.That(_control.GridProperties is DataGrid);
        }

        /// <summary>
        /// Controls the has items by default.
        /// </summary>
        [Test, STAThread]
        public void ControlHasItemsByDefault()
        {
            Assert.IsNotNull(_control.RootNodes);
            Assert.AreEqual(_control.RootNodes, _control.TreeHierarchy?.ItemsSource);
        }

    }
}
