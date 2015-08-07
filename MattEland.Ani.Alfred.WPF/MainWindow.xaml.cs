// ---------------------------------------------------------
// MainWindow.xaml.cs
// 
// Created on:      07/25/2015 at 11:55 PM
// Last Modified:   08/07/2015 at 3:15 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core;
using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.Core.Modules;
using MattEland.Ani.Alfred.Core.Modules.SysMonitor;
using MattEland.Ani.Alfred.Core.Speech;
using MattEland.Ani.Alfred.WPF.Properties;

namespace MattEland.Ani.Alfred.WPF
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public sealed partial class MainWindow : IDisposable
    {
        [NotNull]

        // ReSharper disable once AssignNullToNotNullAttribute
        private static readonly Settings Settings = Settings.Default;

        /// <summary>
        ///     The Alfred Provider that makes the application possible
        /// </summary>
        [NotNull]
        private readonly AlfredProvider _alfred;

        [NotNull]
        private readonly AlfredSpeechConsole _console;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MainWindow" /> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            // Create Alfred. It won't be online and running yet, but create it.
            var platformProvider = new WinClientPlatformProvider();
            _alfred = new AlfredProvider(platformProvider);

            // Give Alfred a way to talk to the application
            var baseConsole = new SimpleConsole(platformProvider);

            // Give Alfred a voice
            _console = new AlfredSpeechConsole(baseConsole);

            _console.Log("WinClient.Initialize", "Console is now online.", LogLevel.Verbose);
            _alfred.Console = _console;

            // Give Alfred some Content
            StandardModuleProvider.AddStandardModules(_alfred);
            SystemModuleProvider.AddStandardModules(_alfred);

            _console.Log("WinClient.Initialize", "Alfred instantiated", LogLevel.Verbose);

            // Data bindings in the UI rely on Alfred
            DataContext = _alfred;

            // Set up the update timer
            var timer = new DispatcherTimer {Interval = TimeSpan.FromSeconds(1)};
            timer.Tick += OnTimerTick;
            timer.Start();
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _alfred.Dispose();
        }

        /// <summary>
        ///     Handles the <see cref="E:TimerTick" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void OnTimerTick(object sender, EventArgs e)
        {
            // If Alfred is online, ask it to update its modules
            if (_alfred.Status == AlfredStatus.Online)
            {
                _alfred.Update();
            }
        }

        /// <summary>
        ///     Handles the <see cref="E:WindowLoaded" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            // Determine whether to auto-start or not based off of settings
            if (Settings.AutoStartAlfred)
            {
                _alfred.Initialize();
            }

            _console.Log("WinClient.Loaded", "The application is now online", LogLevel.Info);
        }

        /// <summary>
        ///     Handles the <see cref="E:WindowClosing" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="CancelEventArgs" /> instance containing the event data.</param>
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