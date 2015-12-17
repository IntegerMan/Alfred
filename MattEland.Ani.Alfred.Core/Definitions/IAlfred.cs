// ---------------------------------------------------------
// IAlfred.cs
// 
// Created on:      08/19/2015 at 9:31 PM
// Last Modified:   09/03/2015 at 1:22 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;

using MattEland.Common.Annotations;

using MattEland.Common.Providers;

namespace MattEland.Ani.Alfred.Core.Definitions
{
    /// <summary>
    ///     An <see langword="interface"/> promising Alfred Framework style capabilities
    /// </summary>
    public interface IAlfred : IPropertyProvider, IHasStatus, IHasContainer<IAlfredContainer>
    {
        /// <summary>
        ///     Gets the command router.
        /// </summary>
        /// <value>
        ///     The command router.
        /// </value>
        [NotNull]
        IAlfredCommandRecipient CommandRouter { get; }

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
        IEnumerable<IPage> RootPages { get; }

        /// <summary>
        ///     Gets the chat provider.
        /// </summary>
        /// <value>
        /// The chat provider.
        /// </value>
        [CanBeNull]
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
        IRegistrationProvider RegistrationProvider { get; }

        /// <summary>
        ///     Gets the search controller. This <see langword="object"/> provides search functionality
        ///     and manages the search and results processing.
        /// </summary>
        /// <value>
        ///     The search controller.
        /// </value>
        [NotNull]
        ISearchController SearchController { get; }

        /// <summary>
        ///     Gets the components registered to Alfred. This will include subsystems as well as various
        ///     helper components.
        /// </summary>
        /// <value>
        ///     The components.
        /// </value>
        [NotNull, ItemNotNull]
        IEnumerable<IAlfredComponent> Components { get; }

    }

}