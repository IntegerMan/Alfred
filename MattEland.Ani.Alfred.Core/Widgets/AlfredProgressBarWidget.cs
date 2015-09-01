// ---------------------------------------------------------
// AlfredProgressBarWidget.cs
// 
// Created on:      08/19/2015 at 9:31 PM
// Last Modified:   08/24/2015 at 11:22 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Globalization;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;

namespace MattEland.Ani.Alfred.Core.Widgets
{
    /// <summary>
    ///     A progress bar widget to be interpreted at the user interface layer
    /// </summary>
    public sealed class AlfredProgressBarWidget : AlfredTextWidget
    {
        private float _maximum = 100;
        private float _minimum;
        private float _value;

        [CanBeNull]
        private string _valueFormatString;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AlfredProgressBarWidget" /> class.
        /// </summary>
        /// <param name="parameters"> The parameters. </param>
        public AlfredProgressBarWidget([NotNull] WidgetCreationParameters parameters) : base(parameters)
        {
        }

        /// <summary>
        ///     Gets a list of properties provided by this item.
        /// </summary>
        /// <value>
        ///     The properties.
        /// </value>
        public override IEnumerable<IPropertyItem> Properties
        {
            get
            {
                // Return base properties
                foreach (var prop in base.Properties)
                {
                    yield return prop;
                }

                yield return new AlfredProperty("Value", ValueText);
                yield return new AlfredProperty("Minimum", Minimum);
                yield return new AlfredProperty("Maximum", Minimum);
            }
        }

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
        /// <value>
        ///     The maximum value.
        /// </value>
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
        /// <value>
        ///     The value.
        /// </value>
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
        /// <value>
        ///     A textual representation of the Value field.
        /// </value>
        public string ValueText
        {
            get
            {
                var formatString = _valueFormatString ?? "{0}";

                return string.Format(CultureInfo.CurrentCulture, formatString, Value);
            }
        }

        /// <summary>
        ///     Gets or sets the value format string. This is used for driving the
        ///     <see cref="ValueText"/> field. This can be null for default representation. Use {0} to
        ///     represent the value.
        /// </summary>
        /// <value>
        ///     The value format string.
        /// </value>
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

        /// <summary>
        ///     Gets the name of the broad categorization or type that this item is.
        /// </summary>
        /// <example>
        ///     Some examples of ItemTypeName values might be "Folder", "Application", "User", etc.
        /// </example>
        /// <value>The item type's name.</value>
        public override string ItemTypeName
        {
            get { return "Progress Bar"; }
        }
    }
}