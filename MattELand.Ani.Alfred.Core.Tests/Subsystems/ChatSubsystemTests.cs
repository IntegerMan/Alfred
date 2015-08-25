// ---------------------------------------------------------
// ChatSubsystemTests.cs
// 
// Created on:      08/25/2015 at 10:53 AM
// Last Modified:   08/25/2015 at 11:34 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat;
using MattEland.Ani.Alfred.Core.Definitions;

using NUnit.Framework;

namespace MattEland.Ani.Alfred.Tests.Subsystems
{
    /// <summary>
    ///     Tests for the <see cref="ChatSubsystem" />
    /// </summary>
    [TestFixture]
    [SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    public class ChatSubsystemTests
    {

        /// <summary>
        ///     Sets up the test environment for each test
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _chat = new ChatSubsystem(new SimplePlatformProvider(), null);
        }

        [NotNull]
        private ChatSubsystem _chat;

        /// <summary>
        ///     Checks that the Chat History and Chat Handler nodes are present in the property providers for
        ///     the Chat Subsystem.
        /// </summary>
        /// <remarks>
        ///     Test ALF-78 for tasks ALF-59 and ALF-61
        /// </remarks>
        [TestCase(ChatHistoryProvider.InstanceDisplayName)]
        [TestCase(ChatHandlersProvider.InstanceDisplayName)]
        public void ChatSubsystemListsCorrectNodes(string name)
        {
            var providers = _chat.PropertyProviders;

            Assert.IsNotNull(providers.Find(name), $"{name} node was missing");
        }

        /// <summary>
        /// Tests that the chat history explorer node has no child nodes initially.
        /// </summary>
        /// <remarks>
        /// Test ALF-79 for story ALF-62
        /// </remarks>
        [Test]
        public void ChatHistoryHasNoNodesInitially()
        {
            var node = _chat.PropertyProviders.Find(ChatHistoryProvider.InstanceDisplayName);
            Assert.IsNotNull(node, "Could not find Chat History node");

            // Check to see that it has no children.
            Assert.AreEqual(0, node.PropertyProviders.Count());
        }
    }

}