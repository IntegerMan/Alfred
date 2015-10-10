// ---------------------------------------------------------
// InputTagHandler.cs
// 
// Created on:      08/19/2015 at 9:31 PM
// Last Modified:   08/24/2015 at 12:37 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using MattEland.Common.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;
using MattEland.Common;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    /// <summary>
    ///     A tag handler that handles the AIML 'input' tag by grabbing historical input from the user and
    ///     rendering that text.
    /// </summary>
    [HandlesAimlTag("input")]
    [UsedImplicitly]
    internal sealed class InputTagHandler : AimlTagHandler
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AimlTagHandler" /> class.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public InputTagHandler([NotNull] TagHandlerParameters parameters) : base(parameters)
        {
        }

        /// <summary>
        ///     Processes the input text and returns the processed value.
        /// </summary>
        /// <returns>The processed output</returns>
        protected override string ProcessChange()
        {
            // If we don't have an input tag, just get the default one
            if (!HasAttribute("index")) { return User.GetInputSentence(); }

            // Grab and validate the index
            var indexText = GetAttribute("index");
            if (indexText.HasText())
            {
                // We can be either in a # format or a #,# format. Check to find out which
                var indexes = indexText.Split(",".ToCharArray());

                //- Grab values for the inputs. This refers to the nth last thing the user said to the engine
                var inputIndex = indexes[0].AsInt();

                //- Sentence index is optional. This refers to the sentence in the input referred to by inputIndex.
                var sentenceIndex = 1;
                if (indexes.Length >= 2) { sentenceIndex = indexes[1].AsInt(); }

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
                                        Resources.InputErrorBadlyFormedIndex.NonNull(),
                                        indexText,
                                        Request.RawInput);

            Error(message);
        }
    }
}