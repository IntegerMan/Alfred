// ---------------------------------------------------------
// IAlfred.cs
// 
// Created on:      08/09/2015 at 6:17 PM
// Last Modified:   08/09/2015 at 6:18 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.Core.Subsystems;
using MattEland.Common.Providers;

namespace MattEland.Ani.Alfred.Core.Definitions
{
    /// <summary>
    ///     An interface promising Alfred Framework style capabilities
    /// </summary>
    public interface IAlfred : IPropertyProvider, IHasContainer
    {
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
        /// Gets the shell command handler that can pass shell commands on to the user interface.
        /// </summary>
        /// <value>The shell command handler.</value>
        [CanBeNull]
        IShellCommandRecipient ShellCommandHandler { get; }

        /// <summary>
        /// Gets the locale.
        /// </summary>
        /// <value>The locale.</value>
        [NotNull]
        CultureInfo Locale { get; }

        /// <summary>
        /// Registers the shell command recipient that will allow the shell to get commands from the Alfred layer.
        /// </summary>
        /// <param name="shell">The command recipient.</param>
        void Register([NotNull] IShellCommandRecipient shell);

        /// <summary>
        /// Registers the user statement handler as the framework's user statement handler.
        /// </summary>
        /// <param name="chatProvider">The user statement handler.</param>
        void Register([NotNull] IChatProvider chatProvider);

        /// <summary>
        ///     Registers a sub system with Alfred.
        /// </summary>
        /// <param name="subsystem">The subsystem.</param>
        void Register([NotNull] IAlfredSubsystem subsystem);

        /// <summary>
        ///     Tells modules to take a look at their content and update as needed.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        ///     Thrown if Alfred is not Online
        /// </exception>
        void Update();

        /// <summary>
        /// Registers the page as a root page.
        /// </summary>
        /// <param name="page">The page.</param>
        void Register([NotNull] IAlfredPage page);
    }

}