// ---------------------------------------------------------
// XamlClientCommand.cs
// 
// Created on:      08/08/2015 at 5:55 PM
// Last Modified:   08/08/2015 at 5:56 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.Windows.Input;

using MattEland.Common.Annotations;

using MattEland.Ani.Alfred.Core;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Common.Providers;

namespace MattEland.Ani.Alfred.PresentationAvalon.Commands
{
    /// <summary>
    ///     A WPF/XAML compliant implementation of <see cref="IAlfredCommand"/>.
    /// </summary>
    public sealed class XamlClientCommand : AlfredCommand, ICommand
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="XamlClientCommand" /> class.
        /// </summary>
        [UsedImplicitly]
        public XamlClientCommand() : this(null, AlfredContainerHelper.ProvideContainer())
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="XamlClientCommand" /> class.
        /// </summary>
        /// <param name="executeAction">The execute action.</param>
        public XamlClientCommand(Action executeAction, IAlfredContainer container) : base(executeAction, container)
        {
        }
    }
}