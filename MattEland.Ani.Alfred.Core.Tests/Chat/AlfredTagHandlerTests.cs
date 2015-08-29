// ---------------------------------------------------------
// AlfredTagHandlerTests.cs
// 
// Created on:      08/17/2015 at 10:55 PM
// Last Modified:   08/17/2015 at 10:55 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Reflection;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat;
using MattEland.Ani.Alfred.Chat.Aiml.TagHandlers;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Common;
using MattEland.Common.Providers;

using NUnit.Framework;

namespace MattEland.Ani.Alfred.Tests.Chat
{
    /// <summary>
    /// A class to test tag handlers for Alfred commands.
    /// </summary>
    [TestFixture]
    [SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    public class AlfredTagHandlerTests : ChatTestsBase
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

            CommonProvider.Container.RegisterDefaultAlfredMappings();

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

        [Test]
        public void AlfredTagHandlerCausesCommandToBeInvoked()
        {
            Say("TEST COMMAND INVOKE");

            var command = Alfred.LastCommand;

            var subsystem = "test";
            var expected = "invoketest";

            Assert.IsNotNull(Engine.Owner, "Chat Engine had no owner. Commands will not be routed without an owner.");
            Assert.That(command != ChatCommand.Empty, "Alfred's last command was an empty command. No command was invoked.");
            Assert.That(command.Subsystem.Matches(subsystem), $"Chat Command had subsystem of '{command.Subsystem}' instead of '{subsystem}'");
            Assert.That(command.Name.Matches(expected), $"Chat Command had command of '{command.Name}' instead of '{expected}'");
        }

    }
}