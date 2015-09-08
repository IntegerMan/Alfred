// ---------------------------------------------------------
// SecondPersonToFirstPersonTagHandler.cs
// 
// Created on:      08/19/2015 at 9:31 PM
// Last Modified:   08/24/2015 at 12:11 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;
using MattEland.Common;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    /// <summary>
    ///     An AIML tag handler that handles converting from the second person "You were" to the first
    ///     person "I am".
    /// </summary>
    [HandlesAimlTag("person2")]
    [UsedImplicitly]
    internal sealed class SecondPersonToFirstPersonTagHandler : AimlTagHandler
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AimlTagHandler" /> class.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public SecondPersonToFirstPersonTagHandler([NotNull] TagHandlerParameters parameters)
            : base(parameters)
        {
        }

        /// <summary>
        ///     Processes the input text and returns the processed value.
        /// </summary>
        /// <returns>The processed output</returns>
        protected override string ProcessChange()
        {
            //- Validate
            if (!NodeName.Matches("person2")) { return string.Empty; }

            // Substitute entries of "You are" with "I am" and the like
            if (Contents.HasText())
            {
                var substitutions = Librarian.SecondPersonToFirstPersonSubstitutions;
                return TextSubstitutionHelper.Substitute(substitutions, Contents);
            }

            // Evaluate everything else and set that as the inner text of this node and then process it
            var star = BuildStarTagHandler();
            Contents = star.Transform().NonNull();

            return Contents.HasText() ? ProcessChange() : string.Empty;
        }
    }
}