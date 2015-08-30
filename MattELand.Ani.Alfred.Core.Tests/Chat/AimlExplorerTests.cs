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
        [NotNull]
        private Random _random;

        [TestFixtureSetUp]
        public void SetUpFixture()
        {
            _random = new Random();
        }

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
                var childName = children[_random.Next(numChildren)];
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
    }
}