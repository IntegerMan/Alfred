﻿// ---------------------------------------------------------
// ConversationTests.cs
// 
// Created on:      08/10/2015 at 2:42 PM
// Last Modified:   08/16/2015 at 4:47 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat;
using MattEland.Common;

using NUnit.Framework;

namespace MattEland.Ani.Alfred.Tests.Chat
{
    /// <summary>
    /// Tests for conversational inquiries on the Alfred chat system.
    /// </summary>
    [TestFixture]
    [SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
    [SuppressMessage("ReSharper", "UnusedParameter.Local")]
    public class ConversationTests : ChatTestsBase
    {
        /// <summary>
        ///     Sets up the testing environment prior to each test run.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            InitChatSystem();
        }

        /// <summary>
        /// Tests that the chat engine has nodes associated with it
        /// </summary>
        [Test]
        public void ChatEngineHasNodes()
        {
            Assert.Greater(Engine.NodeCount, 0, "Chat engine did not have any nodes");
        }

        [Test]
        public void GenderSubstitutionsHasEntries()
        {
            Assert.Greater(Engine.Librarian.GenderSubstitutions.Count, 0, "Settings were not present");
        }

        [Test]
        public void GlobalSettingsHaveEntries()
        {
            Assert.Greater(Engine.Librarian.GlobalSettings.Count, 0, "Settings were not present");
        }

        [Test]
        public void FirstPersonSubstitutionsHasEntries()
        {
            Assert.Greater(Engine.Librarian.FirstPersonToSecondPersonSubstitutions.Count, 0, "Settings were not present");
        }

        [Test]
        public void SecondPersonSubstitutionsHasEntries()
        {
            Assert.Greater(Engine.Librarian.SecondPersonToFirstPersonSubstitutions.Count, 0, "Settings were not present");
        }

        [Test]
        public void SubstitutionsHasEntries()
        {
            Assert.Greater(Engine.Librarian.Substitutions.Count, 0, "Settings were not present");
        }

        [Test]
        public void StartupResultsInGreeting()
        {
            // In reality, this should be _chat.DoInitialGreeting, but it's easier to test the template this way
            var reply = GetReplyTemplate("EVT_STARTUP");

            Assert.IsNotNull(reply);
            Assert.That(reply.Contains("tmp_hi"), "Startup did not give proper template. Actual template was: " + reply);
        }

        [Test]
        public void StartupLeavesLastInputClear()
        {
            var chat = new AimlStatementHandler();
            chat.DoInitialGreeting();

            Assert.That(!chat.LastInput.HasText(), $"Startup did not clear last input. Actual was: {chat.LastInput}");
        }

        [TestCase("Shutdown", "tmp_bye")]
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
            Assert.IsNotNull(template);

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