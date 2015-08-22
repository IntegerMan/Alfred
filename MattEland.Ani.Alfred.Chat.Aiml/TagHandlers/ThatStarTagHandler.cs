// ---------------------------------------------------------
// ThatStarTagHandler.cs
// 
// Created on:      08/12/2015 at 10:58 PM
// Last Modified:   08/15/2015 at 11:54 PM
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
    ///     A tag handler responsible for handling AIML "thatstar" tags. This returns items in the query's
    ///     ThatStar list depending on the specified index (defaults to the first one). The thatstar is
    ///     used to referred to * items inside the "that" tag.
    /// </summary>
    [HandlesAimlTag("thatstar")]
    public class ThatStarTagHandler : AimlTagHandler
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AimlTagHandler" /> class.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        public ThatStarTagHandler([NotNull] TagHandlerParameters parameters)
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

            //- Early exit if it's not what this should be handling
            if (!element.Name.Matches("thatstar"))
            {
                return string.Empty;
            }

            // Handle case of no items in 
            if (Query.ThatStar.Count <= 0)
            {
                Log(string.Format(Locale,
                                  "Encountered a thatstar query with no items in Query.ThatStar on request: {0}",
                                  Request.RawInput),
                    LogLevel.Error);

                return string.Empty;
            }

            // If there's no index, just return the first one
            if (!element.HasAttribute("index"))
            {
                return Query.ThatStar[0].NonNull();
            }

            // With an index, return the elemernt at the specified index.
            var index = element.GetAttribute("index").AsInt();
            if (index > 0)
            {
                return Query.ThatStar[index - 1].NonNull();
            }

            // Nice one, AIML author; looks like a 0 or negative index was specified. Log it and return.
            Log(string.Format(Locale,
                              "An input tag with a badly formed index ({0}) was encountered processing the input: {1}",
                              element.GetAttribute("index"),
                              Request.RawInput),
                LogLevel.Error);

            return string.Empty;
        }
    }
}