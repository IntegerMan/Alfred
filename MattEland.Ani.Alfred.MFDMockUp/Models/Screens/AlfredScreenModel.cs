using System.Collections.Generic;
using System.Diagnostics.Contracts;

using MattEland.Ani.Alfred.Core;
using MattEland.Ani.Alfred.MFDMockUp.Models.Buttons;
using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.Models.Screens
{
    /// <summary>
    ///     A data model for the Alfred status screen. This class cannot be inherited.
    /// </summary>
    public sealed class AlfredScreenModel : ScreenModel
    {
        /// <summary>
        ///     The power button.
        /// </summary>
        [NotNull]
        private readonly ButtonModel _powerButton;

        /// <summary>
        ///     Initializes a new instance of the AlfredScreenModel class.
        /// </summary>
        public AlfredScreenModel([NotNull] AlfredApplication alfredApplication) : base("ALF")
        {
            Contract.Requires(alfredApplication != null);

            AlfredApplication = alfredApplication;

            _powerButton = new ButtonModel("PWR", null);
        }

        /// <summary>
        ///     Process the screen state and outputs any resulting information to the processorResult.
        /// </summary>
        /// <param name="processor"> The processor. </param>
        /// <param name="processorResult"> The processor result. </param>
        protected override void ProcessScreenState(MFDProcessor processor, MFDProcessorResult processorResult)
        {
            // No operation (yet)
        }

        /// <summary>
        ///     Gets the alfred application.
        /// </summary>
        /// <value>
        ///     The alfred application.
        /// </value>
        [NotNull]
        public AlfredApplication AlfredApplication
        {
            get;
        }

        /// <summary>
        ///     Gets the buttons to appear along an <paramref name="edge"/>.
        /// </summary>
        /// <param name="result"> The result. </param>
        /// <param name="edge"> The docking edge for the buttons to appear along. </param>
        /// <returns>
        ///     An enumerator that allows foreach to be used to process the buttons in this collection.
        /// </returns>
        internal override IEnumerable<ButtonModel> GetButtons(MFDProcessorResult result, ButtonStripDock edge)
        {
            if (edge == ButtonStripDock.Right)
            {
                // Spacers
                yield return null;
                yield return null;
                yield return null;

                // Activate / Deactivate
                yield return _powerButton;
            }
        }
    }
}
