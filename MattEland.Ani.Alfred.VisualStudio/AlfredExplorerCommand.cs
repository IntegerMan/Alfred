// ---------------------------------------------------------
// AlfredExplorerCommand.cs
// 
// Created on:      08/31/2015 at 12:08 AM
// Last Modified:   09/01/2015 at 2:00 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.ComponentModel.Design;
using System.Diagnostics.CodeAnalysis;

using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace MattEland.Ani.Alfred.VisualStudio
{
    /// <summary>Command handler</summary>
    internal sealed class AlfredExplorerCommand
    {
        /// <summary>Command ID.</summary>
        public const int CommandId = 256;

        /// <summary>Command menu group (command set GUID).</summary>
        public static readonly Guid CommandSet = new Guid("8d0c60a3-2cb9-474b-bf2a-800ea39a3eb7");

        /// <summary>VS Package that provides this command, not null.</summary>
        private readonly Package _package;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AlfredExplorerCommand" /> class. Adds our
        ///     command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        private AlfredExplorerCommand(Package package)
        {
            if (package == null) { throw new ArgumentNullException(nameof(package)); }

            this._package = package;

            var commandService =
                ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;

            if (commandService != null)
            {
                var menuCommandId = new CommandID(CommandSet, CommandId);
                var menuItem = new MenuCommand(ShowToolWindow, menuCommandId);

                commandService.AddCommand(menuItem);
            }
        }

        /// <summary>Gets the instance of the command.</summary>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [SuppressMessage("ReSharper", @"UnusedAutoPropertyAccessor.Global")]
        public static AlfredExplorerCommand Instance { get; private set; }

        /// <summary>Gets the service provider from the owner package.</summary>
        private IServiceProvider ServiceProvider
        {
            get { return _package; }
        }

        /// <summary>Initializes the singleton instance of the command.</summary>
        /// <param name="package">Owner package, not null.</param>
        public static void Initialize(Package package)
        {
            Instance = new AlfredExplorerCommand(package);
        }

        /// <summary>Shows the tool window when the menu item is clicked.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event args.</param>
        private void ShowToolWindow(object sender, EventArgs e)
        {
            // Get the instance number 0 of this tool window. This window is single instance so this instance
            // is actually the only one.
            // The last flag is set to true so that if the tool window does not exists it will be created.
            var window = _package.FindToolWindow(typeof(AlfredExplorer), 0, true);
            if (window?.Frame == null)
            {
                throw new NotSupportedException("Cannot create Alfred Explorer tool window");
            }

            var windowFrame = (IVsWindowFrame)window.Frame;
            ErrorHandler.ThrowOnFailure(windowFrame.Show());
        }
    }
}