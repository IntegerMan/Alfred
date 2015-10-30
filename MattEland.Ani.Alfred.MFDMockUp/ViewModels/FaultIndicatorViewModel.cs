using System.Diagnostics.Contracts;

using MattEland.Ani.Alfred.MFDMockUp.Models;
using MattEland.Ani.Alfred.PresentationCommon.Helpers;
using MattEland.Common.Annotations;
using Assisticant.Fields;

namespace MattEland.Ani.Alfred.MFDMockUp.ViewModels
{
    /// <summary>
    ///     A ViewModel for a faultIndicator indicator. This is used for rendering faultIndicator instrumentation
    ///     lights. This class cannot be inherited.
    /// </summary>
    [ViewModelFor(typeof(FaultIndicatorModel))]
    public sealed class FaultIndicatorViewModel
    {

        private const double ActiveOpacity = 1.0;
        private const double InactiveOpacity = 0.5;

        /// <summary>
        ///     The model.
        /// </summary>
        [NotNull]
        private readonly FaultIndicatorModel _model;

        /// <summary>
        ///     The opacity.
        /// </summary>
        [NotNull]
        private readonly Computed<double> _opacity;

        /// <summary>
        ///     Initializes a new instance of the FaultIndicatorViewModel class.
        /// </summary>
        /// <param name="faultIndicator"> The fault indicator. </param>
        public FaultIndicatorViewModel([NotNull] FaultIndicatorModel faultIndicator)
        {
            Contract.Requires(faultIndicator != null);

            _model = faultIndicator;
            _opacity = new Computed<double>(() => IsActive ? ActiveOpacity : InactiveOpacity);
        }


        /// <summary>
        ///     Gets the fault indicator label for use in the user interface.
        /// </summary>
        /// <value>
        ///     The indicator label.
        /// </value>
        [NotNull]
        public string IndicatorLabel
        {
            get { return _model.IndicatorLabel; }
        }

        /// <summary>
        ///     Gets the indicator's opacity.
        /// </summary>
        /// <value>
        ///     The indicator's opacity.
        /// </value>
        public double IndicatorOpacity
        {
            get { return _opacity; }
        }

        /// <summary>
        ///     Gets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///     true if this instance is active, false if not.
        /// </value>
        public bool IsActive
        {
            get { return _model.IsActive; }
        }
    }

}