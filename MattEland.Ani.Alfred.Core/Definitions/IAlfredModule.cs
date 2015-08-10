// ---------------------------------------------------------
// IAlfredModule.cs
// 
// Created on:      08/09/2015 at 6:17 PM
// Last Modified:   08/09/2015 at 6:19 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;

using MattEland.Ani.Alfred.Core.Widgets;

namespace MattEland.Ani.Alfred.Core.Definitions
{
    /// <summary>
    ///     Represents a module belonging to a page or subsystem in Alfred.
    /// </summary>
    /// <remarks>
    ///     TODO: This is a marker interface at present. I'd like to see some methods or reasons not to just use
    ///     IAlfredComponent
    /// </remarks>
    public interface IAlfredModule : IAlfredComponent
    {
        /// <summary>
        ///     Gets the user interface widgets for the module.
        /// </summary>
        /// <value>The user interface widgets.</value>
        IEnumerable<AlfredWidget> Widgets { get; }
    }
}