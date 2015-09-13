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
        ///     Initializes a new instance of the <see cref="AlfredModule" /> class.
        /// </summary>
        /// <param name="container"> The container. </param>
        public SearchModule([NotNull] IObjectContainer container) : base(container)
        {
            _searchLabel = new TextWidget("Search:", BuildWidgetParameters(@"lblSearch"));
            _searchButton = new ButtonWidget("Search", CreateCommand(OnSearchClicked), BuildWidgetParameters(@"btnSearch"));
        }

        /// <summary>
        ///     Executes the search clicked action.
        /// </summary>
        private void OnSearchClicked()
        {
            // TODO: Make things click
        }

        /// <summary>
        ///     Handles initialization events
        /// </summary>
        /// <param name="alfred">The Alfred instance.</param>
        protected override void InitializeProtected(IAlfred alfred)
        {
            Register(_searchLabel);
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