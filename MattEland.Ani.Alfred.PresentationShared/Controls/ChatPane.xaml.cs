// ---------------------------------------------------------
// ChatPane.xaml.cs
// 
// Created on:      08/20/2015 at 8:14 PM
// Last Modified:   08/25/2015 at 2:12 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Common;
using MattEland.Common.Providers;

namespace MattEland.Ani.Alfred.PresentationShared.Controls
{
    /// <summary>
    /// Interaction logic for ChatPane.xaml
    /// </summary>
    public sealed partial class ChatPane : IUserInterfaceTestable, IHasContainer
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ChatPane" /> class.
        /// </summary>
        public ChatPane() : this(CommonProvider.Container)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ChatPane"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <param name="container"> The container. </param>
        public ChatPane([NotNull] IObjectContainer container)
        {
            if (container == null) { throw new ArgumentNullException(nameof(container)); }

            Container = container;

            InitializeComponent();
        }

        /// <summary>
        ///     Gets the container.
        /// </summary>
        /// <value>
        ///     The container.
        /// </value>
        public IObjectContainer Container { get; }

        /// <summary>
        ///     Simulates the control's loaded event.
        /// </summary>
        public void SimulateLoadedEvent()
        {
        }

        /// <summary>
        ///     Handles the <see cref="E:SubmitClicked" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        private void OnSubmitClicked(object sender, RoutedEventArgs e)
        {
            HandleSubmitClicked();
        }

        /// <summary>
        ///     Handles the submit button click event by sending the current text to the chat provider.
        /// </summary>
        public void HandleSubmitClicked()
        {
            // Validate input
            var text = InputText?.Text;

            // Send it to the page object (which will route it through to the chat subsystem)
            var chatProvider = DataContext as IChatProvider;
            try
            {
                SendChatMessage(chatProvider, text);
            }
            catch (InvalidOperationException ex)
            {
                ex.Message.ShowAlert("Couldn't Send", Container);
            }
        }

        /// <summary>
        ///     Sends the chat message to the chat provider.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        ///     Thrown when no chat provider is present or no message was specified.
        /// </exception>
        /// <param name="chatProvider"> The chat handler. </param>
        /// <param name="text"> The text. </param>
        public void SendChatMessage([CanBeNull] IChatProvider chatProvider, [CanBeNull] string text)
        {
            if (text.IsEmpty())
            {
                throw new InvalidOperationException("Please type a message before hitting send.");
            }

            // Send the message (if we have a provider)
            if (chatProvider != null)
            {
                chatProvider.HandleUserStatement(text.NonNull().Trim());
            }
            else
            {
                throw new InvalidOperationException("No chat handler has been provided.");
            }

            // Clear out the input for the next time around
            Debug.Assert(InputText != null);
            InputText.Text = string.Empty;
            InputText.Focus();
        }
    }
}