using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;

using Assisticant.Fields;

using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.Models
{
    /// <summary>
    ///     A model that provides buttons based on the current state of a <see cref="MultifunctionDisplay"/>.
    /// </summary>
    internal sealed class ButtonProvider
    {

        [NotNull]
        private readonly Observable<ButtonStripModel> _bottomButtons;

        [NotNull]
        private readonly Observable<ButtonStripModel> _leftButtons;

        /// <summary>
        ///     The owner.
        /// </summary>
        /// <remarks>
        /// TODO: Use this field
        /// </remarks>
        [NotNull]
        private readonly MultifunctionDisplay _owner;

        [NotNull]
        private readonly Observable<ButtonStripModel> _rightButtons;

        [NotNull]
        private readonly Observable<ButtonStripModel> _topButtons;

        /// <summary>
        ///     Initializes a new instance of the ButtonProvider class.
        /// </summary>
        /// <param name="owner"> The owner. </param>
        public ButtonProvider([NotNull] MultifunctionDisplay owner)
        {
            _owner = owner;

            //- Create Observables
            _topButtons = new Observable<ButtonStripModel>(new ButtonStripModel(ButtonStripDock.Top));
            _bottomButtons = new Observable<ButtonStripModel>(new ButtonStripModel(ButtonStripDock.Bottom));
            _leftButtons = new Observable<ButtonStripModel>(new ButtonStripModel(ButtonStripDock.Left));
            _rightButtons = new Observable<ButtonStripModel>(new ButtonStripModel(ButtonStripDock.Right));

            // Ensure buttons are up to date
            UpdateButtons();
        }

        /// <summary>
        ///     Gets the bottom buttons.
        /// </summary>
        /// <value>
        ///     The bottom buttons.
        /// </value>
        [NotNull]
        public ButtonStripModel BottomButtons { get { return _bottomButtons; } }

        /// <summary>
        ///     Gets the left buttons.
        /// </summary>
        /// <value>
        ///     The left buttons.
        /// </value>
        [NotNull]
        public ButtonStripModel LeftButtons { get { return _leftButtons; } }

        /// <summary>
        ///     Gets the right buttons.
        /// </summary>
        /// <value>
        ///     The right buttons.
        /// </value>
        [NotNull]
        public ButtonStripModel RightButtons { get { return _rightButtons; } }

        /// <summary>
        ///     Gets the top buttons.
        /// </summary>
        /// <value>
        ///     The top buttons.
        /// </value>
        [NotNull]
        public ButtonStripModel TopButtons { get { return _topButtons; } }

        /// <summary>
        ///     Updates the buttons.
        /// </summary>
        private void UpdateButtons()
        {
            // Top and bottom buttons relate to views
            // TODO: Populate these items more based on the current view

            TopButtons.SetButtons(new ButtonModel("SYS", true),
                                  new ButtonModel("ALFR"),
                                  new ButtonModel("LOG"),
                                  new ButtonModel("PERF"),
                                  new ButtonModel("MODE"));

            BottomButtons.SetButtons(new ButtonModel("WTHR"),
                                     new ButtonModel("SRCH"),
                                     new ButtonModel("MAP"),
                                     new ButtonModel("FEED"),
                                     new ButtonModel("OPTS"));

            // TODO: Left and right buttons will be based off of the current view

            LeftButtons.SetEmptyButtons(5);
            RightButtons.SetEmptyButtons(5);
        }
    }
}
