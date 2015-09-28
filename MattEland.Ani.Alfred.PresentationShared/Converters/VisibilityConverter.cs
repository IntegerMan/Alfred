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
using MattEland.Ani.Alfred.PresentationCommon.Converters;

namespace MattEland.Ani.Alfred.PresentationAvalon.Converters
{
    /// <summary>
    ///     The visibility converter
    /// </summary>
    public sealed class VisibilityConverter : VisibilityConverterBase, IValueConverter
    {

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
            // Use Shared Presentation Logic to convert the boolean to a visibility value
            return ShouldBeVisible(value) ?
                       Visibility.Visible :
                       Visibility.Collapsed;
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