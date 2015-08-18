// ---------------------------------------------------------
// ChatTestsBase.cs
// 
// Created on:      08/17/2015 at 9:57 PM
// Last Modified:   08/18/2015 at 11:25 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Xml;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat;
using MattEland.Ani.Alfred.Chat.Aiml;
using MattEland.Ani.Alfred.Chat.Aiml.Utils;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Modules;
using MattEland.Ani.Alfred.Core.Modules.SysMonitor;
using MattEland.Ani.Alfred.Tests.Mocks;
using MattEland.Common;

using NUnit.Framework;

namespace MattEland.Ani.Alfred.Tests.Chat
{

    /// <summary>
    ///     An abstract class providing testing capabilities related to chats and commanding
    /// </summary>
    [SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    [SuppressMessage("ReSharper", "ExceptionNotDocumented")]
    public class ChatTestsBase
    {
        [NotNull]
        private TestAlfred _alfred;

        private IChatProvider _chat;
        private AlfredChatSubsystem _chatSubsystem;
        private AlfredCoreSubsystem _coreSubsystem;

        [NotNull]
        private SystemMonitoringSubsystem _sysSubsystem;

        [NotNull]
        private TestSubsystem _testSubsystem;

        [NotNull]
        private User _user;

        [NotNull]
        public User User
        {
            [DebuggerStepThrough]
            get
            {
                return _user;
            }
        }

        public AlfredCoreSubsystem CoreSubsystem
        {
            [DebuggerStepThrough]
            get
            {
                return _coreSubsystem;
            }
        }

        public AlfredChatSubsystem ChatSubsystem
        {
            [DebuggerStepThrough]
            get
            {
                return _chatSubsystem;
            }
        }

        /// <summary>
        ///     Gets or sets the chat engine.
        /// </summary>
        /// <value>The chat engine.</value>
        [NotNull]
        protected ChatEngine Engine { get; set; }

        /// <summary>
        ///     Gets the chat provider.
        /// </summary>
        /// <value>The chat provider.</value>
        public IChatProvider Chat
        {
            [DebuggerStepThrough]
            get
            {
                return _chat;
            }
        }

        /// <summary>
        ///     Gets the alfred framework.
        /// </summary>
        /// <value>The alfred framework.</value>
        [NotNull]
        protected TestAlfred Alfred
        {
            get { return _alfred; }
        }

        [NotNull]
        public TestSubsystem TestSubsystem
        {
            [DebuggerStepThrough]
            get
            {
                return _testSubsystem;
            }
        }

        [NotNull]
        public SystemMonitoringSubsystem SysSubsystem
        {
            [DebuggerStepThrough]
            get
            {
                return _sysSubsystem;
            }
        }

        /// <summary>
        ///     Asserts that the chat input gets a reply template with the specified ID.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="templateId">The template identifier.</param>
        protected void AssertMessageGetsReplyTemplate(string input, string templateId)
        {
            var template = GetReplyTemplate(input);

            AssertTemplateId(template, templateId);
        }

        /// <summary>
        ///     Asserts that the template identifier is found in the response template.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <param name="id">The template identifier.</param>
        private static void AssertTemplateId([NotNull] string template, [NotNull] string id)
        {
            var idString = $"id=\"{id.ToLowerInvariant()}\"";

            Assert.IsTrue(template.ToLowerInvariant().Contains(idString),
                          $"ID '{idString}' was not found. Template was: {template}");
        }

        /// <summary>
        ///     Gets a reply from the chat system on a specified inquiry.
        /// </summary>
        /// <param name="text">The inquiry text.</param>
        /// <returns>The reply</returns>
        [NotNull]
        protected string GetReply([NotNull] string text)
        {
            var response = GetResponse(text);

            Assert.IsNotNull(response.ResponseText, "Response text was null");

            return response.ResponseText;
        }

        /// <summary>
        ///     Says the specified message to Alfred.
        /// </summary>
        /// <param name="message">The message.</param>
        protected void Say([NotNull] string message)
        {
            GetResponse(message);
        }

        /// <summary>
        ///     Gets the response to the chat message.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>The response</returns>
        private UserStatementResponse GetResponse([NotNull] string text)
        {

            var chatProvider = _alfred.ChatProvider;
            Assert.NotNull(chatProvider,
                           "Alfred's chat provider was null when instructed to handle chat message");

            var response = chatProvider.HandleUserStatement(text);

            // Do some basic validity checks
            Assert.NotNull(response, "Response was null");
            Assert.AreEqual(text, response.UserInput);
            return response;
        }

        /// <summary>
        ///     Gets a reply from the chat system on a specified inquiry.
        /// </summary>
        /// <param name="text">The inquiry text.</param>
        /// <returns>The reply</returns>
        [CanBeNull]
        protected string GetReplyTemplate([CanBeNull] string text)
        {
            var response = GetResponse(text);

            Assert.IsNotNull(response, "Alfred's response was null");
            return response.Template;
        }

        /// <summary>
        ///     Initializes the chat system.
        /// </summary>
        protected void InitChatSystem()
        {
            AlfredTestTagHandler.WasInvoked = false;

            _alfred = new TestAlfred();

            // Add Subsystems to alfred

            _coreSubsystem = new AlfredCoreSubsystem(_alfred.PlatformProvider);
            _alfred.Register(_coreSubsystem);

            _chatSubsystem = new AlfredChatSubsystem(_alfred.PlatformProvider, _alfred.Console);
            _alfred.Register(_chatSubsystem);

            _testSubsystem = new TestSubsystem(_alfred.PlatformProvider);
            _alfred.Register(_testSubsystem);

            _sysSubsystem = new SystemMonitoringSubsystem(_alfred.PlatformProvider);
            _alfred.Register(_sysSubsystem);

            // Store Chat Handler Details
            var chatHandler = _chatSubsystem.ChatHandler;
            _chat = chatHandler;

            // Store Engine
            Assert.IsNotNull(chatHandler.ChatEngine, "Chat Engine was null");
            Engine = chatHandler.ChatEngine;
            Engine.LoadAimlFromString(Resources.AimlTestAssets.NonNull());
            _user = new User("Test User", Engine);

            // Start Alfred - doesn't make sense to have a chat test that doesn't need Alfred online first.
            _alfred.Initialize();
            _alfred.Update();
        }

        [NotNull]
        protected TagHandlerParameters BuildTagHandlerParameters(string xml)
        {
            var node = AimlTagHandler.BuildNode(xml);
            return BuildTagHandlerParameters("Testing is fun", node);
        }

        [NotNull]
        private TagHandlerParameters BuildTagHandlerParameters(string input, XmlNode node)
        {
            var query = new SubQuery(input);
            var request = new Request(input, User, Engine);
            var result = new Result(User, Engine, request);
            return new TagHandlerParameters(Engine, User, query, request, result, node);
        }
    }

}