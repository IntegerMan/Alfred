// ---------------------------------------------------------
// TextWidgetTests.cs
// 
// Created on:      08/08/2015 at 6:19 PM
// Last Modified:   08/08/2015 at 6:20 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Widgets;
using MattEland.Testing;

using NUnit.Framework;

namespace MattEland.Ani.Alfred.Tests.Widgets
{
    /// <summary>
    ///     Contains tests related to <see cref="TextWidget" />
    /// </summary>
    [UnitTestProvider]
    public sealed class TextWidgetTests : AlfredTestBase
    {
        private const string TestString = "This is a test";

        [Test]
        public void TextWidgetConstructorTextTests()
        {
            var widget = new TextWidget(TestString, BuildWidgetParams());

            Assert.AreEqual(TestString, widget.Text);
        }

        [Test]
        public void TextWidgetTextTests()
        {
            var widget = new TextWidget(BuildWidgetParams()) { Text = TestString };

            Assert.AreEqual(TestString, widget.Text);
        }

        /// <summary>
        ///     Builds widget parameters.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>The <see cref="WidgetCreationParameters"/>.</returns>
        [NotNull]
        private WidgetCreationParameters BuildWidgetParams(string name = "WidgetTest")
        {
            return new WidgetCreationParameters(name, Container);
        }

    }
}