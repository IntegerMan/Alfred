using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Subsystems;
using MattEland.Common;

using NUnit.Framework;

namespace MattEland.Ani.Alfred.Tests.Chat
{
    /// <summary>
    ///     An <see langword="abstract"/> class containing helpers for testing the
    ///     <see cref="MindExplorerSubsystem" /> and its explorer tree.
    /// </summary>
    [SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    public abstract class ExplorerTestsBase : AlfredTestBase
    {
        /// <summary>
        /// Sets up the environment for each test.
        /// </summary>
        public override void SetUp()
        {
            base.SetUp();

            // We'll always want Alfred to be online
            StartAlfred();

            ExplorerSubsystem = GetSubsystem<MindExplorerSubsystem>("Mind");
        }

        /// <summary>
        ///     Gets or sets the explorer subsystem.
        /// </summary>
        /// <value>
        ///     The explorer subsystem.
        /// </value>
        [NotNull]
        public MindExplorerSubsystem ExplorerSubsystem { get; set; }

        /// <summary>
        ///     Gets the explorer nodes starting at the root.
        /// </summary>
        /// <value>
        ///     The explorer nodes.
        /// </value>
        [NotNull, ItemNotNull]
        protected IEnumerable<IPropertyProvider> ExplorerNodes
        {
            get
            {
                return ExplorerSubsystem.MindExplorerPage.RootNodes;
            }
        }

        /// <summary>
        ///     Gets the Alfred node.
        /// </summary>
        /// <value>
        ///     The Alfred node.
        /// </value>
        [NotNull]
        protected IPropertyProvider AlfredNode
        {
            get
            {
                var node = ExplorerNodes.Find("Alfred");
                node.ShouldNotBeNull("Could not find Alfred node");

                return node;
            }
        }

    }
}