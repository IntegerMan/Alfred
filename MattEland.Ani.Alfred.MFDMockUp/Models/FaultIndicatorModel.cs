using System;
using System.Diagnostics.Contracts;

using Assisticant.Fields;

using MattEland.Common;
using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.Models
{
    /// <summary>
    ///     A data Model for faultIndicator indicators. This class cannot be inherited.
    /// </summary>
    public sealed class FaultIndicatorModel
    {
        /// <summary>
        ///     Initializes a new instance of the FaultIndicatorModel class.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when <paramref name="indicatorLabel"/> is not of the correct length.
        /// </exception>
        /// <param name="indicatorLabel"> The indicator label. </param>
        /// <param name="statusFunction"> The status calculation function. </param>
        public FaultIndicatorModel([NotNull] string indicatorLabel,
            [CanBeNull] Func<FaultIndicatorStatus> statusFunction)
        {
            Contract.Requires(indicatorLabel != null);
            Contract.Ensures(_status != null);
            Contract.Ensures(IndicatorLabel != null);

            // Remove all spacing
            indicatorLabel = indicatorLabel.NonNull().Replace(" ", string.Empty);

            // Validate the sanitized string fits within the allowable parameters
            const int MinCharacters = 1;
            const int MaxCharacters = 8;
            if (indicatorLabel.Length < MinCharacters || indicatorLabel.Length > MaxCharacters)
            {
                var message = string.Format("Indicators must be between {0} and {1} characters long without spacing.",
                                            MinCharacters,
                                            MaxCharacters);

                throw new ArgumentOutOfRangeException(nameof(indicatorLabel), message);
            }

            IndicatorLabel = indicatorLabel;

            _statusFunction = statusFunction;
            _status = new Observable<FaultIndicatorStatus>(FaultIndicatorStatus.Inactive);
        }

        /// <summary>
        ///     Gets the faultIndicator indicator label.
        /// </summary>
        /// <value>
        ///     The indicator label.
        /// </value>
        [NotNull]
        public string IndicatorLabel { get; }

        /// <summary>
        ///     The is active monitoring function.
        /// </summary>
        [CanBeNull]
        private readonly Func<FaultIndicatorStatus> _statusFunction;

        /// <summary>
        ///     The current status of the indicator.
        /// </summary>
        [NotNull]
        private readonly Observable<FaultIndicatorStatus> _status;

        /// <summary>
        ///     Gets or sets a value indicating whether this indicator is active.
        /// </summary>
        /// <value>
        ///     true if this indicator is active, false if not.
        /// </value>
        public FaultIndicatorStatus Status
        {
            get { return _status; }
            set { _status.Value = value; }
        }

        /// <summary>
        ///     Updates the <see cref="Status"/> status of the indicator based on the function provided
        ///     at instance creation.
        /// </summary>
        public void Update()
        {
            var func = _statusFunction;

            if (func != null)
            {
                Status = func();
            }
        }
    }
}