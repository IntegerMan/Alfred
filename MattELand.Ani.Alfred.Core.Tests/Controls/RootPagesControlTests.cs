using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows.Controls;

using JetBrains.Annotations;

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
    public class RootPagesControlTests
    {
        [NotNull]
        private RootPagesControl _control;

        /// <summary>
        /// Sets up the test environment for each test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _control = new RootPagesControl();
        }

        /// <summary>
        /// Ensures that the RootPagesControl has a TabControl named TabPages
        /// </summary>
        [Test, STAThread]
        public void ControlHasTabPages()
        {
            Assert.IsNotNull(_control.TabPages);
            Assert.That(_control.TabPages is TabControl);
        }
    }
}
