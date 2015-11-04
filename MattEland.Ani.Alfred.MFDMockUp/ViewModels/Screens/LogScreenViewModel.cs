// ---------------------------------------------------------
// LogScreenViewModel.cs
// 
// Created on:      11/02/2015 at 9:41 PM
// Last Modified:   11/02/2015 at 9:41 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;

using Assisticant;
using Assisticant.Collections;

using MattEland.Ani.Alfred.MFDMockUp.Models.Screens;
using MattEland.Ani.Alfred.PresentationCommon.Helpers;
using MattEland.Common.Annotations;
using Assisticant.Fields;

using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.MFDMockUp.Models;
using MattEland.Ani.Alfred.MFDMockUp.Models.Buttons;

namespace MattEland.Ani.Alfred.MFDMockUp.ViewModels.Screens
{
    /// <summary>
    ///     A ViewModel for the log screen. This class cannot be inherited.
    /// </summary>
    [ViewModelFor(typeof(LogScreenModel))]
    public sealed class LogScreenViewModel : ScreenViewModel
    {
        private const string NextPageString = "NEXT\nPAGE";
        private const string PrevPageString = "PREV\nPAGE";

        /// <summary>
        ///     The log screen model the data is wrapped around.
        /// </summary>
        [NotNull]
        private readonly LogScreenModel _model;

        /// <summary>
        ///     The current page.
        /// </summary>
        [NotNull]
        private readonly Observable<int> _currentPage;

        /// <summary>
        ///     Size of each page.
        /// </summary>
        [NotNull]
        private readonly Observable<int> _pageSize;

        /// <summary>
        ///     The entries on the current page.
        /// </summary>
        [NotNull, ItemNotNull]
        private readonly ObservableList<LogEntryViewModel> _currentEntries;

        /// <summary>
        ///     The total number of pages.
        /// </summary>
        [NotNull]
        private readonly Observable<int> _totalPages;

        /// <summary>
        ///     The right edge's buttons.
        /// </summary>
        [NotNull, ItemNotNull]
        private readonly List<ButtonModel> _rightButtons;

        /// <summary>
        ///     The left edge's buttons.
        /// </summary>
        [NotNull, ItemNotNull]
        private readonly List<ButtonModel> _leftButtons;

        /// <summary>
        ///     The total number of log entries.
        /// </summary>
        [NotNull]
        private readonly Observable<int> _totalEntries;

        /// <summary>
        ///     true if entries were invalidated and the entries item needs to be updated next update.
        /// </summary>
        private bool _entriesInvalidated = true;

        [NotNull]
        private readonly ActionButtonModel _previousPageButton;

        [NotNull]
        private readonly ActionButtonModel _nextPageButton;

        [NotNull]
        private readonly Observable<bool> _canMoveToNextPage;
        [NotNull]
        private readonly Observable<bool> _canMoveToPreviousPage;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        [UsedImplicitly]
        public LogScreenViewModel([NotNull] LogScreenModel model) : base(model)
        {
            Contract.Requires(model != null);

            _model = model;
            _currentPage = new Observable<int>(1);
            _pageSize = new Observable<int>(12);
            _totalPages = new Observable<int>(1);

            _currentEntries = new ObservableList<LogEntryViewModel>();

            _canMoveToNextPage = new Observable<bool>();
            _canMoveToPreviousPage = new Observable<bool>();

            // Build out the button strips with some spacer items
            _previousPageButton = new ActionButtonModel(PrevPageString, MoveToPreviousPage);
            _leftButtons = new List<ButtonModel>
                            {
                                ButtonStripModel.BuildEmptyButton(),
                                ButtonStripModel.BuildEmptyButton(1),
                                ButtonStripModel.BuildEmptyButton(2),
                                _previousPageButton,
                                ButtonStripModel.BuildEmptyButton(4)
                            };

            _nextPageButton = new ActionButtonModel(NextPageString, MoveToNextPage);
            _rightButtons = new List<ButtonModel>
                            {
                                ButtonStripModel.BuildEmptyButton(),
                                ButtonStripModel.BuildEmptyButton(1),
                                ButtonStripModel.BuildEmptyButton(2),
                                _nextPageButton,
                                ButtonStripModel.BuildEmptyButton(4)
                            };

            _totalEntries = new Observable<int>();
        }

