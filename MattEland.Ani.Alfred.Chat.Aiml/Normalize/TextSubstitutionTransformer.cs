// ---------------------------------------------------------
// TextSubstitutionTransformer.cs
// 
// Created on:      08/12/2015 at 10:37 PM
// Last Modified:   08/13/2015 at 11:54 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Text.RegularExpressions;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;

namespace MattEland.Ani.Alfred.Chat.Aiml.Normalize
{

    /// <summary>
    ///     A TextTransformer that replaces found occurrences of strings
    ///     with their values found in the ChatEngine's settings dictionary.
    /// </summary>
    internal sealed class TextSubstitutionTransformer : TextTransformer
    {

        private const string Marker = "zzMARKERzz";
        private const string WordBoundary = @"\b";

        /// <summary>
        /// Initializes a new instance of the <see cref="TextTransformer" /> class.
        /// </summary>
        /// <param name="chatEngine">The ChatEngine.</param>
        internal TextSubstitutionTransformer([NotNull] ChatEngine chatEngine)
            : base(chatEngine)
        {
        }

        /// <summary>
        ///     Processes the input text and returns the processed value.
        /// </summary>
        /// <returns>The processed output</returns>
        protected override string ProcessChange()
        {
            /* Replace simple substitutions found in the input string with values
            in the substitutions dictionary. This is useful for common words
            that are equivalent in meaning. */

            return Substitute(ChatEngine.Substitutions, InputString);
        }

        /// <summary>
        /// Substitutes all occurrences of the words in the dictionary found in input with the
        /// values stored in that dictionary.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="input">The input.</param>
        /// <returns>A modified version of the input string.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// </exception>
        [NotNull]
        internal static string Substitute([NotNull] SettingsDictionary dictionary,
                                          [CanBeNull] string input)
        {
            //- Validate
            if (input == null)
            {
                input = string.Empty;
            }
            if (dictionary == null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            //- Grab our setting names
            var settingNames = dictionary.Keys;
            if (settingNames == null)
            {
                return input;
            }

            // Look for each setting settingName in the input string to replace it with our setting value
            foreach (var settingName in settingNames)
            {
                var settingValue = dictionary.GetValue(settingName);

                input = Substitute(input, settingName, settingValue);
            }

            // Remove our marker string from the string
            return input.Replace(Marker, string.Empty);
        }

        /// <summary>
        /// Substitutes whole word instances of settingName in input with settingValue.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="settingName">Name of the setting.</param>
        /// <param name="settingValue">The setting value.</param>
        /// <returns>System.String.</returns>
        [NotNull]
        private static string Substitute([CanBeNull] string input, [CanBeNull] string settingName,
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
            var replacement = string.Format("{0}{1}{0}", Marker, settingValue.Trim());

            // Check for bad things in the name and make them regex safe
            var sanitizedName =
                settingName.Replace(@"\", "").Replace(")", @"\)").Replace("(", @"\(").Replace(".", @"\.").Trim();

            // Replaces the variable settingName with the setting value
            var pattern = $"{WordBoundary}{sanitizedName}{WordBoundary}";

            return Regex.Replace(input, pattern, replacement, RegexOptions.IgnoreCase);
        }
    }
}