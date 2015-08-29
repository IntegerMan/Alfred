// ---------------------------------------------------------
// ButtonWidgetTests.cs
// 
// Created on:      08/08/2015 at 6:18 PM
// Last Modified:   08/08/2015 at 6:20 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Widgets;

using NUnit.Framework;

namespace MattEland.Ani.Alfred.Tests.Widgets
{
    /// <summary>
    ///     Tests testing the
    ///     <see cref="ButtonWidget" />
    ///     class
    /// </summary>
    [TestFixture]
    public class ButtonWidgetTests : AlfredTestBase
    {
        [NotNull]
        private readonly SimplePlatformProvider _platformProvider = new SimplePlatformProvider();

        [Test]
        public void ButtonCommandDefaultsToNull()
        {
            var button = new ButtonWidget(BuildWidgetParams());

            Assert.IsNull(button.ClickCommand);
        }

        /// <summary>
        ///     Asserts that setting up a button and command but not clicking the button doesn't fire
        ///     the command. This is really an excuse to test different ways of setting up the commands.
        /// </summary>
        [Test]
        public void ButtonCommandsDoNotExecuteWhenButtonIsNotClicked()
        {
            var executed = false;
            Action executeAction = () => { executed = true; };
            var command = _platformProvider.CreateCommand(executeAction);

            var button = new ButtonWidget("Click Me", command, BuildWidgetParams());

            Assert.IsNotNull(button.ClickCommand, "Button's ClickCommand was null");
            Assert.IsFalse(executed, "The button was invoked but the button was not set");
        }


        /// <summary>
        ///     Builds widget parameters.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>The WidgetCreationParameters.</returns>
        [NotNull]
        private WidgetCreationParameters BuildWidgetParams(string name = "WidgetTest")
        {
            return new WidgetCreationParameters(name);
        }

        [Test]
        public void ButtonCommandsExecuteWhenClicked()
        {
            var executed = false;
            var command = _platformProvider.CreateCommand();
            command.ExecuteAction = () => { executed = true; };

            var button = new ButtonWidget(BuildWidgetParams()) { ClickCommand = command };
            button.Click();

            Assert.IsTrue(executed, "The button was invoked but the executed flag was not set");
        }

        [Test]
        public void CreatingAButtonWithParameterSetsText()
        {
            const string TestText = "Test Text";

            var button = new ButtonWidget(TestText, null, BuildWidgetParams());

            Assert.AreEqual(TestText, button.Text, "Button Text was not set as expected");
        }

        [Test]
        public void NewButtonsDefaultToNullText()
        {
            var button = new ButtonWidget(BuildWidgetParams());

            Assert.IsNull(button.Text, "Button's text was set to something other than null after instantiation.");
        }
    }
}