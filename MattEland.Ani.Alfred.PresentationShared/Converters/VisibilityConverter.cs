// ---------------------------------------------------------
// VisibilityConverter.cs
// 
// Created on:      09/03/2015 at 11:00 PM
// Last Modified:   09/07/2015 at 10:04 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

using JetBrains.Annotations;

namespace MattEland.Ani.Alfred.PresentationAvalon.Converters
{
    /// <summary>
    ///     The visibility converter
    /// </summary>
    public sealed class VisibilityConverter : IValueConverter
    {
        /// <summary>
        ///     Gets or sets a value indicating whether the visibility value is inverted so that
        ///     <see langword="false"/> is visible and <see langword="true"/> is not.
        /// </summary>
        /// <value>
        /// <c>true</c> if inverted; otherwise, <c>false</c> .
        /// </value>
        [UsedImplicitly]
        public bool Invert { get; set; }

        /// <summary>
        ///     Converts a <paramref name="value" /> .
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        ///     A converted value. If the method returns <see langword="null" /> , the valid
        ///     <see langword="null" /> <paramref name="value" /> is used.
        /// </returns>
        public object Convert(
            [CanBeNull] object value,
            [CanBeNull] Type targetType,
            [CanBeNull] object parameter,
            [CanBeNull] CultureInfo culture)
        {
            var result = false;

            if (value != null)
            {
                // Work with boolean values
                bool tryBool;
                if (bool.TryParse(value.ToString(), out tryBool)) { result = tryBool; }
            }

            // If we're inverting, flip around which output we'll push out
            if (Invert) { result = !result; }

            // Convert the bool to a visibility
            return result ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        ///     Converts a <paramref name="value" /> .
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <exception cref="NotSupportedException">
        /// <see cref="VisibilityConverter.ConvertBack" /> is not supported.
        /// </exception>
        /// <returns>
        ///     A converted value. If the method returns null, the valid <see langword="null" />
        ///     <paramref name="value" /> is used.
        /// </returns>
        public object ConvertBack(
            [CanBeNull] object value,
            [CanBeNull] Type targetType,
            [CanBeNull] object parameter,
            [CanBeNull] CultureInfo culture)
        {
            throw new NotSupportedException("ConvertBack is not supported");
        }
    }
}