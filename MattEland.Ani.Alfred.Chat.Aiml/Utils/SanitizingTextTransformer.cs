// ---------------------------------------------------------
// SanitizingTextTransformer.cs
// 
// Created on:      08/13/2015 at 10:31 PM
// Last Modified:   08/16/2015 at 1:02 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Text.RegularExpressions;

using JetBrains.Annotations;

using MattEland.Common;

namespace MattEland.Ani.Alfred.Chat.Aiml.Utils
{
    /// <summary>
    ///     A text transformer to remove illegal characters.
    /// </summary>
    public class SanitizingTextTransformer : TextTransformer
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="TextTransformer" /> class.
        /// </summary>
        /// <param name="chatEngine">The ChatEngine.</param>
        public SanitizingTextTransformer([NotNull] ChatEngine chatEngine)
            : base(chatEngine)
        {
            StripperRegexPattern = @"[^0-9a-zA-Z_]";
        }

        /// <summary>
        ///     Gets or sets the illegal character stripper regex pattern.
        /// </summary>
        /// <value>The stripper regex pattern.</value>
        [NotNull]
        public string StripperRegexPattern { get; }

        /// <summary>
        ///     Processes the input text and returns the processed value.
        /// </summary>
        /// <returns>The processed output</returns>
        protected override string ProcessChange()
        {
            // Removes illegal characters and replaces them with spaces
            var regex = new Regex(StripperRegexPattern, RegexOptions.IgnorePatternWhitespace);

            return regex.Replace(InputString.NonNull(), " ");
        }
    }
}