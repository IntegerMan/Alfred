// ---------------------------------------------------------
// IMetricProviderFactory.cs
// 
// Created on:      08/18/2015 at 1:54 PM
// Last Modified:   08/18/2015 at 1:58 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

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
        /// </summary>
        /// <param name="categoryName">Name of the category.</param>
        /// <param name="counterName">Name of the counter.</param>
        /// <param name="instanceName">Name of the instance.</param>
        /// <returns>A metric provider</returns>
        [NotNull]
        MetricProviderBase Build(string categoryName,
                                    string counterName,
                                    string instanceName = null);
    }

}