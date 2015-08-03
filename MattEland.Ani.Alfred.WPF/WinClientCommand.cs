// ---------------------------------------------------------
// WinClientCommand.cs
// 
// Created on:      08/03/2015 at 1:43 PM
// Last Modified:   08/03/2015 at 1:50 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.Windows.Input;

using MattEland.Ani.Alfred.Core.Widgets;

namespace MattEland.Ani.Alfred.WPF
{
    /// <summary>
    ///     A WPF compliant version of
    ///     <see
    ///         cref="AlfredCommand" />
    /// </summary>
    public class WinClientCommand : AlfredCommand, ICommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WinClientCommand"/> class.
        /// </summary>
        public WinClientCommand() : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WinClientCommand"/> class.
        /// </summary>
        /// <param name="executeAction">The execute action.</param>
        public WinClientCommand(Action executeAction) : base(executeAction)
        {
        }
    }
}