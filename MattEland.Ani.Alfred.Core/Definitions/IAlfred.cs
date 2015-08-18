// ---------------------------------------------------------
// IAlfred.cs
// 
// Created on:      08/09/2015 at 6:17 PM
// Last Modified:   08/09/2015 at 6:18 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Console;

namespace MattEland.Ani.Alfred.Core.Definitions
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

        /// <summary>
        /// Gets a value indicating whether this instance is online.
        /// </summary>
        /// <value><c>true</c> if this instance is online; otherwise, <c>false</c>.</value>
        bool IsOnline { get; }

        /// <summary>
        /// Gets the chat provider.
        /// </summary>
        /// <value>The chat provider.</value>
        IChatProvider ChatProvider { get; }

        /// <summary>
        /// Gets the platform provider.
        /// </summary>
        /// <value>The platform provider.</value>
        [NotNull]
        IPlatformProvider PlatformProvider { get; }

        /// <summary>
        /// Registers the user statement handler as the framework's user statement handler.
        /// </summary>
        /// <param name="chatProvider">The user statement handler.</param>
        void Register([NotNull] IChatProvider chatProvider);

        /// <summary>
        ///     Registers a sub system with Alfred.
        /// </summary>
        /// <param name="subsystem">The subsystem.</param>
        void Register([NotNull] AlfredSubsystem subsystem);

        /// <summary>
        ///     Tells modules to take a look at their content and update as needed.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        ///     Thrown if Alfred is not Online
        /// </exception>
        void Update();
    }

}