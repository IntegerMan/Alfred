// ---------------------------------------------------------
// ApplicationManager.cs
// 
// Created on:      08/20/2015 at 8:14 PM
// Last Modified:   08/25/2015 at 11:09 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
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
using MattEland.Ani.Alfred.Core.Subsystems;
using MattEland.Common.Providers;

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
        private ChatSubsystem _chatSubsystem;
        private AlfredSpeechConsole _console;
        private bool _enableSpeech;
        private MindExplorerSubsystem _mindExplorerSubsystem;
        private SystemMonitoringSubsystem _systemMonitoringSubsystem;

        private IUserInterfaceDirector _userInterfaceDirector;

        /// <summary>
        /// Gets the container.
        /// </summary>
        /// <value>The container.</value>
        [NotNull]
        public IObjectContainer Container { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object" /> class with
        /// the specified user interface director and platform provider.
        /// </summary>
        /// <param name="platformProvider">The platform provider.</param>
        /// <param name="director">The user interface director</param>
        /// <param name="container">The container.</param>
        /// <param name="enableSpeech">if set to <c>true</c> enable speech.</param>
        public ApplicationManager([CanBeNull] IObjectContainer container,
            [CanBeNull] IUserInterfaceDirector director = null,
            bool enableSpeech = false)
        {

            // Everything will need a container. Provide one.
            Container = container ?? CommonProvider.Container;

            // Use this container whenever a container is requested
            Container.RegisterAsProvidedInstance(typeof(IObjectContainer));

            // Register default mappings
            Container.Register(typeof(AlfredCommand), typeof(XamlClientCommand));

            // TODO: Grab things from the container!

            // Create Alfred. It won't be online and running yet, but create it.
            var bootstrapper = new AlfredBootstrapper(Container);
            _alfred = bootstrapper.Create();
            _alfred.RegisterAsProvidedInstance(typeof(IAlfred), Container);

            // Give Alfred a way to talk to the user and the client a way to log events that are separate from Alfred
            EnableSpeech = enableSpeech;
            _console = InitializeConsole();

            // Set the director. This will, in turn, set the shell
            UserInterfaceDirector = director;

            InitializeSubsystems();

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
        ///     Gets or sets a value indicating whether speech synthesis is enabled.
        /// </summary>
        /// <value><c>true</c> if speech synthesis is enabled; otherwise, <c>false</c>.</value>
        public bool EnableSpeech
        {
            get { return _enableSpeech; }
            set
            {
                _enableSpeech = value;

                if (_console != null) { _console.EnableSpeech = value; }
            }
        }

        /// <summary>
        ///     Gets the root nodes.
        /// </summary>
        /// <value>The root nodes.</value>
        public IEnumerable<IPropertyProvider> RootNodes
        {
            get { yield return _alfred; }
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
        /// <returns>The instantiated console.</returns>
        /// <exception cref="System.ArgumentNullException">platformProvider</exception>
        [NotNull]
        private AlfredSpeechConsole InitializeConsole()
        {
            // Give Alfred a way to talk to the application
            var baseConsole = new SimpleConsole(Container, new ExplorerEventFactory());

            // Give Alfred a voice
            _console = new AlfredSpeechConsole(baseConsole, baseConsole.EventFactory) { EnableSpeech = EnableSpeech };

            _console.Log("AppManager.InitConsole", "Initializing console.", LogLevel.Verbose);
            _alfred.Console = _console;

            // This will be our console
            _console.RegisterAsProvidedInstance(typeof(IConsole));

            return _console;
        }

        /// <summary>
        ///     Initializes and register's Alfred's subsystems
        /// </summary>
        private void InitializeSubsystems()
        {
            // Log header
            const string LogHeader = "AppManager.Initialize";
            _console?.Log(LogHeader, "Initializing subsystems", LogLevel.Verbose);

            // Init Core
            _alfredCoreSubsystem = new AlfredCoreSubsystem(Container);
            _alfred.Register(_alfredCoreSubsystem);

            // Initialize System Monitor - this can throw a few exceptions so may not be available.
            try
            {
                var metricProviderFactory = new CounterMetricProviderFactory();
                _systemMonitoringSubsystem = new SystemMonitoringSubsystem(Container,
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

            // Initialize Chat
            _chatSubsystem = new ChatSubsystem(Container, _alfred.Name);
            _alfred.Register(_chatSubsystem);

            // Initialize Mind Explorer
            _mindExplorerSubsystem = new MindExplorerSubsystem(Container);
            _alfred.Register(_mindExplorerSubsystem);
        }

        /// <summary>
        ///     Updates the module
        /// </summary>
        public void Update()
        {
            // If Alfred is online, ask it to update its modules
            if (_alfred.Status == AlfredStatus.Online) { _alfred.Update(); }
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
            if (_alfred.Status != AlfredStatus.Offline) { _alfred.Shutdown(); }
        }

        /// <summary>
        ///     Initializes the update pump that causes Alfred to update its modules.
        /// </summary>
        private void InitializeUpdatePump()
        {
            var seconds = TimeSpan.FromSeconds(UpdateFrequencyInSeconds);

            var timer = new DispatcherTimer { Interval = seconds };
            timer.Tick += delegate { Update(); };

            timer.Start();
        }
    }
}