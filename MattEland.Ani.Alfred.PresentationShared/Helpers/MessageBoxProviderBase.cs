// ---------------------------------------------------------
// MessageBoxProviderBase.cs
// 
// Created on:      09/05/2015 at 3:07 PM
// Last Modified:   09/05/2015 at 3:07 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Windows;

namespace MattEland.Ani.Alfred.PresentationShared.Helpers
{
    /// <summary>
    ///     A message box provider base class implementation that cascades all message handlers to
    ///     the detailed message box method.
    /// </summary>
    public abstract class MessageBoxProviderBase : IMessageBoxProvider
    {
        /// <summary>
        ///     Shows a <paramref name="message"/> box with an okay button and a standard icon.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="caption">The <paramref name="message"/> caption.</param>
        public void Show(string message, string caption)
        {
            Show(message, caption, MessageBoxButton.OK, MessageBoxImage.None);
        }

        /// <summary>
        ///     Shows a <paramref name="message"/> box with a standard icon.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="caption">The <paramref name="message"/> caption.</param>
        /// <param name="buttons">The buttons to show.</param>
        public void Show(string message, string caption, MessageBoxButton buttons)
        {
            Show(message, caption, buttons, MessageBoxImage.None);
        }

        /// <summary>
        ///     Shows a <paramref name="message"/> box.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="caption">The <paramref name="message"/> caption.</param>
        /// <param name="buttons">The buttons to show.</param>
        /// <param name="icon">The icon to show.</param>
        public abstract void Show(
            string message,
            string caption,
            MessageBoxButton buttons,
            MessageBoxImage icon);
    }
}