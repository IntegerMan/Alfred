// ---------------------------------------------------------
// AlfredToolWindowCommand.cs
// 
// Created on:      08/20/2015 at 9:45 PM
// Last Modified:   08/21/2015 at 11:26 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.ComponentModel.Design;

using JetBrains.Annotations;

using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace MattEland.Ani.Alfred.VisualStudio
{
    /// <summary>
    ///     Command handler
    /// </summary>
    internal sealed class AlfredToolWindowCommand
    {
        /// <summary>
        ///     Command ID.
        /// </summary>
        public const int CommandId = 0x0100;

        /// <summary>
        ///     Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("b5ad629d-c71f-4252-bf26-03961e6b2169");

        /// <summary>
        ///     VS Package that provides this command, not null.
        /// </summary>
        [NotNull]
        private readonly Package _package;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AlfredToolWindowCommand" /> class.
        ///     Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        private AlfredToolWindowCommand([NotNull] Package package)
        {
            if (package == null)
            {
                throw new ArgumentNullException(nameof(package));
            }

            _package = package;

            var commandService =
                ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService != null)
            {
                var menuCommandId = new CommandID(CommandSet, CommandId);
                var menuItem = new MenuCommand(ShowToolWindow, menuCommandId);
                commandService.AddCommand(menuItem);
            }
        }

        /// <summary>
        ///     Gets the instance of the command.
        /// </summary>
        [CanBeNull]
        public static AlfredToolWindowCommand Instance
        {
            [UsedImplicitly]
            get;
            private set;
        }

        /// <summary>
        ///     Gets the service provider from the owner package.
        /// </summary>
        [NotNull]
        private IServiceProvider ServiceProvider
        {
            get { return _package; }
        }

        /// <summary>
        ///     Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static void Initialize([NotNull] Package package)
        {
            Instance = new AlfredToolWindowCommand(package);
        }

        /// <summary>
        ///     Shows the tool window when the menu item is clicked.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event args.</param>
        private void ShowToolWindow([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            // Get the instance number 0 of this tool window. This window is single instance so this instance
            // is actually the only one.
            // The last flag is set to true so that if the tool window does not exists it will be created.
            var window = _package.FindToolWindow(typeof(AlfredToolWindow), 0, true);
            if (window?.Frame == null)
            {
                throw new NotSupportedException("Cannot create tool window");
            }

            var windowFrame = (IVsWindowFrame)window.Frame;
            ErrorHandler.ThrowOnFailure(windowFrame.Show());
        }
    }
}