﻿// ---------------------------------------------------------
// TextWidgetTests.cs
// 
// Created on:      08/08/2015 at 6:19 PM
// Last Modified:   08/08/2015 at 6:20 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

using MattEland.Ani.Alfred.Core.Widgets;

using NUnit.Framework;

namespace MattEland.Ani.Alfred.Core.Tests.Widgets
{
    /// <summary>
    ///     Contains tests related to <see cref="TextWidget" />
    /// </summary>
    [TestFixture]
    public sealed class TextWidgetTests
    {
        private const string TestString = "This is a test";

        [Test]
        public void TextWidgetConstructorTextTests()
        {
            var widget = new TextWidget(TestString);

            Assert.AreEqual(TestString, widget.Text);
        }

        [Test]
        public void TextWidgetTextTests()
        {
            var widget = new TextWidget { Text = TestString };

            Assert.AreEqual(TestString, widget.Text);
        }
    }
}