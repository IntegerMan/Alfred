// ---------------------------------------------------------
// ConversationTests.cs
// 
// Created on:      08/10/2015 at 2:42 PM
// Last Modified:   08/11/2015 at 2:57 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;

using NUnit.Framework;

namespace MattEland.Ani.Alfred.Chat.Tests
{
    /// <summary>
    ///     Tests for conversational inquiries on the Alfred chat system.
    /// </summary>
    [TestFixture]
    [SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
    [SuppressMessage("ReSharper", "UnusedParameter.Local")]
    public class ConversationTests
    {
        /// <summary>
        ///     Sets up the testing environment prior to each test run.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _chat = new AimlStatementHandler();
        }

        [NotNull]
        private AimlStatementHandler _chat;

        /// <summary>
        ///     Asserts that the chat input gets a reply template with the specified ID.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="templateId">The template identifier.</param>
        private void AssertMessageGetsReplyTemplate(string input, string templateId)
        {
            var template = GetReplyTemplate(input);

            AssertTemplateId(template, templateId);
        }

        /// <summary>
        ///     Asserts that the template identifier is found in the response template.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <param name="id">The template identifier.</param>
        private static void AssertTemplateId([NotNull] string template, [NotNull] string id)
        {
            var idString = $"id=\"{id.ToLowerInvariant()}\"";

            Assert.IsTrue(template.ToLowerInvariant().Contains(idString),
                          $"ID '{idString}' was not found. Template was: {template}");
        }

        /// <summary>
        ///     Gets a reply from the chat system on a specified inquiry.
        /// </summary>
        /// <param name="text">The inquiry text.</param>
        /// <returns>The reply</returns>
        [NotNull]
        private string GetReply([CanBeNull] string text)
        {
            var response = GetResponse(text);

            Assert.IsNotNull(response.ResponseText, "Response text was null");

            return response.ResponseText;
        }

        /// <summary>
        ///     Gets the response to the chat message.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>The response</returns>
        private UserStatementResponse GetResponse(string text)
        {

            var response = _chat.HandleUserStatement(text);

            // Do some basic validity checks
            Assert.NotNull(response, "Response was null");
            Assert.AreEqual(text, response.UserInput);
            return response;
        }

        /// <summary>
        ///     Gets a reply from the chat system on a specified inquiry.
        /// </summary>
        /// <param name="text">The inquiry text.</param>
        /// <returns>The reply</returns>
        [NotNull]
        private string GetReplyTemplate([CanBeNull] string text)
        {
            var response = GetResponse(text);
            return response.Template;
        }

        [TestCase("Shutdown", "tmp_shutdown")]
        [TestCase("Status", "tmp_status")]
        public void ChatCoreTests(string input, string templateId)
        {
            AssertMessageGetsReplyTemplate(input, templateId);
        }

        [TestCase("Thanks", "tmp_thanks")]
        [TestCase("Goodbye", "tmp_bye")]
        [TestCase("Hello", "tmp_hi")]
        [TestCase("Help", "tmp_help")]
        [TestCase("Help Commands", "tmp_help_commands")]
        [TestCase("Help Questions", "tmp_help_questions")]
        [TestCase("Help Other", "tmp_help_other")]
        [TestCase("ASDfasd", "tmp_fallback", Description = "Gibberish should hit fallback case")]
        public void ChatTemplateTests(string input, string templateId)
        {
            AssertMessageGetsReplyTemplate(input, templateId);
        }

        [TestCase("Bye", "GOODBYE")]
        [TestCase("Hi", "Hello")]
        [TestCase("Hey", "Hello")]
        [TestCase("Thank You", "Thanks")]
        public void ChatRedirectTests([NotNull] string input, [NotNull] string redirectTo)
        {
            var template = GetReplyTemplate(input);

            var redirectFind = $"<srai>{redirectTo.ToLowerInvariant()}</srai>";

            Assert.IsTrue(template.ToLowerInvariant().Contains(redirectFind),
                          $"The template {template} did not contain a redirect to {redirectTo}");
        }
    }
}