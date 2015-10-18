using System;
using System.Collections.Generic;
using System.Linq;
using Assisticant.Fields;

using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.Models
{
    public sealed class MultifunctionDisplay
    {
        [NotNull]
        private readonly Observable<string> _name = new Observable<string>("<New MFD>");

        [CanBeNull]
        public string Name
        {
            get { return _name; }
            set { _name.Value = value; }
        }

        [NotNull]
        private readonly Observable<double> _screenWidth;

        [NotNull]
        private readonly Observable<double> _screenHeight;

        public MultifunctionDisplay()
        {
            _screenWidth = new Observable<double>(512);
            _screenHeight = new Observable<double>(512);
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
    }
}
