// ---------------------------------------------------------
// IConsole.cs
// 
// Created on:      08/09/2015 at 6:30 PM
// Last Modified:   08/09/2015 at 6:30 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;

using MattEland.Common.Annotations;

using MattEland.Common.Providers;
using MattEland.Ani.Alfred.Core.Definitions;

namespace MattEland.Ani.Alfred.Core.Console
{
    /// <summary>
    ///     An interface describing the display console Alfred can interact with
    /// </summary>
    public interface IConsole : IHasContainer<IAlfredContainer>
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
        void Log([CanBeNull] string title, [CanBeNull] string message, LogLevel level);

        /// <summary>
        /// Gets the console event factory used for creating new events.
        /// </summary>
        /// <value>The console event factory.</value>
        [NotNull]
        ConsoleEventFactory EventFactory { get; }

        /// <summary>
        ///     Gets the number of events in the collection.
        /// </summary>
        /// <value>
        ///     The total number of events.
        /// </value>
        int EventCount { get; }

        /// <summary>
        ///     Clears all events from the log
        /// </summary>
        void Clear();
    }
}