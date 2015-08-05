using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core;
using MattEland.Ani.Alfred.Core.Modules;
using MattEland.Ani.Alfred.Core.Modules.SysMonitor;
using MattEland.Ani.Alfred.WPF.Properties;

namespace MattEland.Ani.Alfred.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public sealed partial class MainWindow
    {

        /// <summary>
        /// The Alfred Provider that makes the application possible
        /// </summary>
        [NotNull]
        private readonly AlfredProvider _alfred;

        [NotNull]
        // ReSharper disable once AssignNullToNotNullAttribute
        private static readonly Settings Settings = Settings.Default;

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
            _alfred = new AlfredProvider(new WinClientPlatformProvider())
            {
                Console = console
            };

            StandardModuleProvider.AddStandardModules(_alfred);

            _alfred.AddModule(new CpuMonitorModule(_alfred.PlatformProvider));
            _alfred.AddModule(new MemoryMonitorModule(_alfred.PlatformProvider));

            console.Log("WinClient.Initialize", "Alfred instantiated");

            // Data bindings in the UI rely on Alfred
            DataContext = _alfred;

            // Set up the update timer
            var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            timer.Tick += OnTimerTick;
            timer.Start();

            // Determine whether to auto-start or not based off of settings
            if (Settings.AutoStartAlfred)
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
            Close();
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
    }
}