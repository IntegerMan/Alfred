// ---------------------------------------------------------
// ExceptionExtensions.cs
// 
// Created on:      08/20/2015 at 10:28 PM
// Last Modified:   08/20/2015 at 10:50 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
        /// <param name="culture">The user interface culture. Defaults to CurrentCulture.</param>
        /// <returns>An exception details message suitable for diagnostic purposes. This includes line breaks.</returns>
        public static string BuildExceptionDetailsMessage([CanBeNull] this Exception ex,
                                                          [CanBeNull] CultureInfo culture = null)
        {
            if (ex == null)
            {
                return string.Empty;
            }

            culture = culture ?? CultureInfo.CurrentCulture;

            var sb = new StringBuilder();
            while (ex != null)
            {
                sb.AppendLine(string.Format(culture,
                                            "{0}: {1}",
                                            ex.GetType().Name,
                                            ex.Message));
                ex = ex.InnerException;
            }

            return sb.ToString();
        }
    }
}