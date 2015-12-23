// ---------------------------------------------------------
// BuiltInTestsScreenViewModel.cs
// 
// Created on:      12/16/2015 at 10:28 PM
// Last Modified:   12/16/2015 at 10:28 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Assisticant.Fields;

using MattEland.Ani.Alfred.MFDMockUp.Models;
using MattEland.Ani.Alfred.MFDMockUp.Models.Screens;
using MattEland.Ani.Alfred.PresentationCommon.Helpers;
using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.ViewModels.Screens
{
    /// <summary>
    ///     A ViewModel for the built in tests screen. This class cannot be inherited.
    /// </summary>
    [ViewModelFor(typeof(BuiltInTestsScreenModel))]
    [UsedImplicitly]
    public sealed class BuiltInTestsScreenViewModel : ScreenViewModel
    {
        [NotNull, ItemNotNull]
        private readonly ICollection<FaultIndicatorViewModel> _activeFaults;

        [NotNull, ItemNotNull]
        private readonly IDictionary<FaultIndicatorModel, FaultIndicatorViewModel> _activeFaultViewModels;

        [NotNull]
        private readonly BuiltInTestsScreenModel _model;

        private DateTime _lastIndicatorUpdate;

        [NotNull]
        private readonly Observable<int> _pageSize;

        private readonly TimeSpan _indicatorUpdateFrequency;

        [NotNull]
        private readonly Computed<bool> _isNoItemsLabelVisible;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:BuiltInTestsScreenViewModel"/> class.
        /// </summary>
        public BuiltInTestsScreenViewModel([NotNull] BuiltInTestsScreenModel screenModel)
            : base(screenModel)
        {
            if (screenModel == null) throw new ArgumentNullException(nameof(screenModel));

            _model = screenModel;
            _activeFaultViewModels = new Dictionary<FaultIndicatorModel, FaultIndicatorViewModel>();
            _pageSize = new Observable<int>();
            _activeFaults = Container.ProvideCollection<FaultIndicatorViewModel>();
            _indicatorUpdateFrequency = TimeSpan.FromSeconds(1.0);
            _isNoItemsLabelVisible = new Computed<bool>(() => _activeFaults.Any());
        }

        /// <summary>
        ///     Gets the faults on the current page.
        /// </summary>
        /// <value>
        ///     The faults on the current page.
        /// </value>
        [NotNull, ItemNotNull]
        public IEnumerable<FaultIndicatorViewModel> FaultsOnCurrentPage
        {
            get
            {
                return PageSize <= 0 ? _activeFaults : _activeFaults.Take(PageSize);
            }
        }

        /// <summary>
        ///     Gets or sets the size of each page of view modeles.
        /// </summary>
        /// <value>
        ///     The size of the page.
        /// </value>
        public int PageSize
        {
            get { return _pageSize; }
            set
            {
                _pageSize.Value = value;
            }
        }

        /// <summary>
        ///     Process the screen state and outputs any resulting information to the processorResult.
        /// </summary>
        /// <param name="processor"> The processor. </param>
        /// <param name="processorResult"> The processor result. </param>
        protected override void ProcessScreenState(MFDProcessor processor, MFDProcessorResult processorResult)
        {
            // Only poll new incidators every so often
            if (_lastIndicatorUpdate == DateTime.MinValue ||
                DateTime.Now - _lastIndicatorUpdate > _indicatorUpdateFrequency)
            {
                // Update the indicators
                UpdateActiveIndicators();

                // This probably only needs to happen once, but it's not too expensive to do every refresh
                var provider = processor.MFD.ButtonProvider;
                PageSize = provider.LeftButtons.Buttons.Count() + provider.RightButtons.Buttons.Count();
            }

        }

        /// <summary>
        ///     Query if the <paramref name="status"/> is an active fault indicator status.
        /// </summary>
        /// <param name="status"> The status. </param>
        /// <returns>
        ///     true if an active fault, false otherwise.
        /// </returns>
        private static bool IsActiveFault(FaultIndicatorStatus status)
        {
            return status == FaultIndicatorStatus.Fault ||
                   status == FaultIndicatorStatus.Warning;
        }

        /// <summary>
        ///     Updates the active indicators collection.
        /// </summary>
        private void UpdateActiveIndicators()
        {
            var oldModels = _activeFaultViewModels.Keys.ToList();

            var models = _model.FaultManager.FaultIndicators;

            var activeModels = models.Where(m => IsActiveFault(m.Status)).ToList();

            var modelsToAdd = activeModels.Where(m => !oldModels.Contains(m));
            var modelsToRemove = oldModels.Where(m => !activeModels.Contains(m));

            // Add new indicators
            foreach (var model in modelsToAdd)
            {
                Debug.Assert(model != null);

                var vm = new FaultIndicatorViewModel(model);
                _activeFaults.Add(vm);
                _activeFaultViewModels[model] = vm;
            }

            // Remove prior indicators that are no longer active
            foreach (var model in modelsToRemove)
            {
                var vm = _activeFaultViewModels[model];
                _activeFaults.Remove(vm);
                _activeFaultViewModels.Remove(model);
            }

            _lastIndicatorUpdate = DateTime.Now;
        }

        /// <summary>
        ///     Gets a value indicating whether or not the no items label should be visible.
        /// </summary>
        /// <value>
        ///     true if the label should be visible, false if not.
        /// </value>
        public bool IsNoItemsLabelVisible
        {
            get { return _isNoItemsLabelVisible; }
        }

    }
}