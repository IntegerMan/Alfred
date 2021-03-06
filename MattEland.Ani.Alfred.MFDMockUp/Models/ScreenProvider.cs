using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

using MattEland.Ani.Alfred.Core.Modules.SysMonitor;
using MattEland.Ani.Alfred.MFDMockUp.Models.Screens;
using MattEland.Common;
using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.Models
{
    /// <summary>
    ///     A screen provider that manages screen creation and distribution. This class cannot be
    ///     inherited.
    /// </summary>
    public sealed class ScreenProvider
    {
        /// <summary>
        ///     The screens.
        /// </summary>
        [NotNull, ItemNotNull]
        private readonly IDictionary<Type, Lazy<ScreenModel>> _screens;

        /// <summary>
        ///     Initializes a new instance of the ScreenProvider class.
        /// </summary>
        /// <param name="mfd"> the display. </param>
        /// <param name="workspace"> The workspace. </param>
        public ScreenProvider([NotNull] MultifunctionDisplay mfd, [NotNull] Workspace workspace)
        {
            Contract.Requires(mfd != null);
            Contract.Requires(workspace != null);

            var alfred = workspace.AlfredApplication;

            _screens = new Dictionary<Type, Lazy<ScreenModel>>
            {
                [typeof(HomeScreenModel)] =
                    new Lazy<ScreenModel>(() => new HomeScreenModel(workspace.FaultManager)),

                [typeof(AlfredScreenModel)] =
                    new Lazy<ScreenModel>(() => new AlfredScreenModel(alfred)),

                [typeof(NotImplementedScreenModel)] =
                    new Lazy<ScreenModel>(() => new NotImplementedScreenModel()),

                [typeof(SystemPerformanceScreenModel)] =
                    new Lazy<ScreenModel>(() =>
                    {
                        var perfSys = alfred.Subsystems.FirstOfType<SystemMonitoringSubsystem>();
                        return new SystemPerformanceScreenModel(perfSys);
                    }),

                [typeof(BuiltInTestsScreenModel)] =
                    new Lazy<ScreenModel>(() => new BuiltInTestsScreenModel(workspace.FaultManager)),

                [typeof(LogScreenModel)] =
                    new Lazy<ScreenModel>(() => new LogScreenModel(workspace.LoggingConsole, mfd))
            };

        }

        /// <summary>
        ///     Contains code contract invariants that describe facts about this class that will be true
        ///     after any public method in this class is called.
        /// </summary>
        [ContractInvariantMethod]
        private void ClassInvariants()
        {
            Contract.Invariant(_screens != null);
            Contract.Invariant(_screens.All(s => s.Key != null));
            Contract.Invariant(_screens.All(s => s.Value != null));
        }

        /// <summary>
        ///     Gets the home screen.
        /// </summary>
        /// <value>
        ///     The home screen.
        /// </value>
        [NotNull]
        public ScreenModel HomeScreen
        {
            get
            {
                return GetScreen(typeof(HomeScreenModel));
            }
        }

        /// <summary>
        ///     Gets the alfred screen.
        /// </summary>
        /// <value>
        ///     The alfred screen.
        /// </value>
        [NotNull]
        public ScreenModel AlfredScreen
        {
            get { return GetScreen(typeof(AlfredScreenModel)); }
        }

        /// <summary>
        ///     Gets the log screen.
        /// </summary>
        /// <value>
        ///     The log screen.
        /// </value>
        [NotNull]
        public ScreenModel LogScreen
        {
            get { return GetScreen(typeof(LogScreenModel)); }
        }

        /// <summary>
        ///     Gets the performance screen.
        /// </summary>
        /// <value>
        ///     The performance screen.
        /// </value>
        [NotNull]
        public ScreenModel PerformanceScreen
        {
            get { return GetScreen(typeof(SystemPerformanceScreenModel)); }
        }

        /// <summary>
        ///     Gets the not implemented screen used for screens that are planned but not yet supported.
        /// </summary>
        /// <value>
        ///     The not implemented screen.
        /// </value>
        [NotNull]
        public ScreenModel NotImplementedScreen
        {
            get
            {
                return GetScreen(typeof(NotImplementedScreenModel));
            }
        }

        /// <summary>
        ///     Gets the built in tests screen.
        /// </summary>
        /// <value>
        ///     The built in tests screen.
        /// </value>
        [NotNull]
        public ScreenModel BuiltInTestsScreen
        {
            get
            {
                return GetScreen(typeof(BuiltInTestsScreenModel));
            }

        }

        /// <summary>
        ///     Gets a screen from the internal store and instantiates it as needed.
        /// </summary>
        /// <exception cref="KeyNotFoundException">
        ///     Thrown when no screen initializer has been defined for <paramref name="type"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     Thrown when the lazy initializer was null.
        /// </exception>
        /// <exception cref="NullReferenceException">
        ///     Thrown when a value was unexpectedly <lang keyword="null" />.
        /// </exception>
        /// <param name="type"> The type of screen to return. </param>
        /// <returns>
        ///     The screen.
        /// </returns>
        [NotNull]
        private ScreenModel GetScreen([NotNull] Type type)
        {
            // Ensure the key is found
            if (!_screens.ContainsKey(type))
            {
                var message = string.Format("Could not find a screen for type {0}", type.FullName);
                throw new KeyNotFoundException(message);
            }

            // Grab the initializer
            var lazyScreen = _screens[type];

            // Sanity check to ensure the initializer was found
            if (lazyScreen == null)
            {
                var message = string.Format("The lazy initializer was null for type {0}", type.FullName);
                throw new InvalidOperationException(message);
            }

            //! Grab the screen from the lazy initializer, creating it as needed
            var screenModel = lazyScreen.Value;

            // Validate that we're returning something
            if (screenModel == null)
            {
                var message = string.Format("The lazy initializer returned null for type {0}", type.FullName);
                throw new NullReferenceException(message);
            }

            return screenModel;
        }
    }
}