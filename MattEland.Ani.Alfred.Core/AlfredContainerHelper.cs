// ---------------------------------------------------------
// AlfredContainerHelper.cs
// 
// Created on:      09/09/2015 at 1:41 AM
// Last Modified:   09/09/2015 at 1:41 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Common.Providers;

namespace MattEland.Ani.Alfred.Core
{
    /// <summary>
    ///     A helper class to define default mappings for Alfred objects in an
    ///     <see cref="IObjectContainer"/>
    /// </summary>
    public static class AlfredContainerHelper
    {
        /// <summary>
        ///     An <see cref="IObjectContainer" /> extension method that applies the default Alfred type
        ///     mappings in the <paramref name="container" /> . This method eases the configuration pains
        ///     in customizing an Alfred instance.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <param name="container"> The container to act on. </param>
        public static void ApplyDefaultAlfredMappings([NotNull] this IObjectContainer container)
        {
            if (container == null) { throw new ArgumentNullException(nameof(container)); }

            container.TryRegister(typeof(IAlfredCommand), typeof(AlfredCommand));
            container.TryRegister(typeof(IAlfred), typeof(AlfredApplication));
            container.TryRegister(typeof(ISearchController), typeof(AlfredSearchController));

        }
    }
}