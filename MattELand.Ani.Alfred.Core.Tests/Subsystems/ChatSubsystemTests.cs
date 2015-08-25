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
using MattEland.Common;

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
        ///     Finds the provider with the specified name.
        /// </summary>
        /// <param name="providers">The providers.</param>
        /// <param name="name">The name.</param>
        /// <returns>The provider</returns>
        [CanBeNull]
        private static IPropertyProvider FindProvider(
            [NotNull] IEnumerable<IPropertyProvider> providers,
            string name)
        {
            return providers.FirstOrDefault(p => p != null && p.DisplayName.Matches(name));
        }

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

            Assert.IsNotNull(FindProvider(providers, name), $"{name} node was missing");
        }
    }
}