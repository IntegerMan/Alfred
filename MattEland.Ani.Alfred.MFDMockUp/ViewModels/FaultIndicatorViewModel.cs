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

        /// <summary>
        ///     The offline indicator visibility.
        /// </summary>
        [NotNull]
        private readonly Computed<Visibility> _offlineVisibility;

        /// <summary>
        ///     The availability indicator visibility.
        /// </summary>
        [NotNull]
        private readonly Computed<Visibility> _availableVisibility;

        /// <summary>
        ///     The online indicator visibility.
        /// </summary>
        [NotNull]
        private readonly Computed<Visibility> _onlineVisibility;

        /// <summary>
        ///     The not online indicator visibility.
        /// </summary>
        [NotNull]
        private readonly Computed<Visibility> _notOnlineVisibility;

        /// <summary>
        ///     The fault indicator visibility.
        /// </summary>
        [NotNull]
        private readonly Computed<Visibility> _faultVisibility;

        /// <summary>
        ///     The warning indicator visibility.
        /// </summary>
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

            _offlineVisibility = new Computed<Visibility>(() => VisibileIfStatus(FaultIndicatorStatus.DisplayOffline));
            _availableVisibility = new Computed<Visibility>(() => VisibileIfStatus(FaultIndicatorStatus.Available));
            _onlineVisibility = new Computed<Visibility>(() => VisibileIfStatus(FaultIndicatorStatus.Online));
            _notOnlineVisibility = new Computed<Visibility>(() => CollapsedIfStatus(FaultIndicatorStatus.Online));
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

        /// <summary>
        ///     Collapsed if <see cref="Status"/> is the expected <paramref name="status"/>.
        /// </summary>
        /// <param name="status"> The indicator's status. </param>
        /// <returns>
        ///     A Visibility.
        /// </returns>
        private Visibility CollapsedIfStatus(FaultIndicatorStatus status)
        {
            return Status != status
                       ? Visibility.Visible
                       : Visibility.Collapsed;
        }

        /// <summary>
        ///     Gets the offline indicator's visibility.
        /// </summary>
        /// <value>
        ///     The offline visibility.
        /// </value>
        public Visibility OfflineVisibility
        {
            [DebuggerStepThrough]
            get
            { return _offlineVisibility; }
        }

        /// <summary>
        ///     Gets the online indicator's visibility.
        /// </summary>
        /// <value>
        ///     The online visibility.
        /// </value>
        public Visibility OnlineVisibility
        {
            [DebuggerStepThrough]
            get
            { return _onlineVisibility; }
        }

        /// <summary>
        ///     Gets the visibility for a NOT Online indicator.
        /// </summary>
        /// <value>
        ///     The not online visibility.
        /// </value>
        public Visibility NotOnlineVisibility
        {
            [DebuggerStepThrough]
            get
            { return _notOnlineVisibility; }
        }

        /// <summary>
        ///     Gets the availability indicator's visibility.
        /// </summary>
        /// <value>
        ///     The availability visibility.
        /// </value>
        public Visibility AvailabilityVisibility
        {
            [DebuggerStepThrough]
            get
            { return _availableVisibility; }
        }

        /// <summary>
        ///     Gets the warning indicator's visibility.
        /// </summary>
        /// <value>
        ///     The warning visibility.
        /// </value>
        public Visibility WarningVisibility
        {
            [DebuggerStepThrough]
            get
            { return _warningVisibility; }
        }

        /// <summary>
        ///     Gets the fault indicator's visibility.
        /// </summary>
        /// <value>
        ///     The fault visibility.
        /// </value>
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

        /// <summary>
        ///     Gets the model this view model is based on.
        /// </summary>
        /// <value>
        ///     The model.
        /// </value>
        [NotNull]
        public FaultIndicatorModel Model
        {
            get { return _model; }
        }

    }

}