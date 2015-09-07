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
    internal sealed class SanitizingTextTransformer : TextTransformerBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="TextTransformerBase" /> class.
        /// </summary>
        /// <param name="chatEngine">The ChatEngine.</param>
        internal SanitizingTextTransformer([NotNull] ChatEngine chatEngine)
            : base(chatEngine)
        {
            SanitizerRegularExpression = @"[^0-9a-zA-Z_]";
        }

        /// <summary>
        ///     Gets the illegal character stripper regular expression pattern. Items matching this
        ///     pattern will be replaced with spaces.
        /// </summary>
        /// <value>
        ///     The stripper regular expression pattern.
        /// </value>
        [NotNull]
        private string SanitizerRegularExpression { get; }

        /// <summary>
        ///     Processes the input text and returns the processed value.
        /// </summary>
        /// <returns>The processed output</returns>
        protected override string ProcessChange()
        {
            // Removes illegal characters and replaces them with spaces
            var regex = new Regex(SanitizerRegularExpression, RegexOptions.IgnorePatternWhitespace);

            return regex.Replace(InputString.NonNull(), " ");
        }
    }
}