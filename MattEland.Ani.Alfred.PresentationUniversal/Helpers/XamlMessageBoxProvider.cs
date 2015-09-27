// ---------------------------------------------------------
// XamlMessageBoxProvider.cs
// 
// Created on:      09/05/2015 at 3:04 PM
// Last Modified:   09/05/2015 at 3:04 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using MattEland.Ani.Alfred.Core.Definitions;
using System.Diagnostics.CodeAnalysis;
using Windows.UI.Popups;

namespace MattEland.Ani.Alfred.PresentationUniversal.Helpers
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
        /// <param name="buttons"> The buttons to show. </param>
        /// <param name="icon"> The icon to show. </param>
        [SuppressMessage("CodeAnalysis", "CS4014")]
        public override void Show(
            string message,
            string caption,
            MessageBoxType type)
        {
            var dialog = new MessageDialog(message, caption);
            dialog.ShowAsync();
        }
    }
}