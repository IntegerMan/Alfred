// ---------------------------------------------------------
// AlfredShellTests.cs
// 
// Created on:      08/18/2015 at 10:35 PM
// Last Modified:   08/18/2015 at 10:35 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Reflection;
using System.Xml;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat;
using MattEland.Ani.Alfred.Chat.Aiml;
using MattEland.Ani.Alfred.Chat.Aiml.TagHandlers;
using MattEland.Ani.Alfred.Chat.Aiml.Utils;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Tests.Chat;

using NUnit.Framework;

namespace MattEland.Ani.Alfred.Tests.Shell
{
    /// <summary>
    /// Tests related to <see cref="IAlfredShell"/> and its utilization.
    /// </summary>
    [TestFixture]
    public sealed class ShellTagHandlerTests : ChatTestsBase
    {
        [NotNull]
        private ShellTagHandler _handler;

        /// <summary>
        /// Sets up the test environment for each test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            InitChatSystem();
            var parameters = BuildTagHandlerParameters("<alfred submodule=\"core\" command=\"shutdown\" />");
            _handler = new ShellTagHandler(parameters);
        }

        [Test]
        public void ShellHandlerHasAppropriateAttributes()
        {
            var type = _handler.GetType();
            var attribute = type.GetCustomAttribute(typeof(HandlesAimlTagAttribute)) as HandlesAimlTagAttribute;

            Assert.IsNotNull(attribute, "Handler did not have the HandlesAimlTag attribute");
            Assert.AreEqual("shell", attribute.Name, "Handler did not handle the expected type");
        }

    }
}