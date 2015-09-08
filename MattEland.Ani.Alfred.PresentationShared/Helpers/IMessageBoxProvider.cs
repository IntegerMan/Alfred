// ---------------------------------------------------------
// IMessageBoxProvider.cs
// 
// Created on:      09/05/2015 at 2:58 PM
// Last Modified:   09/05/2015 at 2:58 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Windows;

namespace MattEland.Ani.Alfred.PresentationShared.Helpers
{
    /// <summary>
    ///     Defines a controller that manages message box behaviors.
    /// </summary>
    public interface IMessageBoxProvider
    {
        /// <summary>
        ///     Shows a message box with an okay button and a standard icon.
        /// </summary>
        /// <param name="message"> The message. </param>
        /// <param name="caption"> The message caption. </param>
        void Show(string message, string caption);

        /// <summary>
        ///     Shows a message box with a standard icon.
        /// </summary>
        /// <param name="message"> The message. </param>
        /// <param name="caption"> The message caption. </param>
        /// <param name="buttons"> The buttons to show. </param>
        void Show(string message, string caption, MessageBoxButton buttons);

        /// <summary>
        ///     Shows a message box.
        /// </summary>
        /// <param name="message"> The message. </param>
        /// <param name="caption"> The message caption. </param>
        /// <param name="buttons"> The buttons to show. </param>
        /// <param name="icon"> The icon to show. </param>
        void Show(string message, string caption, MessageBoxButton buttons, MessageBoxImage icon);
    }
}