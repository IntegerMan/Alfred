// ---------------------------------------------------------
// IAlfredModule.cs
// 
// Created on:      08/09/2015 at 6:17 PM
// Last Modified:   08/09/2015 at 6:19 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;

using MattEland.Ani.Alfred.Core.Widgets;

namespace MattEland.Ani.Alfred.Core.Definitions
{
    /// <summary>
    ///     Represents a module belonging to a page or subsystem in Alfred.
    /// </summary>
    /// <remarks>
    ///     TODO: This is a marker interface at present. I'd like to see some methods or reasons not to just use
    ///     IAlfredComponent
    /// </remarks>
    public interface IAlfredModule : IAlfredComponent
    {
        /// <summary>
        ///     Gets the user interface widgets for the module.
        /// </summary>
        /// <value>The user interface widgets.</value>
        IEnumerable<AlfredWidget> Widgets { get; }

        /// <summary>
        ///     Handles a chat command that may be intended for this module.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="result">The default system response. This should be modified and returned.</param>
        /// <returns><c>true</c> if the command was handled, <c>false</c> otherwise.</returns>
        bool HandleChatCommand(ChatCommand command, AlfredCommandResult result);
    }
}