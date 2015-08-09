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
    public interface IAlfredSubsystem : IAlfredComponent
    {
        /// <summary>
        ///     Gets the pages.
        /// </summary>
        /// <value>The pages.</value>
        [NotNull]
        [ItemNotNull]
        IEnumerable<IAlfredPage> Pages { get; }

        /// <summary>
        ///     Gets the modules associated with this subsystem.
        ///     This does not include modules associated with pages.
        /// </summary>
        /// <value>The modules.</value>
        [NotNull]
        [ItemNotNull]
        IEnumerable<IAlfredModule> Modules { get; }
    }

}