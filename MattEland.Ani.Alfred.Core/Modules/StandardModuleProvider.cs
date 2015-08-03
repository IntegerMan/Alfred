// ---------------------------------------------------------
// StandardModuleProvider.cs
// 
// Created on:      07/29/2015 at 3:01 PM
// Last Modified:   08/03/2015 at 1:58 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;

using JetBrains.Annotations;

namespace MattEland.Ani.Alfred.Core.Modules
{
    /// <summary>
    ///     A utility class for quickly configuring Alfred instances.
    /// </summary>
    public static class StandardModuleProvider
    {
        /// <summary>
        ///     Adds standard modules to an Alfred Provider.
        /// </summary>
        /// <param
        ///     name="alfred">
        ///     The alfred provider.
        /// </param>
        /// <exception
        ///     cref="ArgumentNullException">
        /// </exception>
        public static void AddStandardModules([NotNull] AlfredProvider alfred)
        {
            if (alfred == null)
            {
                throw new ArgumentNullException(nameof(alfred));
            }

            alfred.AddModule(new AlfredCoreModule(alfred.PlatformProvider, alfred));
            alfred.AddModule(new AlfredTimeModule(alfred.PlatformProvider));
        }
    }
}