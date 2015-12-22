using MattEland.Common;
using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.Models
{
    /// <summary>
    ///     A fault indicator provider. This is a convenience class for extracting away an indicator
    ///     with its update functions in a modular way.
    /// </summary>
    public abstract class FaultIndicatorProvider
    {
        /// <summary>
        ///     Initializes a new instance of the FaultIndicatorProvider class.
        /// </summary>
        /// <param name="label"> The label to use for the indicator. </param>
        protected FaultIndicatorProvider(string label)
        {
            Indicator = new FaultIndicatorModel(label.NonNull(), UpdateIndicatorStatus);
        }

        /// <summary>
        ///     Gets the indicator exposed by this provider. This should not be null.
        /// </summary>
        /// <value>
        ///     The indicator.
        /// </value>
        [NotNull]
        public FaultIndicatorModel Indicator { get; }

        /// <summary>
        ///     Calculates and returns the indicator's status.
        /// </summary>
        /// <returns>
        ///     The indicator status.
        /// </returns>
        [NotNull]
        public abstract FaultIndicatorStatusUpdate UpdateIndicatorStatus();
    }
}