// ---------------------------------------------------------
// AlfredProgressBarWidget.cs
// 
// Created on:      08/04/2015 at 3:34 PM
// Last Modified:   08/04/2015 at 4:04 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System.Globalization;

using JetBrains.Annotations;

namespace MattEland.Ani.Alfred.Core.Widgets
{
    /// <summary>
    /// A progress bar widget to be interpreted at the user interface layer
    /// </summary>
    public sealed class AlfredProgressBarWidget : AlfredTextWidget
    {
        private float _maximum = 100;
        private float _minimum;
        private float _value;

        /// <summary>
        ///     Gets or sets the minimum value to display on the bar.
        /// </summary>
        /// <value>The minimum.</value>
        public float Minimum
        {
            get { return _minimum; }
            set
            {
                if (!value.Equals(_minimum))
                {
                    _minimum = value;
                    OnPropertyChanged(nameof(Minimum));
                }
            }
        }

        /// <summary>
        ///     Gets or sets the maximum value to display on the bar.
        /// </summary>
        /// <value>The maximum value.</value>
        public float Maximum
        {
            get { return _maximum; }
            set
            {
                if (!value.Equals(_maximum))
                {
                    _maximum = value;
                    OnPropertyChanged(nameof(Maximum));
                }
            }
        }

        /// <summary>
        ///     Gets or sets the current value.
        /// </summary>
        /// <value>The value.</value>
        public float Value
        {
            get { return _value; }
            set
            {
                if (!value.Equals(_value))
                {
                    _value = value;

                    OnPropertyChanged(nameof(Value));
                    OnPropertyChanged(nameof(ValueText));
                }
            }
        }

        /// <summary>
        ///     Gets the value to display in the user interface for the ToolTip.
        /// </summary>
        /// <value>A textual representation of the Value field.</value>
        public string ValueText
        {
            get
            {
                var formatString = _valueFormatString ?? "{0}";

                return string.Format(CultureInfo.CurrentCulture, formatString, Value);
            }
        }

        [CanBeNull]
        private string _valueFormatString;

        /// <summary>
        /// Gets or sets the value format string. This is used for driving the ValueText field.
        /// This can be null for default representation. Use {0} to represent the value.
        /// </summary>
        /// <value>The value format string.</value>
        [CanBeNull]
        public string ValueFormatString
        {
            get { return _valueFormatString; }
            set
            {
                if (value != _valueFormatString)
                {
                    _valueFormatString = value;

                    OnPropertyChanged(nameof(ValueFormatString));
                    OnPropertyChanged(nameof(ValueText));
                }
            }
        }
    }
}