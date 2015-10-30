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
        /// <param name="isActiveFunction"> The is active monitoring function. </param>
        public FaultIndicatorModel([NotNull] string indicatorLabel,
            [CanBeNull] Func<bool> isActiveFunction)
        {
            Contract.Requires(indicatorLabel != null);
            Contract.Ensures(_isActive != null);
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

            _isActive = new Observable<bool>();
            _isActiveFunction = isActiveFunction;
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
        ///     Whether or not the indicator is active
        /// </summary>
        [NotNull]
        private readonly Observable<bool> _isActive;

        /// <summary>
        ///     The is active monitoring function.
        /// </summary>
        [CanBeNull]
        private readonly Func<bool> _isActiveFunction;

        /// <summary>
        ///     Gets or sets a value indicating whether this indicator is active.
        /// </summary>
        /// <value>
        ///     true if this indicator is active, false if not.
        /// </value>
        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive.Value = value; }
        }

        /// <summary>
        ///     Updates the <see cref="IsActive"/> status of the indicator based on the function provided
        ///     at instance creation.
        /// </summary>
        public void Update()
        {
            var func = _isActiveFunction;

            if (func != null)
            {
                IsActive = func();
            }
        }
    }
}