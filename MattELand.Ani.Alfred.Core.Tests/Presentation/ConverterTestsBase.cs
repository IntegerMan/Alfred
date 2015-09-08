// ---------------------------------------------------------
// ConverterTestsBase.cs
// 
// Created on:      09/07/2015 at 9:54 PM
// Last Modified:   09/07/2015 at 10:34 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Windows.Data;

using JetBrains.Annotations;

using MattEland.Testing;

namespace MattEland.Ani.Alfred.Tests.Presentation
{
    /// <summary>
    ///     A base class for test classes that test <see cref="IValueConverter" /> objects.
    ///     
    ///     This generic class provides strongly typed management of the <see cref="Converter"/> in
    ///     question, ensuring a fresh instance is used for each test and preventing different tests
    ///     from impacting each other causing inconsistent results.
    /// </summary>
    /// <typeparam name="TConverter"> The type of the converter. </typeparam>
    [SuppressMessage("ReSharper", "ExceptionNotDocumentedOptional")]
    [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
    public abstract class ConverterTestsBase<TConverter> : UnitTestBase
        where TConverter : class, IValueConverter
    {

        /// <summary>
        ///     The converter.
        /// </summary>
        [CanBeNull]
        private TConverter _converter;

        /// <summary>
        ///     Gets or sets the converter. The property getter will never return
        ///     <see langword="null"/> and will use the <see cref="Activator" /> to create new
        ///     instances as needed if the backing field is <see langword="null"/>.
        /// </summary>
        /// <value>
        /// The converter.
        /// </value>
        [NotNull]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        [SuppressMessage("ReSharper", "ConvertIfStatementToNullCoalescingExpression")]
        protected TConverter Converter
        {
            get
            {
                // Lazy load the converter using the Activator to create a new instance as needed
                if (_converter == null)
                {
                    _converter = (TConverter)Activator.CreateInstance(typeof(TConverter));
                }

                return _converter;
            }
            set
            {
                // Setting to null is fine here since the getter lazy loads as needed
                _converter = value;
            }
        }

        /// <summary>
        ///     Gets the target type for conversion.
        /// </summary>
        /// <value>
        /// The target type.
        /// </value>
        protected static Type TargetType
        {
            get { return typeof(TConverter); }
        }

        /// <summary>
        ///     Gets the culture.
        /// </summary>
        /// <value>
        /// The culture.
        /// </value>
        protected static CultureInfo Culture
        {
            get { return CultureInfo.CurrentCulture; }
        }

        /// <summary>
        ///     Sets up the environment for each test.
        /// </summary>
        public override void SetUp()
        {
            base.SetUp();

            // Clear out the converter for next time around
            Converter = null;
        }

        /// <summary>
        ///     Converts the <paramref name="value"/> using the <see cref="Converter"/> and returns the
        ///     result.
        /// </summary>
        /// <param name="value"> The value. </param>
        /// <returns>
        ///     The result of the <see cref="IValueConverter.Convert"/> operation.
        /// </returns>
        [CanBeNull]
        protected object Convert([CanBeNull] object value)
        {
            return Converter.Convert(value, TargetType, null, Culture);
        }
    }
}