// ---------------------------------------------------------
// IAlfred.cs
// 
// Created on:      08/09/2015 at 6:17 PM
// Last Modified:   08/09/2015 at 6:18 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Console;

namespace MattEland.Ani.Alfred.Core.Interfaces
{
    /// <summary>
    ///     An interface promising Alfred Framework style capabilities
    /// </summary>
    public interface IAlfred
    {
        /// <summary>
        ///     Gets the console provider. This can be null.
        /// </summary>
        /// <value>The console.</value>
        [CanBeNull]
        IConsole Console { get; }

        /// <summary>
        ///     Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        AlfredStatus Status { get; set; }

        /// <summary>
        ///     Gets the sub systems associated wih Alfred.
        /// </summary>
        /// <value>The sub systems.</value>
        [NotNull]
        [ItemNotNull]
        IEnumerable<IAlfredSubsystem> Subsystems { get; }

        /// <summary>
        ///     Gets the root pages.
        /// </summary>
        /// <value>The root pages.</value>
        [NotNull]
        [ItemNotNull]
        IEnumerable<IAlfredPage> RootPages { get; }

        /// <summary>
        ///     Gets the name.
        /// </summary>
        /// <value>The name.</value>
        [NotNull]
        string Name { get; }

        /// <summary>
        ///     Initializes this instance.
        /// </summary>
        void Initialize();

        /// <summary>
        ///     Shutdowns this instance.
        /// </summary>
        void Shutdown();
    }

}