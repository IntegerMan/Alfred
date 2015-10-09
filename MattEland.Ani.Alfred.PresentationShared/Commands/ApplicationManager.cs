// --------------------------------------------------------- ApplicationManager.cs
// 
// Created on: 09/03/2015 at 11:00 PM Last Modified: 09/04/2015 at 1:37 AM
// 
// Last Modified by: Matt Eland ---------------------------------------------------------

using JetBrains.Annotations;
using MattEland.Ani.Alfred.Chat;
using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Modules.SysMonitor;
using MattEland.Ani.Alfred.Core.Speech;
using MattEland.Ani.Alfred.PresentationAvalon.Helpers;
using MattEland.Ani.Alfred.PresentationCommon.Commands;
using MattEland.Ani.Alfred.Search;
using MattEland.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Threading;

namespace MattEland.Ani.Alfred.PresentationAvalon.Commands
{
    /// <summary>
    /// The application manager takes care of the discrete bits of managing Alfred that shouldn't be
    /// the concern of the client or other user interface elements.
    /// </summary>
    [SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed",
            Justification = "TryDispose is invoked")]
    [SuppressMessage("CodeRush", "Fields should be disposed",
            Justification = "TryDispose is invoked")]
    public sealed class ApplicationManager : ApplicationManagerBase
    {

        /// <summary>
        /// The update frequency in seconds for Alfred's update pump 
        /// </summary>
        private const double UpdateFrequencyInSeconds = 0.25;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class with the specified
        /// user interface director and platform provider.
        /// </summary>
        /// <param name="container"> The container. </param>
        /// <param name="options"> Options for creating the application. </param>
        /// <param name="director"> The user interface director. </param>
        public ApplicationManager(
            [CanBeNull] IAlfredContainer container,
            [NotNull] ApplicationManagerOptions options,
            [CanBeNull] IUserInterfaceDirector director = null)
            : base(container, options, director)
        {
        }


        /// <summary>
        /// Sets up the mappings for types the container will need to provide. 
        /// </summary>
        protected override void ConfigureContainer()
        {
            Container.CollectionType = typeof(SafeObservableCollection<>);

            Container.TryRegister(typeof(IAlfredCommand), typeof(XamlClientCommand));
            Container.TryRegister(typeof(MetricProviderBase), typeof(CounterMetricProvider));
            Container.TryRegister(typeof(IMetricProviderFactory),
                                  typeof(CounterMetricProviderFactory));
            Container.TryRegister(typeof(IMessageBoxProvider), typeof(AvalonMessageBoxProvider));

            base.ConfigureContainer();
        }

        /// <summary>
        /// Initializes the console for the application and returns the instantiated console. 
        /// </summary>
        /// <returns> The instantiated console. </returns>
        [NotNull]
        protected override IConsole InitializeConsole()
        {
            // Give Alfred a way to talk to the application 
            IConsole console = base.InitializeConsole();

            // If we support speech, build the speech console
            if (Options.IsSpeechEnabled)
            {
                var speech = new AlfredSpeechConsole(Container, console, console.EventFactory);

                console = speech;
            }

            return console;
        }

        /// <summary>
        /// Initializes and registers Alfred's subsystems. 
        /// </summary>
        protected override void RegisterSubsystems()
        {
            base.RegisterSubsystems();

            Register(new ChatSubsystem(Container, "Alfred"));
            Register(new SearchSubsystem(Container, Options.BingApiKey, Options.StackOverflowApiKey));

            InitializeSystemMonitoringSubsystem();
        }

        /// <summary>
        /// Initializes the system monitoring subsystem. 
        /// </summary>
        private void InitializeSystemMonitoringSubsystem()
        {
            try
            {
                // This can throw a few exceptions so may not be available. 
                Register(new SystemMonitoringSubsystem(Container));
            }
            catch (Win32Exception ex)
            {
                var message = string.Format(Locale,
                                            "Problem creating system monitoring module: Win32 exception: {0}",
                                            ex.BuildDetailsMessage());

                Console?.Log(LogHeader, message, LogLevel.Error);

            }
            catch (UnauthorizedAccessException ex)
            {
                var message = string.Format(Locale,
                                            "Problem creating system monitoring module: Unauthorized access exception: {0}",
                                            ex.BuildDetailsMessage());

                Console?.Log(LogHeader, message, LogLevel.Error);

            }
        }

        /// <summary>
        /// Initializes the update pump that causes <see cref="Alfred"/> to update its modules. 
        /// </summary>
        protected override void InitializeUpdatePump()
        {
            var seconds = TimeSpan.FromSeconds(UpdateFrequencyInSeconds);

            var timer = new DispatcherTimer { Interval = seconds };

            timer.Tick += delegate { Update(); };

            timer.Start();
        }

    }
}