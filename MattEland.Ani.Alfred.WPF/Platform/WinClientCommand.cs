// ---------------------------------------------------------
// WinClientCommand.cs
// 
// Created on:      08/08/2015 at 5:55 PM
// Last Modified:   08/08/2015 at 5:56 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.Windows.Input;

using MattEland.Ani.Alfred.Core.Widgets;

namespace MattEland.Ani.Alfred.WPF.Platform
{
    /// <summary>
    ///     A WPF compliant implementation of AlfredCommand.
    /// </summary>
    public class WinClientCommand : AlfredCommand, ICommand
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="WinClientCommand" /> class.
        /// </summary>
        public WinClientCommand() : this(null)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="WinClientCommand" /> class.
        /// </summary>
        /// <param name="executeAction">The execute action.</param>
        public WinClientCommand(Action executeAction) : base(executeAction)
        {
        }
    }
}