// ---------------------------------------------------------
// MetricProviderBase.cs
// 
// Created on:      08/18/2015 at 1:50 PM
// Last Modified:   08/18/2015 at 1:50 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;

namespace MattEland.Ani.Alfred.Core.Modules.SysMonitor
{
    /// <summary>
    ///     An abstract class representing an object capable of providing metrics periodically via a
    ///     NextValue method.
    /// </summary>
    public abstract class MetricProviderBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="MetricProviderBase" /> class.
        /// </summary>
        /// <param name="metricName">The name of the metric.</param>
        protected MetricProviderBase(string metricName)
        {
            Name = metricName;
        }

        /// <summary>
        ///     Gets the name of the metric.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged
        ///     resources.
        /// </summary>
        public virtual void Dispose()
        {
        }

        /// <summary>
        ///     Gets the next value from the metric provider
        /// </summary>
        /// <returns>The next value</returns>
        public virtual float NextValue()
        {
            return 0;
        }
    }
}