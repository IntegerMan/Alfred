// ---------------------------------------------------------
// IAlfredPage.cs
// 
// Created on:      08/19/2015 at 9:31 PM
// Last Modified:   08/20/2015 at 11:57 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

namespace MattEland.Ani.Alfred.Core.Definitions
{
    /// <summary>
    ///     Defines an alfred page
    /// </summary>
    public interface IAlfredPage : IAlfredComponent, IAlfredCommandRecipient, IHasIdentifier
    {
        /// <summary>
        ///     Gets a value indicating whether this page is root level.
        /// </summary>
        /// <value><c>true</c> if this page is root level; otherwise, <c>false</c>.</value>
        bool IsRootLevel { get; }
    }
}