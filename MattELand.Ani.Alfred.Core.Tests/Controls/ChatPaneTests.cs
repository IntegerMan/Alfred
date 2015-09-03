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
using MattEland.Ani.Alfred.Tests.Mocks;
using MattEland.Testing;

using NUnit.Framework;

namespace MattEland.Ani.Alfred.Tests.Controls
{
    /// <summary>
    ///     Tests related to <see cref="ChatPane" />
    /// </summary>
    [UnitTestProvider]
    [SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    [SuppressMessage("ReSharper", "IsExpressionAlwaysTrue")]
    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    public sealed class ChatPaneTests : UserInterfaceTestBase
    {

        /// <summary>
        ///     Sets up the test environment for each test.
        /// </summary>
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            _chatPane = new ChatPane();
            _chatProvider = new TestChatProvider();

            // Get the control ready for interaction
            InitializeControl(_chatPane);
        }

        private const string TestText = "Hello Test System";

        [NotNull]
        private ChatPane _chatPane;

        [NotNull]
        private TestChatProvider _chatProvider;

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
            _chatPane.SendChatMessage(_chatProvider, null);
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
        /// </remarks>
        [Test]
        [STAThread]
        public void ChatPaneShouldSendTextToProviderGivenValidText()
        {
            _chatPane.SendChatMessage(_chatProvider, TestText);

            Assert.AreEqual(TestText, _chatProvider.LastInput);
        }

        [Test]
        [STAThread]
        public void ChatPaneSubmitButtonCausesTextToBeSentToProvider()
        {
            _chatPane.InputText.Text = TestText;

            // Chat Pane sends to the chat provider in its data context
            _chatPane.DataContext = _chatProvider;

            _chatPane.HandleSubmitClicked();

            Assert.AreEqual(TestText, _chatProvider.LastInput);
        }
    }

}