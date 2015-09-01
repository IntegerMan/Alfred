// ---------------------------------------------------------
// AimlExplorerTests.cs
// 
// Created on:      08/30/2015 at 12:00 AM
// Last Modified:   08/30/2015 at 12:00 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat;
using MattEland.Ani.Alfred.Chat.Aiml;
using MattEland.Ani.Alfred.Chat.Aiml.Utils;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Subsystems;
using MattEland.Common;

using NUnit.Framework;

using Shouldly;

namespace MattEland.Ani.Alfred.Tests.Chat
{

    /// <summary>
    ///     Tests around the AIML explorer features of being able to explore the AIML tree using the <seealso cref="MindExplorerSubsystem"/>
    /// </summary>
    [TestFixture]
    [SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    public sealed class AimlExplorerTests : ExplorerTestsBase
    {
        /// <summary>
        ///     A complex multi-sentence input designed to confuse Alfred.
        /// </summary>
        private const string ChatComplexInput = "Hi Alfred. What is the date? Time?";

        private const string SimpleChatInput = "Hi";

        /// <summary>
        ///     Ensures that the AIML knowledge node has user-acceptable root text.
        /// </summary>
        /// <remarks>
        ///     Test ALF-102 for sub-task ALF-60.
        /// </remarks>
        [Test]
        public void AimlKnowledgeRootHasMeaningfulName()
        {
            var root = AimlRoot;
            root.Name.ShouldBe("Root Node");
        }

        /// <summary>
        ///     Ensures that the AIML knowledge node child nodes for all base-level topics.
        /// </summary>
        /// <remarks>
        ///     Test ALF-101 for sub-task ALF-60.
        /// </remarks>
        [Test]
        public void AimlKnowledgeRootContainsAllChildren()
        {
            var chatEngine = ChatEngine;
            var chatRoot = chatEngine.RootNode;
            var aimlNodes = chatRoot.Children.Keys;

            var root = AimlRoot;

            // Do a basic count comparison
            var childNodes = root.PropertyProviders.ToList();
            childNodes.Count().ShouldBe(aimlNodes.Count());

            foreach (var child in aimlNodes)
            {
                var match = childNodes.FirstOrDefault(c => c.Name.Matches(child));
                match.ShouldNotBeNull($"Could not find an appropriate mapping for AIML child node: {child}");
            }
        }

        /// <summary>
        ///     Tests that following a random path down the AIML graph, the explorer tree should mimic that path. 
        ///     
        ///     This test is repeated multiple times due to its random nature.
        /// </summary>
        /// <remarks>
        ///     Test ALF-103 for sub-task ALF-60.
        /// </remarks>
        [Test, Repeat(100)]
        public void AimlNodesShouldTraceToTheirEndPoints()
        {
            var chatEngine = ChatEngine;
            var chatRoot = chatEngine.RootNode;

            var explorerNode = AimlRoot;

            Node lastNode = null;
            var node = chatRoot;

            // This will be manually exited when a child has reached the end
            while (true)
            {
                // Protect against infinite loops
                if (lastNode == node)
                {
                    Assert.Fail($"Infinite loop detected on node {node.Word}");
                }
                lastNode = node;

                // If it doesn't have any children, get out of here
                var children = node.Children.Keys.ToList();
                var numChildren = children.Count;

                // Exit if there aren't any children
                if (numChildren <= 0)
                {
                    break;
                }

                // Navigate the AIML Node into a random child
                var childName = children[Randomizer.Next(numChildren)];
                node = node.Children[childName];

                // Navigate to the AIML Node
                explorerNode = explorerNode.Nav(childName);

                // Test on this entry
                node.ShouldNotBeNull();
                node.Word.ShouldBe(explorerNode.DisplayName);
            }
        }

        /// <summary>
        ///     Gets the AIML knowledge root node.
        /// </summary>
        /// <value>
        ///     The AIML knowledge root node.
        /// </value>
        [NotNull]
        private IPropertyProvider AimlRoot
        {
            get
            {
                var handlerNode = AlfredNode.Nav("Chat").Nav("Chat Handlers");
                var knowledgeRoot = handlerNode.PropertyProviders.FirstOrDefault();

                knowledgeRoot.ShouldNotBeNull("Could not find AIML knowledge root");

                return knowledgeRoot;
            }
        }

        /// <summary>
        ///     Checks the naming of the chat history node
        /// </summary>
        [Test]
        public void ChatHistoryShouldHaveCorrectName()
        {
            var history = ChatHistoryNode;
            history.DisplayName.ShouldBe("Chat History");
        }

        /// <summary>
        ///     Tests that chat history starts with one entry.
        /// </summary>
        [Test]
        public void ChatHistoryShouldStartWithOneEntry()
        {
            var history = ChatHistoryNode;
            history.PropertyProviders.Count().ShouldBe(1); // from initial greeting
            history.PropertyProviders.First().ShouldBe<ChatHistoryEntry>();
        }

        /// <summary>
        ///     Verifies that the first node in chat history be from the system with proper settings
        /// </summary>
        [Test]
        public void ChatHistoryInitialGreetingShouldBeFromAlfred()
        {
            var history = ChatHistoryNode;
            var entry = history.PropertyProviders.First().ShouldBeOfType<ChatHistoryEntry>();

            // Validate the entry
            entry.ShouldNotBeNull();
            entry.ShouldSatisfyAllConditions("First chat entry was not valid.",
                () => entry.ChatResult.ShouldNotBeNull(),
                () => entry.IsFromChatEngine.ShouldBeTrue(),
                () => entry.User.Name.ShouldBe(Alfred.DisplayName),
                () => entry.User.IsSystemUser.ShouldBeTrue(),
                () => entry.User.ShouldBeSameAs(ChatEngine.SystemUser));
        }

        /// <summary>
        ///     Chat inputs should not have any child nodes (these represent sub-queries).
        /// </summary>
        [Test]
        public void ChatInputsShouldNotHaveChildren()
        {
            SendMessageToChatEngine(ChatComplexInput);

            var historyNode = ChatHistoryNode;
            var chatNodes = historyNode.PropertyProviders;
            var input = chatNodes.FirstOrDefault(n => n.DisplayName.EndsWith(ChatComplexInput));

            var node = input.ShouldBe<ChatHistoryEntry>();

            node.ShouldSatisfyAllConditions("Chat input was invalid",
                () => node.User.ShouldNotBeNull(),
                () => node.User.IsSystemUser.ShouldBeFalse(),
                () => node.ChatResult.ShouldBeNull(),
                () => node.IsFromChatEngine.ShouldBeFalse());
        }

        /// <summary>
        ///     Sends a message to the chat engine and returns the reply.
        /// </summary>
        /// <param name="input"> The input. </param>
        /// <returns>
        ///     A <see cref="UserStatementResponse"/>.
        /// </returns>
        [CanBeNull]
        private UserStatementResponse SendMessageToChatEngine(string input)
        {
            var chatProvider = Alfred.ChatProvider;
            chatProvider.ShouldNotBeNull();
            return chatProvider.HandleUserStatement(input);
        }

        /// <summary>
        ///     Chat inputs should not have any child nodes (these represent sub-queries).
        /// </summary>
        [Test]
        public void ChatOutputsShouldHaveChildren()
        {
            SendMessageToChatEngine(ChatComplexInput);

            var historyNode = ChatHistoryNode;
            var chatNodes = historyNode.PropertyProviders;
            var responseNode = chatNodes.LastOrDefault();
            responseNode.ShouldNotBeNull();
            responseNode.PropertyProviders.Count().ShouldBe(3);
        }

        /// <summary>
        ///     Chat inputs should not have any child nodes (these represent sub-queries).
        /// </summary>
        [Test]
        public void ChatOutputShouldMatchReturnedResponse()
        {
            var reply = SendMessageToChatEngine(ChatComplexInput);
            reply.ShouldNotBeNull();

            var historyNode = ChatHistoryNode;

            var chatNodes = historyNode.PropertyProviders;
            var responseNode = chatNodes.LastOrDefault();
            responseNode.ShouldNotBeNull();

            var node = responseNode.ShouldBe<ChatHistoryEntry>();
            node.ShouldSatisfyAllConditions("Response node was not in the expected format",
                () => node.ChatResult.ShouldNotBeNull(),
                () => node.ChatResult.ShouldBeSameAs(reply.ResultData),
                () => node.ChatResult.RawInput.ShouldBe(reply.UserInput),
                () => node.Message.ShouldBe(reply.ResponseText),
                () => node.DisplayName.ShouldContain(reply.ResponseText));
        }

        /// <summary>
        ///     Ensure that the <see cref="SubQuery"/> nodes in the UI have the same output and input
        ///     text that you would expect on a simple input. This is an important test for traceability
        ///     purposes.
        /// </summary>
        [Test]
        public void ChatOutputToSimpleInputShouldHaveTraceableReply()
        {
            // Send a short message to the system
            var reply = SendMessageToChatEngine(SimpleChatInput);

            // Validate Reply's user text
            reply.ShouldNotBeNull();
            reply.UserInput.ShouldBe(SimpleChatInput);

            // Drill into the last history entry (Alfred's reply)
            var historyNode = ChatHistoryNode;
            var chatNodes = historyNode.PropertyProviders;
            var responseNode = chatNodes.Last().ShouldBe<ChatHistoryEntry>();

            // Drill into the only SubQuery
            var node = responseNode.PropertyProviders.SingleOrDefault().ShouldBe<ChatSubQueryExplorerNode>();

            // Validate the node's properties
            node.SubQuery.ShouldNotBeNull();
            node.Response.ShouldBe(reply.ResponseText);

            // Verify Properties Emitted
            CheckPropertyValue(node.Properties, "Response", reply.ResponseText);
        }

        /// <summary>
        ///     Checks that the property display name is the expected value.
        /// </summary>
        /// <param name="properties"> The properties. </param>
        /// <param name="propertyName"> Name of the property. </param>
        /// <param name="expectedValue"></param>
        private static void CheckPropertyValue(
            [NotNull] IEnumerable<IPropertyItem> properties,
            [NotNull] string propertyName,
            [CanBeNull] string expectedValue)
        {
            var input = properties.Find(propertyName);
            input.ShouldNotBeNull();
            input.DisplayValue.ShouldBe(expectedValue);
        }

        /// <summary>
        ///     Gets the chat history node in the explorer.
        /// </summary>
        /// <value>
        ///     The chat history node.
        /// </value>
        [NotNull]
        private IPropertyProvider ChatHistoryNode
        {
            get
            {
                var historyNode = AlfredNode.Nav("Chat").Nav("Chat History");
                historyNode.ShouldNotBeNull();
                return historyNode;
            }
        }
    }
}