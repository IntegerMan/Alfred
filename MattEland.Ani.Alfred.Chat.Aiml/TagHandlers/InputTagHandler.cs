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
            if (!element.Name.Matches("input"))
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

                //- Grab values for the inputs. This refers to the nth last thing the user said to the engine
                var inputIndex = indexes[0].AsInt();

                //- Sentence index is optional. This refers to the sentence in the input referred to by inputIndex.
                var sentenceIndex = 1;
                if (indexes.Length >= 2)
                {
                    sentenceIndex = indexes[1].AsInt();
                }

                // Grab the input X inputs ago and Y sentences into that input
                if (inputIndex > 0 && sentenceIndex > 0)
                {
                    return User.GetInputSentence(inputIndex - 1, sentenceIndex - 1);
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
        private void LogBadIndex([NotNull] string indexText)
        {
            var message = string.Format(Locale,
                                           Resources.InputErrorBadlyFormedIndex,
                                           indexText,
                                           Request.RawInput);

            Log(message, LogLevel.Error);
        }
    }
}