// ---------------------------------------------------------
// IMetricProviderFactory.cs
// 
// Created on:      08/19/2015 at 9:31 PM
// Last Modified:   08/23/2015 at 11:39 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace MattEland.Ani.Alfred.Core.Modules.SysMonitor
{
    /// <summary>
    ///     Represents a piece of capability that can build metric providers
    /// </summary>
    public interface IMetricProviderFactory
    {
        /// <summary>
        ///     Builds a metric provider for the specified type.
        ///     Note that some metric providers implement <see cref="IDisposable"/>.
        /// </summary>
        /// <param name="categoryName">Name of the category.</param>
        /// <param name="counterName">Name of the counter.</param>
        /// <param name="instanceName">Name of the instance.</param>
        /// <returns>A metric provider</returns>
        [NotNull]
        MetricProviderBase Build([NotNull] string categoryName,
                                 [NotNull] string counterName,
                                 [CanBeNull] string instanceName = null);

        /// <summary>
        ///     Gets the names of each counter instance in a category.
        /// </summary>
        /// <param name="categoryName">Name of the category.</param>
        /// <returns>A collection of counter instance names</returns>
        [NotNull]
        IEnumerable<string> GetCategoryInstanceNames([NotNull] string categoryName);
    }

}