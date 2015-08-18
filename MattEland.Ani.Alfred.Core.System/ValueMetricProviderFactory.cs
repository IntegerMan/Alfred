namespace MattEland.Ani.Alfred.Core.Modules.SysMonitor
{
    /// <summary>
    /// A factory that builds ValueMetricProviders
    /// </summary>
    /// <remarks>
    /// This is primarily used for testing
    /// </remarks>
    public class ValueMetricProviderFactory : IMetricProviderFactory
    {
        /// <summary>
        ///     Builds a metric provider for the specified type.
        /// </summary>
        /// <param name="categoryName">Name of the category.</param>
        /// <param name="counterName">Name of the counter.</param>
        /// <param name="instanceName">Name of the instance.</param>
        /// <returns>A metric provider</returns>
        public MetricProviderBase Build(string categoryName, string counterName, string instanceName = null)
        {
            return new ValueMetricProvider(instanceName ?? counterName);
        }
    }
}