// ---------------------------------------------------------
// IAlfredPage.cs
// 
// Created on:      08/09/2015 at 6:17 PM
// Last Modified:   08/09/2015 at 6:19 PM
// Original author: Matt Eland
// ---------------------------------------------------------

namespace MattEland.Ani.Alfred.Core.Definitions
{
    /// <summary>
    ///     Defines an alfred page
    /// </summary>
    public interface IAlfredPage : IAlfredComponent
    {
        /// <summary>
        ///     Gets a value indicating whether this page is root level.
        /// </summary>
        /// <value><c>true</c> if this page is root level; otherwise, <c>false</c>.</value>
        bool IsRootLevel { get; }

        /// <summary>
        ///     Handles a chat command that may be intended for this module.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="result">The default system response. This should be modified and returned.</param>
        /// <returns><c>true</c> if the command was handled, <c>false</c> otherwise.</returns>
        bool HandleChatCommand(ChatCommand command, AlfredCommandResult result);
    }
}