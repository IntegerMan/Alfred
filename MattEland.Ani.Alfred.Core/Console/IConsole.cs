// ---------------------------------------------------------
// IConsole.cs
// 
// Created on:      08/09/2015 at 6:30 PM
// Last Modified:   08/09/2015 at 6:30 PM
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
        IEnumerable<IConsoleEvent> Events { get; }

        /// <summary>
        ///     Logs the specified message to the console.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="message">The message.</param>
        /// <param name="level">The logging level.</param>
        void Log([NotNull] string title, [NotNull] string message, LogLevel level);
    }
}