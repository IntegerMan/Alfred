// ---------------------------------------------------------
// IConsoleEvent.cs
// 
// Created on:      08/09/2015 at 6:31 PM
// Last Modified:   08/09/2015 at 6:31 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;

using MattEland.Ani.Alfred.Core.Console;

namespace MattEland.Ani.Alfred.Core.Interfaces
{
    /// <summary>
    ///     Represents an event logged to the monitoring console
    /// </summary>
    public interface IConsoleEvent
    {
        /// <summary>
        /// Gets the title of the event.
        /// </summary>
        /// <value>The title.</value>
        string Title { get; }

        /// <summary>
        /// Gets the logging level.
        /// </summary>
        /// <value>The level.</value>
        LogLevel Level { get; }

        /// <summary>
        ///     Gets the time the event was logged in UTC.
        /// </summary>
        /// <value>The time the event was logged in UTC.</value>
        DateTime UtcTime { get; }

        /// <summary>
        ///     Gets the message.
        /// </summary>
        /// <value>The message.</value>
        string Message { get; }

        /// <summary>
        ///     Gets the time in local system time.
        /// </summary>
        /// <value>The time.</value>
        DateTime Time { get; }
    }
}