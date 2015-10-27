using System.Collections;
using System.Collections.Generic;

using Assisticant.Fields;

using MattEland.Ani.Alfred.MFDMockUp.Models.Buttons;
using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.Models.Screens
{
    /// <summary>
    ///     A model for a multifunction display screen.
    /// </summary>
    public abstract class ScreenModel
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        protected ScreenModel(string buttonText)
        {
            _buttonText = new Observable<string>(buttonText);
        }

        /// <summary>
        ///     The button text.
        /// </summary>
        [NotNull]
        private readonly Observable<string> _buttonText;

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
        ///     Process the current state and outputs any resulting information to the processorResult.
        /// </summary>
        /// <param name="processor"> The processor. </param>
        /// <param name="processorResult"> The processor result. </param>
        public void ProcessCurrentState([NotNull] MFDProcessor processor,
            [NotNull] MFDProcessorResult processorResult)
        {

            ProcessScreenState(processor, processorResult);
        }

        /// <summary>
        ///     Process the screen state and outputs any resulting information to the processorResult.
        /// </summary>
        /// <param name="processor"> The processor. </param>
        /// <param name="processorResult"> The processor result. </param>
        protected abstract void ProcessScreenState([NotNull] MFDProcessor processor,
            [NotNull] MFDProcessorResult processorResult);

        /// <summary>
        ///     Gets the buttons to appear along an <paramref name="edge"/>.
        /// </summary>
        /// <param name="result"> The result. </param>
        /// <param name="edge"> The docking edge for the buttons to appear along. </param>
        /// <returns>
        ///     An enumerator that allows foreach to be used to process the buttons in this collection.
        /// </returns>
        [CanBeNull, ItemNotNull]
        internal virtual IEnumerable<ButtonModel> GetButtons([NotNull] MFDProcessorResult result,
            ButtonStripDock edge)
        {

            // Default implementation is no custom buttons but individual screens can customize this
            return null;
        }
    }
}