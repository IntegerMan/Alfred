﻿using System.ComponentModel;
using System.Windows;

using MattEland.Ani.Alfred.Core;

namespace MattEland.Ani.Alfred.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly AlfredProvider _alfred;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            // Create the console first to log what we're doing
            var console = new WinClientConsole();
            console.Log("WinClient.Initialize", "Console is now online.");

            // Create Alfred. It won't be online and running yet, but create it.
            _alfred = new AlfredProvider
            {
                Console = console
            };
            console.Log("WinClient.Initialize", "Alfred instantiated");

            // Data bindings in the UI rely on Alfred
            this.DataContext = _alfred;
        }

        /// <summary>
        /// Handles the <see cref="E:WindowLoaded" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            // Tell Alfred that everything is configured and ready to go. Doing this after window loaded (vs in constructor) will help reduce UI latency
            _alfred.Initialize();
        }

        /// <summary>
        /// Handles the <see cref="E:ExitClicked" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void OnExitClicked(object sender, RoutedEventArgs e)
        {
            // TODO: A command based model would be better
            this.Close();
        }

        /// <summary>
        /// Handles the <see cref="E:WindowClosing" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="CancelEventArgs"/> instance containing the event data.</param>
        private void OnWindowClosing(object sender, CancelEventArgs e)
        {
            _alfred.Shutdown();
        }
    }
}