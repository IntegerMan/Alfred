// ---------------------------------------------------------
// ButtonWidgetTests.cs
// 
// Created on:      08/03/2015 at 1:15 AM
// Last Modified:   08/03/2015 at 1:15 AM
// Original author: Matt Eland
// ---------------------------------------------------------

using MattEland.Ani.Alfred.Core.Widgets;

using NUnit.Framework;

namespace MattEland.Ani.Alfred.Core.Tests
{
    /// <summary>
    /// Tests testing the <see cref="ButtonWidget" /> class
    /// </summary>
    [TestFixture]
    public class ButtonWidgetTests
    {

        [Test]
        public void CreatingAButtonWithParameterSetsText()
        {
            const string TestText = "Test Text";

            var button = new ButtonWidget(TestText);

            Assert.AreEqual(TestText, button.Text, "Button Text was not set as expected");
        }

        [Test]
        public void NewButtonsDefaultToNullText()
        {
            var button = new ButtonWidget();

            Assert.IsNull(button.Text, "Button's text was set to something other than null after instantiation.");
        }

        [Test]
        public void ButtonCommandDefaultsToNull()
        {
            var button = new ButtonWidget();

            Assert.IsNull(button.ClickCommand);
        }

    }
}