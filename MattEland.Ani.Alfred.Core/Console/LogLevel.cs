// ---------------------------------------------------------
// LogLevel.cs
// 
// Created on:      08/07/2015 at 12:19 AM
// Last Modified:   08/07/2015 at 12:19 AM
// Original author: Matt Eland
// ---------------------------------------------------------

namespace MattEland.Ani.Alfred.Core.Console
{
    /// <summary>
    ///     Represents the logging level of a console event
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        ///     Verbose logging intended for troubleshooting only
        /// </summary>
        Verbose,

        /// <summary>
        ///     Informational data intended to be elevated to user attention
        /// </summary>
        Info,

        /// <summary>
        ///     Warning messages intended to be raised to the user's attention
        /// </summary>
        Warning,

        /// <summary>
        ///     Error messages intended to be raised to the user's attention
        /// </summary>
        Error,

        /// <summary>
        /// Represents user input into the system
        /// </summary>
        UserInput,

        /// <summary>
        /// A system chat response to the user, intended to be spoken aloud or displayed prominently.
        /// </summary>
        ChatResponse
    }
}