// ---------------------------------------------------------
// SimplePlatformProvider.cs
// 
// Created on:      07/26/2015 at 4:32 PM
// Last Modified:   08/03/2015 at 2:04 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Widgets;

namespace MattEland.Ani.Alfred.Core
{
    /// <summary>
    ///     A simplistic platform provider for testing purposes. This class cannot be inherited.
    /// </summary>
    public sealed class SimplePlatformProvider : IPlatformProvider
    {
        /// <summary>
        ///     Generates a collection of the specified type.
        /// </summary>
        /// <typeparam
        ///     name="T">
        ///     The type
        /// </typeparam>
        /// <returns>The collection.</returns>
        [NotNull]
        public ICollection<T> CreateCollection<T>()
        {
            return new Collection<T>();
        }

        /// <summary>
        ///     Creates a platform-friendly version of an AlfredCommand.
        /// </summary>
        /// <returns>An AlfredCommand.</returns>
        [NotNull]
        public AlfredCommand CreateCommand()
        {
            return new AlfredCommand();
        }

        /// <summary>
        /// Creates a platform-friendly version of an AlfredCommand with a pre-defined action.
        /// </summary>
        /// <param name="executeAction">The action a button click should execute.</param>
        /// <returns>An AlfredCommand.</returns>
        [NotNull]
        public AlfredCommand CreateCommand([CanBeNull] Action executeAction)
        {
            return new AlfredCommand(executeAction);
        }
    }
}