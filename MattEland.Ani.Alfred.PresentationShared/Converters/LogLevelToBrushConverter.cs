// ---------------------------------------------------------
// LogLevelToBrushConverter.cs
// 
// Created on:      08/08/2015 at 5:55 PM
// Last Modified:   08/08/2015 at 5:56 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using JetBrains.Annotations;
using MattEland.Ani.Alfred.Core.Console;

namespace MattEland.Ani.Alfred.PresentationShared.Converters
{
    /// <summary>
    ///     An IValueConverter that converts LogLevels to brushes
    /// </summary>
    public sealed class LogLevelToBrushConverter : IValueConverter
    {
        [NotNull]
        private Brush _defaultBrush;

        [NotNull]
        private Brush _errorBrush;

        [NotNull]
        private Brush _infoBrush;

        [NotNull]
        private Brush _verboseBrush;

        [NotNull]
        private Brush _warningBrush;

        [NotNull]
        private Brush _userInputBrush;

        [NotNull]
        private Brush _chatResponseBrush;

        /// <summary>
        ///     Initializes a new instance of the <see cref="LogLevelToBrushConverter" /> class.
        /// </summary>
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute",
            Justification = "Brushes are guaranteed to return non-null values")]
        public LogLevelToBrushConverter()
        {
            _errorBrush = Brushes.DarkRed;
            _infoBrush = Brushes.White;
            _warningBrush = Brushes.LightCoral;
            _verboseBrush = Brushes.DimGray;
            _defaultBrush = Brushes.DarkOrchid;
            _userInputBrush = Brushes.PaleGoldenrod;
            _chatResponseBrush = Brushes.SteelBlue;
        }

        /// <summary>
        ///     Gets or sets the verbose brush.
        /// </summary>
        /// <value>The verbose brush.</value>
        [NotNull]
        public Brush VerboseBrush
        {
            get { return _verboseBrush; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }
                _verboseBrush = value;
            }
        }

        /// <summary>
        ///     Gets or sets the error brush.
        /// </summary>
        /// <value>The error brush.</value>
        [NotNull]
        public Brush ErrorBrush
        {
            get { return _errorBrush; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }
                _errorBrush = value;
            }
        }

        /// <summary>
        ///     Gets or sets the default brush. This brush is used when no match is determined.
        /// </summary>
        /// <value>The default brush.</value>
        [NotNull]
        public Brush DefaultBrush
        {
            get { return _defaultBrush; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }
                _defaultBrush = value;
            }
        }

        /// <summary>
        ///     Gets or sets the warning brush.
        /// </summary>
        /// <value>The warning brush.</value>
        [NotNull]
        public Brush WarningBrush
        {
            get { return _warningBrush; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }
                _warningBrush = value;
            }
        }

        /// <summary>
        ///     Gets or sets the info brush.
        /// </summary>
        /// <value>The info brush.</value>
        [NotNull]
        public Brush InfoBrush
        {
            get { return _infoBrush; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }
                _infoBrush = value;
            }
        }

        /// <summary>
        ///     Gets or sets the UserInput brush.
        /// </summary>
        /// <value>The UserInput brush.</value>
        [NotNull]
        public Brush UserInputBrush
        {
            get { return _userInputBrush; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }
                _userInputBrush = value;
            }
        }

        /// <summary>
        ///     Gets or sets the ChatResponse brush.
        /// </summary>
        /// <value>The ChatResponse brush.</value>
        [NotNull]
        public Brush ChatResponseBrush
        {
            get { return _chatResponseBrush; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }
                _chatResponseBrush = value;
            }
        }

        /// <summary>
        ///     Converts a value to a brush where value is intended to be a LogLevel.
        /// </summary>
        /// <returns>
        ///     A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Null won't work ever
            if (value == null)
            {
                return DefaultBrush;
            }

            // Cast as a LogLevel
            LogLevel level;
            try
            {
                level = (LogLevel)value;
            }
            catch (InvalidCastException)
            {
                return DefaultBrush;
            }

            return GetBrushForLevel(level);
        }

        /// <summary>
        /// Gets the brush for specific log level.
        /// </summary>
        /// <param name="level">The log level.</param>
        /// <returns>The brush.</returns>
        private Brush GetBrushForLevel(LogLevel level)
        {

            // Grab the appropriate brush
            switch (level)
            {
                case LogLevel.Error:
                    return ErrorBrush;

                case LogLevel.Info:
                    return InfoBrush;

                case LogLevel.Warning:
                    return WarningBrush;

                case LogLevel.Verbose:
                    return VerboseBrush;

                case LogLevel.UserInput:
                    return UserInputBrush;

                case LogLevel.ChatResponse:
                case LogLevel.ChatNotification:
                    return ChatResponseBrush;

                default:
                    return DefaultBrush;
            }
        }

        /// <summary>
        ///     Converts a value.
        /// </summary>
        /// <returns>
        ///     A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}