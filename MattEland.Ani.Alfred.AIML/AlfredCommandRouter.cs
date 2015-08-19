// ---------------------------------------------------------
// AlfredCommandRouter.cs
// 
// Created on:      08/18/2015 at 12:27 AM
// Last Modified:   08/18/2015 at 1:25 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core;
using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Common;

namespace MattEland.Ani.Alfred.Chat
{
    /// <summary>
    ///     This class takes commands in and handles them by routing them to various components in an
    ///     Alfred application.
    /// </summary>
    public class AlfredCommandRouter : IAlfredCommandRecipient, IShellCommandRecipient
    {
        /// <summary>
        ///     Gets or sets the alfred instance.
        /// </summary>
        /// <value>The alfred instance.</value>
        [CanBeNull]
        public IAlfred Alfred { get; set; }

        /// <summary>
        ///     Processes an Alfred Command. If the command is handled, result should be modified accordingly
        ///     and the method should return true. Returning false will not stop the message from being
        ///     propagated.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="result">The result. If the command was handled, this should be updated.</param>
        /// <returns><c>True</c> if the command was handled; otherwise false.</returns>
        public bool ProcessAlfredCommand(ChatCommand command, AlfredCommandResult result)
        {
            var alfred = Alfred;
            if (alfred == null)
            {
                return false;
            }

            // Extract our target for loop readability
            var target = command.Subsystem;

            // Commands are very, very important and need to be logged.
            alfred.Console?.Log("CommandRouting",
                                "Command '" + command.Name + "' raised for subsystem '" + target +
                                "' with data value of '" + command.Data + "'.",
                                LogLevel.Info);

            // Send the command to each subsystem. These will in turn send it on to their pages and modules.
            foreach (var subsystem in alfred.Subsystems)
            {
                // If the command isn't for the subsystem, move on.
                if (target.HasText() && !target.Matches(subsystem.Id))
                {
                    continue;
                }

                // Send the command to the subsystem for routing. If it's handled, the subsystem will return true.
                if (subsystem.ProcessAlfredCommand(command, result))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Processes a shell command by sending it on to the user interface layer.
        /// </summary>
        /// <param name="command">The command.</param>
        public void ProcessShellCommand(ShellCommand command)
        {
            // Commands are very, very important and need to be logged.
            Alfred?.Console?.Log("CommandRouting",
                                "Shell Command '" + command.Name + "' raised targeting '" + command.Target +
                                "' with data value of '" + command.Data + "'.", LogLevel.Info);

            var shell = Alfred?.ShellCommandHandler;

            shell?.ProcessShellCommand(command);
        }
    }
}