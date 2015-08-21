// ---------------------------------------------------------
// LogLevelToBrushConverter.cs
// 
// Created on:      08/08/2015 at 5:55 PM
// Last Modified:   08/08/2015 at 5:56 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using JetBrains.Annotations;
using MattEland.Ani.Alfred.Core.Console;

namespace MattEland.Ani.Alfred.PresentationShared.Converters
{
    /// <summary>
    ///     An IValueConverter that converts LogLevels to brushes
    /// </summary>
    public sealed class LogLevelToBrushConverter : DependencyObject, IValueConverter
    {

        /// <summary>
        ///     Gets or sets the verbose brush.
        /// </summary>
        /// <value>The verbose brush.</value>
        [NotNull]
        public Brush VerboseBrush
        {
            get { return (Brush)GetValue(VerboseBrushProperty); }
            set { SetValue(VerboseBrushProperty, value); }
        }

        /// <summary>
        /// The Verbose Brush dependency property
        /// </summary>
        [NotNull]
        public static readonly DependencyProperty VerboseBrushProperty =
            DependencyProperty.Register("VerboseBrush", typeof(Brush), typeof(LogLevelToBrushConverter), new PropertyMetadata(Brushes.DimGray));


        /// <summary>
        ///     Gets or sets the Error brush.
        /// </summary>
        /// <value>The Error brush.</value>
        [NotNull]
        public Brush ErrorBrush
        {
            get { return (Brush)GetValue(ErrorBrushProperty); }
            set { SetValue(ErrorBrushProperty, value); }
        }

        /// <summary>
        /// The Error Brush dependency property
        /// </summary>
        [NotNull]
        public static readonly DependencyProperty ErrorBrushProperty =
            DependencyProperty.Register("ErrorBrush", typeof(Brush), typeof(LogLevelToBrushConverter), new PropertyMetadata(Brushes.DarkRed));

        /// <summary>
        ///     Gets or sets the Default brush.
        /// </summary>
        /// <value>The Default brush.</value>
        [NotNull]
        public Brush DefaultBrush
        {
            get { return (Brush)GetValue(DefaultBrushProperty); }
            set { SetValue(DefaultBrushProperty, value); }
        }

        /// <summary>
        /// The Default Brush dependency property
        /// </summary>
        [NotNull]
        public static readonly DependencyProperty DefaultBrushProperty =
            DependencyProperty.Register("DefaultBrush", typeof(Brush), typeof(LogLevelToBrushConverter), new PropertyMetadata(Brushes.DarkMagenta));

        /// <summary>
        ///     Gets or sets the Warning brush.
        /// </summary>
        /// <value>The Warning brush.</value>
        [NotNull]
        public Brush WarningBrush
        {
            get { return (Brush)GetValue(WarningBrushProperty); }
            set { SetValue(WarningBrushProperty, value); }
        }

        /// <summary>
        /// The Warning Brush dependency property
        /// </summary>
        [NotNull]
        public static readonly DependencyProperty WarningBrushProperty =
            DependencyProperty.Register("WarningBrush", typeof(Brush), typeof(LogLevelToBrushConverter), new PropertyMetadata(Brushes.LightCoral));



        /// <summary>
        ///     Gets or sets the Info brush.
        /// </summary>
        /// <value>The Info brush.</value>
        [NotNull]
        public Brush InfoBrush
        {
            get { return (Brush)GetValue(InfoBrushProperty); }
            set { SetValue(InfoBrushProperty, value); }
        }

        /// <summary>
        /// The Info Brush dependency property
        /// </summary>
        [NotNull]
        public static readonly DependencyProperty InfoBrushProperty =
            DependencyProperty.Register("InfoBrush", typeof(Brush), typeof(LogLevelToBrushConverter), new PropertyMetadata(Brushes.White));

        /// <summary>
        ///     Gets or sets the UserInput brush.
        /// </summary>
        /// <value>The UserInput brush.</value>
        [NotNull]
        public Brush UserInputBrush
        {
            get { return (Brush)GetValue(UserInputBrushProperty); }
            set { SetValue(UserInputBrushProperty, value); }
        }

        /// <summary>
        /// The UserInput Brush dependency property
        /// </summary>
        [NotNull]
        public static readonly DependencyProperty UserInputBrushProperty =
            DependencyProperty.Register("UserInputBrush", typeof(Brush), typeof(LogLevelToBrushConverter), new PropertyMetadata(Brushes.PaleGoldenrod));

        /// <summary>
        ///     Gets or sets the ChatResponse brush.
        /// </summary>
        /// <value>The ChatResponse brush.</value>
        [NotNull]
        public Brush ChatResponseBrush
        {
            get { return (Brush)GetValue(ChatResponseBrushProperty); }
            set { SetValue(ChatResponseBrushProperty, value); }
        }

        /// <summary>
        /// The ChatResponse Brush dependency property
        /// </summary>
        [NotNull]
        public static readonly DependencyProperty ChatResponseBrushProperty =
            DependencyProperty.Register("ChatResponseBrush", typeof(Brush), typeof(LogLevelToBrushConverter), new PropertyMetadata(Brushes.SteelBlue));


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
        /// Converts a value. 
        /// </summary>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <param name="value">The value that is produced by the binding target.</param><param name="targetType">The type to convert to.</param><param name="parameter">The converter parameter to use.</param><param name="culture">The culture to use in the converter.</param>
        /// <exception cref="NotSupportedException">This method is not supported on this converter.</exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
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

    }
}