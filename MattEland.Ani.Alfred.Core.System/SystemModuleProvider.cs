// ---------------------------------------------------------
// SystemModuleProvider.cs
// 
// Created on:      08/05/2015 at 1:17 AM
// Last Modified:   08/05/2015 at 1:19 AM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace MattEland.Ani.Alfred.Core.Modules.SysMonitor
{
    /// <summary>
    ///     A utility class for quickly configuring Alfred instances with System-based modules.
    /// </summary>
    public static class SystemModuleProvider
    {
        /// <summary>
        ///     Adds system modules to an Alfred Provider.
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
                              new MemoryMonitorModule(alfred.PlatformProvider),
                              new CpuMonitorModule(alfred.PlatformProvider),
                              new DiskMonitorModule(alfred.PlatformProvider)
                          };

            // Add lots of modules in bulk
            alfred.RegisterModules(modules);

            alfred.RegisterSubSystem(new SystemMonitoringSystem(alfred.PlatformProvider));
        }
    }
}