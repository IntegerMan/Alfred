// ---------------------------------------------------------
// LogLevelToBrushConverter.cs
// 
// Created on:      08/20/2015 at 8:14 PM
// Last Modified:   08/22/2015 at 12:09 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;

using MattEland.Common.Annotations;

using MattEland.Ani.Alfred.Core.Console;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI;
using Windows.UI.Xaml.Media;
using System.Diagnostics.Contracts;

namespace MattEland.Ani.Alfred.PresentationUniversal.Converters
{
    /// <summary>
    ///     An IValueConverter that converts LogLevels to brushes
    /// </summary>
    public sealed class LogLevelToBrushConverter : DependencyObject, IValueConverter
    {
        /// <summary>
        ///     The Verbose Brush dependency property
        /// </summary>
        [NotNull]
        public static readonly DependencyProperty VerboseBrushProperty =
            DependencyProperty.Register("VerboseBrush",
                                        typeof(Brush),
                                        typeof(LogLevelToBrushConverter),
                                        new PropertyMetadata(new SolidColorBrush(Colors.DimGray)));

        /// <summary>
        ///     The Error Brush dependency property
        /// </summary>
        [NotNull]
        public static readonly DependencyProperty ErrorBrushProperty =
            DependencyProperty.Register("ErrorBrush",
                                        typeof(Brush),
                                        typeof(LogLevelToBrushConverter),
                                        new PropertyMetadata(new SolidColorBrush(Colors.DarkRed)));

        /// <summary>
        ///     The Default Brush dependency property
        /// </summary>
        [NotNull]
        public static readonly DependencyProperty DefaultBrushProperty =
            DependencyProperty.Register("DefaultBrush",
                                        typeof(Brush),
                                        typeof(LogLevelToBrushConverter),
                                        new PropertyMetadata(new SolidColorBrush(Colors.DarkMagenta)));

        /// <summary>
        ///     The Warning Brush dependency property
        /// </summary>
        [NotNull]
        public static readonly DependencyProperty WarningBrushProperty =
            DependencyProperty.Register("WarningBrush",
                                        typeof(Brush),
                                        typeof(LogLevelToBrushConverter),
                                        new PropertyMetadata(new SolidColorBrush(Colors.LightCoral)));

        /// <summary>
        ///     The Info Brush dependency property
        /// </summary>
        [NotNull]
        public static readonly DependencyProperty InfoBrushProperty =
            DependencyProperty.Register("InfoBrush",
                                        typeof(Brush),
                                        typeof(LogLevelToBrushConverter),
                                        new PropertyMetadata(new SolidColorBrush(Colors.White)));

        /// <summary>
        ///     The UserInput Brush dependency property
        /// </summary>
        [NotNull]
        public static readonly DependencyProperty UserInputBrushProperty =
            DependencyProperty.Register("UserInputBrush",
                                        typeof(Brush),
                                        typeof(LogLevelToBrushConverter),
                                        new PropertyMetadata(new SolidColorBrush(Colors.Orchid)));

        /// <summary>
        ///     The ChatResponse Brush dependency property
        /// </summary>
        [NotNull]
        public static readonly DependencyProperty ChatResponseBrushProperty =
            DependencyProperty.Register("ChatResponseBrush",
                                        typeof(Brush),
                                        typeof(LogLevelToBrushConverter),
                                        new PropertyMetadata(new SolidColorBrush(Colors.SteelBlue)));

        /// <summary>
        ///     Gets or sets the verbose brush.
        /// </summary>
        /// <value>The verbose brush.</value>
        /// <exception cref="ArgumentNullException"
        ///            accessor="set">
        ///     <paramref name="value" /> is <see langword="null" />.
        /// </exception>
        [NotNull]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public Brush VerboseBrush
        {
            get { return (Brush)GetValue(VerboseBrushProperty); }
            set
            {
                Contract.Requires(value != null);

                SetValue(VerboseBrushProperty, value);
            }
        }

