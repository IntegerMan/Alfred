// ---------------------------------------------------------
// ApplicationManager.cs
// 
// Created on:      08/09/2015 at 11:21 PM
// Last Modified:   08/18/2015 at 2:16 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat;
using MattEland.Ani.Alfred.Core;
using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Modules;
using MattEland.Ani.Alfred.Core.Modules.SysMonitor;
using MattEland.Ani.Alfred.Core.Speech;
using MattEland.Ani.Alfred.WPF.Platform;
using MattEland.Common;

using Res = MattEland.Ani.Alfred.WPF.Properties.Resources;

namespace MattEland.Ani.Alfred.WPF
{
    /// <summary>
    ///     The application manager takes care of the discrete bits of managing Alfred that
    ///     shouldn't be the concern of MainWindow or other user interface elements.
    /// </summary>
    public sealed class ApplicationManager : IDisposable
    {
        /// <summary>
        ///     The Alfred Provider that makes the application possible
        /// </summary>
        [NotNull]
        private readonly AlfredApplication _alfred;

        private AlfredCoreSubsystem _alfredCoreSubsystem;
        private AlfredChatSubsystem _chatSubsystem;
        private AlfredSpeechConsole _console;
        private SystemMonitoringSubsystem _systemMonitoringSubsystem;

        [NotNull]
        private WpfShellCommandManager _shellManager;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        /// <param name="window">The owning window</param>
        /// <exception cref="ArgumentNullException"><paramref name="window"/> is <see langword="null" />.</exception>
        public ApplicationManager([NotNull] MainWindow window)
        {
            if (window == null)
            {
                throw new ArgumentNullException(nameof(window));
            }

            // Create Alfred. It won't be online and running yet, but create it.
            var platformProvider = new WinClientPlatformProvider();
            var bootstrapper = new AlfredBootstrapper(platformProvider);
            _alfred = bootstrapper.Create();

            // Give Alfred a way to talk to the user and the client a way to log events that are separate from Alfred
            _console = InitializeConsole(platformProvider);

            // Hook up our shell manager now that we have a way of communicating with Alfred
            _shellManager = new WpfShellCommandManager(window, _alfred);
            _alfred.Register(_shellManager);

            InitializeSubsystems(_alfred.PlatformProvider);
        }

        /// <summary>
        ///     The Alfred Provider that makes the application possible
        /// </summary>
        [NotNull]
        [UsedImplicitly]
        public AlfredApplication Alfred
        {
            [DebuggerStepThrough]
            get
            {
                return _alfred;
            }
        }

        /// <summary>
        ///     Gets the console.
        /// </summary>
        /// <value>The console.</value>
        public IConsole Console
        {
            get { return _console; }
        }

        /// <summary>
        ///     Disposes of loose resources
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed",
            MessageId = "_console")]
        [SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed",
            MessageId = "_systemMonitoringSubsystem")]
        public void Dispose()
        {
            _systemMonitoringSubsystem?.Dispose();
            _console?.Dispose();
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

            _console.Log(Res.InitializeConsoleLogHeader.NonNull(),
                         Res.ConsoleOnlineLogMessage.NonNull(),
                         LogLevel.Verbose);
            _alfred.Console = _console;

            return _console;
        }

        /// <summary>
        ///     Initializes and register's Alfred's subsystems
        /// </summary>
        /// <param name="platformProvider">The platform provider</param>
        private void InitializeSubsystems([NotNull] IPlatformProvider platformProvider)
        {
            // Log header
            const string LogHeader = "WinClient.Initialize";
            _console?.Log(LogHeader, Res.InitializeModulesLogMessage.NonNull(), LogLevel.Verbose);

            // Init Core
            _alfredCoreSubsystem = new AlfredCoreSubsystem(platformProvider);
            _alfred.Register(_alfredCoreSubsystem);

            // Init System Monitor - this can throw a few exceptions so may not be available.
            try
            {
                var metricProviderFactory = new CounterMetricProviderFactory();
                _systemMonitoringSubsystem = new SystemMonitoringSubsystem(platformProvider,
                                                                           metricProviderFactory);
                _alfred.Register(_systemMonitoringSubsystem);
            }
            catch (Win32Exception ex)
            {
                _console?.Log(LogHeader,
                              string.Format(Locale, "Problem creating system monitoring module: Win32 exception: {0}", ex.Message),
                              LogLevel.Error);
            }
            catch (UnauthorizedAccessException ex)
            {
                _console?.Log(LogHeader,
                              string.Format(Locale, "Problem creating system monitoring module: Unauthorized access exception: {0}", ex.Message),
                              LogLevel.Error);
            }

            // Init Chat
            _chatSubsystem = new AlfredChatSubsystem(platformProvider, _alfred.Console);
            _alfred.Register(_chatSubsystem);

        }

        /// <summary>
        /// Gets the current culture's locale information.
        /// </summary>
        /// <value>The locale.</value>
        [NotNull]
        public CultureInfo Locale
        {
            get
            {
                //? It might make sense to make this a property on Alfred.
                return CultureInfo.CurrentCulture;
            }
        }

        /// <summary>
        ///     Updates the module
        /// </summary>
        public void Update()
        {
            // If Alfred is online, ask it to update its modules
            if (_alfred.Status == AlfredStatus.Online)
            {
                _alfred.Update();
            }
        }

        /// <summary>
        ///     Starts Alfred
        /// </summary>
        public void Start()
        {
            _alfred.Initialize();
        }

        /// <summary>
        ///     Stops Alfred
        /// </summary>
        public void Stop()
        {
            // Make sure we clean up Alfred
            if (_alfred.Status != AlfredStatus.Offline)
            {
                _alfred.Shutdown();
            }
        }
    }
}