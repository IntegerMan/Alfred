// ---------------------------------------------------------
// HideIfNoTextConverter.cs
// 
// Created on:      10/26/2015 at 11:44 PM
// Last Modified:   10/26/2015 at 11:44 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

using MattEland.Common;

namespace MattEland.Ani.Alfred.PresentationAvalon.Converters
{
    /// <summary>
    ///     A value converter that returns Collapsed if null or empty string or Visible otherwise.
    /// </summary>
    public sealed class HideIfNoTextConverter : IValueConverter
    {
        /// <summary>
        ///     Converts a value.
        /// </summary>
        /// <param name="value"> The value produced by the binding source. </param>
        /// <param name="targetType"> Type of the target. </param>
        /// <param name="parameter"> The parameter. </param>
        /// <param name="culture"> The culture. </param>
        /// <returns>
        ///     A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.AsNonNullString().IsEmpty()
                ? Visibility.Collapsed
                : Visibility.Visible;
        }

        /// <summary>
        ///     Converts a value.
        /// </summary>
        /// <exception cref="NotSupportedException">
        ///     Thrown always as the requested operation is not supported.
        /// </exception>
        /// <param name="value"> The value that is produced by the binding target. </param>
        /// <param name="targetType"> Type of the target. </param>
        /// <param name="parameter"> The parameter. </param>
        /// <param name="culture"> The culture. </param>
        /// <returns>
        ///     A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}