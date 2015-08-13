// ---------------------------------------------------------
// CommonExtensions.cs
// 
// Created on:      08/05/2015 at 2:16 PM
// Last Modified:   08/08/2015 at 7:32 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using JetBrains.Annotations;

namespace MattEland.Common
{
    /// <summary>
    ///     Extension methods commonly used
    /// </summary>
    public static class CommonExtensions
    {
        /// <summary>
        ///     Ensures that the passed in string is not null and returns either the input string or string.empty.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>A sanitized string</returns>
        [NotNull]
        public static string NonNull([CanBeNull] this string input)
        {
            return input ?? string.Empty;
        }

        /// <summary>
        /// Ensures that the passed in string is not null and returns either the input string or replacement.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="replacement">The replacement string when input is null.</param>
        /// <returns>A sanitized string</returns>
        [NotNull]
        public static string IfNull([CanBeNull] this string input, [NotNull] string replacement)
        {
            if (replacement == null)
            {
                throw new ArgumentNullException(nameof(replacement));
            }

            return input ?? replacement;
        }

        /// <summary>
        /// Extension method to compare two strings and return true if they're equal.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="other">The other string.</param>
        /// <param name="comparison">The comparison type. Defaults to ordinal ignoring case.</param>
        /// <returns><c>true</c> if the strings are equal, <c>false</c> otherwise.</returns>
        [NotNull]
        public static bool Compare([CanBeNull] this string input, [CanBeNull] string other, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            return string.Compare(input, other, comparison) == 0;
        }

        /// <summary>
        ///     Gets the Version of this module's assembly based on the AssemblyVersionAttribute.
        /// </summary>
        /// <param name="caller">The caller.</param>
        /// <returns>The Version of this module's assembly</returns>
        /// <exception cref="System.ArgumentNullException">caller</exception>
        [CanBeNull]
        public static Version GetAssemblyVersion([NotNull] this object caller)
        {
            if (caller == null)
            {
                throw new ArgumentNullException(nameof(caller));
            }

            try
            {
                var assembly = caller.GetType().Assembly;
                var assemblyName = new AssemblyName(assembly.FullName);
                return assemblyName.Version;
            }
            catch (IOException)
            {
                return null;
            }
        }

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