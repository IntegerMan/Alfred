// ---------------------------------------------------------
// StarTagHandler.cs
// 
// Created on:      08/19/2015 at 9:31 PM
// Last Modified:   08/24/2015 at 12:43 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;
using MattEland.Common;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    /// <summary>
    ///     An AIML tag handler that handles the "star" tag by returning the referenced item in the
    ///     InputStar collection for the query. This is a way of referencing the rest of a
    ///     sentence after handling part of the input.
    /// </summary>
    [HandlesAimlTag("star"), PublicAPI]
    public sealed class StarTagHandler : AimlTagHandler
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AimlTagHandler" /> class.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        public StarTagHandler([NotNull] TagHandlerParameters parameters) : base(parameters)
        {
        }

        /// <summary>
        ///     Processes the input text and returns the processed value.
        /// </summary>
        /// <returns>The processed output</returns>
        protected override string ProcessChange()
        {
            // Grab whatever * refers to in this particular query
            var inputStar = Query.InputStar;

            // If there's nothing in inputStar, log it and move on
            if (inputStar.Count <= 0)
            {
                Error(string.Format(Locale,
                                    Resources.StarErrorNoInputStarElements.NonNull(),
                                    Request.RawInput));

                return string.Empty;
            }

            // If they don't specify anything just get last value
            if (!HasAttribute("index")) { return inputStar[0].NonNull(); }

            // Grab the index as an integer
            var indexText = GetAttribute("index");
            var index = indexText.AsInt(-1) - 1;

            // Bounds check followed by fetching the star value,
            if (index >= 0 & index < inputStar.Count) { return inputStar[index].NonNull(); }

            // If we got here it was an invalid index
            Error(string.Format(Locale, Resources.StarErrorBadIndex.NonNull(), Request.RawInput));

            return string.Empty;
        }
    }
}