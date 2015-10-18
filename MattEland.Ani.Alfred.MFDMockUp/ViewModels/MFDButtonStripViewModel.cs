using System;
using System.Windows.Controls;

using MattEland.Ani.Alfred.MFDMockUp.Models;
using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.ViewModels
{
    /// <summary>
    ///     A ViewModel for a multifunction display's button strip bordering one side of the screen.
    ///     This class cannot be inherited.
    /// </summary>
    public sealed class MFDButtonStripViewModel
    {
        [NotNull]
        private readonly ButtonStripModel _model;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MFDButtonStripViewModel"/> class.
        /// </summary>
        /// <param name="model"> The model. </param>
        public MFDButtonStripViewModel([NotNull] ButtonStripModel model)
        {
            _model = model;
        }

        /// <summary>
        ///     Gets the docked state of the button strip.
        /// </summary>
        /// <value>
        ///     The docked state of the button strip.
        /// </value>
        public ButtonStripDock Dock
        {
            get { return _model.Dock; }
        }

        /// <summary>
        ///     Gets the orientation of the arrangement panel.
        /// </summary>
        /// <value>
        ///     The orientation.
        /// </value>
        public Orientation Orientation
        {
            get
            {
                switch (Dock)
                {
                    case ButtonStripDock.Top:
                    case ButtonStripDock.Bottom:
                        return Orientation.Horizontal;

                    case ButtonStripDock.Left:
                    case ButtonStripDock.Right:
                    default:
                        return Orientation.Vertical;
                }

            }
        }
    }
}