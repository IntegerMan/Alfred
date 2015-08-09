// ---------------------------------------------------------
// IPlatformProvider.cs
// 
// Created on:      08/09/2015 at 6:17 PM
// Last Modified:   08/09/2015 at 6:19 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Widgets;

namespace MattEland.Ani.Alfred.Core.Interfaces
{
    /// <summary>
    ///     An interface that promises the ability to build platform-specific implementations of commonly needed items.
    ///     This lets each platform provide their preferred version, for example, providing ObservableCollection for
    ///     lists or ICommand friendly implementations of commands.
    /// </summary>
    public interface IPlatformProvider
    {
        /// <summary>
        ///     Generates a collection of the specified type.
        /// </summary>
        /// <typeparam name="T">
        ///     The type the collection will contain.
        /// </typeparam>
        /// <returns>The collection.</returns>
        [NotNull]
        ICollection<T> CreateCollection<T>();

        /// <summary>
        ///     Creates a platform-friendly version of an AlfredCommand.
        /// </summary>
        /// <returns>An AlfredCommand.</returns>
        [NotNull]
        AlfredCommand CreateCommand();

        /// <summary>
        ///     Creates a platform-friendly version of an AlfredCommand with a pre-defined action.
        /// </summary>
        /// <param name="executeAction">The action a button click should execute.</param>
        /// <returns>An AlfredCommand.</returns>
        [NotNull]
        AlfredCommand CreateCommand(Action executeAction);
    }
}