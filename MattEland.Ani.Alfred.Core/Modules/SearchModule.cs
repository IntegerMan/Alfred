// ---------------------------------------------------------
// SearchModule.cs
// 
// Created on:      09/13/2015 at 12:27 PM
// Last Modified:   09/13/2015 at 12:27 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using JetBrains.Annotations;

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
        ///     Initializes a new instance of the <see cref="AlfredModule" /> class.
        /// </summary>
        /// <param name="container"> The container. </param>
        public SearchModule([NotNull] IObjectContainer container) : base(container)
        {
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