// ---------------------------------------------------------
// InputTagHandler.cs
// 
// Created on:      08/12/2015 at 10:47 PM
// Last Modified:   08/15/2015 at 12:03 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;
using MattEland.Ani.Alfred.Core.Console;
using MattEland.Common;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    /// <summary>
    /// A tag handler that handles the AIML 'input' tag by grabbing historical input from the user and rendering that text.
    /// </summary>
    [HandlesAimlTag("input")]
    public class InputTagHandler : AimlTagHandler
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AimlTagHandler" /> class.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        public InputTagHandler([NotNull] TagHandlerParameters parameters)
            : base(parameters)
        {
        }

        /// <summary>
        ///     Processes the input text and returns the processed value.
        /// </summary>
        /// <returns>The processed output</returns>
        protected override string ProcessChange()
        {
            var element = TemplateElement;
            if (element == null || !element.Name.Matches("input"))
            {
                return string.Empty;
            }

            // If we don't have an input tag, just get the default one
            if (!element.HasAttribute("index"))
            {
                return User.GetInputSentence();
            }

            // Grab and validate the index
            var indexText = element.GetAttribute("index");
            if (indexText.HasText())
            {

                // We can be either in a # format or a #,# format. Check to find out which
                var indexes = indexText.Split(",".ToCharArray());

                if (indexes.Length >= 2)
                {
                    // We're looking at [InputsAgo],[SentenceInInput]

                    //- Grab values for the inputs, keeping in mind bad data is possible
                    var inputIndex = -1;
                    int.TryParse(indexes[0]?.Trim(), out inputIndex);

                    var sentenceIndex = -1;
                    int.TryParse(indexes[1]?.Trim(), out sentenceIndex);

                    //- If we have some usable values, go with those
                    if (inputIndex > 0 && sentenceIndex > 0)
                    {
                        // Grab the input X inputs ago and Y sentences into that input
                        return User.GetInputSentence(inputIndex - 1, sentenceIndex - 1);
                    }
                }
                else
                {
                    // This is a single index so we don't care which sentence was part of the input, we just care about the index order.
                    int num;
                    if (int.TryParse(indexText, out num) && num > 0)
                    {
                        // Grab the first sentence of X inputs ago
                        return User.GetInputSentence(num - 1);
                    }
                }
            }

            // If we hit this point, it's bad input. Log and move on.
            LogBadIndex(indexText);

            return string.Empty;
        }

        /// <summary>
        ///     Logs a bad index.
        /// </summary>
        /// <param name="indexText">The index text.</param>
        private void LogBadIndex(string indexText)
        {
            var message = string.Format(Locale,
                                           Resources.InputErrorBadlyFormedIndex,
                                           indexText,
                                           Request.RawInput);

            Log(message, LogLevel.Error);
        }
    }
}