// ---------------------------------------------------------
// AlfredTagHandlerTests.cs
// 
// Created on:      08/17/2015 at 10:55 PM
// Last Modified:   08/17/2015 at 10:55 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Xml;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat;
using MattEland.Ani.Alfred.Chat.Aiml;
using MattEland.Ani.Alfred.Chat.Aiml.TagHandlers;
using MattEland.Ani.Alfred.Chat.Aiml.Utils;

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
        public void SetUp()
        {
            InitChatSystem();

            var parameters = BuildTagHandlerParameters("<alfred submodule=\"core\" command=\"shutdown\" />");
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



    }
}