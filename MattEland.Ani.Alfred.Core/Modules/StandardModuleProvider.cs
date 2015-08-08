// ---------------------------------------------------------
// StandardModuleProvider.cs
// 
// Created on:      07/29/2015 at 3:01 PM
// Last Modified:   08/05/2015 at 1:17 AM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;

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
        /// <param name="alfred">
        ///     The alfred provider.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        public static void AddStandardModules([NotNull] AlfredProvider alfred)
        {
            if (alfred == null)
            {
                throw new ArgumentNullException(nameof(alfred));
            }

            // Define our modules
            var modules = new List<AlfredModule>
                          {
                              new AlfredPowerModule(alfred.PlatformProvider, alfred),
                              new AlfredTimeModule(alfred.PlatformProvider),
                              new AlfredSubSystemListModule(alfred.PlatformProvider)
                          };

            // Add lots of modules in bulk
            alfred.Register(modules);
        }
    }
}