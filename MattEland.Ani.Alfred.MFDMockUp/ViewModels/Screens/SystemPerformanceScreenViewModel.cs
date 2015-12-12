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

using Assisticant.Collections;
using Assisticant.Fields;

using MattEland.Ani.Alfred.MFDMockUp.Models;
using MattEland.Ani.Alfred.MFDMockUp.Models.Screens;
using MattEland.Ani.Alfred.MFDMockUp.ViewModels.Widgets;
using MattEland.Ani.Alfred.PresentationCommon.Helpers;
using MattEland.Common;
using MattEland.Common.Annotations;
using MattEland.Presentation.Logical.Widgets;

namespace MattEland.Ani.Alfred.MFDMockUp.ViewModels.Screens
{
    /// <summary>
    ///     A ViewModel for the system performance screen. This class cannot be inherited.
    /// </summary>
    [ViewModelFor(typeof(SystemPerformanceScreenModel))]
    [UsedImplicitly]
    public sealed class SystemPerformanceScreenViewModel : ScreenViewModel
    {
        /// <summary>
        ///     The performance screen model.
        /// </summary>
        [NotNull]
        private readonly SystemPerformanceScreenModel _model;

        [NotNull]
        private readonly Computed<bool> _showStandbyLabel;

        [NotNull, ItemNotNull]
        private readonly ObservableList<WidgetViewModel> _currentWidgets;

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

            // This will be updated later on, but for now, we'll start it at an empty collection to protect against null
            _currentWidgets = new ObservableList<WidgetViewModel>();
            _widgetCache = new Dictionary<string, WidgetViewModel>();
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
            get
            {
                Contract.Ensures(_currentWidgets.Count > 0);

                if (!_currentWidgets.Any()) InitializePerformanceCounterWidgets();

                // Always return the observable collection
                return _currentWidgets;
            }
        }

        private void InitializePerformanceCounterWidgets()
        {
            // We don't want to be clearing things out, but also assume that if this is called all items are gone
            Contract.Requires(_currentWidgets.Count == 0);

            // Generate the new collection of widgets
            var widgets = _model.Widgets.Select(w => GetOrCreateViewModelFor(w, w.Name)).ToList();

            foreach (var widgetViewModel in widgets)
            {
                _currentWidgets.Add(widgetViewModel);
            }
        }

        [NotNull, ItemNotNull]
        private readonly IDictionary<string, WidgetViewModel> _widgetCache;

        /// <summary>
        ///     Gets a cached view model or creates a new view model for the specified
        ///     <paramref name="widget"/>.
        /// </summary>
        /// <remarks>
        ///     We could use widget.Name instead of <paramref name="id"/>, but this way lets us use
        ///     contract assertions and provides some better flexibility should the identifying function
        ///     change.
        /// </remarks>
        /// <param name="widget"> The widget. </param>
        /// <param name="id"> The identifier. </param>
        /// <returns>
        ///     The view model.
        /// </returns>
        [NotNull]
        private WidgetViewModel GetOrCreateViewModelFor([NotNull] IWidget widget, [NotNull] string id)
        {
            Contract.Requires(widget != null);
            Contract.Requires(id.HasText());
            Contract.Ensures(_widgetCache.ContainsKey(id));
            Contract.Ensures(Contract.Result<WidgetViewModel>() != null);

            WidgetViewModel widgetVM = null;

            if (_widgetCache.ContainsKey(id))
            {
                widgetVM = _widgetCache[id];
            }

            if (widgetVM == null)
            {
                widgetVM = WidgetViewModelFactory.ViewModelFor(widget);

                _widgetCache[id] = widgetVM;
            }

            return widgetVM;
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
        ///     Gets the error text representing the last error associated with this module.
        /// </summary>
        /// <value>
        ///     The error text.
        /// </value>
        public string ErrorText
        {
            get
            {
                return _model.Subsystem.LastError?.Message;
            }
        }

        /// <summary>
        ///     Process the screen state and outputs any resulting information to the processorResult.
        /// </summary>
        /// <param name="processor"> The processor. </param>
        /// <param name="processorResult"> The processor result. </param>
        protected override void ProcessScreenState(MFDProcessor processor,
                                                   MFDProcessorResult processorResult)
        {
            // Update the current widgets to the current values from the widget model
            foreach (var widgetViewModel in _currentWidgets)
            {
                widgetViewModel.UpdateValues();
            }
        }
    }

}