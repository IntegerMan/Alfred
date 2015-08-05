// ---------------------------------------------------------
// AlfredProvider.cs
// 
// Created on:      07/25/2015 at 11:30 PM
// Last Modified:   08/05/2015 at 3:20 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Console;

namespace MattEland.Ani.Alfred.Core
{
    /// <summary>
    ///     Coordinates providing personal assistance to a user interface and receiving settings and queries back from the user
    ///     interface.
    /// </summary>
    public sealed class AlfredProvider : NotifyPropertyChangedBase, IDisposable
    {
        /// <summary>
        ///     The platform provider
        /// </summary>
        [NotNull]
        private readonly IPlatformProvider _platformProvider;

        /// <summary>
        ///     The status controller
        /// </summary>
        [NotNull]
        private readonly AlfredStatusController _statusController;

        /// <summary>
        ///     The status
        /// </summary>
        private AlfredStatus _status;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AlfredProvider" /> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public AlfredProvider([NotNull] IPlatformProvider provider)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            _statusController = new AlfredStatusController(this);

            _platformProvider = provider;

            Modules = provider.CreateCollection<AlfredModule>();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AlfredProvider" /> class.
        /// </summary>
        public AlfredProvider() : this(new SimplePlatformProvider())
        {
        }

        /// <summary>
        ///     Gets or sets the console provider. This can be null.
        /// </summary>
        /// <value>The console.</value>
        [CanBeNull]
        public IConsole Console { get; set; }

        /// <summary>
        ///     Gets the name and version of Alfred.
        /// </summary>
        /// <value>The name and version.</value>
        [NotNull]
        public static string NameAndVersion
        {
            get { return string.Format(CultureInfo.CurrentCulture, "{0} {1}", Name, Version); }
        }

        /// <summary>
        ///     Gets the name of the framework.
        /// </summary>
        /// <value>The name.</value>
        [NotNull]
        public static string Name
        {
            get { return Resources.AlfredProvider_Name.NonNull(); }
        }

        /// <summary>
        ///     Gets the name of the framework.
        /// </summary>
        /// <value>The name.</value>
        [NotNull]
        public static string Version
        {
            get
            {
                // TODO: Grab the assembly version
                return "0.1";
            }
        }

        /// <summary>
        ///     Gets the modules.
        /// </summary>
        /// <value>The modules.</value>
        [NotNull]
        [ItemNotNull]
        public ICollection<AlfredModule> Modules { get; }

        /// <summary>
        ///     Gets the status.
        /// </summary>
        /// <value>The status.</value>
        public AlfredStatus Status
        {
            get { return _status; }
            internal set
            {
                if (value != _status)
                {
                    _status = value;
                    OnPropertyChanged(nameof(Status));
                }
            }
        }

        /// <summary>
        ///     Gets the collection provider used for cross platform portability.
        /// </summary>
        /// <value>The collection provider.</value>
        [NotNull]
        public IPlatformProvider PlatformProvider
        {
            get { return _platformProvider; }
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            foreach (var module in Modules)
            {
                module.Dispose();
            }
        }

        /// <summary>
        ///     Tells Alfred it's okay to start itself up and begin operating.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        ///     Thrown if Alfred is already Online
        /// </exception>
        public void Initialize()
        {
            // This logic is a bit lengthy, so we'll have the status controller take care of it
            _statusController.Initialize();
        }

        /// <summary>
        ///     Tells Alfred to go ahead and shut down.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        ///     Thrown if Alfred is already Offline
        /// </exception>
        public void Shutdown()
        {
            // This process is a little lengthy so we'll have the status controller handle it
            _statusController.Shutdown();
        }

        /// <summary>
        ///     Tells modules to take a look at their content and update as needed.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        ///     Thrown if Alfred is not Online
        /// </exception>
        public void Update()
        {
            // Error check
            if (Status != AlfredStatus.Online)
            {
                throw new InvalidOperationException("Alfred must be online in order to update modules.");
            }

            foreach (var module in Modules)
            {
                module.Update();
            }
        }

        /// <summary>
        ///     Adds a module to alfred.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        ///     Thrown if Alfred is not Offline
        /// </exception>
        public void AddModule([NotNull] AlfredModule module)
        {
            if (module == null)
            {
                throw new ArgumentNullException(nameof(module));
            }

            AssertMustBeOffline();

            Modules.Add(module);
        }

        /// <summary>
        ///     Adds modules to Alfred in bulk.
        /// </summary>
        /// <param name="modules">The modules.</param>
        /// <exception cref="System.ArgumentNullException">
        ///     modules must be provided
        /// </exception>
        public void AddModules([NotNull] IEnumerable<AlfredModule> modules)
        {
            // Standard validation
            if (modules == null)
            {
                throw new ArgumentNullException(nameof(modules));
            }

            AssertMustBeOffline();

            // Ad each module using the standard AddModule function for now. This will make it easier to modify the process of registering a module
            foreach (var module in modules)
            {
                if (module == null)
                {
                    throw new ArgumentNullException(
                        nameof(modules),
                        Resources.AlfredProvider_AddModules_ErrorNullModule);
                }
                AddModule(module);
            }
        }

        /// <summary>
        ///     Checks that Alfred must be offline and throws an exception if it isn't.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">
        ///     Alfred must be offline in order to add modules.
        /// </exception>
        private void AssertMustBeOffline()
        {
            if (Status != AlfredStatus.Offline)
            {
                throw new InvalidOperationException(Resources.AlfredProvider_AssertMustBeOffline_ErrorNotOffline);
            }
        }
    }
}