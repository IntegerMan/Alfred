// ---------------------------------------------------------
// SearchModule.cs
// 
// Created on:      09/13/2015 at 12:27 PM
// Last Modified:   09/13/2015 at 12:27 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Widgets;
using MattEland.Common;
using MattEland.Common.Providers;

namespace MattEland.Ani.Alfred.Core.Modules
{
    /// <summary>
    ///     A search module used for entering text and executing searches. This class cannot be
    ///     inherited.
    /// </summary>
    public sealed class SearchModule : AlfredModule
    {
        /// <summary>
        ///     The search label.
        /// </summary>
        [NotNull]
        private TextWidget _searchLabel;

        /// <summary>
        ///     The search button.
        /// </summary>
        [NotNull]
        private ButtonWidget _searchButton;

        /// <summary>
        ///     The search text.
        /// </summary>
        [NotNull]
        private TextBoxWidget _searchText;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AlfredModule" /> class.
        /// </summary>
        /// <param name="container"> The container. </param>
        public SearchModule([NotNull] IObjectContainer container) : base(container)
        {
            _searchLabel = new TextWidget("Search:", BuildWidgetParameters(@"lblSearch"));
            _searchText = new TextBoxWidget(BuildWidgetParameters(@"txtSearch"));
            _searchButton = new ButtonWidget("Search", CreateCommand(OnSearchClicked), BuildWidgetParameters(@"btnSearch"));

            LayoutType = LayoutType.HorizontalStackPanel;
            Width = double.NaN;
        }

        /// <summary>
        ///     Executes the search clicked action.
        /// </summary>
        private void OnSearchClicked()
        {
            var searchText = _searchText.Text;

            if (searchText.IsEmpty())
            {
                const string Message = "Please enter a value to search for before clicking Search.";
                Message.ShowAlert("Cannot Search", Container);
            }
            else
            {
                // Start searching
                AlfredInstance?.SearchController.PerformSearch(searchText.NonNull());
            }
        }

        /// <summary>
        ///     Handles initialization events
        /// </summary>
        /// <param name="alfred">The Alfred instance.</param>
        protected override void InitializeProtected(IAlfred alfred)
        {
            Register(_searchLabel);
            Register(_searchText);
            Register(_searchButton);
        }

        /// <summary>
        ///     Gets the name of the component.
        /// </summary>
        /// <value>The name of the component.</value>
        public override string Name
        {
            get { return "Search"; }
        }
    }
}