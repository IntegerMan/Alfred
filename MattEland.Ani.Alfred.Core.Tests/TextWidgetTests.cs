using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MattEland.Ani.Alfred.Core.Widgets;

using NUnit.Framework;

namespace MattEland.Ani.Alfred.Core.Tests
{
    /// <summary>
    /// Contains tests related to <see cref="TextWidget"/>
    /// </summary>
    [TestFixture]
    public class TextWidgetTests
    {
        private const string TestString = "This is a test";

        [Test]
        public void TextWidgetTextTests()
        {
            var widget = new TextWidget { Text = TestString };

            Assert.AreEqual(TestString, widget.Text);
        }

        [Test]
        public void TextWidgetConstructorTextTests()
        {
            var widget = new TextWidget(TestString);

            Assert.AreEqual(TestString, widget.Text);
        }

    }
}
