// ---------------------------------------------------------
// MatchState.cs
// 
// Created on:      08/12/2015 at 10:27 PM
// Last Modified:   08/13/2015 at 2:47 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

namespace MattEland.Ani.Alfred.Chat.Aiml.Utils
{
    /// <summary>
    ///     The Match State
    /// </summary>
    public enum MatchState
    {
        /// <summary>
        ///     Represents user input
        /// </summary>
        UserInput,

        /// <summary>
        ///     Represents a "That" node
        /// </summary>
        That,

        /// <summary>
        ///     Represents an Aiml topic
        /// </summary>
        Topic
    }
}