//------------------------------------------------------------------------------
// <copyright file="AlfredExplorerControl.xaml.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Windows;

using MattEland.Ani.Alfred.Core.Subsystems;

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

            // Grab the Container
            var container = AlfredPackage.AlfredInstance.Container;

            // Grab the subsystem and bind the page to it
            var mindExplorer = container.Provide<MindExplorerSubsystem>();
            DataContext = mindExplorer.MindExplorerPage;
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