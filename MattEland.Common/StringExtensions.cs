// ---------------------------------------------------------
// StringExtensions.cs
// 
// Created on:      08/12/2015 at 2:12 PM
// Last Modified:   08/14/2015 at 11:26 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;

using JetBrains.Annotations;

namespace MattEland.Common
{
    /// <summary>
    ///     Contains extension methods dealing with string operations.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        ///     Determines if the input string is has text (is not null and has text beyond whitespace).
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns><c>true</c> if the string has text; otherwise, <c>false</c>.</returns>
        [UsedImplicitly]
        public static bool HasText([CanBeNull] this string input)
        {
            return !input.IsNullOrWhitespace();
        }

        /// <summary>
        ///     Determines if the input string is null or an empty string.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns><c>true</c> if the string is null or empty; otherwise, <c>false</c>.</returns>
        [UsedImplicitly]
        public static bool IsEmpty([CanBeNull] this string input)
        {
            return string.IsNullOrEmpty(input);
        }

        /// <summary>
        ///     Determines if the input string is null, an empty string, or whitespace.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns><c>true</c> if the string is null or whitespace; otherwise, <c>false</c>.</returns>
        [UsedImplicitly]
        public static bool IsNullOrWhitespace([CanBeNull] this string input)
        {
            return string.IsNullOrWhiteSpace(input);
        }

        /// <summary>
        ///     Ensures that the passed in string is not null and returns either the input string or
        ///     string.empty.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>A sanitized string</returns>
        [NotNull]
        public static string NonNull([CanBeNull] this string input)
        {
            return input ?? string.Empty;
        }

        /// <summary>
        ///     Ensures that the passed in string is not null and returns either the input string or
        ///     replacement.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="replacement">The replacement string when input is null.</param>
        /// <returns>A sanitized string</returns>
        /// <exception cref="ArgumentNullException"><paramref name="replacement" /> is <see langword="null" />.</exception>
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
        ///     Extension method to compare two strings and return true if they're equal according to the
        ///     comparison type used. By default strings are compared ordinally ignoring casing.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="other">The other string.</param>
        /// <param name="comparison">The comparison type. Defaults to ordinal ignoring case.</param>
        /// <returns><c>true</c> if the strings are equal, <c>false</c> otherwise.</returns>
        [NotNull]
        public static bool Matches([CanBeNull] this string input,
                                   [CanBeNull] string other,
                                   StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            return string.Compare(input, other, comparison) == 0;
        }
    }
}