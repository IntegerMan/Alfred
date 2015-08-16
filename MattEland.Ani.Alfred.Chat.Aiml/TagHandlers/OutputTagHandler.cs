// ---------------------------------------------------------
// OutputTagHandler.cs
// 
// Created on:      08/12/2015 at 10:57 PM
// Last Modified:   08/15/2015 at 11:06 PM
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
    ///     An AIML tag handler that handles the "that" tag and refers to the nTh last thing the bot said
    ///     to the user.
    /// </summary>
    [HandlesAimlTag("that")]
    public class OutputTagHandler : AimlTagHandler
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AimlTagHandler" /> class.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        public OutputTagHandler([NotNull] TagHandlerParameters parameters)
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
            if (element != null && element.Name.Matches("that"))
            {
                // If we just said that, grab the first sentence of the first output.
                if (element.Attributes.Count == 0)
                {
                    return User.GetOutputSentence();
                }

                if (element.HasAttribute("index"))
                {
                    var indexText = element.GetAttribute("index");

                    if (indexText.HasText())
                    {
                        //- Interpret what the user said
                        var indiciesText = indexText.Split(",".ToCharArray());

                        //- Grab the output index - the nth last interaction result with the chat engine
                        int outputIndex;
                        int.TryParse(indiciesText[0]?.Trim(), out outputIndex);

                        //- Grab the sentence index referring to the sentence within the output - defaulting to 1 if not specified
                        var sentenceIndex = 1;
                        if (indiciesText.Length >= 2)
                        {
                            int.TryParse(indiciesText[1]?.Trim(), out sentenceIndex);
                        }

                        // Return the thing the chat engine said at the specified point in time
                        if (outputIndex > 0 & sentenceIndex > 0)
                        {
                            return User.GetOutputSentence(outputIndex - 1, sentenceIndex - 1);
                        }

                        // If we got here we were invalid but did have some text. Log it.
                        Log(
                            string.Format(Locale,
                                          "An input tag with a badly formed index ({0}) was encountered processing the input: {1}",
                                          indiciesText,
                                          Request.RawInput),
                            LogLevel.Error);

                    }

                }

            }

            return string.Empty;

        }
    }

}