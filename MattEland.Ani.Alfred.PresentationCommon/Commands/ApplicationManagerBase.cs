// --------------------------------------------------------- ApplicationManager.cs
// 
// Created on: 09/03/2015 at 11:00 PM Last Modified: 09/04/2015 at 1:37 AM
// 
// Last Modified by: Matt Eland ---------------------------------------------------------

using MattEland.Common.Annotations;
using MattEland.Ani.Alfred.Core;
using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Subsystems;
using MattEland.Common;
using MattEland.Common.Providers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Globalization;

namespace MattEland.Ani.Alfred.PresentationCommon.Commands
{
    /// <summary>
    /// The application manager takes care of the discrete bits of managing Alfred that shouldn't be
    /// the concern of the client or other user interface elements.
    /// </summary>
    [SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed",
            Justification = "TryDispose is invoked")]
    [SuppressMessage("CodeRush", "Fields should be disposed",
            Justification = "TryDispose is invoked")]
    public abstract class ApplicationManagerBase : IDisposable, IHasContainer<IAlfredContainer>
    {
        public const string LogHeader = "AppManager.Initialize";

        /// <summary>
        /// The Alfred Provider that makes the application possible 
        /// </summary>
        [NotNull]
        private readonly AlfredApplication _alfred;

        /// <summary>
        ///     The user interface director.
        /// </summary>
        [CanBeNull]
        private IUserInterfaceDirector _userInterfaceDirector;

        [ContractInvariantMethod]
        private void Invariants()
        {
            Contract.Invariant(_alfred != null);
            Contract.Invariant(Container != null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class with the specified
        /// user interface director and platform provider.
        /// </summary>
        /// <param name="container"> The container. </param>
        /// <param name="options"> Options for creating the application. </param>
        /// <param name="director"> The user interface director. </param>
        protected ApplicationManagerBase(
            [CanBeNull] IAlfredContainer container,
            [NotNull] ApplicationManagerOptions options,
            [CanBeNull] IUserInterfaceDirector director = null)
        {
            Options = options;

            // Everything will need a container. Provide one. 
            Container = container ?? new AlfredContainer();

            // Register default mappings 
            ConfigureContainer();

            // Create Alfred. It won't be online and running yet, but create it. 
            _alfred = Container.Provide<AlfredApplication>(Container);
            Container.Alfred = _alfred;

            // Give Alfred a way to talk to the user and the client a way to log events that are
            // separate from Alfred
            Console = InitializeConsole();

            // Set the director. This will, in turn, set the shell 
            UserInterfaceDirector = director;

            InitializeSubsystems();

            // Create an update pump on a dispatcher timer that will automatically get Alfred to
            // regularly update any modules it has
            InitializeUpdatePump();
        }

        /// <summary>
        ///     The Alfred Provider that makes the application possible.
        /// </summary>
        /// <value>
        ///     The alfred.
        /// </value>
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
        /// Gets the console. 
        /// </summary>
        /// <value> The console. </value>
        [CanBeNull]
        public IConsole Console
        {
            get
            {
                return Container.Console;
            }
            set
            {
                Container.Console = value;
            }
        }

        /// <summary>
        ///     Gets the container.
        /// </summary>
        /// <value>
        ///     The container.
        /// </value>
        [NotNull]
        public IAlfredContainer Container { get; }

        /// <summary>
        ///     Gets the current culture's locale information.
        /// </summary>
        /// <value>
        ///     The locale.
        /// </value>
        [NotNull]
        public CultureInfo Locale
        {
            get
            {
                return Container.Locale;
            }
        }

        /// <summary>
        /// Gets options for controlling application creation. 
        /// </summary>
        /// <value> The options. </value>
        [NotNull]
        public ApplicationManagerOptions Options { get; }

        /// <summary>
        /// Gets the root nodes. 
        /// </summary>
        /// <value> The root nodes. </value>
        public IEnumerable<IPropertyProvider> RootNodes
        {
            get { yield return _alfred; }
        }

        /// <summary>
        /// Gets the user interface director. 
        /// </summary>
        /// <value> The user interface director. </value>
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
                        Alfred.Register(new ShellCommandManager(Container, value, Alfred));
                    }
                }
            }
        }

        /// <summary>
        /// Disposes of loose resources 
        /// </summary>
        public void Dispose()
        {
            _alfred.TryDispose();
        }

        /// <summary>
        /// Starts Alfred 
        /// </summary>
        public void Start()
        {
            _alfred.Initialize();
        }

        /// <summary>
        /// Stops Alfred 
        /// </summary>
        public void Stop()
        {
            // Make sure we clean up Alfred 
            if (_alfred.Status != AlfredStatus.Offline) { _alfred.Shutdown(); }
        }

        /// <summary>
        /// Updates the module 
        /// </summary>
        public void Update()
        {
            // If Alfred is online, ask it to update its modules 
            if (_alfred.Status == AlfredStatus.Online) { _alfred.Update(); }
        }

        /// <summary>
        /// Sets up the mappings for types the container will need to provide. 
        /// </summary>
        protected virtual void ConfigureContainer()
        {
            Container.TryRegister(typeof(IAlfred), typeof(AlfredApplication));
            Container.TryRegister(typeof(ISearchController), typeof(AlfredSearchController));

            // Add defaults last - after we've customized the preferences 
            Container.ApplyDefaultAlfredMappings();
        }

        /// <summary>
        /// Initializes the console for the application and returns the instantiated console. 
        /// </summary>
        /// <returns> The instantiated console. </returns>
        [NotNull]
        protected virtual IConsole InitializeConsole()
        {
            // Give Alfred a way to talk to the application 
            return new SimpleConsole(Container, new ExplorerEventFactory());
        }

        /// <summary>
        ///     Registers the individual subsystems.
        /// </summary>
        protected virtual void RegisterSubsystems()
        {
            Register(new AlfredCoreSubsystem(Container, Options.IncludeSearchModuleOnSearchPage));
            Register(new MindExplorerSubsystem(Container, Options.ShowMindExplorerPage));
        }
        /// <summary>
        /// Initializes and registers Alfred's subsystems. 
        /// </summary>
        private void InitializeSubsystems()
        {
            // Log header 
            Console?.Log(LogHeader, "Initializing subsystems", LogLevel.Verbose);

            // TODO: It'd be nice to replace this with reflection-based type loading 

            // Add Standard Subsystems 
            RegisterSubsystems();

            // Add any dynamic subsystems 
            foreach (var subsystem in Options.AdditionalSubsystems)
            {
                Alfred.Register(subsystem);
            }
        }

        /// <summary>
        /// Initializes the update pump that causes <see cref="Alfred"/> to update its modules. 
        /// </summary>
        protected abstract void InitializeUpdatePump();

        /// <summary>
        /// Registers this instance. 
        /// </summary>
        /// <param name="subsystem"> The subsystem. </param>
        protected void Register([NotNull] IAlfredSubsystem subsystem)
        {
            Contract.Requires(subsystem != null, "subsystem is null.");

            // Register the subsystem with its own type as the instance of that type 
            subsystem.RegisterAsProvidedInstance(Container);

            // Add the subsystem to Alfred 
            Alfred.Register(subsystem);
        }
    }
}