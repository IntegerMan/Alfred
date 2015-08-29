// ---------------------------------------------------------
// ChatSubsystemTests.cs
// 
// Created on:      08/25/2015 at 10:53 AM
// Last Modified:   08/25/2015 at 3:25 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat;
using MattEland.Ani.Alfred.PresentationShared.Commands;
using MattEland.Common.Providers;

using NUnit.Framework;

namespace MattEland.Ani.Alfred.Tests.Subsystems
{
    /// <summary>
    ///     Tests for the <see cref="ChatSubsystem" />
    /// </summary>
    [TestFixture]
    [SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    public class ChatSubsystemTests : AlfredTestBase
    {

        /// <summary>
        ///     Sets up the test environment for each test
        /// </summary>
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            _chat = new ChatSubsystem(Container, null, "Alfredo");
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
        [TestCase("Chat History")]
        [TestCase(ChatHandlersProvider.InstanceDisplayName)]
        public void ChatSubsystemListsCorrectNodes(string name)
        {
            var providers = _chat.PropertyProviders;

            Assert.IsNotNull(providers.Find(name), $"{name} node was missing");
        }

        /// <summary>
        ///     Tests that the chat history explorer node has nodes after a few chat exchanges
        /// </summary>
        /// <remarks>
        ///     Test ALF-80 for story ALF-62
        /// </remarks>
        [Test]
        public void ChatHistoryHasNodesAfterConversation()
        {
            // Initialize an Alfred application
            var app = new ApplicationManager(false);
            var alfred = app.Alfred;
            var chat = alfred.Subsystems.First(s => s is ChatSubsystem);
            alfred.Initialize();

            // Find the chat system
            var node = chat.PropertyProviders.Find("Chat History");
            Assert.IsNotNull(node, "Could not find Chat History node");

            /* The chat history node is going to already have a few entries from coming online. 
            We want a count of new history entries after startup. */

            var count = node.PropertyProviders.Count();

            // Say the same thing repeatedly to Alfred, thus driving him insane.
            const string ChatInput = "What's your favorite color?";
            const int NumChats = 25;
            for (var i = 0; i < NumChats; i++)
            {
                alfred.ChatProvider.HandleUserStatement(ChatInput);
            }

            // Grab the response once and move it to a list so we can examine it
            var children = node.PropertyProviders.ToList();

            /* We expect there to be a statement and a reply for every entry in the chat history
            plus the initial number of chat entries. */

            Assert.AreEqual(count + (NumChats * 2),
                            children.Count(),
                            "The chat history did not have the expected number of nodes");

            // We expect that what we said will only appear on nodes originating from the "user".
            Assert.AreEqual(NumChats, children.Count(p => p.DisplayName.Contains(ChatInput)));
        }

        /// <summary>
        ///     Tests that the chat history explorer node has no child nodes initially.
        /// </summary>
        /// <remarks>
        ///     Test ALF-79 for story ALF-62
        /// </remarks>
        [Test]
        public void ChatHistoryHasNoNodesInitially()
        {
            var node = _chat.PropertyProviders.Find("Chat History");
            Assert.IsNotNull(node, "Could not find Chat History node");

            // Check to see that it has no children.
            Assert.AreEqual(0, node.PropertyProviders.Count());
        }
    }

}