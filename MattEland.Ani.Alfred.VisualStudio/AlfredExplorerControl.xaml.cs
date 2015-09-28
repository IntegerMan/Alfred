//------------------------------------------------------------------------------
// <copyright file="AlfredExplorerControl.xaml.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Linq;
using System.Windows;

using MattEland.Ani.Alfred.Core.Pages;
using MattEland.Ani.Alfred.Core.Subsystems;
using MattEland.Ani.Alfred.PresentationAvalon.Commands;
using MattEland.Common;

namespace MattEland.Ani.Alfred.VisualStudio
{

    /// <summary>
    /// Interaction logic for AlfredExplorerControl.
    /// </summary>
    public partial class AlfredExplorerControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AlfredExplorerControl"/> class.
        /// </summary>
        public AlfredExplorerControl()
        {
            InitializeComponent();

            // Get the subsystem and bind to it
            var alfred = AlfredPackage.AlfredInstance.Alfred;
            var mind = alfred.Subsystems.FirstOrDefault(s => s.Id.Matches("Mind")) as MindExplorerSubsystem;
            var explorerPage = mind?.MindExplorerPage;

            // Set the page and its nodes into the data context
            DataContext = explorerPage;
            Explorer.RootNodes = explorerPage?.RootNodes;
        }

        /// <summary>
        ///     Handles the control's loaded event.
        /// </summary>
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e"> Event information to send to registered event handlers. </param>
        private void OnControlLoaded(object sender, RoutedEventArgs e)
        {
        }
    }
}