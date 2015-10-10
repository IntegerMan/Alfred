// ---------------------------------------------------------
// PerformanceCounterMetricProvider.cs
// 
// Created on:      08/18/2015 at 11:29 AM
// Last Modified:   08/18/2015 at 3:12 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.ComponentModel;
using System.Diagnostics;

using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.Core.Modules.SysMonitor
{
    /// <summary>
    ///     A metric provider based on a performance counter.
    /// </summary>
    public sealed class CounterMetricProvider : MetricProviderBase, IDisposable
    {
        [NotNull]
        private readonly PerformanceCounter _counter;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CounterMetricProvider" /> class.
        /// </summary>
        /// <param name="categoryName">Name of the category.</param>
        /// <param name="counterName">Name of the counter.</param>
        /// <param name="instance">The instance.</param>
        /// <exception cref="Win32Exception">An error occurred when accessing a system API.</exception>
        /// <exception cref="UnauthorizedAccessException">
        ///     Code that is executing without administrative
        ///     privileges attempted to read a performance counter.
        /// </exception>
        public CounterMetricProvider([NotNull] string categoryName,
                                     [NotNull] string counterName,
                                     [CanBeNull] string instance = null)

            : base(instance ?? counterName)
        {
            _counter = new PerformanceCounter(categoryName, counterName, instance, true);

            /* Tell the performance counter to start getting values. These tend to always return 
               0.0 as the first value and then provide accurate data after that */
            _counter.NextValue();
        }

        /// <summary>
        ///     Gets the next value from the metric provider
        /// </summary>
        /// <returns>The next vaue</returns>
        /// <exception cref="Win32Exception">An error occurred when accessing a system API.</exception>
        /// <exception cref="UnauthorizedAccessException">
        ///     Code that is executing without administrative
        ///     privileges attempted to read a performance counter.
        /// </exception>
        public override float NextValue()
        {
            return _counter.NextValue();
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged
        ///     resources.
        /// </summary>
        public void Dispose()
        {
            _counter.Dispose();
        }
    }

}