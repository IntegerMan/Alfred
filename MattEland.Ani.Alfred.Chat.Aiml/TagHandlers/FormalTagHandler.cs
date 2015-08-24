// ---------------------------------------------------------
// FormalTagHandler.cs
// 
// Created on:      08/19/2015 at 9:31 PM
// Last Modified:   08/24/2015 at 12:04 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Text;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;
using MattEland.Common;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    /// <summary>
    ///     A tag handler that handles the "formal" AIML tag by taking the inner text and
    ///     Returning It In Formal Case.
    /// 
    ///     See "http://www.alicebot.org/TR/2005/WD-aiml/#section-formal" for more on the formal tag
    /// </summary>
    [HandlesAimlTag("formal")]
    public class FormalTagHandler : AimlTagHandler
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AimlTagHandler" /> class.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        public FormalTagHandler([NotNull] TagHandlerParameters parameters) : base(parameters) { }

        /// <summary>
        ///     Processes the input text and returns the processed value.
        /// </summary>
        /// <returns>The processed output</returns>
        protected override string ProcessChange()
        {
            //- Ensure we're looking at the correct tag
            if (!NodeName.Matches("formal") || Contents.IsEmpty()) { return string.Empty; }

            // Start lowercase
            var stringBuilder = new StringBuilder();
            var innerText = Contents.ToLower(Locale);

            // Cycle through each word in the string
            foreach (var word in innerText.Split())
            {
                //- Sanity check
                if (word == null) { continue; }

                //? ALF-19: It'd be cool to optionally NOT formal case "a" "it" "is" etc.

                // Grab the first letter
                var newWord = word.Substring(0, 1).ToUpper(Locale);

                // Tack on the rest of the word
                if (word.Length > 1) { newWord += word.Substring(1); }

                // Build out the new sentence with word breaks
                stringBuilder.Append(newWord);
                stringBuilder.Append(" ");
            }

            // Return Our New Sentence In Formal Case
            //- The trim will kill the trailing space the last appendFormat added
            return stringBuilder.ToString().Trim();
        }
    }
}