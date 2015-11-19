using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;

using Assisticant.Fields;

using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.Models.Buttons
{
    /// <summary>
    ///     A model that provides buttons based on the current state of a <see cref="MultifunctionDisplay"/>.
    /// </summary>
    public sealed class ButtonProvider : IButtonClickListener
    {
        /// <summary>
        ///     The bottom button strip.
        /// </summary>
        [NotNull]
        private readonly Observable<ButtonStripModel> _bottomButtons;

        /// <summary>
        ///     The left button strip.
        /// </summary>
        [NotNull]
        private readonly Observable<ButtonStripModel> _leftButtons;

        /// <summary>
        ///     The owner.
        /// </summary>
        [NotNull]
        private readonly MultifunctionDisplay _owner;

        /// <summary>
        ///     The right button strip.
        /// </summary>
        [NotNull]
        private readonly Observable<ButtonStripModel> _rightButtons;

        /// <summary>
        ///     The top button strip.
        /// </summary>
        [NotNull]
        private readonly Observable<ButtonStripModel> _topButtons;

        [NotNull]
        private MasterModeBase _masterMode;

        /// <summary>
        ///     Gets or sets the master mode.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when the value is null.
        /// </exception>
        /// <value>
        ///     The master mode.
        /// </value>
        [NotNull]
        public MasterModeBase MasterMode
        {
            [DebuggerStepThrough]
            get
            { return _masterMode; }
            [DebuggerStepThrough]
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                _masterMode = value;
            }
        }

        /// <summary>
        ///     Initializes a new instance of the ButtonProvider class.
        /// </summary>
        /// <param name="owner"> The owner. </param>
        /// <param name="mode"> The display master mode. </param>
        public ButtonProvider([NotNull] MultifunctionDisplay owner,
            [NotNull] MasterModeBase mode)
        {
            _owner = owner;
            _masterMode = mode;

            //- Create Observables
            _topButtons = new Observable<ButtonStripModel>(new ButtonStripModel(this, ButtonStripDock.Top));
            _bottomButtons = new Observable<ButtonStripModel>(new ButtonStripModel(this, ButtonStripDock.Bottom));
            _leftButtons = new Observable<ButtonStripModel>(new ButtonStripModel(this, ButtonStripDock.Left));
            _rightButtons = new Observable<ButtonStripModel>(new ButtonStripModel(this, ButtonStripDock.Right));
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
        ///     Gets screen change buttons.
        /// </summary>
        /// <returns>
        ///     An array of buttons.
        /// </returns>
        private ButtonModel[] GetScreenChangeButtons()
        {
            return MasterMode.GetScreenChangeButtons().ToArray();
        }

        /// <summary>
        ///     Gets screen command buttons.
        /// </summary>
        /// <returns>
        ///     An array of buttons.
        /// </returns>
        private ButtonModel[] GetScreenCommandButtons()
        {
            return MasterMode.GetScreenCommandButtons().ToArray();
        }

        /// <summary>
        ///     Executes when a button is clicked.
        /// </summary>
        /// <param name="button"> The button. </param>
        public void OnButtonClicked([NotNull] ButtonModel button)
        {
            _owner.OnButtonClicked(button);
        }

        /// <summary>
        ///     Processes the current state by updating the buttons accordingly.
        /// </summary>
        /// <param name="processor"> The processor. </param>
        /// <param name="result"> The result. </param>
        internal void ProcessCurrentState([NotNull] MFDProcessor processor,
            [NotNull] MFDProcessorResult result)
        {
            //- Contract Validate
            Contract.Requires(processor != null);
            Contract.Requires(result != null);

            var mode = result.CurrentMasterMode;

            // Top and bottom buttons relate to views
            TopButtons.SetButtons(GetScreenChangeButtons());
            BottomButtons.SetButtons(GetScreenCommandButtons());

            // Left and right buttons are based off of the current view

            var screen = result.CurrentScreenViewModel;

            if (screen != null)
            {
                LeftButtons.SetButtons(screen.GetButtons(result, ButtonStripDock.Left));
                RightButtons.SetButtons(screen.GetButtons(result, ButtonStripDock.Right));
            }
            else
            {
                LeftButtons.SetEmptyButtons(5);
                RightButtons.SetEmptyButtons(5);
            }

            foreach (var button in Buttons)
            {
                button.ProcessCurrentState(processor, result);
            }
        }

        /// <summary>
        ///     Gets the buttons in all of the button strips.
        /// </summary>
        /// <value>
        ///     The buttons.
        /// </value>
        [NotNull, ItemNotNull]
        internal IEnumerable<ButtonModel> Buttons
        {
            get
            {
                foreach (var button in TopButtons.Buttons)
                {
                    yield return button;
                }
                foreach (var button in RightButtons.Buttons)
                {
                    yield return button;
                }
                foreach (var button in BottomButtons.Buttons)
                {
                    yield return button;
                }
                foreach (var button in LeftButtons.Buttons)
                {
                    yield return button;
                }
            }
        }
    }

}
