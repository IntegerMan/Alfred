// ---------------------------------------------------------
// ConversationTests.cs
// 
// Created on:      08/10/2015 at 2:42 PM
// Last Modified:   08/16/2015 at 4:47 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Security;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat;
using MattEland.Ani.Alfred.Chat.Aiml;

using NUnit.Framework;

namespace MattEland.Ani.Alfred.Tests.Chat
{
    /// <summary>
    ///     Tests for commands embedded in AIML that impact the functioning of the Alfred system.
    /// </summary>
    [TestFixture]
    public class ChatCommandTests : ChatTestsBase
    {
        /// <summary>
        ///     Sets up the testing environment prior to each test run.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            InitChatSystem();
        }

        [Test]
        [Ignore("Currently working on this")]
        public void ShutdownCausesAlfredToBeOffline()
        {
            Say("Shutdown");

            Assert.IsFalse(Alfred.IsOnline, "Alfred was not offline after shutdown was handled.");
        }

    }
}