        /// <summary>
        ///     Gets or sets the Error brush.
        /// </summary>
        /// <exception cref="ArgumentNullException"
        ///            accessor="set">
        ///     <paramref name="value" /> is <see langword="null" />.
        /// </exception>
        [NotNull]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public Brush ErrorBrush
        {
            get { return (Brush)GetValue(ErrorBrushProperty); }
            set
            {
                Contract.Requires(value != null);

                SetValue(ErrorBrushProperty, value);
            }
        }

        /// <summary>
        ///     Gets or sets the Default brush.
        /// </summary>
        /// <value>The Default brush.</value>
        /// <exception cref="ArgumentNullException"
        ///            accessor="set">
        ///     <paramref name="value" /> is <see langword="null" />.
        /// </exception>
        [NotNull]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public Brush DefaultBrush
        {
            get { return (Brush)GetValue(DefaultBrushProperty); }
            set
            {
                Contract.Requires(value != null);

                SetValue(DefaultBrushProperty, value);
            }
        }

        /// <summary>
        ///     Gets or sets the Warning brush.
        /// </summary>
        /// <value>The Warning brush.</value>
        /// <exception cref="ArgumentNullException"
        ///            accessor="set">
        ///     <paramref name="value" /> is <see langword="null" />.
        /// </exception>
        [NotNull]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public Brush WarningBrush
        {
            get { return (Brush)GetValue(WarningBrushProperty); }
            set
            {
                Contract.Requires(value != null);

                SetValue(WarningBrushProperty, value);
            }
        }

        /// <summary>
        ///     Gets or sets the Info brush.
        /// </summary>
        /// <value>The Info brush.</value>
        /// <exception cref="ArgumentNullException"
        ///            accessor="set">
        ///     <paramref name="value" /> is <see langword="null" />.
        /// </exception>
        [NotNull]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public Brush InfoBrush
        {
            get { return (Brush)GetValue(InfoBrushProperty); }
            set
            {
                Contract.Requires(value != null);

                SetValue(InfoBrushProperty, value);
            }
        }

        /// <summary>
        ///     Gets or sets the UserInput brush.
        /// </summary>
        /// <value>The UserInput brush.</value>
        /// <exception cref="ArgumentNullException"
        ///            accessor="set">
        ///     <paramref name="value" /> is <see langword="null" />.
        /// </exception>
        [NotNull]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public Brush UserInputBrush
        {
            get { return (Brush)GetValue(UserInputBrushProperty); }
            set
            {
                Contract.Requires(value != null);

                SetValue(UserInputBrushProperty, value);
            }
        }

        /// <summary>
        ///     Gets or sets the ChatResponse brush.
        /// </summary>
        /// <value>The ChatResponse brush.</value>
        /// <exception cref="ArgumentNullException"
        ///            accessor="set">
        ///     <paramref name="value" /> is <see langword="null" />.
        /// </exception>
        [NotNull]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public Brush ChatResponseBrush
        {
            get { return (Brush)GetValue(ChatResponseBrushProperty); }
            set
            {
                Contract.Requires(value != null);

                SetValue(ChatResponseBrushProperty, value);
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
        [UsedImplicitly]
        [NotNull]
        public object Convert([CanBeNull] object value,
                              [CanBeNull] Type targetType,
                              [CanBeNull] object parameter,
                              [CanBeNull] string culture)
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
        ///     Converts a value.
        /// </summary>
        /// <returns>
        ///     A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <exception cref="NotSupportedException">This method is not supported on this converter.</exception>
        [UsedImplicitly]
        [CanBeNull]
        public object ConvertBack([CanBeNull] object value,
                                  [CanBeNull] Type targetType,
                                  [CanBeNull] object parameter,
                                  [CanBeNull] string culture)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        ///     Gets the brush for specific log level.
        /// </summary>
        /// <param name="level">The log level.</param>
        /// <returns>The brush.</returns>
        [NotNull]
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