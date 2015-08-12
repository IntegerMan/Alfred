// ---------------------------------------------------------
// AlfredBootstrapper.cs
// 
// Created on:      08/09/2015 at 5:10 PM
// Last Modified:   08/09/2015 at 5:29 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.Diagnostics;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.Core.Definitions;

namespace MattEland.Ani.Alfred.Core
{
    /// <summary>
    ///     A utility class used for instantiating the Alfred Framework
    /// </summary>
    public sealed class AlfredBootstrapper
    {
        [CanBeNull]
        private IConsole _console;

        [NotNull]
        private IPlatformProvider _platformProvider;

        [NotNull]
        private IStatusController _statusController;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AlfredBootstrapper" /> class.
        /// </summary>
        public AlfredBootstrapper() : this(new SimplePlatformProvider(), new AlfredStatusController(), null)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AlfredBootstrapper" /> class.
        /// </summary>
        /// <param name="platformProvider">The platform provider.</param>
        public AlfredBootstrapper([NotNull] IPlatformProvider platformProvider)
            : this(platformProvider, new AlfredStatusController(), null)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AlfredBootstrapper" /> class.
        /// </summary>
        /// <param name="statusController">The status controller.</param>
        public AlfredBootstrapper([NotNull] IStatusController statusController)
            : this(new SimplePlatformProvider(), statusController, null)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AlfredBootstrapper" /> class.
        /// </summary>
        /// <param name="platformProvider">The platform provider.</param>
        /// <param name="statusController">The status controller.</param>
        public AlfredBootstrapper([NotNull] IPlatformProvider platformProvider,
                                  [NotNull] IStatusController statusController)
            : this(platformProvider, statusController, null)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AlfredBootstrapper" /> class.
        /// </summary>
        /// <param name="platformProvider">The platform provider.</param>
        /// <param name="statusController">The status controller.</param>
        /// <param name="console">The console.</param>
        /// <exception cref="System.ArgumentNullException">
        /// </exception>
        public AlfredBootstrapper([NotNull] IPlatformProvider platformProvider,
                                  [NotNull] IStatusController statusController,
                                  [CanBeNull] IConsole console)
        {
            if (platformProvider == null)
            {
                throw new ArgumentNullException(nameof(platformProvider));
            }
            if (statusController == null)
            {
                throw new ArgumentNullException(nameof(statusController));
            }
            _console = console;
            _platformProvider = platformProvider;
            _statusController = statusController;
        }

        /// <summary>
        ///     Gets or sets the platform provider.
        /// </summary>
        /// <value>The platform provider.</value>
        /// <exception cref="System.ArgumentNullException"></exception>
        [NotNull]
        public IPlatformProvider PlatformProvider
        {
            [DebuggerStepThrough]
            get
            { return _platformProvider; }
            [DebuggerStepThrough]
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }
                _platformProvider = value;
            }
        }

        /// <summary>
        ///     Gets or sets the status controller.
        /// </summary>
        /// <value>The status controller.</value>
        /// <exception cref="System.ArgumentNullException"></exception>
        [NotNull]
        public IStatusController StatusController
        {
            [DebuggerStepThrough]
            get
            { return _statusController; }
            [DebuggerStepThrough]
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }
                _statusController = value;
            }
        }

        /// <summary>
        ///     Gets or sets the console.
        /// </summary>
        /// <value>The console.</value>
        [CanBeNull]
        public IConsole Console
        {
            [DebuggerStepThrough]
            get
            { return _console; }
            [DebuggerStepThrough]
            set
            { _console = value; }
        }

        /// <summary>
        ///     Creates a new instance of Alfred using this instance's properties and returns it.
        /// </summary>
        /// <returns>The Alfred instance.</returns>
        [NotNull]
        public AlfredApplication Create()
        {
            var alfred = new AlfredApplication(_platformProvider, _statusController);
            alfred.Console = _console;

            return alfred;
        }
    }
}