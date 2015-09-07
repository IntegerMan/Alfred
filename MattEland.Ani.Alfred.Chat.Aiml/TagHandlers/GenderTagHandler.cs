// ---------------------------------------------------------
// GenderTagHandler.cs
// 
// Created on:      08/12/2015 at 10:45 PM
// Last Modified:   08/14/2015 at 6:14 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;
using MattEland.Common;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    /// <summary>
    ///     A tag handler for the AIML "gender" tag that substitutes gender pronouns for their equivalent
    ///     value in the chat engine's GenderSubstitutions
    /// 
    ///     See "http://www.alicebot.org/TR/2005/WD-aiml/#section-gender" for more on the gender tag.
    /// </summary>
    [HandlesAimlTag("gender")]
    [UsedImplicitly]
    internal sealed class GenderTagHandler : AimlTagHandler
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AimlTagHandler" /> class.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        public GenderTagHandler([NotNull] TagHandlerParameters parameters)
            : base(parameters)
        {
        }

        /// <summary>
        ///     Processes the input text and returns the processed value.
        /// </summary>
        /// <returns>The processed output</returns>
        protected override string ProcessChange()
        {
            /* Note: this tag handler uses recursion to recursively iterate through 
            InnerText and find text that can be evaluated using substitution */

            //- Ensure this is the correct tag
            if (NodeName.Matches("gender"))
            {
                return string.Empty;
            }

            // If we have text inside the gender tag, just transform and return that
            if (Contents.HasText())
            {
                /* Substitute occurrences of gender words with other values. This typically 
                   inverts the gender on gender pronouns ("he" / "she" / etc.) */

                var substitutions = Librarian.GenderSubstitutions;
                return TextSubstitutionHelper.Substitute(substitutions, Contents);
            }

            // Apply an AIML "star" tag and store the results as our inner text
            var star = BuildStarTagHandler();
            Contents = star.Transform().NonNull();

            //! If we still have text, we'll need to use recursion to evaluate our new value
            return Contents.HasText() ? ProcessChange() : string.Empty;
        }

    }
}