// ---------------------------------------------------------
// IAlfredPage.cs
// 
// Created on:      08/09/2015 at 6:17 PM
// Last Modified:   08/09/2015 at 6:19 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using MattEland.Ani.Alfred.Core.Modules;

namespace MattEland.Ani.Alfred.Core.Interfaces
{
    /// <summary>
    ///     Defines an alfred page
    /// </summary>
    public interface IAlfredPage : IAlfredComponent
    {
        /// <summary>
        ///     Gets a value indicating whether this page is root level.
        /// </summary>
        /// <value><c>true</c> if this page is root level; otherwise, <c>false</c>.</value>
        bool IsRootLevel { get; }
    }
}