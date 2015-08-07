// ---------------------------------------------------------
// MainPage.xaml.cs
// 
// Created on:      07/26/2015 at 12:07 AM
// Last Modified:   08/07/2015 at 12:42 AM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;

using Windows.UI.Xaml;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core;
using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.Core.Modules;

namespace MattEland.Ani.Alfred.Win8
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage
    {
        [NotNull]
        private readonly AlfredProvider _alfred;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MainPage" /> class.
        /// </summary>
        public MainPage()
        {

            InitializeComponent();

            // Create Alfred. It won't be online and running yet, but create it.
            var platformProvider = new Win8ClientPlatformProvider();
            _alfred = new AlfredProvider(platformProvider);

            // Create the console
            var console = new SimpleConsole(platformProvider);
            console.Log("MetroClient.Initialize", "Console is now online.", LogLevel.Verbose);
            _alfred.Console = console;

            // Register Modules
            StandardModuleProvider.AddStandardModules(_alfred);

            // Win 8 app cannot reference the System Modules

            console.Log("MetroClient.Initialize", "Alfred instantiated", LogLevel.Verbose);

            // Data bindings in the UI rely on Alfred
            DataContext = _alfred;

            // Set up the update timer
            var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            timer.Tick += OnTimerTick;
            timer.Start();

            // TODO: Support Auto-Start

        }

        /// <summary>
        ///     Handles the <see cref="E:TimerTick" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void OnTimerTick(object sender, object e)
        {
            // If Alfred is online, ask it to update its modules
            if (_alfred.Status == AlfredStatus.Online)
            {
                _alfred.Update();
            }
        }
    }
}