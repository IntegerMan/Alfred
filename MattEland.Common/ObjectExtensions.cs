// ---------------------------------------------------------
// ObjectExtensions.cs
// 
// Created on:      08/23/2015 at 2:16 PM
// Last Modified:   08/23/2015 at 2:18 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Globalization;

using JetBrains.Annotations;

namespace MattEland.Common
{
    /// <summary>
    /// Extension methods for all classes.
    /// </summary>
    public static class ObjectExtensions
    {

        /// <summary>
        ///     Attempts to call <see cref="IDisposable.Dispose" /> on the object by casting it to a
        ///     disposable. If the item is not IDisposable, no action will be taken.
        /// </summary>
        /// <param name="item">The item.</param>
        public static void TryDispose([CanBeNull] this object item)
        {
            var disposable = item as IDisposable;
            disposable?.Dispose();
        }

        /// <summary>
        /// Turns the object into a string representation with null results represented as string.Empty.
        /// </summary>
        /// <param name="item">The item. Can be null.</param>
        /// <returns>A non-null string representation of item.</returns>
        [NotNull]
        public static string AsNonNullString([CanBeNull] this object item)
        {
            if (item == null) { return string.Empty; }

            var result = item.ToString();
            return result.NonNull();
        }
    }
}