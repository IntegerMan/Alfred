// ---------------------------------------------------------
// BooleanToVisibilityConverter.cs
// 
// Created on:      08/01/2015 at 10:32 PM
// Last Modified:   08/06/2015 at 4:44 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace MattEland.Ani.Alfred.Win8
{
    /// <summary>
    ///     An IValueConverter converting from a boolean to a Visibility.
    /// </summary>
    public class BooleanToVisibilityConverter : IValueConverter
    {
        /// <summary>
        ///     Converts the specified value to a boolean.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="language">The language.</param>
        /// <returns>The result of the conversion.</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            // Guard against null values
            if (value == null)
            {
                return Visibility.Collapsed;
            }

            // Okay, we have an actual value. We'll inspect it and then handle it according to its type
            bool isVisible;

            if (value is bool)
            {
                // It's boolean! Great!
                isVisible = (bool)value;
            }
            else if (value is Visibility)
            {
                // What the heck? We'll judge based on if it is Visible already
                isVisible = ((Visibility)value == Visibility.Visible);
            }
            else
            {
                // It's some other value. We'll just trust bool.TryParse with it
                bool.TryParse(value.ToString(), out isVisible);
            }

            // Translate from our boolean to a Visibility value
            return isVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        ///     Converts backwards.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="language">The language.</param>
        /// <returns>System.Object.</returns>
        /// <exception cref="System.NotImplementedException">ConvertBack is not supported by this converter</exception>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException("ConvertBack is not supported by this converter");
        }
    }
}