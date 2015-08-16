// ---------------------------------------------------------
// TextSubstitutionHelper.cs
// 
// Created on:      08/12/2015 at 10:37 PM
// Last Modified:   08/16/2015 at 5:25 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Globalization;
using System.Text.RegularExpressions;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;
using MattEland.Common;

namespace MattEland.Ani.Alfred.Chat.Aiml.Normalize
{

    /// <summary>
    ///     A text transformation helper class that replaces found occurrences of strings
    ///     with their values found in the settings provider.
    /// </summary>
    internal static class TextSubstitutionHelper
    {
        private const string Marker = "zzMARKERzz";
        private const string WordBoundary = @"\b";

        /// <summary>
        ///     Substitutes all occurrences of the words in the settings provider found in input with the
        ///     replacement values stored in that provider.
        /// </summary>
        /// <param name="settings">The settings manager.</param>
        /// <param name="input">The input.</param>
        /// <returns>A modified version of the input string.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// </exception>
        [NotNull]
        internal static string Substitute([NotNull] SettingsManager settings,
                                          [CanBeNull] string input)
        {
            //- Validate
            if (input == null)
            {
                input = string.Empty;
            }
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            //- Grab our setting names
            var settingNames = settings.Keys;

            // Look for each setting settingName in the input string to replace it with our setting value
            foreach (var settingName in settingNames)
            {
                if (settingName == null)
                {
                    continue;
                }

                var settingValue = settings.GetValue(settingName);

                input = Substitute(input, settingName, settingValue);
            }

            // Remove our marker string from the string
            return input.Replace(Marker, string.Empty);
        }

        /// <summary>
        ///     Substitutes whole word instances of settingName in input with settingValue.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="settingName">Name of the setting.</param>
        /// <param name="settingValue">The setting value.</param>
        /// <returns>System.String.</returns>
        [NotNull]
        private static string Substitute([CanBeNull] string input,
                                         [CanBeNull] string settingName,
                                         [CanBeNull] string settingValue)
        {
            //- Sanity check
            if (input == null)
            {
                input = string.Empty;
            }
            if (settingName == null || settingValue == null)
            {
                return input;
            }

            // Surround the setting with our marker string
            var replacement =
                string.Format(CultureInfo.CurrentCulture, "{0}{1}{0}", Marker, settingValue.Trim())
                      .NonNull();

            // Check for bad things in the name and make them regex safe
            var sanitizedName =
                settingName.Replace(@"\", "")
                           .Replace(")", @"\)")
                           .Replace("(", @"\(")
                           .Replace(".", @"\.")
                           .Trim();

            // Replaces the variable settingName with the setting value
            var pattern =
                string.Format(CultureInfo.CurrentCulture, "{0}{1}{0}", WordBoundary, sanitizedName)
                      .NonNull();

            return Regex.Replace(input, pattern, replacement, RegexOptions.IgnoreCase);
        }
    }
}