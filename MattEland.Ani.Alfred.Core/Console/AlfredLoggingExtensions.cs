// ---------------------------------------------------------
// AlfredExtensions.cs
// 
// Created on:      08/31/2015 at 11:11 AM
// Last Modified:   08/31/2015 at 11:11 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Diagnostics;

using MattEland.Common.Annotations;

using MattEland.Common.Providers;
using MattEland.Ani.Alfred.Core.Definitions;

namespace MattEland.Ani.Alfred.Core.Console
{
    /// <summary>
    ///     Extension methods for dealing with the Alfred framework
    /// </summary>
    public static class AlfredLoggingExtensions
    {

        /// <summary>
        ///     A string extension method that logs messages to the console.
        /// </summary>
        /// <param name="message"> The message to act on. </param>
        /// <param name="title"> The title. </param>
        /// <param name="level"> The level. </param>
        /// <param name="container"> The container. </param>
        public static void Log([CanBeNull] this string message, [CanBeNull] string title, LogLevel level, [CanBeNull] IAlfredContainer container)
        {
            // Grab the console from the container
            if (container == null)
            {
                container = AlfredContainerHelper.ProvideContainer();
            }
            var console = container.Console;

            // Use the other extension method to log it
            message.Log(title, level, console);
        }

        /// <summary>
        ///     A string extension method that logs messages to the <paramref name="console"/>.
        /// </summary>
        /// <param name="message"> The message to log. </param>
        /// <param name="title"> The title. </param>
        /// <param name="level"> The level. </param>
        /// <param name="console"> The console. </param>
        public static void Log(
            [CanBeNull] this string message,
            [CanBeNull] string title,
            LogLevel level,
            [CanBeNull] IConsole console)
        {
            if (console != null)
            {
                // Perform the logging
                console.Log(title, message, level);
            }
            else
            {
                Debug.WriteLine($"Could not log message: {title}: {message}");
            }
        }

        /// <summary>
        ///     A string extension method that shows an alert message box.
        /// </summary>
        /// <param name="message"> The message. </param>
        /// <param name="caption"> The caption. </param>
        /// <param name="container"> The container used for logging and message box provisions. </param>
        public static void ShowAlert(this string message, string caption, IAlfredContainer container)
        {
            // Log it. If we have a speech-enabled console, this will be spoken aloud.
            message.Log(caption, LogLevel.ChatNotification, container);

            // Use the message box provider (if one is present) to show the alert
            var provider = container.TryProvide<IMessageBoxProvider>();
            provider?.ShowAlert(message, caption);

        }
    }
}