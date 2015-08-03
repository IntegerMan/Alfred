// ---------------------------------------------------------
// Win8ClientCommand.cs
// 
// Created on:      08/03/2015 at 2:04 PM
// Last Modified:   08/03/2015 at 2:24 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.Windows.Input;

using MattEland.Ani.Alfred.Core.Widgets;

namespace MattEland.Ani.Alfred.Win8
{
    // TODO: It'd be great to be able to reuse the same one that WPF uses

    /// <summary>
    ///     A Windows 8 XAML compliant version of
    ///     <see
    ///         cref="AlfredCommand" />
    /// </summary>
    public class Win8ClientCommand : AlfredCommand, ICommand
    {
        /// <summary>
        ///     Initializes a new instance of the
        ///     <see
        ///         cref="Win8ClientCommand" />
        ///     class.
        /// </summary>
        public Win8ClientCommand() : this(null)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the
        ///     <see
        ///         cref="Win8ClientCommand" />
        ///     class.
        /// </summary>
        /// <param
        ///     name="executeAction">
        ///     The execute action.
        /// </param>
        public Win8ClientCommand(Action executeAction) : base(executeAction)
        {
        }
    }
}