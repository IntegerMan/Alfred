// ---------------------------------------------------------
// OutputTagHandler.cs
// 
// Created on:      08/19/2015 at 9:31 PM
// Last Modified:   08/24/2015 at 12:41 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;
using MattEland.Common;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    /// <summary>
    ///     An AIML tag handler that handles the "that" tag and refers to the nTh last thing the bot said
    ///     to the user.
    /// </summary>
    [HandlesAimlTag("that")]
    [UsedImplicitly]
    internal sealed class OutputTagHandler : AimlTagHandler
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AimlTagHandler" /> class.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        public OutputTagHandler([NotNull] TagHandlerParameters parameters) : base(parameters)
        {
        }

        /// <summary>
        ///     Processes the input text and returns the processed value.
        /// </summary>
        /// <returns>The processed output</returns>
        protected override string ProcessChange()
        {
            // If we just said that, grab the first sentence of the first output.
            if (!HasAttribute("index")) { return string.Empty; }

            //- Get the index value and validate it
            var indexText = GetAttribute("index");
            if (!indexText.HasText()) { return string.Empty; }

            //- Interpret what the user said
            var indiciesText = indexText.Split(",".ToCharArray());

            //- Grab the output index - the nth last interaction result with the chat engine
            var outputIndex = indiciesText[0].AsInt();

            //- Grab the sentence index referring to the sentence within the output - defaulting to 1 if not specified
            var sentenceIndex = 1;
            if (indiciesText.Length >= 2) { sentenceIndex = indiciesText[1].AsInt(); }

            // Return the thing the chat engine said at the specified point in time
            if (outputIndex > 0 & sentenceIndex > 0)
            {
                return User.GetOutputSentence(outputIndex - 1, sentenceIndex - 1);
            }

            // If we got here we were invalid but did have some text. Log it.
            Error(string.Format(Locale,
                                Resources.OutputTagHandlerProcessChangeInvalidIndex.NonNull(),
                                indiciesText,
                                Request.RawInput));

            return string.Empty;
        }
    }

}