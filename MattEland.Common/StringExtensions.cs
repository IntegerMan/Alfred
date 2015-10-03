// ---------------------------------------------------------
// StringExtensions.cs
// 
// Created on:      08/19/2015 at 9:31 PM
// Last Modified:   09/04/2015 at 11:23 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Globalization;
using System.Text;

using JetBrains.Annotations;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace MattEland.Common
{
    /// <summary>
    ///     Contains extension methods dealing with string operations.
    /// </summary>
    [PublicAPI]
    public static class StringExtensions
    {
        /// <summary>
        ///     Determines if the <paramref name="input"/> string is has text (is not
        ///     <see langword="null"/> and has text beyond whitespace).
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns><c>true</c> if the string has text; otherwise, <c>false</c> .</returns>
        [System.Diagnostics.Contracts.Pure]
        public static bool HasText([CanBeNull] this string input)
        {
            return !input.IsNullOrWhitespace();
        }

        /// <summary>
        ///     Determines if the <paramref name="input"/> string is <see langword="null"/> or an
        ///     empty string.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>
        ///     <c>true</c> if the string is <see langword="null"/> or empty; otherwise, 
        /// <c>false</c> .
        /// </returns>
        public static bool IsEmpty([CanBeNull] this string input)
        {
            return string.IsNullOrEmpty(input.NonNull().Trim());
        }

        /// <summary>
        ///     Determines if the <paramref name="input"/> string is null, an empty string, or
        ///     whitespace.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>
        ///     <c>true</c> if the string is <see langword="null"/> or whitespace; otherwise, 
        /// <c>false</c> .
        /// </returns>
        public static bool IsNullOrWhitespace([CanBeNull] this string input)
        {
            return string.IsNullOrWhiteSpace(input);
        }

        /// <summary>
        ///     Ensures that the passed in string is not <see langword="null"/> and returns either
        ///     the <paramref name="input"/> string or string.empty.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>A sanitized string</returns>
        [NotNull]
        public static string NonNull([CanBeNull] this string input)
        {
            return input ?? string.Empty;
        }

        /// <summary>
        ///     Ensures that the passed in string is not <see langword="null"/> and returns either
        ///     the <paramref name="input"/> string or replacement.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="replacement">
        /// The replacement string when <paramref name="input"/> is null.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="replacement" /> is <see langword="null" /> .
        /// </exception>
        /// <returns>A sanitized string</returns>
        [NotNull]
        public static string IfNull([CanBeNull] this string input, [NotNull] string replacement)
        {
            if (replacement == null) { throw new ArgumentNullException(nameof(replacement)); }

            return input ?? replacement;
        }

        /// <summary>
        ///     Extension method to compare two strings and return <see langword="true"/> if they're
        ///     equal.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="other">The other string.</param>
        /// <returns><c>true</c> if the strings are equal, <c>false</c> otherwise.</returns>
        public static bool Matches([CanBeNull] this string input, [CanBeNull] string other)
        {
            return Matches(input, other, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        ///     Extension method to compare two strings and return <see langword="true"/> if they're
        ///     equal according to the <paramref name="comparison"/> type used.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="other">The other string.</param>
        /// <param name="comparison">The comparison type.</param>
        /// <returns><c>true</c> if the strings are equal, <c>false</c> otherwise.</returns>
        public static bool Matches(
            [CanBeNull] this string input,
            [CanBeNull] string other,
            StringComparison comparison)
        {
            return string.Compare(input, other, comparison) == 0;
        }

        /// <summary>
        ///     Converts the <paramref name="input"/> string to an integer, falling back to the
        ///     fallback value on parse error.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="fallbackValue">The fallback value. Defaults to 0.</param>
        /// <returns>The parsed value or the fallback value in case of parse error.</returns>
        public static int AsInt([CanBeNull] this string input, int fallbackValue = 0)
        {
            int output;

            // Safely cast the input as an integer. Failure will not throw an exception
            if (!int.TryParse(input?.Trim(), out output))
            {
                // In case of failure, use our fallback value
                output = fallbackValue;
            }

            return output;
        }

        /// <summary>
        ///     Converts the <paramref name="input"/> string to a double, falling back to the
        ///     fallback value on parse error.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="fallbackValue">The fallback value. Defaults to 0.</param>
        /// <returns>The parsed value or the fallback value in case of parse error.</returns>
        public static double AsDouble([CanBeNull] this string input, double fallbackValue = 0)
        {
            double output;

            // Safely cast the input as an integer. Failure will not throw an exception
            if (!double.TryParse(input?.Trim(), out output))
            {
                // In case of failure, use our fallback value
                output = fallbackValue;
            }

            return output;
        }

        /// <summary>
        ///     Appends to a string builder either as a standard append or as an AppendLine,
        ///     depending on <paramref name="useNewLine"/>
        /// </summary>
        /// <param name="stringBuilder">The string builder.</param>
        /// <param name="message">The message.</param>
        /// <param name="useNewLine">Whether or not to include line breaks.</param>
        public static void AppendConditional(
            [CanBeNull] this StringBuilder stringBuilder,
            [CanBeNull] string message,
            bool useNewLine)
        {
            if (stringBuilder == null) { return; }

            if (useNewLine)
            {
                stringBuilder.AppendLine(message);
            }
            else
            {
                stringBuilder.Append(message);
            }
        }

        /// <summary>
        ///     Formats the specified <paramref name="input"/> using the given
        ///     <paramref name="culture"/> (or current culture) and optional format string.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="culture">The culture.</param>
        /// <param name="outputFormat">The format string to use.</param>
        /// <returns>The formatted string</returns>
        [NotNull]
        public static string Format(
            [CanBeNull] this IFormattable input,
            [CanBeNull] IFormatProvider culture = null,
            [CanBeNull] string outputFormat = null)
        {
            culture = culture ?? CultureInfo.CurrentCulture;

            return input?.ToString(outputFormat, culture) ?? string.Empty;
        }

        /// <summary>
        ///     Formats the given string using the user's culture
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>System.String.</returns>
        [NotNull]
        public static string ForUser([CanBeNull] this IFormattable input)
        {
            return input.AsNonNullString();
        }

        /// <summary>
        ///     Formats the given string using invariant culture.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>The formatted string</returns>
        [NotNull]
        public static string Invariant([CanBeNull] this IFormattable input)
        {
            return input?.Format(CultureInfo.InvariantCulture) ?? string.Empty;
        }

        /// <summary>
        ///     Gets a comma delimited string of values.
        /// </summary>
        /// <param name="values"> The string values. </param>
        /// <returns>
        ///     The comma delimited string.
        /// </returns>
        public static string BuildCommaDelimitedString([CanBeNull] this IEnumerable<string> values)
        {
            // Guard against null
            if (values == null) return string.Empty;

            // Build out a comma separated list of values
            var builder = new StringBuilder();
            foreach (var tag in values)
            {
                builder.AppendFormat("{0}, ", tag);
            }

            var result = builder.ToString();

            // Remove the last ", " from the string
            if (result.Length > 2)
            {
                result = result.Substring(0, result.Length - 2);
            }
            return result;
        }

    }
}