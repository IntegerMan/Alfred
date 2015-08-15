// ---------------------------------------------------------
// GenderTagHandler.cs
// 
// Created on:      08/12/2015 at 10:45 PM
// Last Modified:   08/14/2015 at 6:14 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml.Normalize;
using MattEland.Ani.Alfred.Chat.Aiml.Utils;
using MattEland.Common;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    /// <summary>
    ///     A tag handler for the AIML "gender" tag that subsitutes gender pronouns for their equivalent
    ///     value in the chat engine's GenderSubstitutions
    /// 
    ///     See "http://www.alicebot.org/TR/2005/WD-aiml/#section-gender" for more on the gender tag.
    /// </summary>
    [HandlesAimlTag("gender")]
    public class GenderTagHandler : AimlTagHandler
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
            if (TemplateNode.Name.ToLower() != "gender")
            {
                return string.Empty;
            }

            // If we have text inside the gender tag, just transform and return that
            if (TemplateNode.InnerText.HasText())
            {
                /* Substitute occurrences of gender words with other values. This typically 
                   inverts the gender on gender pronouns ("he" / "she" / etc.) */

                var substitutions = ChatEngine.GenderSubstitutions;
                return TextSubstitutionTransformer.Substitute(substitutions,
                                                              TemplateNode.InnerText);
            }

            // Apply an AIML "star" tag and store the results as our inner text
            var star = BuildStarTagHandler();
            if (star == null)
            {
                return string.Empty;
            }
            TemplateNode.InnerText = star.Transform().NonNull();

            //! If we still have text, we'll need to use recursion to evaluate our new value
            return TemplateNode.InnerText.HasText() ? ProcessChange() : string.Empty;
        }

    }
}