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
using MattEland.Ani.Alfred.Chat.Aiml.TagHandlers;
using MattEland.Ani.Alfred.Chat.Aiml.Utils;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Modules;
using MattEland.Ani.Alfred.Core.Modules.SysMonitor;
using MattEland.Ani.Alfred.Core.Subsystems;
using MattEland.Ani.Alfred.Tests.Mocks;
using MattEland.Common;
using MattEland.Common.Providers;

using NUnit.Framework;

namespace MattEland.Ani.Alfred.Tests.Chat
{

    /// <summary>
    ///     An <c>abstract</c> class providing testing capabilities related to chats and commanding
    /// </summary>
    [SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    [SuppressMessage("ReSharper", "ExceptionNotDocumented")]
    public class ChatTestsBase : AlfredTestBase
    {
        [NotNull]
        private TestAlfred _alfred;

        private IChatProvider _chat;

        [NotNull]
        private SystemMonitoringSubsystem _sysSubsystem;

        [NotNull]
        private TestSubsystem _testSubsystem;

        [NotNull]
        public TestShell Shell
        {
            [DebuggerStepThrough]
            get;
            private set;
        }

        [NotNull]
        public ValueMetricProviderFactory MetricProviderFactory
        {
            [DebuggerStepThrough]
            get;
            private set;
        }

        [NotNull]
        public User User
        {
            [DebuggerStepThrough]
            get;
            private set;
        }

        public AlfredCoreSubsystem CoreSubsystem
        {
            [DebuggerStepThrough]
            get;
            private set;
        }

        public ChatSubsystem ChatSubsystem
        {
            [DebuggerStepThrough]
            get;
            private set;
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
        ///     Gets the Alfred framework.
        /// </summary>
        /// <value>The aAfred framework.</value>
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
        protected void AssertMessageGetsReplyTemplate([NotNull] string input, [NotNull] string templateId)
        {
            var template = GetReplyTemplate(input);

            AssertTemplateId(template, templateId);
        }

        /// <summary>
        ///     Asserts that the template identifier is found in the response template.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <param name="id">The template identifier.</param>
        [SuppressMessage("ReSharper", "UnusedParameter.Local")]
        private static void AssertTemplateId([CanBeNull] string template, [NotNull] string id)
        {
            var idString = $"id=\"{id.ToLowerInvariant()}\"";

            Assert.IsTrue(template != null && template.ToLowerInvariant().Contains(idString),
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
        /// <param name="text">The chat message.</param>
        /// <returns>The reply</returns>
        [CanBeNull]
        protected string GetReplyTemplate([NotNull] string text)
        {
            var response = GetResponse(text);

            Assert.IsNotNull(response, "Alfred's response was null");
            return response.Template;
        }

        /// <summary>
        ///     Initializes the chat system.
        /// </summary>
        protected void InitializeChatSystem()
        {
            AlfredTestTagHandler.WasInvoked = false;

            _alfred = new TestAlfred();

            // Add Subsystems to Alfred

            CoreSubsystem = new AlfredCoreSubsystem(CommonProvider.Container, _alfred.PlatformProvider);
            _alfred.Register(CoreSubsystem);

            ChatSubsystem = new ChatSubsystem(Container, _alfred.Console, "Alfredo");
            _alfred.Register(ChatSubsystem);

            _testSubsystem = new TestSubsystem(Container, _alfred.PlatformProvider);
            _alfred.Register(_testSubsystem);

            MetricProviderFactory = new ValueMetricProviderFactory();
            _sysSubsystem = new SystemMonitoringSubsystem(Container, _alfred.PlatformProvider, MetricProviderFactory);
            _alfred.Register(_sysSubsystem);

            // Store Chat Handler Details
            var chatHandler = ChatSubsystem.ChatHandler;
            _chat = chatHandler;

            // Store Engine
            Assert.IsNotNull(chatHandler.ChatEngine, "Chat Engine was null");
            Engine = chatHandler.ChatEngine;
            Engine.LoadAimlFromString(Resources.AimlTestAssets.NonNull());
            User = new User("Test User");

            // Set up a shell handler to respond to events
            Shell = new TestShell();
            _alfred.Register(Shell);

            // Start Alfred - doesn't make sense to have a chat test that doesn't need Alfred online first.
            _alfred.Initialize();
            _alfred.Update();
        }

        /// <summary>
        /// Builds the tag handler parameters.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <returns>The <see cref="TagHandlerParameters"/>.</returns>
        [NotNull]
        protected TagHandlerParameters BuildTagHandlerParameters([NotNull] string xml)
        {
            var element = AimlTagHandler.BuildElement(xml);
            return BuildTagHandlerParameters("Testing is fun", element);
        }

        /// <summary>
        /// Builds the tag handler parameters.
        /// </summary>
        /// <param name="input">The input text.</param>
        /// <param name="element">The element.</param>
        /// <returns>Parameters for these elements.</returns>
        [NotNull]
        private TagHandlerParameters BuildTagHandlerParameters([NotNull] string input, [NotNull] XmlElement element)
        {
            var query = new SubQuery();
            var request = new Request(input, User, Engine);
            var result = new Result(User, Engine, request);
            return new TagHandlerParameters(Engine, User, query, request, result, element);
        }

        /// <summary>
        /// Builds a strongly typed tag handler for the given tag name from the given XML using a Tag Handler Factory and dynamic creation.
        /// </summary>
        /// <typeparam name="T">The type of tag handler</typeparam>
        /// <param name="tagName">Name of the tag.</param>
        /// <param name="xml">The XML.</param>
        /// <returns>The strongly typed tag handler.</returns>
        [NotNull]
        protected T BuildTagHandler<T>([NotNull] string tagName, [NotNull] string xml) where T : AimlTagHandler
        {
            var factory = new TagHandlerFactory(Engine);

            var dynamicHandler = factory.BuildTagHandlerDynamic(tagName, BuildTagHandlerParameters(xml));
            Assert.IsNotNull(dynamicHandler);

            var typeHandler = (T)dynamicHandler;
            Assert.IsNotNull(typeHandler);

            return typeHandler;
        }
    }

}