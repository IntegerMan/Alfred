// ---------------------------------------------------------
// VisibilityConverterTests.cs
// 
// Created on:      09/07/2015 at 10:05 AM
// Last Modified:   09/07/2015 at 10:10 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

using MattEland.Ani.Alfred.PresentationShared.Converters;
using MattEland.Testing;

using NUnit.Framework;

using Shouldly;

namespace MattEland.Ani.Alfred.Tests.Presentation
{

    /// <summary>
    ///     Provides tests around <see cref="VisibilityConverter" />
    /// </summary>
    [UnitTestProvider]
    [SuppressMessage("ReSharper", "ExceptionNotDocumentedOptional")]
    public sealed class VisibilityConverterTests : ConverterTestsBase<VisibilityConverter>
    {

        /// <summary>
        ///     Tests that passing <see langword="true" /> into the
        ///     <see cref="VisibilityConverter"/> results in
        ///     <see cref="System.Windows.Visibility.Visible" /> .
        /// </summary>
        [Test]
        public void TrueResultsInVisible()
        {
            //! Arrange
            Converter.Invert = false;

            //! Act
            var result = Convert(true);

            //! Assert
            result.ShouldBe(Visibility.Visible);
        }

        /// <summary>
        ///     Tests that passing <see langword="false" /> into the
        ///     <see cref="VisibilityConverter"/> results in
        ///     <see cref="System.Windows.Visibility.Collapsed" /> .
        /// </summary>
        [Test]
        public void FalseResultsInCollapsed()
        {
            //! Arrange
            Converter.Invert = false;

            //! Act
            var result = Convert(false);

            //! Assert
            result.ShouldBe(Visibility.Collapsed);
        }

        /// <summary>
        ///     Tests that passing unexpected inputs into the
        ///     <see cref="VisibilityConverter"/> results in
        ///     <see cref="System.Windows.Visibility.Collapsed" /> .
        /// </summary>
        [Test]
        public void ConverterDefaultsToCollapsed()
        {
            //! Arrange
            var converter = Converter;

            //! Act
            var result = Convert("MUFASA MUFASA MUFASA!");

            //! Assert
            result.ShouldBe(Visibility.Collapsed);
        }

        /// <summary>
        ///     Tests that passing a textual representation of <see langword="true"/> into the
        ///     <see cref="VisibilityConverter" /> results in
        ///     <see cref="System.Windows.Visibility.Visible" /> .
        /// </summary>
        [Test]
        public void TrueStringResultsInVisible()
        {

            //! Arrange
            Converter.Invert = false;

            //! Act
            var result = Convert("TRUE");

            //! Assert
            result.ShouldBe(Visibility.Visible);

        }

        /// <summary>
        ///     Tests that passing <see langword="true"/> into the
        ///     <see cref="VisibilityConverter" /> when <see cref="VisibilityConverter.Invert"/> is
        ///     <see langword="true"/> results in
        ///     <see cref="System.Windows.Visibility.Collapsed" /> .
        /// </summary>
        [Test]
        public void WhenInvertedTrueReturnsCollapsed()
        {

            //! Arrange
            Converter.Invert = true;

            //! Act
            var result = Convert(true);

            //! Assert
            result.ShouldBe(Visibility.Collapsed);

        }

        /// <summary>
        ///     Tests that passing <see langword="false"/> into the
        ///     <see cref="VisibilityConverter" /> when <see cref="VisibilityConverter.Invert"/> is
        ///     <see langword="true"/> results in
        ///     <see cref="System.Windows.Visibility.Visible" /> .
        /// </summary>
        [Test]
        public void WhenInvertedFalseReturnsVisible()
        {

            //! Arrange
            Converter.Invert = true;

            //! Act
            var result = Convert(false);

            //! Assert
            result.ShouldBe(Visibility.Visible);

        }

        /// <summary>
        ///     Tests that passing <see langword="null"/> into the
        ///     <see cref="VisibilityConverter" /> when <see cref="VisibilityConverter.Invert"/> is
        ///     <see langword="true"/> results in
        ///     <see cref="System.Windows.Visibility.Visible" /> .
        /// </summary>
        [Test]
        public void NullWhenInvertedReturnsVisible()
        {
            //! Arrange
            Converter.Invert = true;

            //! Act
            var result = Convert(null);

            //! Assert
            result.ShouldBe(Visibility.Visible);
        }

        /// <summary>
        ///     Tests that <see cref="VisibilityConverter.ConvertBack"/> throws a
        ///     <see cref="NotSupportedException"/>.
        /// </summary>
        [Test, ExpectedException(typeof(NotSupportedException))]
        public void ConvertBackThrowsNotSupported()
        {
            //! Arrange

            var converter = Converter;
            converter.Invert = true;

            //! Act / Assert (Exception expected)

            converter.ConvertBack(Visibility.Visible, TargetType, null, Culture);
        }
    }
}