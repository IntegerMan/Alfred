// ---------------------------------------------------------
// ApplySubstitutions.cs
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
    internal sealed class ApplySubstitutions : TextTransformer
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="TextTransformer" /> class.
        /// </summary>
        /// <param name="chatEngine">The ChatEngine.</param>
        internal ApplySubstitutions(ChatEngine chatEngine)
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
        ///     Substitutes all occurrances of the words in the dictionary found in input with the
        ///     values stored in that dictionary.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="input">The input.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// </exception>
        [NotNull]
        internal static string Substitute([NotNull] SettingsDictionary dictionary,
                                          [NotNull] string input)
        {
            //- Validate
            if (dictionary == null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            //- Grab our setting names
            var settingNames = dictionary.SettingNames;
            if (settingNames == null)
            {
                return input;
            }

            //- Define replacement strings
            const string Marker = "zzMARKERzz";
            const string WordBoundary = @"\b";

            // Look for each setting name in the input string to replace it with our setting value
            foreach (var name in settingNames)
            {
                //- Sanity check
                if (name == null)
                {
                    continue;
                }

                //- Grab the setting value
                var setting = dictionary.grabSetting(name);
                if (setting == null)
                {
                    continue;
                }

                // Surround the setting with our marker string
                var replacement = string.Format("{0}{1}{0}", Marker, setting.Trim());

                // Replaces the variable name with the setting value
                var sanitizedName =
                    name.Replace(@"\", "").Replace(")", @"\)").Replace("(", @"\(").Replace(".", @"\.").Trim();

                var pattern = $"{WordBoundary}{sanitizedName}{WordBoundary}";
                input = Regex.Replace(input, pattern, replacement, RegexOptions.IgnoreCase);
            }

            // Remove our marker string from the string
            return input.Replace(Marker, string.Empty);
        }
    }
}