// ---------------------------------------------------------
// LogBrushTests.cs
// 
// Created on:      09/07/2015 at 9:43 PM
// Last Modified:   09/07/2015 at 9:43 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Media;

using MattEland.Ani.Alfred.Core.Console;

using MattEland.Common.Testing;

using NUnit.Framework;

using Shouldly;
using MattEland.Ani.Alfred.PresentationAvalon.Converters;

namespace MattEland.Ani.Alfred.Tests.Presentation
{
    /// <summary>
    ///     Contains tests related to <see cref="LogLevelToBrushConverter" />
    /// </summary>
    [UnitTestProvider]
    [SuppressMessage("ReSharper", "ExceptionNotDocumentedOptional")]
    [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
    public sealed class LogLevelToBrushConverterTests : ConverterTestsBase<LogLevelToBrushConverter>
    {
        /// <summary>
        ///     Sets up the environment for each test.
        /// </summary>
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            /* Set up the converter with an odd assortment of mismatched colors. 
               The important thing is that the colors are all different and are all different than their defaults. */
            var converter = new LogLevelToBrushConverter
            {
                ChatResponseBrush = Brushes.AliceBlue,
                DefaultBrush = Brushes.Orange,
                ErrorBrush = Brushes.BlueViolet,
                InfoBrush = Brushes.DarkSeaGreen,
                UserInputBrush = Brushes.CornflowerBlue,
                VerboseBrush = Brushes.Crimson,
                WarningBrush = Brushes.DarkKhaki
            };

            Converter = converter;
        }

        /// <summary>
        ///     <see cref="LogLevel.Warning"/> log entries result in the
        ///     <see cref="LogLevelToBrushConverter.WarningBrush"/>.
        /// </summary>
        [Test]
        public void WarningLogEntryResultsInCorrectBrush()
        {
            //! Arrange
            const LogLevel Level = LogLevel.Warning;

            //! Act
            var result = Convert(Level);

            //! Assert
            result.ShouldBe(Converter.WarningBrush);
        }

        /// <summary>
        ///     <see cref="LogLevel.Verbose"/> log entries result in the
        ///     <see cref="LogLevelToBrushConverter.VerboseBrush"/>.
        /// </summary>
        [Test]
        public void VerboseLogEntryResultsInCorrectBrush()
        {
            //! Arrange
            const LogLevel Level = LogLevel.Verbose;

            //! Act
            var result = Convert(Level);

            //! Assert
            result.ShouldBe(Converter.VerboseBrush);
        }

        /// <summary>
        ///     <see cref="LogLevel.Error"/> log entries result in the
        ///     <see cref="LogLevelToBrushConverter.ErrorBrush"/>.
        /// </summary>
        [Test]
        public void ErrorLogEntryResultsInCorrectBrush()
        {
            //! Arrange
            const LogLevel Level = LogLevel.Error;

            //! Act
            var result = Convert(Level);

            //! Assert
            result.ShouldBe(Converter.ErrorBrush);
        }

        /// <summary>
        ///     <see cref="LogLevel.Error"/> log entries result in the
        ///     <see cref="LogLevelToBrushConverter.ErrorBrush"/>.
        /// </summary>
        [Test]
        public void InfoLogEntryResultsInCorrectBrush()
        {
            //! Arrange
            const LogLevel Level = LogLevel.Info;

            //! Act
            var result = Convert(Level);

            //! Assert
            result.ShouldBe(Converter.InfoBrush);
        }

        /// <summary>
        ///     <see cref="LogLevel.UserInput"/> log entries result in the
        ///     <see cref="LogLevelToBrushConverter.UserInputBrush"/>.
        /// </summary>
        [Test]
        public void UserInputLogEntryResultsInCorrectBrush()
        {
            //! Arrange
            const LogLevel Level = LogLevel.UserInput;

            //! Act
            var result = Convert(Level);

            //! Assert
            result.ShouldBe(Converter.UserInputBrush);
        }

        /// <summary>
        ///     <see cref="LogLevel.ChatNotification"/> log entries result in the
        ///     <see cref="LogLevelToBrushConverter.ChatResponseBrush"/>.
        /// </summary>
        [Test]
        public void ChatNotificationLogEntryResultsInCorrectBrush()
        {
            //! Arrange
            const LogLevel Level = LogLevel.ChatNotification;

            //! Act
            var result = Convert(Level);

            //! Assert
            result.ShouldBe(Converter.ChatResponseBrush);
        }

        /// <summary>
        ///     <see cref="LogLevel.ChatResponse"/> log entries result in the
        ///     <see cref="LogLevelToBrushConverter.ChatResponseBrush"/>.
        /// </summary>
        [Test]
        public void ChatResponseLogEntryResultsInCorrectBrush()
        {
            //! Arrange
            const LogLevel Level = LogLevel.ChatResponse;

            //! Act
            var result = Convert(Level);

            //! Assert
            result.ShouldBe(Converter.ChatResponseBrush);
        }

        /// <summary>
        ///     Unexpected values result in the
        ///     <see cref="LogLevelToBrushConverter.DefaultBrush"/>.
        /// </summary>
        [Test]
        public void NonsenseInputResultsInCorrectBrush()
        {
            //! Arrange
            const string Value = "Forty Two";

            //! Act
            var result = Convert(Value);

            //! Assert
            result.ShouldBe(Converter.DefaultBrush);
        }

        /// <summary>
        ///     <see langword="null"/> values result in the
        ///     <see cref="LogLevelToBrushConverter.DefaultBrush"/>.
        /// </summary>
        [Test]
        public void NullInputResultsInCorrectBrush()
        {
            //! Arrange
            const object Value = null;

            //! Act
            var result = Convert(Value);

            //! Assert
            result.ShouldBe(Converter.DefaultBrush);
        }

        /// <summary>
        ///     <see cref="LogLevelToBrushConverter.ConvertBack"/> should throw a
        ///     <see cref="NotSupportedException"/>.
        /// </summary>
        [Test, ExpectedException(typeof(NotSupportedException))]
        public void ConvertBackIsNotSupported()
        {
            //! Arrange
            var value = Converter.DefaultBrush;

            //! Act / Assert (Expected Exception)
            Converter.ConvertBack(value, typeof(LogLevel), null, Culture);
        }
    }
}