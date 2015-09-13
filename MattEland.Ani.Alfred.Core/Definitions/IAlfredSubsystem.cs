// ---------------------------------------------------------
// IAlfredSubsystem.cs
// 
// Created on:      08/09/2015 at 6:17 PM
// Last Modified:   08/09/2015 at 6:19 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;

using JetBrains.Annotations;

namespace MattEland.Ani.Alfred.Core.Definitions
{
    /// <summary>
    ///     A subsystem of Alfred
    /// </summary>
    public interface IAlfredSubsystem : IAlfredComponent, IAlfredCommandRecipient
    {
        /// <summary>
        ///     Gets the pages.
        /// </summary>
        /// <value>The pages.</value>
        [NotNull]
        [ItemNotNull]
        IEnumerable<IPage> Pages { get; }

        /// <summary>
        /// Gets the root-level pages provided by this subsystem.
        /// </summary>
        /// <value>The root-level pages.</value>
        [NotNull, ItemNotNull]
        IEnumerable<IPage> RootPages { get; }

        /// <summary>
        /// Gets the identifier for the subsystem to be used in command routing.
        /// </summary>
        /// <value>The identifier for the subsystem.</value>
        [NotNull]
        string Id { get; }

        /// <summary>
        ///     Gets the search providers.
        /// </summary>
        /// <value>
        ///     The search providers.
        /// </value>
        [NotNull, ItemNotNull]
        IEnumerable<ISearchProvider> SearchProviders { get; }
    }

}