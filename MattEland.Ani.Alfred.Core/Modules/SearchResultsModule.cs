// ---------------------------------------------------------
// SearchModule.cs
// 
// Created on:      09/13/2015 at 12:27 PM
// Last Modified:   09/13/2015 at 12:27 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

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
        /// <summary>
        ///     The results label.
        /// </summary>
        [NotNull]
        private readonly TextWidget _lblResults;

        /// <summary>
        ///     The results list widget.
        /// </summary>
        [NotNull]
        private readonly Repeater _listResults;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SearchResultsModule" /> class.
        /// </summary>
        /// <param name="container"> The container. </param>
        public SearchResultsModule([NotNull] IObjectContainer container) : base(container)
        {
            LayoutType = LayoutType.VerticalStackPanel;

            Width = double.NaN;

            _lblResults = new TextWidget(BuildWidgetParameters(@"lblResults"));
            _listResults = new Repeater(BuildWidgetParameters(@"listResults"));
        }

        /// <summary>
        ///     Handles initialization events
        /// </summary>
        /// <param name="alfred">The Alfred instance.</param>
        protected override void InitializeProtected(IAlfred alfred)
        {
            UpdateStatusMessage();

            // Register Controls
            Register(_lblResults);
            Register(_listResults);
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
            _lblResults.Text = SearchController?.StatusMessage;
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
        public TextWidget ResultsLabel
        {
            get { return _lblResults; }
        }

        /// <summary>
        ///     Gets the results list widget.
        /// </summary>
        /// <value>
        ///     The list of results.
        /// </value>
        public Repeater ResultsList
        {
            get { return _listResults; }
        }
    }
}