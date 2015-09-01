// ---------------------------------------------------------
// Class1.cs
// 
// Created on:      09/01/2015 at 12:56 PM
// Last Modified:   09/01/2015 at 1:04 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using MattEland.Common;

using Shouldly;

namespace MattEland.Testing
{
    /// <summary>
    ///     A collection of helper extension methods for Unit tests.
    /// </summary>
    public static class TestExtensions
    {

        /// <summary>
        ///     Asserts that the <paramref name="source" /> object is not <see langword="null" /> and
        ///     fails the test if it is.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="failureMessage">The failure message.</param>
        /// <returns>
        ///     The <paramref name="source"/> object.
        /// </returns>
        [AssertionMethod]
        [NotNull]
        public static object ShouldNotBeNull(
            [AssertionCondition(AssertionConditionType.IS_NOT_NULL)] [CanBeNull] this object source,
            string failureMessage = "Object should not be null but was")
        {
            source.ShouldNotBe(null, failureMessage);

            return source;
        }

        /// <summary>
        ///     Asserts that the <paramref name="source" /> object is <see langword="null" /> and fails
        ///     the test if it is not.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="failureMessage">The failure message.</param>
        /// <returns>
        ///     The <paramref name="source"/> object.
        /// </returns>
        [AssertionMethod]
        [CanBeNull]
        public static object ShouldBeNull(
            [AssertionCondition(AssertionConditionType.IS_NULL)] [CanBeNull] this object source,
            string failureMessage = "Object should be null but was not")
        {
            source.ShouldBe(null, failureMessage);

            return source;
        }

        /// <summary>
        ///     A boolean extension method that asserts that the <paramref name="actual" /> value will be
        ///     <see langword="false" /> and fails if it is true.
        /// </summary>
        /// <param name="actual">the actual value to test.</param>
        /// <param name="failureMessage">The failure message to display if the test fails.</param>
        /// <returns>
        ///     <see langword="true" /> if it succeeds. <see langword="false" /> will never be returned as that
        ///     will cause an assertion to trigger.
        /// </returns>
        public static bool ShouldBeTrue(
            this bool actual,
            string failureMessage = "The result was true instead of false")
        {
            actual.ShouldBe(true);

            return true;
        }

        /// <summary>
        ///     A boolean extension method that asserts that the <paramref name="actual" /> value will be
        ///     <see langword="false" /> and fails if it is true.
        /// </summary>
        /// <param name="actual">the actual value to test.</param>
        /// <param name="failureMessage">The failure message to display if the test fails.</param>
        /// <returns>
        ///     <see langword="true" /> if it succeeds. <see langword="false" /> will never be returned as that
        ///     will cause an assertion to trigger.
        /// </returns>
        public static bool ShouldBeFalse(
            this bool actual,
            string failureMessage = "The result was false instead of true")
        {
            actual.ShouldBe(false);

            return true;
        }

        /// <summary>
        ///     An <see langword="object" /> extension method that tries to cast the
        ///     <paramref name="actual" /> object to the specified type parameter and returns a strongly- typed
        ///     result if the action succeeds.
        /// </summary>
        /// <typeparam name="T">The type to cast to.</typeparam>
        /// <param name="actual">The actual object to act on.</param>
        /// <param name="failureMessage">The failure message.</param>
        /// <returns>The <paramref name="actual" /> object cast to the specified type.</returns>
        [NotNull]
        public static T ShouldBe<T>(
            [CanBeNull] this object actual,
            [CanBeNull] string failureMessage = null)
        {
            // Validate Input
            actual.ShouldNotBeNull("The actual object was null prior to being cast.");

            var cast = actual.ShouldBeOfType<T>();

            // Ensure we have an adequate failure message
            if (failureMessage.IsEmpty())
            {
                failureMessage = $"{actual} was not of type {typeof(T).Name}";
            }
            cast.ShouldNotBeNull(failureMessage.NonNull());

            return cast;
        }

        /// <summary>Enumerates the <paramref name="collection" /> and returns a strongly-typed collection.</summary>
        /// <typeparam name="T">The type each item should be cast to.</typeparam>
        /// <param name="collection">The collection to act on.</param>
        /// <param name="failureMessage">The failure message.</param>
        /// <returns>
        ///     An enumerator that allows <see langword="foreach" /> to be used to process should all
        ///     items <paramref name="collection" /> as a strongly-typed list.
        /// </returns>
        [NotNull]
        public static IList<T> ShouldAllBe<T>(
            [CanBeNull] this IEnumerable collection,
            [CanBeNull] string failureMessage = null)
        {
            // Validate Input
            collection.ShouldNotBeNull("The collection object was null prior to being cast.");

            // Ensure we have an adequate failure message
            if (failureMessage.IsEmpty())
            {
                failureMessage = $"An item in the collection was not of type {typeof(T).Name}";
            }

            // Cast all items and return them in a strongly-typed List<T>
            return (from object item in collection select item.ShouldBe<T>(failureMessage)).ToList();
        }
    }
}