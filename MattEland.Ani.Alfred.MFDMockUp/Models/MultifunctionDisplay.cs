using System;
using System.Collections.Generic;
using System.Linq;
using Assisticant.Fields;

using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.Models
{

    public sealed class MultifunctionDisplay
    {
        /// <summary>
        ///     The default screen size.
        /// </summary>
        public const double DefaultScreenSize = 256;

        [NotNull]
        private readonly Observable<ButtonStripModel> _bottomButtons;

        [NotNull]
        private readonly Observable<ButtonStripModel> _leftButtons;

        [NotNull]
        private readonly Observable<string> _name;

        [NotNull]
        private readonly Observable<ButtonStripModel> _rightButtons;

        [NotNull]
        private readonly Observable<double> _screenHeight;

        [NotNull]
        private readonly Observable<double> _screenWidth;

        [NotNull]
        private readonly Observable<ButtonStripModel> _topButtons;

        public MultifunctionDisplay()
        {
            _name = new Observable<string>("<New MFD>");

            _screenWidth = new Observable<double>(DefaultScreenSize);
            _screenHeight = new Observable<double>(DefaultScreenSize);

            _topButtons = new Observable<ButtonStripModel>(new ButtonStripModel(ButtonStripDock.Top));
            _bottomButtons = new Observable<ButtonStripModel>(new ButtonStripModel(ButtonStripDock.Bottom));
            _leftButtons = new Observable<ButtonStripModel>(new ButtonStripModel(ButtonStripDock.Left));
            _rightButtons = new Observable<ButtonStripModel>(new ButtonStripModel(ButtonStripDock.Right));
        }

        /// <summary>
        ///     Gets or sets the bottom buttons.
        /// </summary>
        /// <value>
        ///     The bottom buttons.
        /// </value>
        [NotNull]
        public ButtonStripModel BottomButtons
        {
            get { return _bottomButtons; }
            set { _bottomButtons.Value = value; }
        }

        /// <summary>
        ///     Gets or sets the left buttons.
        /// </summary>
        /// <value>
        ///     The left buttons.
        /// </value>
        [NotNull]
        public ButtonStripModel LeftButtons
        {
            get { return _leftButtons; }
            set { _leftButtons.Value = value; }
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

        [NotNull]
        public ButtonStripModel RightButtons
        {
            get { return _rightButtons; }
            set { _rightButtons.Value = value; }
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

        [NotNull]
        public ButtonStripModel TopButtons
        {
            get { return _topButtons; }
            set { _topButtons.Value = value; }
        }
    }
}
