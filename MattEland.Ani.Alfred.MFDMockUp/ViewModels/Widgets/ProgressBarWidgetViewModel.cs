using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

using Assisticant.Fields;

using MattEland.Ani.Alfred.Core.Widgets;
using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.ViewModels.Widgets
{
    /// <summary>
    ///     A ViewModel for a progress bar widget. This class cannot be inherited.
    /// </summary>
    public sealed class ProgressBarWidgetViewModel : WidgetViewModel
    {
        /// <summary>
        ///     The minimum value.
        /// </summary>
        [NotNull]
        private readonly Observable<double> _minValue;

        /// <summary>
        ///     The maximum value.
        /// </summary>
        [NotNull]
        private readonly Observable<double> _maxValue;

        /// <summary>
        ///     The current value.
        /// </summary>
        [NotNull]
        private readonly Observable<double> _currentValue;

        /// <summary>
        ///     Initializes a new instance of the ProgressBarWidgetViewModel class.
        /// </summary>
        /// <param name="widget"> The widget. </param>
        public ProgressBarWidgetViewModel([NotNull] ProgressBarWidget widget) : base(widget)
        {
            Contract.Requires(widget != null);

            //- Set up the concrete model
            ProgressBar = widget;

            //- Set up observables
            _minValue = new Observable<double>(0);
            _maxValue = new Observable<double>(100);
            _currentValue = new Observable<double>(0);

            // Grab values from the current state of the progress bar
            UpdateValues();
        }

        /// <summary>
        ///     Contains code contract invariants that describe facts about this class that will be true
        ///     after any public method in this class is called.
        /// </summary>
        [ContractInvariantMethod]
        [SuppressMessage("Usage", "CC0068:Unused Method", Justification = "Faulty analysis")]
        [SuppressMessage("Style", "CC0048:Use string interpolation instead of String.Format", Justification = "Faulty analysis")]
        [SuppressMessage("ReSharper", "RedundantAssignment")]
        private void ClassInvariants()
        {
            //- Not null invariants
            Contract.Invariant(_minValue != null);
            Contract.Invariant(_maxValue != null);
            Contract.Invariant(_currentValue != null);
            Contract.Invariant(ProgressBar != null);

            // Ensure that MinValue <= CurrentValue <= MaxValue
            string debugMessage;
            debugMessage = string.Format("Current = {0}, Min = {2}, Max = {1}",
                                         CurrentValue,
                                         MaxValue,
                                         MinValue);

            Contract.Invariant(MinValue <= MaxValue, debugMessage);
            Contract.Invariant(CurrentValue >= MinValue, debugMessage);
            Contract.Invariant(CurrentValue <= MaxValue, debugMessage);
        }

        /// <summary>
        ///     Gets the progress bar widget.
        /// </summary>
        /// <value>
        ///     The progress bar.
        /// </value>
        [NotNull]
        public ProgressBarWidget ProgressBar { get; }

        /// <summary>
        ///     Sets the minimum value of the progress bar.
        /// </summary>
        /// <value>
        ///     The minimum value.
        /// </value>
        public double MinValue
        {
            [DebuggerStepThrough]
            get
            { return _minValue; }
            set { _minValue.Value = value; }
        }

        /// <summary>
        ///     Sets the maximum value of the progress bar.
        /// </summary>
        /// <value>
        ///     The maximum value.
        /// </value>
        public double MaxValue
        {
            [DebuggerStepThrough]
            get
            { return _maxValue; }
            set { _maxValue.Value = value; }
        }

        /// <summary>
        ///     Sets the current value of the progress bar.
        /// </summary>
        /// <value>
        ///     The current value.
        /// </value>
        public double CurrentValue
        {
            [DebuggerStepThrough]
            get
            { return _currentValue; }
            set { _currentValue.Value = value; }
        }

        /// <summary>
        ///     Updates the values from the model.
        /// </summary>
        public override void UpdateValues()
        {
            // Use members so that invariants aren't evaluated until end of method
            _minValue.Value = ProgressBar.Minimum;
            _maxValue.Value = ProgressBar.Maximum;
            _currentValue.Value = ProgressBar.Value;
        }
    }
}