        /// <summary>
        ///     Contains invariants related to this class
        /// </summary>
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_model != null);
            Contract.Invariant(_currentPage != null);
            Contract.Invariant(_currentPage.Value > 0);
            Contract.Invariant(_leftButtons != null);
            Contract.Invariant(_rightButtons != null);
            Contract.Invariant(_pageSize != null);
            Contract.Invariant(_pageSize.Value > 0);
            Contract.Invariant(_currentEntries != null);
            Contract.Invariant(_canMoveToNextPage != null);
            Contract.Invariant(_canMoveToPreviousPage != null);
            Contract.Invariant(_totalPages != null);
            Contract.Invariant(_totalPages.Value >= 1);
            Contract.Invariant(_previousPageButton != null);
            Contract.Invariant(_nextPageButton != null);
        }

        /// <summary>
        ///     Gets the log entries.
        /// </summary>
        /// <value>
        ///     The log entries.
        /// </value>
        [NotNull, ItemNotNull]
        public IEnumerable<LogEntryViewModel> LogEntries
        {
            [DebuggerStepThrough]
            get
            {
                return _currentEntries;
            }
        }

        /// <summary>
        ///     The current page.
        /// </summary>
        [NotNull]
        public int CurrentPage
        {
            [DebuggerStepThrough]
            get
            { return _currentPage; }
            set
            {
                if (_currentPage.Value != value)
                {
                    // Change the page
                    _currentPage.Value = value;

                    // Invalidate so we refresh our entries
                    _entriesInvalidated = true;
                }
            }
        }

        /// <summary>
        ///     The total number of pages.
        /// </summary>
        [NotNull]
        public int TotalPages
        {
            [DebuggerStepThrough]
            get
            { return _totalPages; }
        }

        /// <summary>
        ///     Size of each page.
        /// </summary>
        [NotNull]
        public Observable<int> PageSize
        {
            [DebuggerStepThrough]
            get
            { return _pageSize; }
        }

        /// <summary>
        ///     Moves to the next page.
        /// </summary>
        public void MoveToNextPage()
        {
            CurrentPage = Math.Min(TotalPages, CurrentPage + 1);
        }

        /// <summary>
        ///     Gets a value indicating whether we can move to the next page.
        /// </summary>
        /// <value>
        ///     true if we can move to the next page, false if not.
        /// </value>
        public bool CanMoveToNextPage
        {
            get { return _canMoveToNextPage; }
        }

        /// <summary>
        ///     Moves to the previous page.
        /// </summary>
        public void MoveToPreviousPage()
        {
            CurrentPage = Math.Max(1, CurrentPage - 1);
        }

        /// <summary>
        ///     The total number of log entries.
        /// </summary>
        public int TotalEntries
        {
            [DebuggerStepThrough]
            get
            { return _totalEntries; }
            set { _totalEntries.Value = value; }
        }

        /// <summary>
        ///     Gets a value indicating whether we can move to the previous page.
        /// </summary>
        /// <value>
        ///     true if we can move to the previous page, false if not.
        /// </value>
        public bool CanMoveToPreviousPage
        {
            get { return _canMoveToPreviousPage; }
        }

        /// <summary>
        ///     Gets the buttons to appear along an <paramref name="edge"/>.
        /// </summary>
        /// <param name="result"> The result. </param>
        /// <param name="edge"> The docking edge for the buttons to appear along. </param>
        /// <returns>
        ///     An enumerator that allows foreach to be used to process the buttons in this collection.
        /// </returns>
        internal override IEnumerable<ButtonModel> GetButtons(MFDProcessorResult result, ButtonStripDock edge)
        {
            if (edge == ButtonStripDock.Left) return _leftButtons;
            if (edge == ButtonStripDock.Right) return _rightButtons;

            return null;
        }

        /// <summary>
        ///     Process the screen state and outputs any resulting information to the processorResult.
        /// </summary>
        /// <param name="processor"> The processor. </param>
        /// <param name="processorResult"> The processor result. </param>
        protected override void ProcessScreenState(MFDProcessor processor, MFDProcessorResult processorResult)
        {
            var currentEntryCount = _model.LoggedEvents.Count();

            // Check to see if we need to update our entries
            if (_entriesInvalidated || TotalEntries < currentEntryCount)
            {
                // Repopulate the current page
                UpdateCurrentEntries();

                // Update Paging factors
                _totalPages.Value = Math.Max(1, currentEntryCount / _pageSize.Value);
                _canMoveToNextPage.Value = CurrentPage < TotalPages;
                _canMoveToPreviousPage.Value = CurrentPage > 1;

                // Update the button text
                _nextPageButton.Text = CanMoveToNextPage ? NextPageString : string.Empty;
                _previousPageButton.Text = CanMoveToPreviousPage ? PrevPageString : string.Empty;

                // Mark it as valid so we don't have to update the entries every cycle
                _entriesInvalidated = false;
            }

            TotalEntries = currentEntryCount;
        }

        /// <summary>
        ///     Updates the current entries collection.
        /// </summary>
        private void UpdateCurrentEntries()
        {
            _currentEntries.Clear();

            // Grab the current page's items
            var pageEntries = _model.LoggedEvents.Skip((CurrentPage - 1) * PageSize).Take(PageSize);

            // For these items, create view models.
            foreach (var eventModel in pageEntries)
            {
                Debug.Assert(eventModel != null);

                // TODO: If we bump into perf. issues, we might want to utilize VMLocator for VM recycling
                var entryViewModel = new LogEntryViewModel(eventModel);

                _currentEntries.Add(entryViewModel);
            }
        }

    }

}