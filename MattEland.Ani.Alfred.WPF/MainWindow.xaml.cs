﻿// ---------------------------------------------------------
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

using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.WPF.Properties;
using MattEland.Common;

using Res = MattEland.Ani.Alfred.WPF.Properties.Resources;

namespace MattEland.Ani.Alfred.WPF
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public sealed partial class MainWindow : IDisposable
    {
        [NotNull]
        private readonly ApplicationManager _app;

        /// <summary>
        ///     The update frequency in seconds for Alfred's update pump
        /// </summary>
        private const double UpdateFrequencyInSeconds = 0.25;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MainWindow" /> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            // Do not allow topmost window mode while debugging
            Topmost = false;
#endif
            _app = new ApplicationManager();

            // DataBindings rely on Alfred presently as there hasn't been a need for a page ViewModel yet
            DataContext = _app;

            // Set up the update timer
            InitializeUpdatePump();

            _app.Console?.Log("WinClient.Initialize", Res.InitializationCompleteLogMessage.NonNull(), LogLevel.Verbose);
        }

        /// <summary>
        ///     Initializes the update pump that causes Alfred to update its modules.
        /// </summary>
        private void InitializeUpdatePump()
        {
            var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(UpdateFrequencyInSeconds) };
            timer.Tick += delegate
                          {
                              _app.Update();
                          };

            timer.Start();
        }

        /// <summary>
        ///     Handles the <see cref="E:WindowLoaded" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            var logHeader = Res.WinClientLoadedLogHeader.NonNull();

            // Determine whether to auto-start or not based off of settings
            Debug.Assert(Settings.Default != null, "Settings.Default != null");
            if (Settings.Default.AutoStartAlfred)
            {
                _app.Console?.Log(logHeader, Res.AutoStartAlfredLogMessage.NonNull(), LogLevel.Verbose);
                _app.Start();
            }

            // Auto-select the first tab; if any are present
            AutoSelectFirstTab();

            // Log that we're good to go
            _app.Console?.Log(logHeader, Res.AppOnlineLogMessage.NonNull(), LogLevel.Info);
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
            _app.Stop();
        }

        /// <summary>
        /// Dispose of all allocated resources
        /// </summary>
        [SuppressMessage("ReSharper", "UseNullPropagation")]
        public void Dispose()
        {
            _app.Dispose();
        }
    }
}