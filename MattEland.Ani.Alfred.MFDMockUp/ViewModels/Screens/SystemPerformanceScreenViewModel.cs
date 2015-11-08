// ---------------------------------------------------------
// SystemPerformanceScreenViewModel.cs
// 
// Created on:      11/04/2015 at 9:15 PM
// Last Modified:   11/05/2015 at 3:51 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Windows;

using Assisticant.Fields;

using MattEland.Ani.Alfred.MFDMockUp.Models;
using MattEland.Ani.Alfred.MFDMockUp.Models.Screens;
using MattEland.Ani.Alfred.MFDMockUp.ViewModels.Widgets;
using MattEland.Ani.Alfred.PresentationCommon.Helpers;
using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.ViewModels.Screens
{
    /// <summary>
    ///     A ViewModel for the system performance screen. This class cannot be inherited.
    /// </summary>
    [ViewModelFor(typeof(SystemPerformanceScreenModel))]
    public sealed class SystemPerformanceScreenViewModel : ScreenViewModel
    {
        /// <summary>
        ///     The performance screen model.
        /// </summary>
        [NotNull]
        private readonly SystemPerformanceScreenModel _model;

        [NotNull]
        private readonly Computed<bool> _showStandbyLabel;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <param name="screenModel"> The screen model. </param>
        public SystemPerformanceScreenViewModel([NotNull] SystemPerformanceScreenModel screenModel)
            : base(screenModel)
        {
            Contract.Requires(screenModel != null);

            _model = screenModel;
            _showStandbyLabel = new Computed<bool>(() => !_model.IsSubsystemOnline);
        }

        /// <summary>
        ///     Gets the performance widgets.
        /// </summary>
        /// <value>
        ///     The performance widgets.
        /// </value>
        [NotNull, ItemNotNull]
        public IEnumerable<WidgetViewModel> PerformanceWidgets
        {
            get { return _model.Widgets.Select(WidgetViewModelFactory.ViewModelFor); }
        }

        /// <summary>
        ///     Gets whether or not to show the standby label.
        /// </summary>
        /// <value>
        ///     True if the label should be shown, false if the main content should be shown.
        /// </value>
        public bool ShowStandbyLabel
        {
            get { return _showStandbyLabel.Value; }
        }

        /// <summary>
        ///     Process the screen state and outputs any resulting information to the processorResult.
        /// </summary>
        /// <param name="processor"> The processor. </param>
        /// <param name="processorResult"> The processor result. </param>
        protected override void ProcessScreenState(MFDProcessor processor,
                                                   MFDProcessorResult processorResult)
        {
        }
    }

}