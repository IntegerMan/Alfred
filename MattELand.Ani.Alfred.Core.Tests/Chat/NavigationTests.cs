// ---------------------------------------------------------
// ConversationTests.cs
// 
// Created on:      08/19/2015 at 9:31 PM
// Last Modified:   09/03/2015 at 1:07 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Linq.Expressions;

using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Common;
using MattEland.Testing;

using Moq;

using NUnit.Framework;

namespace MattEland.Ani.Alfred.Tests.Chat
{
    /// <summary>
    ///     Tests for navigational inquiries on the Alfred chat system.
    /// </summary>
    [UnitTestProvider]
    public sealed class NavigationTests : ChatTestsBase
    {
        /// <summary>
        ///     Sets up the testing environment prior to each test run.
        /// </summary>
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
        }

        [TestCase("Navigate Unknown", "tmp_nav_unknown")]
        public void ChatNavigateTemplateTests(string input, string templateId)
        {
            // Simple handler to always return string.Empty for this test case.
            var shell = new Mock<IShellCommandRecipient>();
            shell.Setup(s => s.ProcessShellCommand(It.IsAny<ShellCommand>())).Returns(string.Empty);

            InitializeChatSystem(shell.Object);

            AssertMessageGetsReplyTemplate(input, templateId);
        }

        /// <summary>
        ///     Tests that navigation failure maps to the correct template.
        /// </summary>
        [Test]
        public void NavFailNavigatesToFailure()
        {
            var shell = new Mock<IShellCommandRecipient>();

            // Any data mentioning failure goes to fail
            SetupShellCommandHandler(shell,
                c => c.Name.Matches("Nav") && c.Target.Matches("Pages") && c.Data.Contains("Fail"),
                "Navigate Failed");

            InitializeChatSystem(shell: shell.Object);

            AssertMessageGetsReplyTemplate("Navigate Test Fail", @"tmp_nav_failed");
        }

        /// <summary>
        ///     Tests that targeted navigation maps to the correct AIML template.
        /// </summary>
        /// <param name="input"> The input text that is sent to the chat system. </param>
        /// <param name="dataValue"> The command data value </param>
        [TestCase("Navigate Log", "Log")]
        [TestCase("Navigate Home", "Core")]
        public void TargetedNavigationSucceeds(string input, string dataValue)
        {
            var shell = new Mock<IShellCommandRecipient>();

            // Ensure we get a command with a data value that we're expecting
            SetupShellCommandHandler(shell,
                c => c.Name.Matches("Nav") && c.Target.Matches("Pages") && c.Data.Matches(dataValue),
                "Navigate Success");

            InitializeChatSystem(shell: shell.Object);

            AssertMessageGetsReplyTemplate(input, @"tmp_nav_success");
        }

        /// <summary>
        ///     Sets up the shell command handler mock.
        /// </summary>
        /// <param name="shell"> The shell. </param>
        /// <param name="match"> The match condition. </param>
        /// <param name="returnValue"> The return value. </param>
        private static void SetupShellCommandHandler(Mock<IShellCommandRecipient> shell,
            Expression<Func<ShellCommand, bool>> match, string returnValue)
        {
            shell.Setup(r => r.ProcessShellCommand(It.Is<ShellCommand>(match))).Returns(returnValue);
        }
    }
}