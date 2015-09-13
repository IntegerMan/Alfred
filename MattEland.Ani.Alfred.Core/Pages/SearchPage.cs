// ---------------------------------------------------------
// SearchPage.cs
// 
// Created on:      09/13/2015 at 12:04 PM
// Last Modified:   09/13/2015 at 12:04 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Common.Providers;

namespace MattEland.Ani.Alfred.Core.Pages
{
    /// <summary>
    ///     A search page that allows the user to search many aspects of Alfred and view search results.
    /// </summary>
    public class SearchPage : AlfredPage
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AlfredPage" /> class.
        /// </summary>
        /// <param name="container"> The container. </param>
        public SearchPage([NotNull] IObjectContainer container) : base(container, "Search", "SearchPage")
        {
        }

        /// <summary>
        ///     Gets the children of the component. Depending on the type of component this is, the
        ///     children will vary in their own types.
        /// </summary>
        /// <value>
        ///     The children.
        /// </value>
        public override IEnumerable<IAlfredComponent> Children
        {
            get
            {
                yield break;
            }
        }
    }
}