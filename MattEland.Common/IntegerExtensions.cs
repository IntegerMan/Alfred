// ---------------------------------------------------------
// IntegerExtensions.cs
// 
// Created on:      08/16/2015 at 12:01 AM
// Last Modified:   08/16/2015 at 12:01 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace MattEland.Common
{
    /// <summary>
    /// A collection of extension methods related to integer operations.
    /// </summary>
    [PublicAPI]
    public static class IntegerExtensions
    {

        /// <summary>
        /// Determines whether an index is within the bounds of the specified collection.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="collection">The collection. Cannot be null.</param>
        /// <returns><c>true</c> if the index is within bounds of the specified collection; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="ArgumentNullException"><paramref name="collection" /> is <see langword="null" />.</exception>
        public static bool IsWithinBoundsOf(this int index, [NotNull] ICollection collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            return index >= 0 && index < collection.Count;
        }

        /// <summary>
        /// Determines whether an index is within the bounds of the specified collection.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="collection">The collection. Cannot be null.</param>
        /// <returns><c>true</c> if the index is within bounds of the specified collection; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="ArgumentNullException"><paramref name="collection" /> is <see langword="null" />.</exception>
        public static bool IsWithinBoundsOf<T>(this int index, [NotNull] IList<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            return index >= 0 && index < collection.Count;
        }

        /// <summary>
        ///     Returns the <paramref name="singular" /> value if <paramref name="count"/> is 1.
        ///     Otherwise <paramref name="plural"/> is returned.
        /// </summary>
        /// <param name="count"> The count. </param>
        /// <param name="singular">
        ///     The singular return value. The pound character can be used to represent
        ///     <paramref name="count"/> in the output value.
        /// </param>
        /// <param name="plural">
        ///     The plural return value. The pound character can be used to represent
        ///     <paramref name="count"/> in the output value.
        /// </param>
        /// <returns>
        ///     The <paramref name="singular"/> value if <paramref name="count"/> is 1. Otherwise
        ///     <paramref name="plural"/> is returned.
        /// </returns>
        [NotNull]
        public static string Pluralize(this int count, string singular, string plural)
        {

            // Based on the input value, return the appropriate result, substituting count for #
            return count == 1
                ? singular.NonNull().Replace("#", count.ToString())
                : plural.NonNull().Replace("#", count.ToString());
        }
    }
}