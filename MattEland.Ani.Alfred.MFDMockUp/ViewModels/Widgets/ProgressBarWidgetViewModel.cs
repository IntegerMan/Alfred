using System.Diagnostics;
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

            ProgressBar = widget;

            _minValue = new Observable<double>(0);
            _maxValue = new Observable<double>(100);
            _currentValue = new Observable<double>(0);
        }

        /// <summary>
        ///     Contains code contract invariants that describe facts about this class that will be true
        ///     after any public method in this class is called.
        /// </summary>
        [ContractInvariantMethod]
        private void ClassInvariants()
        {
            Contract.Invariant(_minValue != null);
            Contract.Invariant(_maxValue != null);
            Contract.Invariant(_currentValue != null);

            Contract.Invariant(ProgressBar != null);
            Contract.Invariant(MinValue <= MaxValue);
            Contract.Invariant(CurrentValue >= MinValue);
            Contract.Invariant(CurrentValue <= MaxValue);
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
            MinValue = ProgressBar.Minimum;
            CurrentValue = ProgressBar.Value;
            MaxValue = ProgressBar.Maximum;
        }
    }
}