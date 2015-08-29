// ---------------------------------------------------------
// AlfredTagHandlerTests.cs
// 
// Created on:      08/17/2015 at 10:55 PM
// Last Modified:   08/17/2015 at 10:55 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

using MattEland.Ani.Alfred.Chat.Aiml.TagHandlers;
using MattEland.Common;

using NUnit.Framework;

namespace MattEland.Ani.Alfred.Tests.Chat
{
    /// <summary>
    /// A class to test tag handlers for "srai" tags.
    /// </summary>
    [TestFixture]
    [SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    public class RedirectTagHandlerTests : ChatTestsBase
    {
        /// <summary>
        /// Sets up the test fixture for each test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            InitializeChatSystem();

        }

        [Test]
        public void RedirectOnNoTextOutputsEmpty()
        {
            var parameters = BuildTagHandlerParameters("<srai />");
            var handler = new RedirectTagHandler(parameters);

            var output = handler.Transform();

            Assert.That(output.IsEmpty(), $"Output was not empty as expected but {output}");
        }

        [Test]
        public void RedirectOnNoTextDoesNotSpawnSecondRequest()
        {
            var parameters = BuildTagHandlerParameters("<srai />");
            var handler = new RedirectTagHandler(parameters);

            var output = handler.Transform();

            Assert.IsNull(handler.Request.Child, "Second request was spawned");
        }

        [Test]
        public void RedirectWithTextSpawnsSecondRequest()
        {
            var parameters = BuildTagHandlerParameters("<srai>Batman</srai>");
            var handler = new RedirectTagHandler(parameters);

            var output = handler.Transform();

            Assert.IsNotNull(handler.Request.Child, "Second request was not spawned");
        }

        [Test]
        public void RedirectWithTextResultsInText()
        {
            var parameters = BuildTagHandlerParameters("<srai>Batman</srai>");
            var handler = new RedirectTagHandler(parameters);

            var output = handler.Transform();

            Assert.That(output.HasText(), "Output was empty after redirect.");
        }

    }
}