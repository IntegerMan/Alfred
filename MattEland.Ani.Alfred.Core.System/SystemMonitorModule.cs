// ---------------------------------------------------------
// SystemMonitorModule.cs
// 
// Created on:      08/04/2015 at 10:04 PM
// Last Modified:   08/04/2015 at 10:08 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Interfaces;

namespace MattEland.Ani.Alfred.Core.Modules.SysMonitor
{
    /// <summary>
    ///     The SystemMonitorModule is an abstract class for modules commonly working with performance counters.
    /// </summary>
    public abstract class SystemMonitorModule : AlfredModule
    {
        /// <summary>
        ///     The performance counter instance name for total results
        /// </summary>
        public const string TotalInstanceName = "_Total";

        /// <summary>
        ///     Initializes a new instance of the <see cref="SystemMonitorModule" /> class.
        /// </summary>
        /// <param name="platformProvider">The platform provider.</param>
        protected SystemMonitorModule([NotNull] IPlatformProvider platformProvider) : base(platformProvider)
        {
        }

        /// <summary>
        ///     Gets the next counter value safely, defaulting to 0 on any error.
        /// </summary>
        /// <param name="counter">The counter.</param>
        /// <returns>The value returned from the counter</returns>
        protected static float GetNextCounterValueSafe([CanBeNull] PerformanceCounter counter)
        {
            return GetNextCounterValueSafe(counter, 0);
        }

        /// <summary>
        ///     Gets the next counter value safely, defaulting to the defaultValue on any error.
        /// </summary>
        /// <param name="counter">The counter.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The value returned from the counter</returns>
        protected static float GetNextCounterValueSafe([CanBeNull] PerformanceCounter counter, float defaultValue)
        {
            try
            {
                return counter?.NextValue() ?? 0;
            }
            catch (Win32Exception)
            {
                return defaultValue;
            }
            catch (PlatformNotSupportedException)
            {
                return defaultValue;
            }
            catch (UnauthorizedAccessException)
            {
                return defaultValue;
            }
            catch (InvalidOperationException)
            {
                return defaultValue;
            }
        }
    }
}