// ---------------------------------------------------------
// ApplicationManager.cs
// 
// Created on:      08/09/2015 at 11:21 PM
// Last Modified:   08/09/2015 at 11:43 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat;
using MattEland.Ani.Alfred.Core;
using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Modules;
using MattEland.Ani.Alfred.Core.Modules.SysMonitor;
using MattEland.Ani.Alfred.Core.Speech;
using MattEland.Ani.Alfred.WPF.Platform;

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
        private readonly AlfredProvider _alfred;

        private AlfredControlSubsystem _alfredControlSubsystem;
        private AlfredChatSubsystem _chatSubsystem;
        private AlfredSpeechConsole _console;
        private SystemMonitoringSubsystem _systemMonitoringSubsystem;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        public ApplicationManager()
        {
            // Create Alfred. It won't be online and running yet, but create it.
            var platformProvider = new WinClientPlatformProvider();
            var bootstrapper = new AlfredBootstrapper(platformProvider);
            _alfred = bootstrapper.Create();

            // Give Alfred a way to talk to the user and the client a way to log events that are separate from Alfred
            _console = InitializeConsole(platformProvider);

            InitializeModules();
        }

        /// <summary>
        ///     The Alfred Provider that makes the application possible
        /// </summary>
        [NotNull]
        [UsedImplicitly]
        public AlfredProvider Alfred
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
        [SuppressMessage("ReSharper", "UseNullPropagation")]
        public void Dispose()
        {
            if (_systemMonitoringSubsystem != null)
            {
                _systemMonitoringSubsystem?.Dispose();
            }
            if (_console != null)
            {
                _console?.Dispose();
            }
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

        private void InitializeModules()
        {
            _console?.Log("WinClient.Initialize", Res.InitializeModulesLogMessage.NonNull(), LogLevel.Verbose);

            var provider = _alfred.PlatformProvider;

            _alfredControlSubsystem = new AlfredControlSubsystem(provider);
            _systemMonitoringSubsystem = new SystemMonitoringSubsystem(provider);
            _chatSubsystem = new AlfredChatSubsystem(provider);

            _alfred.Register(_alfredControlSubsystem);
            _alfred.Register(_systemMonitoringSubsystem);
            _alfred.Register(_chatSubsystem);

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