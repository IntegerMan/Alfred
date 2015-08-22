// ---------------------------------------------------------
// XamlClientCommand.cs
// 
// Created on:      08/08/2015 at 5:55 PM
// Last Modified:   08/08/2015 at 5:56 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.Windows.Input;

using MattEland.Ani.Alfred.Core.Definitions;

namespace MattEland.Ani.Alfred.PresentationShared.Commands
{
    /// <summary>
    ///     A WPF/XAML compliant implementation of AlfredCommand.
    /// </summary>
    public sealed class XamlClientCommand : AlfredCommand, ICommand
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="XamlClientCommand" /> class.
        /// </summary>
        public XamlClientCommand() : this(null)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="XamlClientCommand" /> class.
        /// </summary>
        /// <param name="executeAction">The execute action.</param>
        public XamlClientCommand(Action executeAction) : base(executeAction)
        {
        }
    }
}