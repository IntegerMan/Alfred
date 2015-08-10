// ---------------------------------------------------------
// ChatPane.xaml.cs
// 
// Created on:      08/09/2015 at 11:57 PM
// Last Modified:   08/10/2015 at 12:31 AM
// Original author: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;

using MattEland.Ani.Alfred.Chat;

namespace MattEland.Ani.Alfred.WPF.Controls
{
    /// <summary>
    ///     Interaction logic for ChatPane.xaml
    /// </summary>
    public partial class ChatPane
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ChatPane" /> class.
        /// </summary>
        public ChatPane()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     Handles the <see cref="E:SubmitClicked" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        private void OnSubmitClicked(object sender, RoutedEventArgs e)
        {
            var chatPage = (ChatPage)DataContext;

            // Send it to the page object (which will route it through to the chat subsystem)
            var response = chatPage.HandleUserStatement(txtInput.Text.Trim());

            // If it was a success, we'll also want to clear out the input
            if (response != null && response.WasHandled)
            {
                txtInput.Text = string.Empty;
                txtInput.Focus();
            }

        }
    }
}