// ---------------------------------------------------------
// ChatPaneTests.cs
// 
// Created on:      08/25/2015 at 10:00 AM
// Last Modified:   08/25/2015 at 10:44 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.PresentationShared.Controls;
using MattEland.Testing;

using Moq;

using NUnit.Framework;

using Shouldly;

namespace MattEland.Ani.Alfred.Tests.Controls
{
    /// <summary>
    ///     Tests related to <see cref="ChatPane" />
    /// </summary>
    [UnitTestProvider]
    [SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    [SuppressMessage("ReSharper", "IsExpressionAlwaysTrue")]
    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    [SuppressMessage("ReSharper", "ExceptionNotDocumented")]
    public sealed class ChatPaneTests : UserInterfaceTestBase
    {

        /// <summary>
        ///     Sets up the test environment for each test.
        /// </summary>
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            _chatPane = new ChatPane(Container);
            _chatMock = new Mock<IChatProvider>(MockBehavior.Strict);

            // Chat Pane gets the chat provider in its data context
            _chatPane.DataContext = _chatMock.Object;

            // Get the control ready for interaction
            InitializeControl(_chatPane);
        }

        private const string TestText = "Hello Test System";

        [NotNull]
        private ChatPane _chatPane;

        /// <summary>
        ///     The chat provider mock.
        /// </summary>
        [NotNull]
        private Mock<IChatProvider> _chatMock;

        /// <summary>
        ///     Ensures that a submit button is part of the chat pane.
        /// </summary>
        /// <remarks>
        ///     See feature ALF-46
        /// </remarks>
        [Test]
        [STAThread]
        public void ChatPaneHasSubmitButton()
        {
            var button = _chatPane.SubmitButton;
            AssertHasButton(button);
            Assert.AreEqual("Submit", button.Content);
        }

        /// <summary>
        ///     Ensures that a text box is part of the chat pane.
        /// </summary>
        /// <remarks>
        ///     See feature ALF-46
        /// </remarks>
        [Test]
        [STAThread]
        public void ChatPaneHasTextBox()
        {
            AssertHasTextBox(_chatPane.InputText);
        }

        /// <summary>
        ///     Ensures that chat will throw an <see cref="InvalidOperationException" /> when sending text to
        ///     no provider.
        /// </summary>
        /// <remarks>
        ///     <see cref="InvalidOperationException" /> will be handled in the UI via a message box.
        /// 
        ///     Test ALF-71 for feature ALF-46
        /// </remarks>
        [Test]
        [STAThread]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChatPaneNotShouldSendTextToProviderWhenNoTextIsPresent()
        {
            _chatPane.SendChatMessage(_chatMock.Object, null);
        }

        /// <summary>
        ///     Ensures that chat will throw an <see cref="InvalidOperationException" /> when sending text to
        ///     no provider.
        /// </summary>
        /// <remarks>
        ///     <see cref="InvalidOperationException" /> will be handled in the UI via a message box.
        /// 
        ///     See feature ALF-46
        /// </remarks>
        [Test]
        [STAThread]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChatPaneNotShouldSendTextWhenNoProviderIsPresent()
        {
            _chatPane.SendChatMessage(null, TestText);
        }

        /// <summary>
        ///     Ensures that chat will send text to its <see cref="IChatProvider" /> when both are specified.
        /// </summary>
        /// <remarks>
        ///     <see cref="InvalidOperationException" /> will be handled in the UI via a message box.
        /// 
        ///     Test ALF-70 for feature ALF-46
        /// 
        ///     This method is primarily testing the <see cref="ChatPane.SendChatMessage"/> method.
        /// </remarks>
        [Test]
        [STAThread]
        public void ChatPaneShouldSendTextToProviderGivenValidText()
        {
            //! Arrange
            ConfigureChatMockToReceiveText(TestText);

            //! Act
            _chatPane.SendChatMessage(_chatMock.Object, TestText);

            //! Assert
            _chatMock.Object.LastInput.ShouldBe(TestText);
            _chatMock.Verify(c => c.HandleUserStatement(TestText), Times.Once);
        }

        /// <summary>
        ///     Tests that when the chat pane's submit button is clicked and a chat provider and
        ///     input text is present, the chat provider is sent the inputted text.
        /// </summary>
        /// <remarks>
        ///     This method is effectively testing the <see cref="ChatPane.HandleSubmitClicked" /> method simulating a submit button click.
        /// </remarks>
        [Test]
        [STAThread]
        public void ChatPaneSubmitButtonCausesTextToBeSentToProvider()
        {
            //! Arrange
            _chatPane.InputText.Text = TestText;

            // HandleUserStatement must be called with the expected text
            ConfigureChatMockToReceiveText(TestText);

            //! Act
            _chatPane.HandleSubmitClicked();

            //! Assert
            _chatMock.Verify(c => c.HandleUserStatement(TestText), Times.Once);
            _chatMock.Object.LastInput.ShouldBe(TestText);
        }

        /// <summary>
        ///     Configures the chat provider mock to expect the text.
        /// </summary>
        /// <param name="input"> The input. </param>
        private void ConfigureChatMockToReceiveText(string input)
        {
            _chatMock.Setup(c => c.HandleUserStatement(It.Is<string>(i => i == input)))

                     // After the call, set up the LastInput property to return the input
                     .Callback(() => _chatMock.SetupGet(m => m.LastInput).Returns(input))

                     // HandleUserStatement returns a blank response (we're not testing the return value)
                     .Returns(new UserStatementResponse());
        }
    }

}