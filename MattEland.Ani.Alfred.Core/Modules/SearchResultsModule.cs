// ---------------------------------------------------------
// SearchModule.cs
// 
// Created on:      09/13/2015 at 12:27 PM
// Last Modified:   09/13/2015 at 12:27 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Widgets;
using MattEland.Common.Providers;

namespace MattEland.Ani.Alfred.Core.Modules
{
    /// <summary>
    ///     A search results module used for examining search results and drilling into a single
    ///     result for more details.
    /// </summary>
    public sealed class SearchResultsModule : AlfredModule
    {

        [NotNull]
        private ICollection<IWidget> _resultWidgets;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SearchResultsModule" /> class.
        /// </summary>
        /// <param name="container"> The container. </param>
        public SearchResultsModule([NotNull] IObjectContainer container) : base(container)
        {
            LayoutType = LayoutType.VerticalStackPanel;

            Width = double.NaN;

            _resultWidgets = Container.ProvideCollection<IWidget>();

            ResultsLabel = new TextWidget(BuildWidgetParameters(@"lblResults"));
            ResultsList = new Repeater(BuildWidgetParameters(@"listResults"))
            {
                ItemsSource = _resultWidgets
            };

        }

        /// <summary>
        ///     Handles shutdown events
        /// </summary>
        protected override void ShutdownProtected()
        {
            // Unsubscribe from events
            if (AlfredInstance != null)
            {
                AlfredInstance.SearchController.ResultAdded -= OnResultAdded;
            }

            _resultWidgets.Clear();
        }

        /// <summary>
        ///     Handles initialization events
        /// </summary>
        /// <param name="alfred">The Alfred instance.</param>
        protected override void InitializeProtected(IAlfred alfred)
        {
            UpdateStatusMessage();

            _resultWidgets.Clear();

            // When a new result is added, we need to know so we can build a widget for it
            alfred.SearchController.ResultAdded += OnResultAdded;

            // Register Controls
            Register(ResultsLabel);
            Register(ResultsList);
        }

        /// <summary>
        ///     Handles when a new result is added by adding a new widget to the collection
        /// </summary>
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e"> Event information to send to registered event handlers. </param>
        private void OnResultAdded(object sender, SearchResultEventArgs e)
        {
            var result = e.Result;
            Debug.Assert(result != null);

            var resultIndex = _resultWidgets.Count + 1;

            var widget = new TextWidget(BuildWidgetParameters(string.Format("result{0}", resultIndex)))
            {
                Text = result.Title,
                DataContext = result
            };

            _resultWidgets.Add(widget);
        }

        /// <summary>
        ///     Updates the component
        /// </summary>
        protected override void UpdateProtected()
        {
            UpdateStatusMessage();
        }

        /// <summary>
        ///     Updates the status message with the latest status from the <see cref="SearchController"/>.
        /// </summary>
        private void UpdateStatusMessage()
        {
            ResultsLabel.Text = SearchController?.StatusMessage;
        }

        /// <summary>
        ///     Gets the name of the module.
        /// </summary>
        /// <value>The name of the module.</value>
        public override string Name
        {
            get { return "Search Results"; }
        }

        /// <summary>
        ///     Gets the search controller.
        /// </summary>
        /// <value>
        ///     The search controller.
        /// </value>
        [CanBeNull]
        public ISearchController SearchController
        {
            get { return AlfredInstance?.SearchController; }
        }

        /// <summary>
        ///     Gets the results label.
        /// </summary>
        /// <value>
        ///     The results label.
        /// </value>
        [NotNull]
        public TextWidget ResultsLabel { get; }

        /// <summary>
        ///     Gets the results list widget.
        /// </summary>
        /// <value>
        ///     The list of results.
        /// </value>
        [NotNull]
        public Repeater ResultsList { get; }
    }
}