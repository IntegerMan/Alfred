// ---------------------------------------------------------
// StarTagHandler.cs
// 
// Created on:      08/12/2015 at 10:56 PM
// Last Modified:   08/15/2015 at 11:35 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;
using MattEland.Ani.Alfred.Core.Console;
using MattEland.Common;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    /// <summary>
    ///     An AIML tag handler that handles the "star" tag by returning the refernced item in the
    ///     InputStar collection for the query. This is a way of referencing the rest of a
    ///     sentence after handling part of the input.
    /// </summary>
    [HandlesAimlTag("star")]
    public class StarTagHandler : AimlTagHandler
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AimlTagHandler" /> class.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        public StarTagHandler([NotNull] TagHandlerParameters parameters)
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
            if (element != null && element.Name.Matches("star"))
            {
                // Grab whatever * refers to in this particular query
                var inputStar = Query.InputStar;

                // If there's nothing in inputStar, log it and move on
                if (inputStar.Count <= 0)
                {
                    Log(string.Format(Locale,
                                      Resources.StarErrorNoInputStarElements,
                                      Request.RawInput),
                        LogLevel.Error);

                    return string.Empty;
                }

                // If they don't specify anything just get last value
                if (!element.HasAttribute("index"))
                {
                    return inputStar[0];
                }

                // Grab the index as an integer
                var indexText = element.GetAttribute("index");
                var index = indexText.AsInt(-1) - 1;

                // Bounds check followed by fetching the star value,
                if (index >= 0 & index < inputStar.Count)
                {
                    return inputStar[index];
                }

                Log(string.Format(Locale,
                                  Resources.StarErrorBadIndex,
                                  Request.RawInput),
                    LogLevel.Error);
            }

            return string.Empty;
        }
    }
}