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

    }
}