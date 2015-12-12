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

using MattEland.Common.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Common.Providers;

namespace MattEland.Ani.Alfred.Core.Modules.SysMonitor
{
    /// <summary>
    ///     The SystemMonitorModule is an <see langword="abstract"/> class for modules commonly working with performance counters.
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
        /// <param name="container"> The container. </param>
        /// <param name="metricProvider"> The metric provider. </param>
        protected SystemMonitorModule([NotNull] IAlfredContainer container,
                                      [NotNull] IMetricProviderFactory metricProvider) : base(container)
        {
            // Use the provided metric provider
            _metricProvider = metricProvider;
        }

        /// <summary>
        /// Gets the next <paramref name="counter" /> value safely, defaulting to 0 on any error.
        /// </summary>
        /// <param name="counter">The counter.</param>
        /// <returns>
        /// The value returned from the <paramref name="counter"/>
        /// </returns>
        protected float GetNextCounterValueSafe([CanBeNull] MetricProviderBase counter)
        {
            return GetNextCounterValueSafe(counter, 0);
        }

        /// <summary>
        /// Gets the next <paramref name="counter" /> value safely, defaulting to the
        /// <paramref name="defaultValue" /> on any error.
        /// </summary>
        /// <param name="counter">The counter.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>
        /// The value returned from the <paramref name="counter"/>
        /// </returns>
        protected float GetNextCounterValueSafe([CanBeNull] MetricProviderBase counter, float defaultValue)
        {
            try
            {
                return counter?.NextValue() ?? 0;
            }
            catch (Win32Exception ex)
            {
                LastErrorInstance = Container.HandleException(ex,
                                                              "PRFVAL-01",
                                                              "Win32 error reading value");

                return defaultValue;
            }
            catch (PlatformNotSupportedException ex)
            {
                LastErrorInstance = Container.HandleException(ex,
                                                              "PRFVAL-02",
                                                              "Platform error reading value");
                return defaultValue;
            }
            catch (UnauthorizedAccessException ex)
            {
                LastErrorInstance = Container.HandleException(ex,
                                                              "PRFVAL-03",
                                                              "Unathorized access reading value");
                return defaultValue;
            }
            catch (InvalidOperationException ex)
            {
                LastErrorInstance = Container.HandleException(ex,
                                                              "PRFVAL-04",
                                                              "Invalid operation reading value");

                return defaultValue;
            }
        }

        [NotNull]
        private readonly IMetricProviderFactory _metricProvider;

        /// <summary>
        /// Gets the metric provider.
        /// </summary>
        /// <value>The metric provider.</value>
        [NotNull]
        public IMetricProviderFactory MetricProvider
        {
            [DebuggerStepThrough]
            get
            { return _metricProvider; }
        }
    }
}