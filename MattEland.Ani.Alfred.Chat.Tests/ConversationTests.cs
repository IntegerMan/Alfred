// ---------------------------------------------------------
// ConversationTests.cs
// 
// Created on:      08/10/2015 at 2:42 PM
// Last Modified:   08/16/2015 at 4:47 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Security;

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
        /// <exception cref="IOException">An I/O error occurred.</exception>
        /// <exception cref="DirectoryNotFoundException">Attempted to set a local path that cannot be found.</exception>
        /// <exception cref="SecurityException">The caller does not have the appropriate permission.</exception>
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

        [TestCase("Bye", "tmp_bye")]
        [TestCase("Hi", "tmp_hi")]
        [TestCase("Hey", "tmp_hi")]
        [TestCase("Thank You", "tmp_thanks")]
        public void ChatRedirectTests([NotNull] string input, [NotNull] string redirectTemplateId)
        {
            var template = GetReplyTemplate(input);

            Assert.IsTrue(template.ToLowerInvariant().Contains(redirectTemplateId),
                          $"The template {template} did not redirect to the template with an Id tag of {redirectTemplateId}");
        }

        /// <summary>
        ///     Asserts that asking chat for the date gives the correct date.
        /// </summary>
        /// <remarks>
        ///     Test for ALF-4
        /// </remarks>
        [Test]
        public void AskingForCurrentDayContainsCurrentDay()
        {
            var now = DateTime.Now;
            var reply = GetReply("Which day is it?");

            var expected = now.ToString("D");
            Assert.That(reply.Contains(expected),
                        $"Asked for current day at {now} and got a reply of '{reply}' which did not contain {expected}");
        }

        /// <summary>
        ///     Asserts that asking chat for the time gives the correct time.
        /// </summary>
        /// <remarks>
        ///     Test for ALF-3
        /// </remarks>
        [Test]
        public void AskingForCurrentTimeContainsCurrentTime()
        {
            var now = DateTime.Now;
            var reply = GetReply("What time is it?");

            var expected = now.ToString("t");
            Assert.That(reply.Contains(expected),
                        $"Asked for current time at {now} and got a reply of '{reply}' which did not contain {expected}");
        }
        /// <summary>
        ///     Asserts that when working through redirects, the engine can get the deepest redirect template.
        /// </summary>
        /// <remarks>
        ///     Test for ALF-10
        /// </remarks>
        [Test]
        public void AskingForCurrentTimeRevealsRedirectedTemplate()
        {
            var reply = GetReplyTemplate("What time is it?");

            Assert.That(reply.Contains("<date"), $"The reply template of {reply} did not contain a date tag. Redirects are likely broken.");
        }
    }
}