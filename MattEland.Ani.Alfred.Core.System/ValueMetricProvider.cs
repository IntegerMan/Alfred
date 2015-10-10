// ---------------------------------------------------------
// ValueMetricProvider.cs
// 
// Created on:      08/18/2015 at 1:50 PM
// Last Modified:   08/18/2015 at 1:50 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;

using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.Core.Modules.SysMonitor
{
    /// <summary>
    ///     A value-based metric provider that provides a value from its Value property when NextValue is
    ///     called.
    /// </summary>
    /// <remarks>
    ///     This class is intended primarily for testing purposes.
    /// </remarks>
    public sealed class ValueMetricProvider : MetricProviderBase
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueMetricProvider" /> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="name">The name of the metric.</param>
        /// <param name="value">The value to return</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="ArgumentNullException"><paramref name="factory" /> is <see langword="null" />.</exception>
        [SuppressMessage("ReSharper", "CodeAnnotationAnalyzer")]
        public ValueMetricProvider([NotNull] ValueMetricProviderFactory factory,
                                   [NotNull] string name, float? value = null) : base(name)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            Factory = factory;
            Value = value;
        }

        /// <summary>
        /// Gets or sets the value provider factory.
        /// </summary>
        /// <value>The factory.</value>
        [NotNull]
        public ValueMetricProviderFactory Factory { get; }

        /// <summary>
        ///     Gets or sets the value that will be provided the next time NextValue() is called.
        /// </summary>
        /// <value>The value.</value>
        [UsedImplicitly]
        public float? Value
        {
            get;
            set;
        }

        /// <summary>
        ///     Gets the next value from the metric provider
        /// </summary>
        /// <returns>The next value</returns>
        public override float NextValue()
        {
            if (Value.HasValue)
            {
                return Value.Value;
            }

            return Factory.DefaultValue;
        }
    }
}