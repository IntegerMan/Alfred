// ---------------------------------------------------------
// XamlMessageBoxProvider.cs
// 
// Created on:      09/05/2015 at 3:04 PM
// Last Modified:   09/05/2015 at 3:04 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using MattEland.Ani.Alfred.Core.Definitions;
using System.Windows;

namespace MattEland.Ani.Alfred.PresentationAvalon.Helpers
{

    /// <summary>
    ///     An XAML message box provider.
    /// </summary>
    internal sealed class XamlMessageBoxProvider : MessageBoxProviderBase
    {
        /// <summary>
        ///     Shows a message box.
        /// </summary>
        /// <param name="message"> The message. </param>
        /// <param name="caption"> The message caption. </param>
        /// <param name="alertType"> Type of the alert. </param>
        public override void Show(
            string message,
            string caption,
            MessageBoxType alertType)
        {
            switch (alertType)
            {
                case MessageBoxType.Notification:
                    MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Information);
                    break;

                case MessageBoxType.Warning:
                    MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Warning);
                    break;

                case MessageBoxType.Error:
                    MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Error);
                    break;

                default:
                    MessageBox.Show(message, caption);
                    break;
            }
        }
    }
}