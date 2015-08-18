// ---------------------------------------------------------
// TextSubstitutionHelper.cs
// 
// Created on:      08/12/2015 at 10:37 PM
// Last Modified:   08/18/2015 at 4:57 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

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
    public static class TextSubstitutionHelper
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
        [NotNull]
        public static string Substitute([CanBeNull] SettingsManager settings,
                                        [CanBeNull] string input)
        {
            //- Validate
            input = input.NonNull();
            if (settings == null)
            {
                return input;
            }

            //- Grab our setting names
            var settingNames = settings.Keys;

            // Look for each setting settingName in the input string to replace it with our setting value
            foreach (var settingName in settingNames)
            {
                var settingValue = settings.GetValue(settingName);

                input = Substitute(input, settingName, settingValue);
            }

            return input;
        }

        /// <summary>
        ///     Substitutes whole word instances of settingName in input with settingValue.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="settingName">Name of the setting.</param>
        /// <param name="settingValue">The setting value.</param>
        /// <returns>A string with words substituted</returns>
        [NotNull]
        public static string Substitute([CanBeNull] string input,
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

            var locale = CultureInfo.CurrentCulture;

            // Surround the setting with our marker string
            var replacement =
                string.Format(locale, "{0}{1}{0}", Marker, settingValue.Trim())
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
                string.Format(locale, "{0}{1}{0}", WordBoundary, sanitizedName)
                      .NonNull();

            var substitute = Regex.Replace(input, pattern, replacement, RegexOptions.IgnoreCase);

            // Remove the marker string from the string
            return substitute.Replace(Marker, string.Empty);
        }
    }
}