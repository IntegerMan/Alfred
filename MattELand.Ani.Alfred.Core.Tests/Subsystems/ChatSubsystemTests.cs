// ---------------------------------------------------------
// ChatSubsystemTests.cs
// 
// Created on:      08/25/2015 at 10:53 AM
// Last Modified:   08/25/2015 at 10:53 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Common;

using NUnit.Framework;

namespace MattEland.Ani.Alfred.Tests.Subsystems
{
    /// <summary>
    /// Tests for the <see cref="ChatSubsystem"/>
    /// </summary>
    [TestFixture]
    [SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    public class ChatSubsystemTests
    {
        [NotNull]
        private ChatSubsystem _chat;

        /// <summary>
        /// Sets up the test environment for each test
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _chat = new ChatSubsystem(new SimplePlatformProvider(), null);
        }

        /// <summary>
        /// Checks that the Chat History node is present in the property providers for the Chat Subsystem
        /// </summary>
        /// <remarks>
        /// Test ALF-78 for task ALF-61
        /// </remarks>
        [Test]
        public void ChatSubsystemListsNodeForChatHistory()
        {
            var providers = _chat.PropertyProviders;

            Assert.That(providers.Any(p => p.DisplayName.Matches(ChatHistoryContainer.InstanceDisplayName)), "Chat history node was missing");
        }

        /// <summary>
        /// Checks that the Chat Handlers node is present in the property providers for the Chat Subsystem
        /// </summary>
        /// <remarks>
        /// Test ALF-78 for task ALF-59
        /// </remarks>
        [Test, Ignore]
        public void ChatSubsystemListsNodeForAimlHandlers()
        {
            var providers = _chat.PropertyProviders;

            Assert.That(providers.Any(p => p.DisplayName.Matches("Chat Handlers")), "Chat handlers node was missing");
        }

    }
}