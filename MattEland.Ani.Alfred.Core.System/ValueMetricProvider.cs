﻿// ---------------------------------------------------------
// ValueMetricProvider.cs
// 
// Created on:      08/18/2015 at 1:50 PM
// Last Modified:   08/18/2015 at 1:50 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;

using JetBrains.Annotations;

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
        ///     Initializes a new instance of the <see cref="ValueMetricProvider" /> class.
        /// </summary>
        /// <param name="name">The name of the metric.</param>
        /// <exception cref="ArgumentNullException"><paramref name="factory"/> is <see langword="null" />.</exception>
        public ValueMetricProvider([NotNull] ValueMetricProviderFactory factory, string name) : base(name)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            Factory = factory;
        }

        /// <summary>
        /// Gets or sets the value provider factory.
        /// </summary>
        /// <value>The factory.</value>
        [NotNull]
        public ValueMetricProviderFactory Factory { get; set; }

        /// <summary>
        ///     Gets or sets the value that will be provided the next time NextValue() is called.
        /// </summary>
        /// <value>The value.</value>
        public float? Value { get; set; }

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