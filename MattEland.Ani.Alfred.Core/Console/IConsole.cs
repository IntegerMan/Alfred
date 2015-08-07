// ---------------------------------------------------------
// IConsole.cs
// 
// Created on:      07/26/2015 at 2:23 PM
// Last Modified:   08/07/2015 at 12:37 AM
// Original author: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;

using JetBrains.Annotations;

namespace MattEland.Ani.Alfred.Core.Console
{
    /// <summary>
    ///     An interface describing the display console Alfred can interact with
    /// </summary>
    public interface IConsole
    {
        /// <summary>
        ///     Gets the console events.
        /// </summary>
        /// <value>The console events.</value>
        [NotNull]
        [ItemNotNull]
        IEnumerable<ConsoleEvent> Events { get; }

        /// <summary>
        ///     Logs the specified message to the console.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="message">The message.</param>
        /// <param name="level">The logging level.</param>
        void Log([NotNull] string title, [NotNull] string message, LogLevel level);
    }
}