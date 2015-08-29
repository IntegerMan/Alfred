// ---------------------------------------------------------
// AlfredTestExtensions.cs
// 
// Created on:      08/25/2015 at 11:44 AM
// Last Modified:   08/28/2015 at 1:11 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Common;

using NUnit.Framework;

using Shouldly;

namespace MattEland.Ani.Alfred.Tests
{
    /// <summary>
    ///     Contains extension methods useful for testing parts of various Alfred assemblies.
    /// </summary>
    internal static class AlfredTestExtensions
    {
        /// <summary>
        ///     Finds the provider with the specified name.
        /// </summary>
        /// <param name="providers">The providers.</param>
        /// <param name="name">The name.</param>
        /// <returns>The provider</returns>
        [CanBeNull]
        public static IPropertyProvider Find(
            [NotNull] this IEnumerable<IPropertyProvider> providers,
            string name)
        {
            return providers.FirstOrDefault(p => p != null && p.DisplayName.Matches(name));
        }

        /// <summary>
        /// Asserts that the source object is not null and fails the test if it is.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="failureMessage">The failure message.</param>
        [AssertionMethod]
        public static void ShouldNotBeNull(
            [AssertionCondition(AssertionConditionType.IS_NOT_NULL)] [CanBeNull] this object source, string failureMessage = "Object should not be null but was")
        {
            source.ShouldNotBe(null, failureMessage);
        }

        /// <summary>
        /// Asserts that the source object is null and fails the test if it is not.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="failureMessage">The failure message.</param>
        [AssertionMethod]
        public static void ShouldBeNull(
            [AssertionCondition(AssertionConditionType.IS_NULL)] [CanBeNull] this object source, string failureMessage = "Object should be null but was not")
        {
            source.ShouldBe(null, failureMessage);
        }
    }
}