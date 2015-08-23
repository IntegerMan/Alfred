// ---------------------------------------------------------
// RootPagesControl.xaml.cs
// 
// Created on:      08/23/2015 at 12:52 PM
// Last Modified:   08/23/2015 at 12:57 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;
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
        private void HandleControlLoaded([CanBeNull] object sender, [NotNull] RoutedEventArgs e)
        {
            SelectFirstTab();
        }

        public bool HandlePageNavigationCommand(ShellCommand command)
        {
            if (!command.Data.HasText() || TabPages == null)
            {
                return false;
            }

            return SelectionHelper.SelectItemById(TabPages, command.Data);
        }
    }
}