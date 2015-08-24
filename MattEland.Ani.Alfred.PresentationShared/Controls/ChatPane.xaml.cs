// ---------------------------------------------------------
// ChatPane.xaml.cs
// 
// Created on:      08/20/2015 at 8:14 PM
// Last Modified:   08/24/2015 at 6:21 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;

using MattEland.Ani.Alfred.Core.Definitions;

namespace MattEland.Ani.Alfred.PresentationShared.Controls
{
    /// <summary>
    ///     Interaction logic for ChatPane.xaml
    /// </summary>
    public sealed partial class ChatPane
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
            // Validate input
            var text = InputText.Text;
            if (string.IsNullOrWhiteSpace(text))
            {
                MessageBox.Show("Please type a message before hitting send.",
                                "No Message",
                                MessageBoxButton.OK,
                                MessageBoxImage.Asterisk);
                return;
            }

            // Send it to the page object (which will route it through to the chat subsystem)
            var chatHandler = (IChatProvider)DataContext;
            chatHandler.HandleUserStatement(text.Trim());

            // Clear out the input for the next time around
            InputText.Text = string.Empty;
            InputText.Focus();
        }
    }
}