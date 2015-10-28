using System;
using System.Collections.Generic;

using MattEland.Ani.Alfred.MFDMockUp.Models.Screens;
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

            _screens = new Dictionary<Type, Lazy<ScreenModel>>
            {
                [typeof(HomeScreenModel)] =
                    new Lazy<ScreenModel>(() => new HomeScreenModel()),

                [typeof(BootupScreenModel)] =
                    new Lazy<ScreenModel>(() => new BootupScreenModel(HomeScreen)),

                [typeof(AlfredScreenModel)] =
                    new Lazy<ScreenModel>(() => new AlfredScreenModel(workspace.AlfredApplication))
            };

        }

        /// <summary>
        ///     Gets the boot screen.
        /// </summary>
        /// <value>
        ///     The boot screen.
        /// </value>
        [NotNull]
        public ScreenModel BootScreen
        {
            get { return GetScreen(typeof(BootupScreenModel)); }
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
            Lazy<ScreenModel> lazyScreen = _screens[type];

            // Sanity check to ensure the initializer was found
            if (lazyScreen == null)
            {
                var message = string.Format("The lazy initializer was null for type {0}", type.FullName);
                throw new InvalidOperationException(message);
            }

            //! Grab the screen from the lazy initializer, creating it as needed
            ScreenModel screenModel = lazyScreen.Value;

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