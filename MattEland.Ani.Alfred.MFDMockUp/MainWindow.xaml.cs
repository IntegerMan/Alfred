using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

using Assisticant;

using MattEland.Ani.Alfred.MFDMockUp.ViewModels;
using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [UsedImplicitly]
    public sealed partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     Event handler. Called by MainWindow for on loaded events.
        /// </summary>
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e"> Routed event information. </param>
        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            // The main data context is an assisticant view, so unwrap it
            var mainViewModel = ForView.Unwrap<MainViewModel>(DataContext);

            // Start the actual engine. This will boot up the MFDs.
            mainViewModel.StartApplication();
        }
    }
}
