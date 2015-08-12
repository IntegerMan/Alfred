// ---------------------------------------------------------
// CommandDispatcher.cs
// 
// Created on:      08/12/2015 at 1:38 AM
// Last Modified:   08/12/2015 at 1:38 AM
// Original author: Matt Eland
// ---------------------------------------------------------

using System.Globalization;
using System.Linq;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.Core.Definitions;

namespace MattEland.Ani.Alfred.Core
{
    /// <summary>
    /// A utility class structured around routing UserStatementResponses containing commands to their intended 
    /// subsystems and to return the result of those operations to the caller
    /// </summary>
    internal static class CommandDispatcher
    {
        /// <summary>
        /// Handles a chat command by dispatching it to the appropriate subsystem(s) and returning the end result.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="alfred">The alfred.</param>
        /// <returns>A result of the chat activity</returns>
        [NotNull]
        internal static AlfredCommandResult HandleChatCommand(UserStatementResponse response, [NotNull] IAlfred alfred)
        {
            var command = response.Command;

            //- Do nothing with empty commands
            var defaultResult = BuildDefaultCommandResult(response);
            if (command == ChatCommand.Empty)
            {
                return defaultResult;
            }

            //- Extract and normalize the destination ID this message is intended for
            var dest = command.Subsystem?.ToUpperInvariant();

            //- Sanity check to ensure the tag specified a command
            if (string.IsNullOrWhiteSpace(command.Command))
            {
                alfred.Console?.Log("Chat.Routing",
                                    "Encountered a command without a command name. Ignoring.",
                                    LogLevel.Warning);

                return defaultResult;
            }

            // Route the command and get back results
            AlfredCommandResult result;
            if (string.IsNullOrWhiteSpace(dest) && dest != "ALL")
            {
                // If this is intended for a specific subsystem, only route it to that subsystem
                result = GetDirectedCommandResult(response, command, alfred);
            }
            else
            {
                // Otherwise this must be a broadcast message and should be sent to each subsystem in turn
                result = GetBroadcastCommandResult(response, command, alfred);
            }

            //- null values become default
            return result ?? defaultResult;
        }

        /// <summary>
        ///     Gets the result of a broadcast command.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="command">The command.</param>
        /// <param name="alfred">The alfred instance.</param>
        /// <returns>MattEland.Ani.Alfred.Core.AlfredCommandResult.</returns>
        private static AlfredCommandResult GetBroadcastCommandResult(UserStatementResponse response,
                                                                     ChatCommand command,
                                                                     [NotNull] IAlfred alfred)
        {
            // This wasn't intended for a particular system. Send it to each subsystem
            foreach (var subsystem in alfred.Subsystems)
            {
                // Make sure each subsystem gets a pristine result object.
                var result = BuildDefaultCommandResult(response);
                if (subsystem.HandleChatCommand(command, result))
                {
                    alfred.Console?.Log("Chat.Routing",
                                        string.Format(CultureInfo.CurrentCulture,
                                                      "Command {0} was handled by {1}",
                                                      command.Command,
                                                      subsystem.Name),
                                        LogLevel.Info);

                    return result;
                }
            }

            // No subsystem took it. Log and move on.
            alfred.Console?.Log("Chat.Routing",
                                string.Format(CultureInfo.CurrentCulture,
                                              "Command {0} went unhandled by all subsystems",
                                              command.Command),
                                LogLevel.Warning);
            return null;
        }

        /// <summary>
        ///     Gets the result of a command directed to a single subsystem.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="command">The command.</param>
        /// <param name="alfred">The Alfred instance</param>
        /// <returns>The result.</returns>
        [CanBeNull]
        private static AlfredCommandResult GetDirectedCommandResult(UserStatementResponse response,
                                                                    ChatCommand command,
                                                                    [NotNull] IAlfred alfred)
        {
            // Find the subsystem matching the one in the command
            var id = command.Subsystem?.ToUpperInvariant();
            var subsystem = alfred.Subsystems.FirstOrDefault(s => s.Id.ToUpperInvariant() == id);

            // It's possible to specify something bogus. Shield against that
            if (subsystem == null)
            {
                alfred.Console?.Log("Chat.Routing",
                                    "Could not find a target subsystem with an Id of " + id,
                                    LogLevel.Warning);

                return null;
            }

            // Send the command to the subsystem
            var result = BuildDefaultCommandResult(response);
            if (subsystem.HandleChatCommand(command, result))
            {
                alfred.Console?.Log("Chat.Routing",
                                    string.Format(CultureInfo.CurrentCulture,
                                                  "Command {0} was handled by {1}",
                                                  command.Command,
                                                  subsystem.Name),
                                    LogLevel.Info);

                return result;
            }

            // It's also possible that the subsystem didn't handle it.
            // In that case log it and we'll go with the defaults.
            alfred.Console?.Log("Chat.Routing",
                                string.Format(CultureInfo.CurrentCulture,
                                              "Command {0} was directed to {1} but went unhandled",
                                              command.Command,
                                              subsystem.Name),
                                LogLevel.Info);

            return null;
        }

        /// <summary>
        ///     Builds the default command result from a given response.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <returns>The command result.</returns>
        [NotNull]
        private static AlfredCommandResult BuildDefaultCommandResult(UserStatementResponse response)
        {
            return new AlfredCommandResult
            {
                NewLastInput = response.UserInput,
                NewLastOutput = response.ResponseText,
                RedirectToChat = null
            };
        }
    }
}