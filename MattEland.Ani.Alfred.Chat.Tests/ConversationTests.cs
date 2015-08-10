using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using JetBrains.Annotations;

using NUnit.Framework;

namespace MattEland.Ani.Alfred.Chat.Tests
{
    /// <summary>
    /// Tests for conversational inquiries on the Alfred chat system.
    /// </summary>
    [TestFixture]
    [SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    public class ConversationTests
    {
        [NotNull]
        private AimlStatementHandler _chat;

        /// <summary>
        /// Sets up the testing environment prior to each test run.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _chat = new AimlStatementHandler();
        }

        [Test]
        public void NameEndsInAlfred()
        {
            var reply = GetReply("What's your name?");

            Assert.IsTrue(reply.EndsWith(" Alfred."), $"'{reply}' did not end in ' Alfred.'");
        }

        /// <summary>
        /// Gets a reply from the chat system on a specified inquiry.
        /// </summary>
        /// <param name="text">The inquiry text.</param>
        /// <returns>The reply</returns>
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        [NotNull]
        private string GetReply([CanBeNull] string text)
        {
            var response = _chat.HandleUserStatement(text);

            // Do some basic validity checks
            Assert.NotNull(response, "Response was null");
            Assert.IsTrue(response.WasHandled);
            Assert.AreEqual(text, response.UserInput);
            Assert.IsNotNull(response.ResponseText, "Response text was null");

            return response.ResponseText;
        }

    }
}
