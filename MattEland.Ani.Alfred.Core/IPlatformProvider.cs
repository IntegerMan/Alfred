// ---------------------------------------------------------
// IPlatformProvider.cs
// 
// Created on:      07/26/2015 at 4:29 PM
// Last Modified:   08/03/2015 at 2:02 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Widgets;

namespace MattEland.Ani.Alfred.Core
{
    /// <summary>
    ///     An interface that promises the ability to build platform-specific implentations of commonly needed items.
    ///     This lets each platform provide their preferred version, for example, providing ObservableCollection for
    ///     lists or ICommand friendly implementations of commands.
    /// </summary>
    public interface IPlatformProvider
    {
        /// <summary>
        ///     Generates a collection of the specified type.
        /// </summary>
        /// <typeparam
        ///     name="T">
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
    }
}