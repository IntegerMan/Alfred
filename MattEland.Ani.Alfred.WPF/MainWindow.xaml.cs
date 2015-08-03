using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;

using MattEland.Ani.Alfred.Core;
using MattEland.Ani.Alfred.Core.Modules;

namespace MattEland.Ani.Alfred.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public sealed partial class MainWindow : Window
    {

        private const bool AutoInitialize = true;

        private readonly AlfredProvider _alfred;
        private readonly WinClientConsole _console = new WinClientConsole();

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            // Create the console first to log what we're doing
            _console.Log("WinClient.Initialize", "Console is now online.");

            // Create Alfred. It won't be online and running yet, but create it.
            _alfred = new AlfredProvider(new WinClientPlatformProvider())
            {
                Console = _console
            };

            StandardModuleProvider.AddStandardModules(_alfred);

            _console.Log("WinClient.Initialize", "Alfred instantiated");

            // Data bindings in the UI rely on Alfred
            this.DataContext = _alfred;

            // Set up the update timer
            var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            timer.Tick += OnTimerTick;
            timer.Start();

            if (AutoInitialize)
            {
                _alfred.Initialize();
            }

        }

        /// <summary>
        /// Handles the <see cref="E:TimerTick" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnTimerTick(object sender, EventArgs e)
        {
            // If Alfred is online, ask it to update its modules
            if (_alfred.Status == AlfredStatus.Online)
            {
                _alfred.Update();
            }
        }

        /// <summary>
        /// Handles the <see cref="E:WindowLoaded" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {

            // No longer doing anything. We might want to have Alfred auto-initialize here...

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
            // Make sure we clean up Alfred
            if (_alfred.Status != AlfredStatus.Offline)
            {
                _alfred.Shutdown();
            }
        }

        /// <summary>
        /// Handles the <see cref="E:InitializeClicked" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void OnInitializeClicked(object sender, RoutedEventArgs e)
        {
            // TODO: A command model would be better here.

            try
            {
                _alfred.Initialize();
            }
            catch (InvalidOperationException ex)
            {

                // This happens if Alfred is online already.

                MessageBox.Show(
                    ex.Message,
                    "Could not Initialize Alfred",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation);
            }
        }

        /// <summary>
        /// Handles the <see cref="E:ShutdownClicked" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void OnShutdownClicked(object sender, RoutedEventArgs e)
        {
            // TODO: A command model would be better here.

            try
            {
                _alfred.Shutdown();
            }
            catch (InvalidOperationException ex)
            {

                // This happens if Alfred is offline already.

                MessageBox.Show(
                    ex.Message,
                    "Could not Shut Down Alfred",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation);
            }

        }
    }
}