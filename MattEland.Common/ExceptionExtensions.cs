// ---------------------------------------------------------
// ExceptionExtensions.cs
// 
// Created on:      08/20/2015 at 10:28 PM
// Last Modified:   08/21/2015 at 12:44 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

using JetBrains.Annotations;

namespace MattEland.Common
{
    /// <summary>
    ///     Contains extension methods related to <see cref="Exception" /> classes.
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        ///     Builds an exception details message out of the exception, including its nested inner
        ///     exceptions.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <param name="stringBuilder">The string builder.</param>
        /// <param name="useNewLine">Whether or not to use new lines when building the message.</param>
        /// <param name="culture">The user interface culture. Defaults to CurrentCulture.</param>
        /// <returns>An exception details message suitable for diagnostic purposes.</returns>
        [NotNull]
        public static string BuildDetailsMessage([CanBeNull] this Exception ex,
                                                          [CanBeNull] StringBuilder stringBuilder =
                                                              null,
                                                          bool useNewLine = true,
                                                          [CanBeNull] CultureInfo culture = null)
        {
            // Make sure we don't get nulls
            if (ex == null)
            {
                return string.Empty;
            }

            // Ensure objects exist
            culture = culture ?? CultureInfo.CurrentCulture;
            stringBuilder = stringBuilder ?? new StringBuilder();

            while (ex != null)
            {
                // Log the message keeping formatting in mind
                const string Format = "{0}: {1} ";
                var message = string.Format(culture, Format, ex.GetType().Name, ex.Message);
                stringBuilder.AppendConditional(message, useNewLine);

                /* - This can be lengthy and resulted in out of memory exceptions during testing
                var rex = ex as ReflectionTypeLoadException;
                if (rex != null)
                {
                    BuildExceptionAdditionalDetails(rex, stringBuilder, useNewLine, culture);
                }
                */

                ex = ex.InnerException;
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        ///     Builds additional details for a ReflectionTypeLoadException.
        /// </summary>
        /// <param name="rex">The rex.</param>
        /// <param name="stringBuilder">The string builder.</param>
        /// <param name="useNewLine">if set to <c>true</c> new lines will be used in the message.</param>
        /// <param name="culture">The culture.</param>
        private static void BuildAdditionalDetails(
            [NotNull] ReflectionTypeLoadException rex,
            [NotNull] StringBuilder stringBuilder,
            bool useNewLine,
            [NotNull] CultureInfo culture)
        {

            if (rex.LoaderExceptions == null)
            {
                return;
            }

            var loaderExceptions = rex.LoaderExceptions.ToList();
            if (loaderExceptions.Any())
            {
                stringBuilder.AppendConditional(Resources.ExceptionExtensionsLoadingLoaderExceptions, useNewLine);

                // Loop through each loader exception and recursively call the core exception details message on it
                foreach (var loaderException in loaderExceptions)
                {
                    var detailsMessage =
                        loaderException.BuildDetailsMessage(stringBuilder,
                                                                     useNewLine,
                                                                     culture);

                    stringBuilder.AppendConditional(detailsMessage, useNewLine);
                }
            }
        }
    }
}