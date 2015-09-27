// ---------------------------------------------------------
// IMessageBoxProvider.cs
// 
// Created on:      09/05/2015 at 2:58 PM
// Last Modified:   09/05/2015 at 2:58 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

namespace MattEland.Ani.Alfred.Core.Definitions
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
        ///     Shows an alert message box with an okay button and an exclamation icon.
        /// </summary>
        /// <param name="message"> The message. </param>
        /// <param name="caption"> The message caption. </param>
        void ShowAlert(string message, string caption);

        /// <summary>
        ///     Shows an alert message box.
        /// </summary>
        /// <param name="message"> The message. </param>
        /// <param name="caption"> The message caption. </param>
        /// <param name="alertType"> Type of the alert. </param>
        void Show(string message, string caption, MessageBoxType alertType);
    }
}