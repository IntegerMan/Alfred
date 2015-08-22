// ---------------------------------------------------------
// AlfredToolWindowControl.xaml.cs
// 
// Created on:      08/20/2015 at 9:45 PM
// Last Modified:   08/21/2015 at 10:55 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.PresentationShared.Commands;
using MattEland.Ani.Alfred.PresentationShared.Helpers;
using MattEland.Common;

namespace MattEland.Ani.Alfred.VisualStudio
{

    /// <summary>
    ///     Interaction logic for AlfredToolWindowControl.
    /// </summary>
    public partial class AlfredToolWindowControl : IUserInterfaceDirector
    {
        [NotNull]
        private readonly ApplicationManager _app;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AlfredToolWindowControl" /> class.
        /// </summary>
        /// <exception cref="Exception">
        ///     If any exception was encountered during startup, it will be logged and
        ///     rethrown to the Visual Studio Host.
        /// </exception>
        [SuppressMessage("ReSharper", "ThrowingSystemException")]
        [SuppressMessage("ReSharper", "CatchAllClause")]
        public AlfredToolWindowControl()
        {
            try
            {
                InitializeComponent();

                _app = AlfredPackage.AlfredInstance;

                // DataBindings rely on Alfred presently as there hasn't been a need for a page ViewModel yet
                DataContext = _app;

                _app.Console?.Log(Properties.Resources.AlfredToolWindowControlInitializeLogHeader,
                                  Properties.Resources.AlfredToolWindowControlInitializeLogMessage,
                                  LogLevel.Verbose);
            }
            catch (Exception ex)
            {
                const string Caption = "Problem Starting Tool Window";
                var message = ex.BuildDetailsMessage();

#if DEBUG
                Debugger.Break();
                Debug.Fail(Caption, message);
#else
                MessageBox.Show(message,
                                Caption,
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
#endif

                // We shouldn't load the page in a bad state. Crash the application
                // ReSharper disable once HeuristicUnreachableCode
                throw;
            }
        }

        /// <summary>
        ///     Handles the page navigation command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Whether or not the command was handled</returns>
        public bool HandlePageNavigationCommand(ShellCommand command)
        {
            if (!command.Data.HasText() || tabPages == null)
            {
                return false;
            }

            return SelectionHelper.SelectItemById(tabPages, command.Data);
        }

        /// <summary>
        ///     Handles the <see cref="E:Loaded" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void OnWindowLoaded([CanBeNull] object sender, [NotNull] RoutedEventArgs e)
        {
            var logHeader = "VSClient.Loaded";

            // Auto-select the first tab if any tabs are present
            Debug.Assert(tabPages != null);
            SelectionHelper.SelectFirstItem(tabPages);

            // Log that we're good to go
            _app.Console?.Log(logHeader, "Window is now loaded", LogLevel.Info);
        }
    }
}