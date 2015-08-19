// ---------------------------------------------------------
// WpfShellCommandManager.cs
// 
// Created on:      08/18/2015 at 11:11 PM
// Last Modified:   08/18/2015 at 11:11 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core;
using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.Core.Definitions;

namespace MattEland.Ani.Alfred.WPF
{
    /// <summary>
    /// The command manager for the WPF application
    /// </summary>
    public class WpfShellCommandManager : IShellCommandRecipient
    {
        [NotNull]
        private readonly MainWindow _window;

        [NotNull]
        private readonly AlfredApplication _alfred;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="window"/> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="alfred"/> is <see langword="null" />.</exception>
        public WpfShellCommandManager([NotNull] MainWindow window, [NotNull] AlfredApplication alfred)
        {
            if (window == null)
            {
                throw new ArgumentNullException(nameof(window));
            }
            if (alfred == null)
            {
                throw new ArgumentNullException(nameof(alfred));
            }

            _window = window;
            _alfred = alfred;
        }

        /// <summary>
        /// Processes a shell command by sending it on to the user interface layer.
        /// </summary>
        /// <param name="command">The command.</param>
        public void ProcessShellCommand(ShellCommand command)
        {
            _alfred.Console?.Log("WPF.ShellCommand", "Received shell command: " + command, LogLevel.Info);

            switch (command.Name.ToUpperInvariant())
            {
                case "NAV":
                case "NAVIGATE":
                case "NAVIGATION":
                    HandleNavigationCommand(command);
                    break;

            }
        }

        /// <summary>
        /// Handles a shell navigation command.
        /// </summary>
        /// <param name="command">The command.</param>
        private void HandleNavigationCommand(ShellCommand command)
        {
            switch (command.Target.ToUpperInvariant())
            {
                case "PAGES":
                    _window.HandlePageNavigationCommand(command);
                    break;
            }
        }
    }
}