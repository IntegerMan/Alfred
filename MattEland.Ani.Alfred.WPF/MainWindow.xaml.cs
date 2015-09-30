// ---------------------------------------------------------
// MainWindow.xaml.cs
// 
// Created on:      07/25/2015 at 11:55 PM
// Last Modified:   08/08/2015 at 5:56 PM
//
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.PresentationAvalon.Commands;
using MattEland.Ani.Alfred.WPF.Properties;
using MattEland.Common;
using MattEland.Common.Providers;

namespace MattEland.Ani.Alfred.WPF
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public sealed partial class MainWindow : IDisposable, IUserInterfaceDirector, IHasContainer
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="MainWindow" /> class.
        /// </summary>
        [UsedImplicitly]
        public MainWindow() : this(CommonProvider.Container, true)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MainWindow" /> class.
        /// </summary>
        /// <param name="container"> The container. </param>
        /// <param name="enableSpeech"> true to enable speech output, false to disable speech. </param>
        public MainWindow(IObjectContainer container, bool enableSpeech)
        {
            Container = container;

            InitializeComponent();

#if DEBUG
            // Do not allow topmost window mode while debugging
            Topmost = false;
#endif
            var options = new ApplicationManagerOptions
            {
                IsSpeechEnabled = enableSpeech,
                ShowMindExplorerPage = true,
                BingApiKey = Settings.Default.BingApiKey,
                IncludeSearchModuleOnSearchPage = false
            };

            Application = new ApplicationManager(Container, options, this);

            // DataBindings rely on Alfred presently as there hasn't been a need for a page ViewModel yet
            DataContext = Application;

            "Initialization Complete".Log("WinClient.Initialize", LogLevel.Verbose, Container);
        }

        /// <summary>
        ///     Gets or sets the container.
        /// </summary>
        /// <value>
        ///     The container.
        /// </value>
        [NotNull]
        public IObjectContainer Container { get; }

        /// <summary>
        ///     Gets the application.
        /// </summary>
        /// <value>
        ///     The application.
        /// </value>
        [NotNull]
        public ApplicationManager Application { get; }

        /// <summary>
        ///     Handles the <see cref="E:WindowLoaded" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            const string LogHeader = "WinClient.Loaded";

            // Determine whether to auto-start or not based off of settings
            Debug.Assert(Settings.Default != null);
            if (Settings.Default.AutoStartAlfred)
            {
                "Automatically starting Alfred".Log(LogHeader, LogLevel.Verbose, Container);

                Application.Start();
            }

            // Log that we're good to go
            "The application has loaded.".Log(LogHeader, LogLevel.Verbose, Container);
        }

        /// <summary>
        ///     Handles the <see cref="E:WindowClosing" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="CancelEventArgs" /> instance containing the event data.</param>
        private void OnWindowClosing(object sender, CancelEventArgs e)
        {
            Application.Stop();
        }

        /// <summary>
        /// Dispose of all allocated resources
        /// </summary>
        [SuppressMessage("ReSharper", "UseNullPropagation")]
        public void Dispose()
        {
            Application.Dispose();
        }

        /// <summary>
        ///     Handles the page navigation <paramref name="command" /> .
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Whether or not the <paramref name="command"/> was handled</returns>
        public bool HandlePageNavigationCommand(ShellCommand command)
        {
            Debug.Assert(PagesControl != null);

            return PagesControl.HandlePageNavigationCommand(command);
        }

        /// <summary>
        /// Handles the event when a web page is requested.
        /// </summary>
        /// <param name="url">The URL that was requested.</param>
        public void HandleWebPageRequested(string url)
        {
            Process.Start(url);
        }

    }
}