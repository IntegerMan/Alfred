// ---------------------------------------------------------
// SentenceTagHandler.cs
// 
// Created on:      08/19/2015 at 9:31 PM
// Last Modified:   08/24/2015 at 12:12 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;
using MattEland.Common;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    /// <summary>
    ///     An AIML tag handler for interpreting the AIML "sentence" tag. This causes the first letter of
    ///     every sentence to be capitalized while the rest run as lowercase.
    /// </summary>
    [HandlesAimlTag("sentence")]
    [UsedImplicitly]
    internal sealed class SentenceTagHandler : AimlTagHandler
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AimlTagHandler" /> class.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public SentenceTagHandler([NotNull] TagHandlerParameters parameters) : base(parameters)
        {
        }

        /// <summary>
        ///     Processes the input input and returns the processed value.
        /// </summary>
        /// <returns>The processed output</returns>
        protected override string ProcessChange()
        {
            if (Contents.HasText()) { return ProcessSentenceText(Contents); }

            // Evaluate everything else and stick that in our inner text
            var star = BuildStarTagHandler();
            Contents = star.Transform().NonNull();

            // Iterate over everything again if we now have values
            return Contents.HasText() ? ProcessChange() : string.Empty;
        }

        /// <summary>
        ///     Handles the sentence tag input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>The sentence text for the input value</returns>
        [NotNull]
        private string ProcessSentenceText([CanBeNull] string input)
        {
            var sentenceSplitters = ChatEngine.SentenceSplitters;

            //- Declare loop variables
            var stringBuilder = new StringBuilder();

            // Start out with a capitalized first letter
            var isNewSentence = true;

            //- Loop through all letters
            var letters = input.NonNull().Trim().ToCharArray();
            foreach (var t in letters)
            {
                var letterString = t.ToString();

                if (new Regex("[a-zA-Z]").IsMatch(letterString))
                {
                    stringBuilder.Append(isNewSentence
                                             ? letterString.ToUpper(Locale)
                                             : letterString.ToLower(Locale));
                }
                else
                {
                    // It's not a letter, we don't care about its casing
                    stringBuilder.Append(letterString);
                }

                // Evaluate for next loop through the iteration
                isNewSentence = sentenceSplitters.Contains(letterString);
            }

            return stringBuilder.ToString();
        }
    }
}