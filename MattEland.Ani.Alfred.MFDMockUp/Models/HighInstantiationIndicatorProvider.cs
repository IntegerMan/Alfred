using MattEland.Common;
using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.Models
{
    /// <summary>
    ///     A high instantiation indicator provider that presents an indicator based on the frequency of object instantiations.
    /// This class cannot be inherited.
    /// </summary>
    public sealed class HighInstantiationIndicatorProvider : FaultIndicatorProvider
    {

        [NotNull]
        private readonly InstantiationMonitor _monitor;

        /// <summary>
        ///     Initializes a new instance of the HighInstantiationIndicatorProvider class.
        /// </summary>
        public HighInstantiationIndicatorProvider() : base("HI-INST")
        {
            _monitor = InstantiationMonitor.Instance;
        }

        /// <summary>
        ///     Calculates and returns the high instantiation indicator's status.
        /// </summary>
        /// <returns>
        ///     The high instantiation indicator status.
        /// </returns>
        public override FaultIndicatorStatusUpdate UpdateIndicatorStatus()
        {

            // Figure out our current status
            var status = GetStatus();

            // Build out an indicator
            var newItems = _monitor.NewItemsLastMeasurement;
            var message = string.Format("{0} {1} since last update",
                                        newItems,
                                        newItems.Pluralize("instantiation", "instantiations"));

            // Return our current status
            return new FaultIndicatorStatusUpdate(status, message);
        }

        /// <summary>
        ///     Gets the status based on the number of items instantiated since last frame.
        /// </summary>
        /// <returns>
        ///     The indicator status.
        /// </returns>
        private FaultIndicatorStatus GetStatus()
        {
            var newItems = _monitor.NewItemsLastMeasurement;

            return newItems < HighInstantiationCountWarnThreshhold
                       ? FaultIndicatorStatus.Inactive
                       : (newItems < HighInstantiationCountFaultThreshhold
                              ? FaultIndicatorStatus.Warning
                              : FaultIndicatorStatus.Fault);
        }

        /// <summary>
        ///     The high instantiation count threshhold. If the instantiation count for a given frame
        ///     matches or exceeds this, this indicator will be tripped as a warning.
        /// </summary>
        public int HighInstantiationCountWarnThreshhold { get; set; } = 8;

        /// <summary>
        ///     The high instantiation count threshhold. If the instantiation count for a given frame
        ///     matches or exceeds this, this indicator will be tripped as a fault.
        /// </summary>
        public int HighInstantiationCountFaultThreshhold { get; set; } = 32;

    }
}