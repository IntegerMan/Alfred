// ---------------------------------------------------------
// MainWindow.xaml.cs
// 
// Created on:      07/25/2015 at 11:55 PM
// Last Modified:   08/08/2015 at 5:56 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Threading;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core;
using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Modules;
using MattEland.Ani.Alfred.Core.Modules.SysMonitor;
using MattEland.Ani.Alfred.Core.Speech;
using MattEland.Ani.Alfred.WPF.Platform;
using MattEland.Ani.Alfred.WPF.Properties;

namespace MattEland.Ani.Alfred.WPF
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public sealed partial class MainWindow : IDisposable
    {
        /// <summary>
        ///     The update frequency in seconds for Alfred's update pump
        /// </summary>
        private const double UpdateFrequencyInSeconds = 0.25;

        [NotNull]

        // ReSharper disable once AssignNullToNotNullAttribute
        private static readonly Settings Settings = Settings.Default;

        /// <summary>
        ///     The Alfred Provider that makes the application possible
        /// </summary>
        [NotNull]
        private readonly AlfredProvider _alfred;

        private SystemMonitoringSubsystem _systemMonitoringSubsystem;
        private AlfredControlSubsystem _alfredControlSubsystem;
        private AlfredSpeechConsole _console;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MainWindow" /> class.
        /// </summary>
        public MainWindow()
        {

#if DEBUG
            // Do not allow topmost window mode while debugging
            Topmost = false;
#endif

            InitializeComponent();

            // Create Alfred. It won't be online and running yet, but create it.
            var platformProvider = new WinClientPlatformProvider();
            var bootstrapper = new AlfredBootstrapper(platformProvider);
            _alfred = bootstrapper.Create();

            // Give Alfred a way to talk to the user and the client a way to log events that are separate from Alfred
            _console = InitializeConsole(platformProvider);

            // Give Alfred some Content
            InitializeAlfredModules();

            // DataBindings rely on Alfred presently as there hasn't been a need for a page ViewModel yet
            DataContext = _alfred;


            // Set up the update timer
            InitializeUpdatePump();

            _console.Log("WinClient.Initialize", Properties.Resources.InitializationCompleteLogMessage.NonNull(), LogLevel.Verbose);
        }

        private void InitializeAlfredModules()
        {
            _console.Log("WinClient.Initialize", Properties.Resources.InitializeModulesLogMessage.NonNull(), LogLevel.Verbose);

            var provider = _alfred.PlatformProvider;

            _alfredControlSubsystem = new AlfredControlSubsystem(provider);
            _systemMonitoringSubsystem = new SystemMonitoringSubsystem(provider);

            _alfred.Register(_alfredControlSubsystem);
            _alfred.Register(_systemMonitoringSubsystem);

        }

        /// <summary>
        ///     Initializes the update pump that causes Alfred to update its modules.
        /// </summary>
        private void InitializeUpdatePump()
        {
            var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(UpdateFrequencyInSeconds) };
            timer.Tick += delegate
                          {
                              // If Alfred is online, ask it to update its modules
                              if (_alfred.Status == AlfredStatus.Online)
                              {
                                  _alfred.Update();
                              }
                          };

            timer.Start();
        }

        /// <summary>
        ///     Initializes the console for the application and returns the instantiated console.
        /// </summary>
        /// <param name="platformProvider">The platform provider.</param>
        /// <returns>The instantiated console.</returns>
        /// <exception cref="System.ArgumentNullException">platformProvider</exception>
        [NotNull]
        private AlfredSpeechConsole InitializeConsole([NotNull] IPlatformProvider platformProvider)
        {
            if (platformProvider == null)
            {
                throw new ArgumentNullException(nameof(platformProvider));
            }

            // Give Alfred a way to talk to the application
            var baseConsole = new SimpleConsole(platformProvider);

            // Give Alfred a voice
            _console = new AlfredSpeechConsole(baseConsole);

            _console.Log(Properties.Resources.InitializeConsoleLogHeader.NonNull(), Properties.Resources.ConsoleOnlineLogMessage.NonNull(), LogLevel.Verbose);
            _alfred.Console = _console;

            return _console;
        }

        /// <summary>
        ///     Handles the <see cref="E:WindowLoaded" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            var logHeader = Properties.Resources.WinClientLoadedLogHeader.NonNull();

            // Determine whether to auto-start or not based off of settings
            if (Settings.AutoStartAlfred)
            {
                _console.Log(logHeader, Properties.Resources.AutoStartAlfredLogMessage.NonNull(), LogLevel.Verbose);
                _alfred.Initialize();
            }

            // Auto-select the first tab; if any are present
            AutoSelectFirstTab();

            // Log that we're good to go
            _console.Log(logHeader, Properties.Resources.AppOnlineLogMessage.NonNull(), LogLevel.Info);
        }

        /// <summary>
        /// Automatically selects the first tab if no tab is selected.
        /// </summary>
        private void AutoSelectFirstTab()
        {
            Debug.Assert(tabPages != null);
            if (tabPages.SelectedItem == null && tabPages.HasItems)
            {
                tabPages.SelectedIndex = 0;
            }
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

        /// <summary>
        /// Dispose of all allocated resources
        /// </summary>
        [SuppressMessage("ReSharper", "UseNullPropagation")]
        public void Dispose()
        {
            if (_systemMonitoringSubsystem != null)
            {
                _systemMonitoringSubsystem.Dispose();
            }

            if (_console != null)
            {
                _console.Dispose();
            }
        }
    }
}