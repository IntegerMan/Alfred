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
using MattEland.Ani.Alfred.Core.Modules.SysMonitor;
using MattEland.Ani.Alfred.Core.Subsystems;
using MattEland.Ani.Alfred.Tests.Mocks;
using MattEland.Common;
using MattEland.Common.Providers;

using Moq;

using NUnit.Framework;

namespace MattEland.Ani.Alfred.Tests.Chat
{

    /// <summary>
    ///     An <c>abstract</c> class providing testing capabilities related to chats and commanding
    /// </summary>
    [SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    [SuppressMessage("ReSharper", "ExceptionNotDocumented")]
    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    public class ChatTestsBase : AlfredTestBase
    {

        [NotNull]
        private SystemMonitoringSubsystem _sysSubsystem;

        [NotNull]
        private IAlfredSubsystem _testSubsystem;

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

            var chatProvider = Alfred.ChatProvider;
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
        protected void InitializeChatSystem(IShellCommandRecipient shell = null)
        {
            AlfredTestTagHandler.WasInvoked = false;

            // Create Alfred and make it so the tests can find him via the Container
            var alfred = new TestAlfred(Container);
            alfred.RegisterAsProvidedInstance(Container);
            alfred.RegisterAsProvidedInstance(typeof(IAlfred), Container);

            // Add Subsystems to Alfred
            CoreSubsystem = new AlfredCoreSubsystem(Container);
            alfred.Register(CoreSubsystem);

            ChatSubsystem = new ChatSubsystem(Container, "Alfredo");
            alfred.Register(ChatSubsystem);

            var mock = BuildMockSubsystem(MockBehavior.Loose);
            _testSubsystem = mock.Object;
            alfred.Register(_testSubsystem);

            _sysSubsystem = new SystemMonitoringSubsystem(Container);
            alfred.Register(_sysSubsystem);

            MetricProviderFactory = Container.Provide<ValueMetricProviderFactory>();

            // Store Chat Handler Details
            var chatHandler = ChatSubsystem.ChatHandler;

            // Store Engine
            Assert.IsNotNull(chatHandler.ChatEngine, "Chat Engine was null");
            Engine = chatHandler.ChatEngine;
            Engine.LoadAimlFromString(Resources.AimlTestAssets.NonNull());
            User = new User("Test User");

            if (shell != null)
            {
                alfred.Register(shell);
            }

            // Start Alfred - doesn't make sense to have a chat test that doesn't need Alfred online first.
            alfred.Initialize();
            alfred.Update();
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
            var result = new ChatResult(User, Engine, request);
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