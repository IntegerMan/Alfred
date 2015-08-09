// ---------------------------------------------------------
// WinClientPlatformProvider.cs
// 
// Created on:      08/08/2015 at 5:55 PM
// Last Modified:   08/08/2015 at 5:56 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using MattEland.Ani.Alfred.Core;
using MattEland.Ani.Alfred.Core.Interfaces;
using MattEland.Ani.Alfred.Core.Widgets;

namespace MattEland.Ani.Alfred.WPF.Platform
{
    /// <summary>
    ///     A collection provider that provides WPF preferred Observable Collections
    /// </summary>
    public sealed class WinClientPlatformProvider : IPlatformProvider
    {
        /// <summary>
        ///     Creates a collection of the specified type.
        /// </summary>
        /// <typeparam name="T">The type the collection will contain</typeparam>
        /// <returns>The collection</returns>
        public ICollection<T> CreateCollection<T>()
        {
            return new ObservableCollection<T>();
        }

        /// <summary>
        ///     Creates a platform-friendly version of an AlfredCommand.
        /// </summary>
        /// <returns>An AlfredCommand.</returns>
        public AlfredCommand CreateCommand()
        {
            return new WinClientCommand();
        }

        /// <summary>
        ///     Creates a platform-friendly version of an AlfredCommand with a pre-defined action.
        /// </summary>
        /// <param name="executeAction">The action a button click should execute.</param>
        /// <returns>An AlfredCommand.</returns>
        public AlfredCommand CreateCommand(Action executeAction)
        {
            return new WinClientCommand(executeAction);
        }
    }
}