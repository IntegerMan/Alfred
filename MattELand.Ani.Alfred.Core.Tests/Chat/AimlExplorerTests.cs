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

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Subsystems;

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
        ///     Ensures that the knowledge node has user-acceptable root text
        /// </summary>
        /// <remarks>
        /// Test ALF-102 for sub-task ALF-60
        /// </remarks>
        [Test]
        public void AimlKnowledgeRootHasMeaningfulName()
        {
            var root = AimlRoot;
            root.Name.ShouldBe("Root Node");
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