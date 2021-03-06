// ---------------------------------------------------------
// IPage.cs
// 
// Created on:      08/19/2015 at 9:31 PM
// Last Modified:   08/20/2015 at 11:57 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.ComponentModel;

namespace MattEland.Ani.Alfred.Core.Definitions
{
    /// <summary>
    ///     Defines a user interface page's view model
    /// </summary>
    public interface IPage : IAlfredComponent, IAlfredCommandRecipient, IHasIdentifier, INotifyPropertyChanged
    {
        /// <summary>
        ///     Gets a value indicating whether this page is root level.
        /// </summary>
        /// <value><c>true</c> if this page is root level; otherwise, <c>false</c>.</value>
        bool IsRootLevel { get; }

        /// <summary>
        ///     Gets or sets the type of the layout used for this page.
        /// </summary>
        /// <value>
        ///     The type of the layout.
        /// </value>
        LayoutType LayoutType { get; set; }
    }
}