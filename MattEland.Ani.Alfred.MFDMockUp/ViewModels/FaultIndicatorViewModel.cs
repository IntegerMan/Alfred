using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Windows;

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

        /// <summary>
        ///     The model.
        /// </summary>
        [NotNull]
        private readonly FaultIndicatorModel _model;

        [NotNull]
        private readonly Computed<Visibility> _offlineVisibility;

        [NotNull]
        private readonly Computed<Visibility> _availableVisibility;

        [NotNull]
        private readonly Computed<Visibility> _onlineVisibility;

        [NotNull]
        private readonly Computed<Visibility> _faultVisibility;

        [NotNull]
        private readonly Computed<Visibility> _warningVisibility;

        /// <summary>
        ///     Initializes a new instance of the FaultIndicatorViewModel class.
        /// </summary>
        /// <param name="faultIndicator"> The fault indicator. </param>
        public FaultIndicatorViewModel([NotNull] FaultIndicatorModel faultIndicator)
        {
            Contract.Requires(faultIndicator != null);
            Contract.Ensures(_model != null);

            _model = faultIndicator;
            _offlineVisibility =
                new Computed<Visibility>(
                    () =>
                    VisibileIfStatus(FaultIndicatorStatus.DisplayOffline));

            _availableVisibility = new Computed<Visibility>(() => VisibileIfStatus(FaultIndicatorStatus.Available));
            _onlineVisibility = new Computed<Visibility>(() => VisibileIfStatus(FaultIndicatorStatus.Online));
            _faultVisibility = new Computed<Visibility>(() => VisibileIfStatus(FaultIndicatorStatus.Fault));
            _warningVisibility = new Computed<Visibility>(() => VisibileIfStatus(FaultIndicatorStatus.Warning));
        }

        /// <summary>
        ///     Visibile if <see cref="Status"/> is the expected <paramref name="status"/>.
        /// </summary>
        /// <param name="status"> The indicator's status. </param>
        /// <returns>
        ///     A Visibility.
        /// </returns>
        private Visibility VisibileIfStatus(FaultIndicatorStatus status)
        {
            return Status == status
                       ? Visibility.Visible
                       : Visibility.Collapsed;
        }

        public Visibility OfflineVisibility
        {
            [DebuggerStepThrough]
            get
            { return _offlineVisibility; }
        }

        public Visibility OnlineVisibility
        {
            [DebuggerStepThrough]
            get
            { return _onlineVisibility; }
        }

        public Visibility WarningVisibility
        {
            [DebuggerStepThrough]
            get
            { return _warningVisibility; }
        }

        public Visibility FaultVisibility
        {
            [DebuggerStepThrough]
            get
            { return _faultVisibility; }
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
        ///     Gets a value indicating the status of this indicator.
        /// </summary>
        /// <value>
        ///     The indicator's status.
        /// </value>
        public FaultIndicatorStatus Status
        {
            get { return _model.Status; }
        }

    }

}