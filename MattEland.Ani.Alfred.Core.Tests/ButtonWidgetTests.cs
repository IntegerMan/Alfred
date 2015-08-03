﻿// ---------------------------------------------------------
// ButtonWidgetTests.cs
// 
// Created on:      08/03/2015 at 1:15 AM
// Last Modified:   08/03/2015 at 2:10 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Widgets;

using NUnit.Framework;

namespace MattEland.Ani.Alfred.Core.Tests
{
    /// <summary>
    ///     Tests testing the
    ///     <see
    ///         cref="ButtonWidget" />
    ///     class
    /// </summary>
    [TestFixture]
    public class ButtonWidgetTests
    {
        [NotNull]
        private readonly SimplePlatformProvider _platformProvider = new SimplePlatformProvider();

        [Test]
        public void ButtonCommandDefaultsToNull()
        {
            var button = new ButtonWidget();

            Assert.IsNull(button.ClickCommand);
        }

        /// <summary>
        /// Asserts that setting up a button and command but not clicking the button doesn't fire
        /// the command. This is really an excuse to test different ways of setting up the commands.
        /// </summary>
        [Test]
        public void ButtonCommandsDoNotExecuteWhenButtonIsNotClicked()
        {
            var executed = false;
            Action executeAction = () => { executed = true; };
            var command = _platformProvider.CreateCommand(executeAction);

            var button = new ButtonWidget(command);

            Assert.IsNotNull(button.ClickCommand, "Button's ClickCommand was null");
            Assert.IsFalse(executed, "The button was invoked but the button was not set");
        }

        [Test]
        public void ButtonCommandsExecuteWhenClicked()
        {
            var executed = false;
            var command = _platformProvider.CreateCommand();
            command.ExecuteAction = () => { executed = true; };

            var button = new ButtonWidget { ClickCommand = command };
            button.Click();

            Assert.IsTrue(executed, "The button was invoked but the executed flag was not set");
        }

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

    }
}