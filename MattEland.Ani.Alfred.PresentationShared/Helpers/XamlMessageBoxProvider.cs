// ---------------------------------------------------------
// XamlMessageBoxProvider.cs
// 
// Created on:      09/05/2015 at 3:04 PM
// Last Modified:   09/05/2015 at 3:04 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Windows;

namespace MattEland.Ani.Alfred.PresentationShared.Helpers
{

    /// <summary>
    ///     An XAML message box provider.
    /// </summary>
    public sealed class XamlMessageBoxProvider : MessageBoxProviderBase
    {

        /// <summary>
        ///     Shows a message box.
        /// </summary>
        /// <param name="message"> The message. </param>
        /// <param name="caption"> The message caption. </param>
        /// <param name="buttons"> The buttons to show. </param>
        /// <param name="icon"> The icon to show. </param>
        public override void Show(
            string message,
            string caption,
            MessageBoxButton buttons,
            MessageBoxImage icon)
        {
            MessageBox.Show(message, caption, buttons, icon);
        }
    }
}