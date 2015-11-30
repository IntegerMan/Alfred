// ---------------------------------------------------------
// ScreenModel.cs
// 
// Created on:      10/24/2015 at 11:14 PM
// Last Modified:   11/03/2015 at 2:56 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics.Contracts;

using Assisticant.Fields;

using MattEland.Ani.Alfred.MFDMockUp.Models.Buttons;
using MattEland.Ani.Alfred.MFDMockUp.ViewModels.Screens;
using MattEland.Common;
using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.Models.Screens
{
    /// <summary>
    ///     A model for a multifunction display screen.
    /// </summary>
    public abstract class ScreenModel
    {

        /// <summary>
        ///     The button text.
        /// </summary>
        [NotNull]
        private readonly Observable<string> _buttonText;

        [NotNull, ItemNotNull]
        private readonly IDictionary<object, ScreenViewModel> _viewModels;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        protected ScreenModel(string buttonText)
        {
            Contract.Requires(buttonText.HasText());
            Contract.Ensures(_buttonText != null);
            Contract.Ensures(_buttonText.Value == buttonText);
            Contract.Ensures(_viewModels != null);

            _buttonText = new Observable<string>(buttonText);
            _viewModels = new Dictionary<object, ScreenViewModel>();
        }

        /// <summary>
        ///     Gets or sets the abbreviated page's name for use in a navigation button.
        /// </summary>
        /// <value>
        ///     The button text.
        /// </value>
        [CanBeNull]
        public string ButtonText
        {
            get { return _buttonText; }
            set { _buttonText.Value = value; }
        }

        /// <summary>
        ///     Gets a value indicating whether the sensor of interest indicator is shown.
        /// </summary>
        /// <value>
        ///     true if the screen shows the sensor of interest indicator, false if not.
        /// </value>
        public virtual bool ShowSensorOfInterestIndicator
        {
            get { return true; }
        }

        /// <summary>
        ///     Gets the command buttons associated with this screen. Return null for default commands.
        /// </summary>
        /// <value>
        ///     The command buttons.
        /// </value>
        [CanBeNull]
        public virtual IList<ButtonModel> CommandButtons
        {
            get { return null; }
        }

        /// <summary>
        ///     Process the current state and outputs any resulting information to the processorResult.
        /// </summary>
        /// <param name="processor"> The processor. </param>
        /// <param name="processorResult"> The processor result. </param>
        public void ProcessCurrentState([NotNull] MFDProcessor processor,
                                        [NotNull] MFDProcessorResult processorResult)
        {
            ProcessScreenState(processor, processorResult);

            // Tell any attached view model (for this display only!) to process as well
            var viewModel = GetViewModelFor(processor.MFD);
            viewModel?.ProcessCurrentState(processor, processorResult);
        }

        /// <summary>
        ///     Process the screen state and outputs any resulting information to the processorResult.
        /// </summary>
        /// <param name="processor"> The processor. </param>
        /// <param name="processorResult"> The processor result. </param>
        protected abstract void ProcessScreenState([NotNull] MFDProcessor processor,
                                                   [NotNull] MFDProcessorResult processorResult);

        /// <summary>
        ///     Gets view model for the specified <paramref name="display"/>.
        /// </summary>
        /// <param name="display"> The display. </param>
        /// <returns>
        ///     The view model for the display.
        /// </returns>
        [CanBeNull]
        public ScreenViewModel GetViewModelFor([NotNull] object display)
        {
            Contract.Requires(display != null);

            if (!_viewModels.ContainsKey(display))
            {
                return null;
            }

            return _viewModels[display];
        }

        /// <summary>
        ///     Sets the view model for the specified <paramref name="display"/>.
        /// </summary>
        /// <param name="display"> The display. </param>
        /// <param name="viewModel"> The view model. </param>
        public void SetViewModelFor([NotNull] object display, [NotNull] ScreenViewModel viewModel)
        {
            Contract.Requires(display != null);
            Contract.Requires(viewModel != null);
            Contract.Ensures(_viewModels.ContainsKey(display));
            Contract.Ensures(_viewModels[display] == viewModel);

            _viewModels[display] = viewModel;
        }
    }
}