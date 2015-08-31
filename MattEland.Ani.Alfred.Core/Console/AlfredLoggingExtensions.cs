// ---------------------------------------------------------
// AlfredExtensions.cs
// 
// Created on:      08/31/2015 at 11:11 AM
// Last Modified:   08/31/2015 at 11:11 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;

using JetBrains.Annotations;

using MattEland.Common.Providers;

namespace MattEland.Ani.Alfred.Core.Console
{
    /// <summary>
    ///     Extension methods for dealing with the Alfred framework
    /// </summary>
    public static class AlfredLoggingExtensions
    {
        /// <summary>
        ///     Gets or sets the console used by the logging methods.
        /// </summary>
        /// <value>
        ///     The console.
        /// </value>
        private static IConsole Console { get; set; }

        /// <summary>
        ///     A string extension method that logs messages to the console.
        /// </summary>
        /// <param name="message"> The message to act on. </param>
        /// <param name="title"> The title. </param>
        /// <param name="level"> The level. </param>
        /// <param name="container"> The container. </param>
        public static void Log([CanBeNull] this string message, [CanBeNull] string title, LogLevel level, [CanBeNull] IObjectContainer container = null)
        {
            // If we don't have a console, we'll have to use the container to find one
            if (Console == null)
            {
                container = container ?? CommonProvider.Container;
                Console = container.Provide<IConsole>();
            }

            // Use the other extension method to log it
            message.Log(title, level, Console);
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
                // Save it for later to use in the variant that doesn't store the console.
                Console = console;

                // Perform the logging
                console.Log(title, message, level);
            }
        }
    }
}