// ---------------------------------------------------------
// IAlfred.cs
// 
// Created on:      08/19/2015 at 9:31 PM
// Last Modified:   09/03/2015 at 1:22 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;

using JetBrains.Annotations;

using MattEland.Common.Providers;

namespace MattEland.Ani.Alfred.Core.Definitions
{
    /// <summary>
    ///     An <see langword="interface"/> promising Alfred Framework style capabilities
    /// </summary>
    public interface IAlfred : IPropertyProvider, IHasStatus, IHasContainer
    {

        /// <summary>
        ///     Gets the sub systems associated with Alfred.
        /// </summary>
        /// <value>
        ///     The sub systems.
        /// </value>
        [NotNull]
        [ItemNotNull]
        IEnumerable<IAlfredSubsystem> Subsystems { get; }

        /// <summary>
        ///     Gets the root pages.
        /// </summary>
        /// <value>
        /// The root pages.
        /// </value>
        [NotNull]
        [ItemNotNull]
        IEnumerable<IAlfredPage> RootPages { get; }

        /// <summary>
        ///     Gets the chat provider.
        /// </summary>
        /// <value>
        /// The chat provider.
        /// </value>
        IChatProvider ChatProvider { get; }

        /// <summary>
        ///     Gets the shell command handler that can pass shell commands on to the user interface.
        /// </summary>
        /// <value>
        ///     The shell command handler.
        /// </value>
        [CanBeNull]
        IShellCommandRecipient ShellCommandHandler { get; }

        /// <summary>
        ///     Gets the registration provider that manages registering other components for Alfred.
        /// </summary>
        /// <value>
        ///     The registration provider.
        /// </value>
        [NotNull]
        IProvidesRegistration RegistrationProvider { get; }

    }

}