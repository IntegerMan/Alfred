// ---------------------------------------------------------
// CollectionExtensions.cs
// 
// Created on:      08/14/2015 at 12:55 AM
// Last Modified:   08/14/2015 at 12:59 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

namespace MattEland.Common
{
    /// <summary>
    ///     A set of extension methods related to collections
    /// </summary>
    [PublicAPI]
    public static class CollectionExtensions
    {
        /// <summary>
        ///     Adds an item to a collection safely. This is a convenience method that does null and duplicate checking
        ///     based on the type of item / collection and will throw null reference or invalid operation exceptions if illegal
        ///     circumstances are met.
        /// </summary>
        /// <typeparam name="T">The type of item</typeparam>
        /// <param name="item">The item.</param>
        /// <param name="collection">The collection.</param>
        /// <exception cref="System.ArgumentNullException">
        ///     item, collection
        /// </exception>
        /// <exception cref="System.InvalidOperationException">The specified item was already part of the collection</exception>
        public static void AddSafe<T>([NotNull] this ICollection<T> collection, [NotNull] T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            // This shouldn't happen, but I want to check to make sure
            if (collection.Contains(item))
            {
                throw new InvalidOperationException(Resources.ErrorItemAlreadyInCollection);
            }

            collection.Add(item);
        }

        /// <summary>
        /// Returns the singular value if collection's count of items is 1. Otherwise plural is returned.
        /// </summary>
        /// <typeparam name="T">The type the generic collection is holding.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="singular">The singular return value.</param>
        /// <param name="plural">The plural return value.</param>
        /// <returns>The singular value if the item count is 1. Otherwise plural is returned.</returns>
        [NotNull]
        public static string Pluralize<T>([NotNull] this IEnumerable<T> collection,
                                          [CanBeNull] string singular,
                                          [CanBeNull] string plural)
        {
            var count = collection.Count();
            return count.Pluralize(singular, plural);
        }

        /// <summary>
        ///     A generic list extension method that returns a random item within the
        ///     <paramref name="list"/>.
        /// </summary>
        /// <typeparam name="T"> The type of item the list contains. </typeparam>
        /// <param name="list"> The list to act on. </param>
        /// <param name="randomizer">
        ///     The randomizer. If this is null a new randomizer will be used.
        /// </param>
        /// <returns>
        ///     The random item.
        /// </returns>
        public static T GetRandomItem<T>([NotNull] this IList<T> list, [CanBeNull] Random randomizer = null)
        {
            // Default to a new randomizer if none provided
            randomizer = randomizer ?? new Random();

            // Get a random index somewhere in the list
            int index = randomizer.Next(list.Count);

            // Return the item at the random index
            return list[index];
        }


    }
}