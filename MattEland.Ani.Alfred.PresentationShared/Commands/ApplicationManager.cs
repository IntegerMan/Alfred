// ---------------------------------------------------------
// ApplicationManager.cs
// 
// Created on:      08/20/2015 at 8:14 PM
// Last Modified:   08/22/2015 at 11:42 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Windows.Threading;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat;
using MattEland.Ani.Alfred.Core;
using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Modules.SysMonitor;
using MattEland.Ani.Alfred.Core.Speech;
using MattEland.Ani.Alfred.Core.SubSystems;

namespace MattEland.Ani.Alfred.PresentationShared.Commands
{
    /// <summary>
    ///     The application manager takes care of the discrete bits of managing Alfred
    ///     that shouldn't be the concern of the client or other user interface elements.
    /// </summary>
    public sealed class ApplicationManager : IDisposable
    {
        /// <summary>
        ///     The update frequency in seconds for Alfred's update pump
        /// </summary>
        private const double UpdateFrequencyInSeconds = 0.25;

        /// <summary>
        ///     The Alfred Provider that makes the application possible
        /// </summary>
        [NotNull]
        private readonly AlfredApplication _alfred;

        private AlfredCoreSubsystem _alfredCoreSubsystem;
        private AlfredChatSubsystem _chatSubsystem;
        private AlfredSpeechConsole _console;
        private MindExplorerSubsystem _mindExplorerSubsystem;
        private SystemMonitoringSubsystem _systemMonitoringSubsystem;

        private IUserInterfaceDirector _userInterfaceDirector;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object" /> class with
        ///     the specified user interface director and a XAML platform provider.
        /// </summary>
        /// <param name="director">The user interface director</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="director" /> is
        ///     <see langword="null" />.
        /// </exception>
        public ApplicationManager([NotNull] IUserInterfaceDirector director)
            : this(new XamlPlatformProvider(), director)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object" /> class with
        ///     the specified user interface director and platform provider.
        /// </summary>
        /// <param name="platformProvider"></param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="platformProvider" /> is
        ///     <see langword="null" />.
        /// </exception>
        public ApplicationManager([NotNull] IPlatformProvider platformProvider)
            : this(platformProvider, null)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object" /> class with
        ///     the specified user interface director and platform provider.
        /// </summary>
        /// <param name="platformProvider"></param>
        /// <param name="director">The user interface director</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="platformProvider" /> is
        ///     <see langword="null" />.
        /// </exception>
        public ApplicationManager([NotNull] IPlatformProvider platformProvider,
                                  [CanBeNull] IUserInterfaceDirector director)
        {

            if (platformProvider == null)
            {
                throw new ArgumentNullException(nameof(platformProvider));
            }

            // Create Alfred. It won't be online and running yet, but create it.
            var bootstrapper = new AlfredBootstrapper(platformProvider);
            _alfred = bootstrapper.Create();

            // Give Alfred a way to talk to the user and the client a way to log events that are separate from Alfred
            _console = InitializeConsole(platformProvider);

            // Set the director. This will, in turn, set the shell
            UserInterfaceDirector = director;

            InitializeSubsystems(_alfred.PlatformProvider);

            // Create an update pump on a dispatcher timer that will automatically get Alfred to regularly update any modules it has
            InitializeUpdatePump();
        }

        /// <summary>
        ///     Gets the user interface director.
        /// </summary>
        /// <value>The user interface director.</value>
        [CanBeNull]
        public IUserInterfaceDirector UserInterfaceDirector
        {
            [DebuggerStepThrough]
            get
            {
                return _userInterfaceDirector;
            }
            [DebuggerStepThrough]
            set
            {
                if (_userInterfaceDirector != value)
                {
                    _userInterfaceDirector = value;

                    if (value != null)
                    {
                        // Hook up our shell manager now that we have a way of communicating with Alfred
                        ShellManager = new ShellCommandManager(value, _alfred);
                        _alfred.Register(ShellManager);
                    }
                }

            }
        }

        /// <summary>
        ///     Gets the shell command manager.
        /// </summary>
        /// <value>The shell manager.</value>
        [CanBeNull]
        public ShellCommandManager ShellManager
        {
            [DebuggerStepThrough]
            get;
            [DebuggerStepThrough]
            private set;
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
        [CanBeNull]
        public IConsole Console
        {
            get { return _console; }
        }

        /// <summary>
        ///     Gets the current culture's locale information.
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
        ///     Initializes the console for the application and returns the instantiated
        ///     console.
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

            _console.Log("AppManager.InitConsole",
                         "Initializing console.",
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
            const string LogHeader = "AppManager.Initialize";
            _console?.Log(LogHeader, "Initializing subsystems", LogLevel.Verbose);

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
                              string.Format(Locale,
                                            "Problem creating system monitoring module: Win32 exception: {0}",
                                            ex.Message),
                              LogLevel.Error);
            }
            catch (UnauthorizedAccessException ex)
            {
                _console?.Log(LogHeader,
                              string.Format(Locale,
                                            "Problem creating system monitoring module: Unauthorized access exception: {0}",
                                            ex.Message),
                              LogLevel.Error);
            }

            // Init Chat
            _chatSubsystem = new AlfredChatSubsystem(platformProvider, _alfred.Console);
            _alfred.Register(_chatSubsystem);

            // Init Mind Explorer
            _mindExplorerSubsystem = new MindExplorerSubsystem(platformProvider, _alfred.Console);
            _alfred.Register(_mindExplorerSubsystem);
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

        /// <summary>
        ///     Initializes the update pump that causes Alfred to update its modules.
        /// </summary>
        private void InitializeUpdatePump()
        {
            var seconds = TimeSpan.FromSeconds(UpdateFrequencyInSeconds);

            var timer = new DispatcherTimer
                        {
                            Interval = seconds
                        };
            timer.Tick += delegate { Update(); };

            timer.Start();

        }
    }
}