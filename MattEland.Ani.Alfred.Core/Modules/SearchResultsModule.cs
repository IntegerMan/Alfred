﻿// ---------------------------------------------------------
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
        ///     Initializes a new instance of the <see cref="SearchResultsModule" /> class.
        /// </summary>
        /// <param name="container"> The container. </param>
        public SearchResultsModule([NotNull] IObjectContainer container) : base(container)
        {
            LayoutType = LayoutType.VerticalStackPanel;

            Width = double.NaN;

            _lblResults = new TextWidget(NoSearchesMadeMessage, BuildWidgetParameters(@"lblResults"));
        }

        /// <summary>
        ///     Handles initialization events
        /// </summary>
        /// <param name="alfred">The Alfred instance.</param>
        protected override void InitializeProtected(IAlfred alfred)
        {
            Register(_lblResults);

            // TODO: Register a list control
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
        ///     Gets a message indicating no searches have been made yet
        /// </summary>
        /// <value>
        ///     A message indicating no searches have been made
        /// </value>
        public static string NoSearchesMadeMessage
        {
            get
            {
                return "No searches have been made yet.";
            }
        }
    }
}