// ---------------------------------------------------------
// AlfredTestExtensions.cs
// 
// Created on:      08/25/2015 at 11:44 AM
// Last Modified:   08/28/2015 at 11:35 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core;
using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Modules.SysMonitor;
using MattEland.Common;
using MattEland.Common.Providers;
using MattEland.Testing;

namespace MattEland.Ani.Alfred.Tests
{
    /// <summary>
    ///     Contains extension methods useful for testing parts of various Alfred assemblies.
    /// </summary>
    internal static class AlfredTestExtensions
    {
        /// <summary>
        ///     Registers the default Alfred <paramref name="container" /> with good default testing
        ///     values.
        /// </summary>
        /// <exception cref="ArgumentNullException"> <paramref name="container" /> </exception>
        /// <param name="container"> The container. </param>
        public static void RegisterDefaultAlfredMappings([NotNull] this IObjectContainer container)
        {
            if (container == null) { throw new ArgumentNullException(nameof(container)); }

            var console = new DiagnosticConsole(container);
            console.RegisterAsProvidedInstance(typeof(IConsole), container);

            // Register mappings for promised types
            container.Register(typeof(IConsoleEvent), typeof(ConsoleEvent));
            container.Register(typeof(IAlfredCommand), typeof(AlfredCommand));
            container.Register(typeof(MetricProviderBase), typeof(ValueMetricProvider));

            // We'll want to get at the same factory any time we request a factory for test purposes
            var factory = new ValueMetricProviderFactory();
            factory.RegisterAsProvidedInstance(typeof(IMetricProviderFactory), container);
            factory.RegisterAsProvidedInstance(typeof(ValueMetricProviderFactory), container);
        }

        /// <summary>
        ///     Finds the provider with the specified <paramref name="name" /> .
        /// </summary>
        /// <param name="providers"> The providers. </param>
        /// <param name="name"> The name. </param>
        /// <returns>
        ///     The provider or <see langword="null"/> if not found.
        /// </returns>
        [CanBeNull]
        public static IPropertyProvider Find(
            [NotNull] this IEnumerable<IPropertyProvider> providers,
            string name)
        {
            return providers.FirstOrDefault(p => p != null && p.DisplayName.Matches(name));
        }

        /// <summary>
        ///     Finds the property with the specified <paramref name="name" />.
        /// </summary>
        /// <param name="properties"> The properties. </param>
        /// <param name="name"> The name. </param>
        /// <returns>
        ///     The property or <see langword="null"/> if not found.
        /// </returns>
        [CanBeNull]
        public static IPropertyItem Find(
            [NotNull] this IEnumerable<IPropertyItem> properties,
            string name)
        {
            return properties.FirstOrDefault(p => p != null && p.DisplayName.Matches(name));
        }

        /// <summary>
        ///     Navigates into the property <paramref name="provider"/> with the
        ///     <paramref name="name"/> provided and returns that IPropertyProvider.
        /// </summary>
        /// <param name="provider">The provider to act on.</param>
        /// <param name="name">The name to find.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when one or more required arguments are null.
        /// </exception>
        /// <returns>The IPropertyProvider.</returns>
        [NotNull]
        public static IPropertyProvider Nav([NotNull] this IPropertyProvider provider, string name)
        {
            //- Validate
            if (provider == null) { throw new ArgumentNullException(nameof(provider)); }

            // Find the child we're looking for, allowing null so we can assert a failure message
            var node = provider.PropertyProviders.FirstOrDefault(p => p.Name.Matches(name));

            // Assert a detailed failure if we couldn't find the node
            node.ShouldNotBeNull($"Could not find child of {provider.Name} with name {name}");

            return node;
        }

    }
}