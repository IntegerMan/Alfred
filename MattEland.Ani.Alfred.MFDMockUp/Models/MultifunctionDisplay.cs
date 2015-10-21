using System;
using System.Collections.Generic;
using System.Linq;
using Assisticant.Fields;

using MattEland.Ani.Alfred.MFDMockUp.Views;
using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.Models
{

    public sealed class MultifunctionDisplay
    {
        /// <summary>
        ///     The default screen size.
        /// </summary>
        public const double DefaultScreenSize = 256;

        /// <summary>
        ///     The button provider.
        /// </summary>
        [NotNull]
        private readonly Observable<ButtonProvider> _buttonProvider;

        /// <summary>
        ///     The current view.
        /// </summary>
        [NotNull]
        private readonly Observable<Object> _currentView;

        [NotNull]
        private readonly Observable<string> _name;

        [NotNull]
        private readonly Observable<double> _screenHeight;

        [NotNull]
        private readonly Observable<double> _screenWidth;

        public MultifunctionDisplay()
        {
            _name = new Observable<string>("<New MFD>");

            // TODO: Don't do this. This is a Model creating a View.
            _currentView = new Observable<object>(new DefaultMFDView());

            _screenWidth = new Observable<double>(DefaultScreenSize);
            _screenHeight = new Observable<double>(DefaultScreenSize);

            _buttonProvider = new Observable<ButtonProvider>(new ButtonProvider(this));

        }

        /// <summary>
        ///     Gets the button provider associated with this display.
        /// </summary>
        /// <value>
        ///     The button provider.
        /// </value>
        [NotNull]
        public ButtonProvider ButtonProvider { get { return _buttonProvider; } }

        /// <summary>
        ///     Gets or sets the current view.
        /// </summary>
        /// <value>
        ///     The current view.
        /// </value>
        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView.Value = value; }
        }

        /// <summary>
        ///     Gets or sets the name of the multifunction display. This is different than the name of
        ///     its current view.
        /// </summary>
        /// <value>
        ///     The name of the MFD.
        /// </value>
        [CanBeNull]
        public string Name
        {
            get { return _name; }
            set { _name.Value = value; }
        }

        /// <summary>
        ///     Gets or sets the height of the screen.
        /// </summary>
        /// <value>
        ///     The height of the screen.
        /// </value>
        public double ScreenHeight
        {
            get { return _screenHeight; }
            set { _screenHeight.Value = value; }
        }

        /// <summary>
        ///     Gets or sets the width of the screen.
        /// </summary>
        /// <value>
        ///     The width of the screen.
        /// </value>
        public double ScreenWidth
        {
            get { return _screenWidth; }
            set { _screenWidth.Value = value; }
        }

    }
}
