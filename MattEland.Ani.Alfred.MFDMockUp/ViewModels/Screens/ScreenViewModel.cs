using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;

using MattEland.Ani.Alfred.MFDMockUp.Models;
using MattEland.Ani.Alfred.MFDMockUp.Models.Buttons;
using MattEland.Ani.Alfred.MFDMockUp.Models.Screens;
using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.ViewModels.Screens
{
    /// <summary>
    ///     An abstract ViewModel for screen-related view models.
    /// </summary>
    public abstract class ScreenViewModel : IHasDisplay
    {
        [NotNull]
        private readonly ScreenModel _screenModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        protected ScreenViewModel([NotNull] ScreenModel screenModel)
        {
            Contract.Requires(screenModel != null);
            Contract.Ensures(_screenModel != null);

            _screenModel = screenModel;
        }

        /// <summary>
        ///     Gets the screen model associated with this view model.
        /// </summary>
        /// <value>
        ///     The screen model.
        /// </value>
        [NotNull]
        public ScreenModel ScreenModel
        {
            [DebuggerStepThrough]
            get
            { return _screenModel; }
        }

        /// <summary>
        ///     Process the current state and outputs any resulting information to the processorResult.
        /// </summary>
        /// <param name="processor"> The processor. </param>
        /// <param name="processorResult"> The processor result. </param>
        public void ProcessCurrentState([NotNull] MFDProcessor processor,
                                        [NotNull] MFDProcessorResult processorResult)
        {
            //! Do NOT process the model's current state. Models invoke process on the view model

            ProcessScreenState(processor, processorResult);
        }

        /// <summary>
        ///     Process the screen state and outputs any resulting information to the processorResult.
        /// </summary>
        /// <param name="processor"> The processor. </param>
        /// <param name="processorResult"> The processor result. </param>
        [SuppressMessage("Usage", "CC0057:Unused parameters",
            Justification = "Intended for overriding implementations")]
        protected virtual void ProcessScreenState([NotNull] MFDProcessor processor,
                                                  [NotNull] MFDProcessorResult processorResult)
        {
            // Do nothing
        }

        /// <summary>
        ///     Gets the buttons to appear along an <paramref name="edge"/>.
        /// </summary>
        /// <param name="result"> The result. </param>
        /// <param name="edge"> The docking edge for the buttons to appear along. </param>
        /// <returns>
        ///     An enumerator that allows foreach to be used to process the buttons in this collection.
        /// </returns>
        [CanBeNull, ItemNotNull]
        [SuppressMessage("Usage", "CC0057:Unused parameters",
            Justification = "Intended for overriding implementations")]
        internal virtual IEnumerable<ButtonModel> GetButtons([NotNull] MFDProcessorResult result,
                                                             ButtonStripDock edge)
        {
            // Default implementation is no custom buttons but individual screens can customize this
            return null;
        }

        private MultifunctionDisplay _display;

        /// <summary>
        ///     Gets or sets the display.
        /// </summary>
        /// <value>
        ///     The display.
        /// </value>
        [CanBeNull]
        public MultifunctionDisplay Display
        {
            get { return _display; }
            set
            {
                _display = value;

                if (value != null)
                {
                    _screenModel.SetViewModelFor(value, this);
                }
            }
        }
    }
}
