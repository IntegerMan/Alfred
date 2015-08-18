// ---------------------------------------------------------
// ChatCommandTests.cs
// 
// Created on:      08/17/2015 at 9:56 PM
// Last Modified:   08/18/2015 at 11:26 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Linq;

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
        public void ShutdownCausesAlfredToBeOffline()
        {
            Say("Shutdown");

            Assert.IsFalse(Alfred.IsOnline, "Alfred was not offline after shutdown was handled.");
        }

        [Test]
        public void StatusYieldsRelevantInformation()
        {
            var reply = GetReply("Status");
            var count = Alfred.Subsystems.Count();

            Assert.That(reply.Contains($"The System is Online with a total of {count} Subsystems Present."),
                        $"{reply} did not contain the expected system status text.");
        }

        [Test]
        public void TagHandlersAreInvokedWhenATestingTemplateIsInvoked()
        {
            Say("TEST TAG INVOKE");

            Assert.IsTrue(AlfredTestTagHandler.WasInvoked);
        }

        [Test]
        public void TagHandlersAreNotInvokedOnOtherInput()
        {
            Say("I like turtles");

            Assert.IsFalse(AlfredTestTagHandler.WasInvoked);
        }
    }
}