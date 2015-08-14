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

using JetBrains.Annotations;

namespace MattEland.Common
{
    /// <summary>
    ///     A set of extension methods related to collections
    /// </summary>
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
    }
}