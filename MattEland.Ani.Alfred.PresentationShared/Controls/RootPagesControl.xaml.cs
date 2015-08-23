// ---------------------------------------------------------
// RootPagesControl.xaml.cs
// 
// Created on:      08/23/2015 at 12:52 PM
// Last Modified:   08/23/2015 at 12:57 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Diagnostics;
using System.Windows;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.PresentationShared.Commands;
using MattEland.Ani.Alfred.PresentationShared.Helpers;
using MattEland.Common;

namespace MattEland.Ani.Alfred.PresentationShared.Controls
{
    /// <summary>
    ///     The Root Pages Control contains a <see cref="TabControl" /> with all of Alfred's Root Pages.
    /// </summary>
    public partial class RootPagesControl
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RootPagesControl" /> class.
        /// </summary>
        public RootPagesControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RootPagesControl"/> class.
        /// </summary>
        /// <param name="app">The application manager.</param>
        public RootPagesControl([CanBeNull] ApplicationManager app) : this()
        {
            AppManager = app;
        }

        [CanBeNull]
        private ApplicationManager _appManager;

        /// <summary>
        /// Gets or sets the application manager. This in turn sets the DataContext and the
        ///  ItemsSource property of the tab control.
        /// </summary>
        /// <value>The application manager.</value>
        [CanBeNull]
        public ApplicationManager AppManager
        {
            get
            { return _appManager; }
            set
            {
                _appManager = value;

                DataContext = value;

                if (_appManager != null)
                {
                    var pages = TabPages;
                    Debug.Assert(pages != null);

                    /* HACK: DataBinding doesn't fire quickly enough in VSIX so we have to set the 
                    collection manually. This also helps unit test this in scenarios where DataBinding
                    wouldn't occur. */

                    pages.ItemsSource = _appManager.Alfred.RootPages;

                    // Auto-Select the first tab
                    SelectFirstTab();
                }
            }
        }

        /// <summary>
        ///     Selects the first tab.
        /// </summary>
        public void SelectFirstTab()
        {
            // Auto-select the first tab if any tabs are present
            Debug.Assert(TabPages != null);
            SelectionHelper.SelectFirstItem(TabPages);
        }

        /// <summary>
        ///     Handles the control loaded event by auto-selecting the first tab.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        public void HandleControlLoaded([CanBeNull] object sender, [NotNull] RoutedEventArgs e)
        {
            SelectFirstTab();
        }

        /// <summary>
        /// Handles the page navigation command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool HandlePageNavigationCommand(ShellCommand command)
        {
            if (!command.Data.HasText() || TabPages == null)
            {
                return false;
            }

            return SelectionHelper.SelectItemById(TabPages, command.Data);
        }

        /// <summary>
        /// Simulates the loaded event for testing purposes.
        /// </summary>
        public void SimulateLoadedEvent()
        {
            HandleControlLoaded(null, new RoutedEventArgs(FrameworkElement.LoadedEvent, this));
        }
    }
}