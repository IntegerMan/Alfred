using System;
using System.Collections.Generic;
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

        [NotNull]
        private readonly Observable<ButtonStripModel> _bottomButtons;

        [NotNull]
        private readonly Observable<ButtonStripModel> _leftButtons;

        /// <summary>
        ///     The owner.
        /// </summary>
        [NotNull]
        private readonly MultifunctionDisplay _owner;

        [NotNull]
        private readonly Observable<ButtonStripModel> _rightButtons;

        [NotNull]
        private readonly Observable<ButtonStripModel> _topButtons;

        private readonly ButtonModel _systemButton;
        private readonly ButtonModel _alfredButton;
        private readonly ButtonModel _logButton;
        private readonly ButtonModel _performanceButton;
        private readonly ButtonModel _modeButton;
        private readonly IEnumerable<ButtonModel> _emptyButtons;
        private readonly ButtonModel _weatherButton;
        private readonly ButtonModel _searchButton;
        private readonly ButtonModel _mapButton;
        private readonly ButtonModel _feedButton;
        private readonly ButtonModel _optionsButton;

        /// <summary>
        ///     Initializes a new instance of the ButtonProvider class.
        /// </summary>
        /// <param name="owner"> The owner. </param>
        /// <param name="workspace"> The workspace. </param>
        public ButtonProvider([NotNull] MultifunctionDisplay owner, [NotNull] Workspace workspace)
        {
            _owner = owner;

            //- Create Observables
            _topButtons = new Observable<ButtonStripModel>(new ButtonStripModel(this, ButtonStripDock.Top));
            _bottomButtons = new Observable<ButtonStripModel>(new ButtonStripModel(this, ButtonStripDock.Bottom));
            _leftButtons = new Observable<ButtonStripModel>(new ButtonStripModel(this, ButtonStripDock.Left));
            _rightButtons = new Observable<ButtonStripModel>(new ButtonStripModel(this, ButtonStripDock.Right));

            // Set up buttons
            var screens = owner.ScreenProvider;
            _systemButton = new NavigationButtonModel(screens.HomeScreen, "SYS", this);
            _alfredButton = new NavigationButtonModel(screens.AlfredScreen, "ALFR", this);

            _logButton = new ButtonModel("LOG", this);
            _performanceButton = new ButtonModel("PERF", this);
            _modeButton = new ButtonModel("MODE", this);
            _weatherButton = new ButtonModel("WTHR", this);
            _searchButton = new ButtonModel("SRCH", this);
            _mapButton = new ButtonModel("MAP", this);
            _feedButton = new ButtonModel("FEED", this);
            _optionsButton = new ButtonModel("OPTS", this);

            // Set up placeholder button list
            _emptyButtons = ButtonStripModel.BuildEmptyButtons(5);
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
        ///     Gets bottom buttons.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when one or more arguments are outside the required range.
        /// </exception>
        /// <param name="mode"> The mode. </param>
        /// <returns>
        ///     An array of button model.
        /// </returns>
        private ButtonModel[] GetBottomButtons([NotNull] MFDMode mode)
        {
            IEnumerable<ButtonModel> buttons;

            switch (mode)
            {
                case MFDMode.Default:
                    buttons = new[]
                              {
                                _weatherButton,
                                _searchButton,
                                _mapButton,
                                _feedButton,
                                _optionsButton
                              };
                    break;

                case MFDMode.Bootup:
                    buttons = _emptyButtons;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }

            return buttons.ToArray();

        }

        /// <summary>
        ///     Gets the buttons.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when one or more arguments are outside the required range.
        /// </exception>
        /// <param name="mode"> The mode. </param>
        /// <returns>
        ///     An array of button model.
        /// </returns>
        private ButtonModel[] GetTopButtons(MFDMode mode)
        {
            IEnumerable<ButtonModel> buttons;

            switch (mode)
            {
                case MFDMode.Default:
                    buttons = new[]
                              {
                                  _systemButton,
                                  _alfredButton,
                                  _logButton,
                                  _performanceButton,
                                  _modeButton
                              };
                    break;

                case MFDMode.Bootup:
                    buttons = _emptyButtons;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }

            return buttons.ToArray();
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

            var mode = result.CurrentMode;

            // Top and bottom buttons relate to views
            TopButtons.SetButtons(GetTopButtons(mode));
            BottomButtons.SetButtons(GetBottomButtons(mode));

            // TODO: Left and right buttons will be based off of the current view

            var screen = result.CurrentScreen;

            LeftButtons.SetButtons(screen.GetButtons(result, ButtonStripDock.Left));
            RightButtons.SetButtons(screen.GetButtons(result, ButtonStripDock.Right));

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
