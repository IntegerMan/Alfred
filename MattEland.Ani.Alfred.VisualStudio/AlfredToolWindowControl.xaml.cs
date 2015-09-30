// ---------------------------------------------------------
// AlfredToolWindowControl.xaml.cs
// 
// Created on:      08/20/2015 at 9:45 PM
// Last Modified:   08/25/2015 at 5:45 PM
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
using MattEland.Ani.Alfred.PresentationAvalon.Commands;

using MattEland.Common;
using MattEland.Ani.Alfred.PresentationAvalon.Helpers;
using MattEland.Common.Providers;

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
                Debug.Fail(Caption, message);
#else
                var provider = CommonProvider.Provide<IMessageBoxProvider>();
                provider.Show(message,
                                Caption,
                                MessageBoxType.Error);
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
            if (!command.Data.HasText() || TabPages == null) { return false; }

            return SelectionHelper.SelectItemById(TabPages, command.Data);
        }

        /// <summary>
        /// Handles the event when a web page is requested.
        /// </summary>
        /// <param name="url">The URL that was requested.</param>
        public void HandleWebPageRequested(string url)
        {
            Process.Start(url);
        }
        /// <summary>
        ///     Handles the <see cref="E:Loaded" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void OnWindowLoaded([CanBeNull] object sender, [NotNull] RoutedEventArgs e)
        {
            const string LogHeader = "VSClient.Loaded";

            Debug.Assert(TabPages != null);

            /* HACK: For whatever reason, the binding for tabPages.ItemsSource doesn't
            kick in on the loaded event leaving this control with no selection or items
            to auto-select. If that's the case, forgo the binding and stick the 
            collection in there manually so we have an item that can be auto-selected. */

            if (!TabPages.HasItems && _app.Alfred.RootPages.Any())
            {
                TabPages.ItemsSource = _app.Alfred.RootPages;
            }

            // Auto-select the first tab if any tabs are present
            SelectionHelper.SelectFirstItem(TabPages);

            // Log that we're good to go
            _app.Console?.Log(LogHeader,
                              Properties.Resources.AlfredToolWindowWindowLoadedMessage,
                              LogLevel.Info);
        }
    }
}