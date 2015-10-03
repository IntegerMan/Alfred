// ---------------------------------------------------------
// ShellCommandManager.cs
// 
// Created on:      09/01/2015 at 3:46 PM
// Last Modified:   09/01/2015 at 10:56 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core;
using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Common.Providers;
using MattEland.Common;

namespace MattEland.Ani.Alfred.PresentationAvalon.Commands
{
    /// <summary>
    ///     The command manager for the WPF/XAML application.
    /// </summary>
    public class ShellCommandManager : IShellCommandRecipient, IHasContainer<IAlfredContainer>
    {
        /// <summary>
        ///     Gets the Alfred instance.
        /// </summary>
        /// <value>
        ///     The Alfred instance.
        /// </value>
        [NotNull]
        private AlfredApplication Alfred { get; }

        /// <summary>
        ///     Gets the user interface director.
        /// </summary>
        /// <value>
        ///     The user interface director.
        /// </value>
        [NotNull]
        private IUserInterfaceDirector UIDirector { get; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="object" /> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="director" /> is <see langword="null" /> .
        /// </exception>
        /// <param name="container"> The container. </param>
        /// <param name="director"> The user <see langword="interface" /> director. </param>
        /// <param name="alfred"> The alfred instance. </param>
        internal ShellCommandManager(
            [NotNull] IAlfredContainer container,
            [NotNull] IUserInterfaceDirector director,
            [NotNull] AlfredApplication alfred)
        {
            //- Validate
            if (container == null) { throw new ArgumentNullException(nameof(container)); }
            if (director == null) { throw new ArgumentNullException(nameof(director)); }
            if (alfred == null) { throw new ArgumentNullException(nameof(alfred)); }

            Container = container;
            UIDirector = director;
            Alfred = alfred;
        }

        /// <summary>
        ///     Gets the container.
        /// </summary>
        /// <value>
        ///     The container.
        /// </value>
        public IAlfredContainer Container { get; }

        /// <summary>
        ///     Processes a shell <paramref name="command" /> by sending it on to the user
        ///     <see langword="interface" /> layer.
        /// </summary>
        /// <param name="command"> The command. </param>
        /// <returns>
        ///     A string.
        /// </returns>
        public string ProcessShellCommand(ShellCommand command)
        {
            var message = "Received shell command: " + command;
            message.Log("ShellCommand", LogLevel.Info, Container);

            switch (command.Name.ToUpperInvariant())
            {
                case "NAV":
                    return HandleNavigationCommand(command) ? "NAVIGATE SUCCESS" : "NAVIGATE FAILED";

                case "OPENWEBPAGE":

                    if (UIDirector != null && command.Data.HasText())
                    {
                        UIDirector.HandleWebPageRequested(command.Data);
                    }

                    return string.Empty;

                default:
                    return string.Empty;
            }

        }

        /// <summary>
        ///     Handles a shell navigation command.
        /// </summary>
        /// <param name="command"> The command. </param>
        /// <returns>
        ///     Whether or not the event was handled.
        /// </returns>
        private bool HandleNavigationCommand(ShellCommand command)
        {
            switch (command.Target.ToUpperInvariant())
            {
                case "PAGES":
                    return UIDirector.HandlePageNavigationCommand(command);

                default:
                    return false;
            }
        }
    }
}