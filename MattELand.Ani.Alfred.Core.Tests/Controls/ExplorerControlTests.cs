using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows.Controls;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.PresentationAvalon.Commands;
using MattEland.Ani.Alfred.PresentationAvalon.Controls;
using MattEland.Testing;

using NUnit.Framework;

namespace MattEland.Ani.Alfred.Tests.Controls
{
    /// <summary>
    /// Tests related to <see cref="ExplorerControl"/>
    /// </summary>
    [SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    [SuppressMessage("ReSharper", "IsExpressionAlwaysTrue")]
    [UnitTestProvider]
    public sealed class ExplorerControlTests : UserInterfaceTestBase
    {
        [NotNull]
        private ExplorerControl _control;

        [NotNull]
        private ApplicationManager _app;

        /// <summary>
        /// Sets up the test environment for each test.
        /// </summary>
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            _app = BuildApplicationInstance();

            _control = new ExplorerControl(_app.RootNodes);

            _app.Alfred.Initialize();

            // Get the control ready for interaction
            InitializeControl(_control);
        }

        /// <summary>
        /// Ensures that the control has a <see cref="TreeView"/> named TreeNodes
        /// </summary>
        [Test, STAThread]
        public void ControlHasTreeView()
        {
            AssertHasTreeView(_control.TreeHierarchy);
        }


        /// <summary>
        /// Ensures that the control has a <see cref="DataGrid"/> named GridProperties
        /// </summary>
        [Test, STAThread]
        public void ControlHasDataGrid()
        {
            AssertHasDataGrid(_control.GridProperties);
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
