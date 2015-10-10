// ---------------------------------------------------------
// FirstPersonToSecondPersonTagHandler.cs
// 
// Created on:      08/19/2015 at 9:31 PM
// Last Modified:   08/24/2015 at 12:03 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using MattEland.Common.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;
using MattEland.Common;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    /// <summary>
    ///     A tag handler for the AIML "person" tag. This handles conversions from the first person to the
    ///     second person.
    /// </summary>
    [HandlesAimlTag("person")]
    [UsedImplicitly]
    public sealed class FirstPersonToSecondPersonTagHandler : AimlTagHandler
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AimlTagHandler" /> class.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        public FirstPersonToSecondPersonTagHandler([NotNull] TagHandlerParameters parameters)
            : base(parameters)
        {
        }

        /// <summary>
        ///     Processes the input text and returns the processed value.
        /// </summary>
        /// <returns>The processed output</returns>
        protected override string ProcessChange()
        {
            if (!NodeName.Matches("person")) { return string.Empty; }

            // Substitute entries of "I am" with "You are" and the like
            if (Contents.HasText())
            {
                var substitutions = Librarian.FirstPersonToSecondPersonSubstitutions;
                return TextSubstitutionHelper.Substitute(substitutions, Contents);
            }

            // Evaluate everything else and set that as the inner text of this node and then process it
            var star = BuildStarTagHandler();
            Contents = star.Transform().NonNull();

            if (Contents.HasText())
            {
                // Recursively process the input with the remainder of the inner text
                return ProcessChange();
            }

            return string.Empty;
        }
    }
}