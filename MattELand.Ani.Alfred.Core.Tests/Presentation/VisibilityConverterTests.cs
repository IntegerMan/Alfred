// ---------------------------------------------------------
// VisibilityConverterTests.cs
// 
// Created on:      09/07/2015 at 10:05 AM
// Last Modified:   09/07/2015 at 10:10 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Globalization;
using System.Windows;

using JetBrains.Annotations;

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
    public sealed class VisibilityConverterTests : UnitTestBase
    {
        /// <summary>
        ///     Sets up the environment for each test.
        /// </summary>
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            // Ensure we have a new converter to deal with every test
            _converter = null;
        }

        /// <summary>
        ///     The converter.
        /// </summary>
        [CanBeNull]
        private VisibilityConverter _converter;

        /// <summary>
        ///     Gets the converter.
        /// </summary>
        /// <value>
        /// The converter.
        /// </value>
        [NotNull]
        private VisibilityConverter Converter
        {
            get { return _converter ?? (_converter = new VisibilityConverter()); }
        }

        /// <summary>
        ///     Gets the target type for conversion.
        /// </summary>
        /// <value>
        ///     The target type.
        /// </value>
        private static Type TargetType
        {
            get { return typeof(Visibility); }
        }

        /// <summary>
        ///     Gets the culture.
        /// </summary>
        /// <value>
        ///     The culture.
        /// </value>
        private static CultureInfo Culture
        {
            get { return CultureInfo.CurrentCulture; }
        }

        /// <summary>
        ///     Tests that passing <see langword="true" /> into the
        ///     <see cref="VisibilityConverter"/> results in
        ///     <see cref="System.Windows.Visibility.Visible" /> .
        /// </summary>
        [Test]
        public void TrueResultsInVisible()
        {
            //! Arrange

            var converter = Converter;

            //! Act

            var result = converter.Convert(true, TargetType, null, Culture);

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

            var converter = Converter;

            //! Act

            var result = converter.Convert(false, TargetType, null, Culture);

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

            var result = converter.Convert("MUFASA MUFASA MUFASA!", TargetType, null, Culture);

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

            var converter = Converter;

            //! Act

            var result = converter.Convert("TRUE", TargetType, null, Culture);

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

            var converter = Converter;
            converter.Invert = true;

            //! Act

            var result = converter.Convert(true, TargetType, null, Culture);

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

            var converter = Converter;
            converter.Invert = true;

            //! Act

            var result = converter.Convert(false, TargetType, null, Culture);

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

            var converter = Converter;
            converter.Invert = true;

            //! Act

            var result = converter.Convert(null, TargetType, null, Culture);

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

            var result = converter.ConvertBack(Visibility.Visible, TargetType, null, Culture);
        }
    }
}