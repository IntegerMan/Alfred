// ---------------------------------------------------------
// ValueMetricProviderFactory.cs
// 
// Created on:      08/19/2015 at 9:31 PM
// Last Modified:   08/21/2015 at 5:33 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics;

using JetBrains.Annotations;

namespace MattEland.Ani.Alfred.Core.Modules.SysMonitor
{
    /// <summary>
    ///     A factory that builds ValueMetricProviders
    /// </summary>
    /// <remarks>
    ///     This is primarily used for testing
    /// </remarks>
    public class ValueMetricProviderFactory : IMetricProviderFactory
    {
        /// <summary>
        ///     Gets or sets the value to provide to ValueMetricProviders that don't have values of their own
        ///     set.
        /// </summary>
        /// <value>The value.</value>
        public float DefaultValue { get; set; }

        /// <summary>
        ///     Gets the value providers.
        /// </summary>
        /// <value>The value providers.</value>
        [NotNull]
        [ItemNotNull]
        [UsedImplicitly]
        public ICollection<ValueMetricProvider> Providers { get; } = new List<ValueMetricProvider>();

        /// <summary>
        ///     Gets the category instance names dictionary. This dictionary allows test code to register
        ///     expected responses for when GetCategoryInstanceNames is called.
        /// </summary>
        /// <value>The category instance names.</value>
        [NotNull, ItemNotNull]
        public IDictionary<string, IEnumerable<string>> CategoryInstanceNames { get; } =
            new Dictionary<string, IEnumerable<string>>();

        /// <summary>
        ///     Builds a metric provider for the specified type.
        /// </summary>
        /// <param name="categoryName">Name of the category.</param>
        /// <param name="counterName">Name of the counter.</param>
        /// <param name="instanceName">Name of the instance.</param>
        /// <returns>A metric provider</returns>
        [NotNull]
        public MetricProviderBase Build([NotNull] string categoryName,
                                        [NotNull] string counterName,
                                        [CanBeNull] string instanceName = null)
        {
            var provider = new ValueMetricProvider(this, instanceName ?? counterName);

            // Register the item in the list so it can be accessed during testing
            Providers.Add(provider);

            return provider;
        }

        /// <summary>
        ///     Gets the names of each counter instance in a category.
        /// </summary>
        /// <param name="categoryName">Name of the category.</param>
        /// <returns>A collection of counter instance names</returns>
        [NotNull]
        public IEnumerable<string> GetCategoryInstanceNames([NotNull] string categoryName)
        {
            if (CategoryInstanceNames.ContainsKey(categoryName))
            {
                var instanceNames = CategoryInstanceNames[categoryName];
                Debug.Assert(instanceNames != null);
                return instanceNames;
            }

            // Fallback to an empty list
            return new List<string>();
        }
    }
}