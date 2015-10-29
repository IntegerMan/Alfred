using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;

namespace MattEland.Ani.Alfred.PresentationAvalon.Converters
{
    /// <summary>
    ///     A boolean to <see cref="Brush"/> converter. This class cannot be inherited.
    /// </summary>
    public sealed class BooleanToBrushConverter : IValueConverter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public BooleanToBrushConverter()
        {
            TrueBrush = Brushes.Green;
            FalseBrush = Brushes.Red;
            IndeterminateBrush = Brushes.Yellow;
        }

        /// <summary>
        ///     Gets or sets the <see cref="Brush"/> to use when the value is <c>false</c> during
        ///     <see cref="Convert"/>.
        /// </summary>
        /// <value>
        ///     The <see cref="Brush"/> to use when value is <c>false</c>.
        /// </value>
        public Brush FalseBrush { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="Brush"/> to use when the value is neither <c>true</c> or
        ///     <c>false</c> during <see cref="Convert"/>.
        /// </summary>
        /// <value>
        ///     The <see cref="Brush"/> to use when value is neither <c>true</c> or <c>false</c>.
        /// </value>
        public Brush IndeterminateBrush { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="Brush"/> to use when the value is <c>true</c> during
        ///     <see cref="Convert"/>.
        /// </summary>
        /// <value>
        ///     The <see cref="Brush"/> to use when value is <c>true</c>.
        /// </value>
        public Brush TrueBrush { get; set; }

        /// <summary>
        /// Converts a value from a boolean value to a brush. 
        /// </summary>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <param name="value">The value produced by the binding source.</param><param name="targetType">The type of the binding target property.</param><param name="parameter">The converter parameter to use.</param><param name="culture">The culture to use in the converter.</param>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Handle nulls
            if (value == null) return IndeterminateBrush;

            // Do a parse on the string representation of value and try to handle it as a bool
            bool boolValue;
            if (bool.TryParse(value.ToString(), out boolValue))
            {
                return boolValue ? TrueBrush : FalseBrush;
            }

            // Neither true nor false, so use indeterminate
            return IndeterminateBrush;
        }

        /// <summary>
        /// Converts a value back to its original value. This method is not supported.
        /// </summary>
        /// <returns>
        /// An exception will always be thrown.
        /// </returns>
        /// <param name="value">The value that is produced by the binding target.</param><param name="targetType">The type to convert to.</param><param name="parameter">The converter parameter to use.</param><param name="culture">The culture to use in the converter.</param>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
