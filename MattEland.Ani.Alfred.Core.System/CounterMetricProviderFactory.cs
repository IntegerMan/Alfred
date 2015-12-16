// ---------------------------------------------------------
// CounterMetricProviderFactory.cs
// 
// Created on:      08/19/2015 at 9:31 PM
// Last Modified:   08/23/2015 at 11:39 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace MattEland.Ani.Alfred.Core.Modules.SysMonitor
{
    /// <summary>
    ///     A factory that builds CounterMetricProviders
    /// </summary>
    public sealed class CounterMetricProviderFactory : IMetricProviderFactory
    {
        /// <summary>
        ///     Builds a metric provider for the specified type.
        /// </summary>
        /// <remarks>
        ///     The return type is an <see cref="IDisposable" />.
        ///     Dispose should be called on termination.
        /// </remarks>
        /// <param name="categoryName">Name of the category.</param>
        /// <param name="counterName">Name of the counter.</param>
        /// <param name="instanceName">Name of the instance.</param>
        /// <returns>
        ///     A metric provider. Note that this type implements <see cref="IDisposable" />
        /// </returns>
        /// <exception cref="Win32Exception">An error occurred when accessing a system API.</exception>
        /// <exception cref="UnauthorizedAccessException">
        ///     Code that is executing without administrative
        ///     privileges attempted to read a performance counter.
        /// </exception>
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope",
            Justification = "Callers are required to manage disposing this instance.")]
        public MetricProviderBase Build(string categoryName,
                                        string counterName,
                                        string instanceName = null)
        {
            return new CounterMetricProvider(categoryName, counterName, instanceName);
        }

        /// <summary>
        ///     Gets the names of each counter instance in a category.
        /// </summary>
        /// <param name="categoryName">Name of the category.</param>
        /// <returns>A collection of counter instance names</returns>
        /// <exception cref="Win32Exception">A call to an underlying system API failed. </exception>
        /// <exception cref="UnauthorizedAccessException">
        ///     Code that is executing without administrative
        ///     privileges attempted to read a performance counter.
        /// </exception>
        public IEnumerable<string> GetCategoryInstanceNames(string categoryName)
        {
            var cpuCategory = new PerformanceCounterCategory(categoryName);

            IEnumerable<string> names = cpuCategory.GetInstanceNames();
            return names;
        }
    }
}