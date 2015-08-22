// ---------------------------------------------------------
// person.cs
// 
// Created on:      08/12/2015 at 10:50 PM
// Last Modified:   08/12/2015 at 11:59 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;
using MattEland.Common;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    /// <summary>
    /// A tag handler for the AIML "person" tag. This handles conversions from the first person to the second person.
    /// </summary>
    [HandlesAimlTag("person")]
    public class FirstPersonToSecondPersonTagHandler : AimlTagHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AimlTagHandler" /> class.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        public FirstPersonToSecondPersonTagHandler([NotNull] TagHandlerParameters parameters)
            : base(parameters)
        {
        }

        /// <summary>
        /// Processes the input text and returns the processed value.
        /// </summary>
        /// <returns>The processed output</returns>
        protected override string ProcessChange()
        {
            var node = TemplateNode;
            if (!node.Name.Matches("person"))
            {
                return string.Empty;
            }

            // Substitute entries of "I am" with "You are" and the like
            if (node.InnerText.HasText())
            {
                var substitutions = Librarian.FirstPersonToSecondPersonSubstitutions;
                return TextSubstitutionHelper.Substitute(substitutions, node.InnerText);
            }

            // Evaluate everything else and set that as the inner text of this node and then process it
            var star = BuildStarTagHandler();
            node.InnerText = star.Transform().NonNull();

            if (node.InnerText.HasText())
            {
                // Recursively process the input with the remainder of the inner text
                return ProcessChange();
            }

            return string.Empty;
        }

    }
}