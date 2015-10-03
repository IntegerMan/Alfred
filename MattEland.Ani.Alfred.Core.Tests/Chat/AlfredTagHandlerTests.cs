// ---------------------------------------------------------
// AlfredTagHandlerTests.cs
// 
// Created on:      08/17/2015 at 10:55 PM
// Last Modified:   08/17/2015 at 10:55 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat;
using MattEland.Ani.Alfred.Chat.Aiml.TagHandlers;
using MattEland.Ani.Alfred.Core.Console;
using MattEland.Testing;

using NUnit.Framework;

using Shouldly;

namespace MattEland.Ani.Alfred.Tests.Chat
{
    /// <summary>
    /// A class to test tag handlers for Alfred commands.
    /// </summary>
    [UnitTestProvider]
    [SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    public sealed class AlfredTagHandlerTests : ChatTestsBase
    {
        [NotNull]
        private AlfredTagHandler _handler;

        /// <summary>
        /// Sets up the test fixture for each test.
        /// </summary>
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            InitializeChatSystem();

            var parameters = BuildTagHandlerParameters(@"<alfred submodule=""core"" command=""shutdown"" />");
            _handler = new AlfredTagHandler(parameters);
        }

        [Test]
        [SuppressMessage("ReSharper", "ExceptionNotDocumented")]
        public void AlfredTagHandlerHasHandlesAttribute()
        {
            var type = _handler.GetType();
            var attribute = type.GetCustomAttribute(typeof(HandlesAimlTagAttribute)) as HandlesAimlTagAttribute;

            Assert.IsNotNull(attribute, "Handler did not have the HandlesAimlTag attribute");
            Assert.AreEqual("alfred", attribute.Name, "Handler did not handle the expected type");
        }

        /// <summary>
        ///     Tests that saying something with an AIML command tag causes the command to be routed
        /// </summary>
        [Test]
        public void AlfredTagHandlerCausesCommandToBeInvoked()
        {
            //! Arrange

            var console = AlfredContainer.Console;

            //! Act

            Say("TEST COMMAND INVOKE");

            //! Assert - check that the event log contains an entry for the event firing

            var events = console.Events.Where(e => e.Title == "CommandRouting").ToList();
            events.Count().ShouldBe(1);

            var logEntry = events.First();
            logEntry.ShouldNotBeNull();

            logEntry.Message.ShouldBe(@"Command 'invoketest' raised for subsystem 'test' with data value of ''.");
        }

    }
}