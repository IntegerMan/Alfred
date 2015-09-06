// ---------------------------------------------------------
// AlfredCommandRouter.cs
// 
// Created on:      08/19/2015 at 9:31 PM
// Last Modified:   09/01/2015 at 6:11 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Globalization;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core;
using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Common;
using MattEland.Common.Providers;

namespace MattEland.Ani.Alfred.Chat
{
    /// <summary>
    ///     This class takes commands in and handles them by routing them to various components in an
    ///     Alfred application.
    /// </summary>
    internal class AlfredCommandRouter : IAlfredCommandRecipient, IShellCommandRecipient, IHasContainer
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AlfredCommandRouter" /> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <param name="container"> The container. </param>
        internal AlfredCommandRouter([NotNull] IObjectContainer container)
        {
            if (container == null) { throw new ArgumentNullException(nameof(container)); }

            Container = container;
        }

        /// <summary>
        ///     Gets the container.
        /// </summary>
        /// <value>
        ///     The container.
        /// </value>
        [NotNull]
        public IObjectContainer Container { get; }

        /// <summary>
        ///     Gets or sets the Alfred instance.
        /// </summary>
        /// <value>
        ///     The <see cref="AlfredCommandRouter.Alfred" /> instance.
        /// </value>
        [CanBeNull]
        public IAlfred Alfred { get; set; }

        /// <summary>
        ///     Processes a <see cref="ChatCommand" /> . If the <paramref name="command" /> is handled,
        ///     <paramref name="result" /> should be modified accordingly and the method should return true.
        ///     Returning <see langword="false" /> will not stop the message from being propagated.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="result">
        ///     The result. If the <paramref name="command" /> was handled, this should be
        ///     updated.
        /// </param>
        /// <returns>
        ///     <c>True</c> if the <paramref name="command" /> was handled; otherwise <c>false</c> .
        /// </returns>
        public bool ProcessAlfredCommand(ChatCommand command, ICommandResult result)
        {
            var alfred = Alfred;
            if (alfred == null) { return false; }

            // Extract our target for loop readability
            var target = command.Subsystem;

            // Commands are very important and need to be logged.
            LogCommand(command.Name, target, command.Data);

            // Send the command to each subsystem. These will in turn send it on to their pages and modules.
            foreach (var subsystem in alfred.Subsystems)
            {
                // If the command isn't for the subsystem, move on.
                if (target.HasText() && !target.Matches(subsystem.Id)) { continue; }

                // Send the command to the subsystem for routing. If it's handled, the subsystem will return true.
                if (subsystem.ProcessAlfredCommand(command, result)) { return true; }
            }

            return false;
        }

        /// <summary>
        ///     Processes a <see cref="ShellCommand" /> by sending it on to the user
        ///     <see langword="interface" /> layer.
        /// </summary>
        /// <param name="command"> The command. </param>
        /// <returns>
        ///     A string indicating the result of the command.
        /// </returns>
        [NotNull]
        public string ProcessShellCommand(ShellCommand command)
        {
            // Commands are very, very important and need to be logged.
            LogCommand(command.Name, command.Target, command.Data);

            var shell = Alfred?.ShellCommandHandler;

            var result = shell?.ProcessShellCommand(command);

            return result.NonNull();
        }

        /// <summary>
        ///     Logs a <see cref="ChatCommand" /> or <see cref="ShellCommand" /> .
        /// </summary>
        /// <param name="commandName"> Name of the command. </param>
        /// <param name="commandTarget"> The command target. </param>
        /// <param name="commandData">
        ///     Information describing additional parameters for the command.
        /// </param>
        private void LogCommand(
            [CanBeNull] string commandName,
            [CanBeNull] string commandTarget,
            [CanBeNull] string commandData)
        {
            var message = string.Format(CultureInfo.CurrentCulture,
                                        @"Command '{0}' raised for subsystem '{1}' with data value of '{2}'.",
                                        commandName,
                                        commandTarget.NonNull(),
                                        commandData);

            message.Log("CommandRouting", LogLevel.Info, Container);
        }
    }
}