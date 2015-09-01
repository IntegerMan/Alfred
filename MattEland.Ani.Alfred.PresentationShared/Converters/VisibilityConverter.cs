using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

using JetBrains.Annotations;

namespace MattEland.Ani.Alfred.PresentationShared.Converters
{
    /// <summary>
    /// The visibility converter
    /// </summary>
    public class VisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Gets or sets a value indicating whether the visibility value is inverted so that false is visible and true is not.
        /// </summary>
        /// <value><c>true</c> if inverted; otherwise, <c>false</c>.</value>
        public bool Invert { get; set; }

        /// <summary>
        /// Converts a value. 
        /// </summary>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <param name="value">The value produced by the binding source.</param><param name="targetType">The type of the binding target property.</param><param name="parameter">The converter parameter to use.</param><param name="culture">The culture to use in the converter.</param>
        public object Convert([CanBeNull] object value,
                              [CanBeNull] Type targetType,
                              [CanBeNull] object parameter,
                              [CanBeNull] CultureInfo culture)
        {
            bool result = false;

            if (value != null)
            {
                // Work with boolean values
                bool tryBool;
                if (Boolean.TryParse(value.ToString(), out tryBool))
                {
                    result = tryBool;
                }
            }

            // If we're inverting, flip around which output we'll push out
            if (Invert)
            {
                result = !result;
            }

            // Convert the bool to a visibility
            return result ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        /// Converts a value. 
        /// </summary>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <param name="value">The value that is produced by the binding target.</param><param name="targetType">The type to convert to.</param><param name="parameter">The converter parameter to use.</param><param name="culture">The culture to use in the converter.</param>
        /// <exception cref="NotSupportedException">ConvertBack is not supported</exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("ConvertBack is not supported");
        }
    }
}
