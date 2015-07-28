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

        [Test]
        public void CanInstantiateTextWidget()
        {
            var widget = new TextWidget();
        }

        [Test]
        public void TextWidgetTextTests()
        {
            var widget = new TextWidget { Text = "This is a test" };

            Assert.AreEqual("This is a test", widget.Text);
        }

    }
}
