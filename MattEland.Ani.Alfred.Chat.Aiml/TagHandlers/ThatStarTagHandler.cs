// ---------------------------------------------------------
// ThatStarTagHandler.cs
// 
// Created on:      08/19/2015 at 9:31 PM
// Last Modified:   08/24/2015 at 12:44 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using MattEland.Common.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;
using MattEland.Common;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    /// <summary>
    ///     A tag handler responsible for handling AIML "thatstar" tags. This returns items in the query's
    ///     ThatStar list depending on the specified index (defaults to the first one). The thatstar is
    ///     used to referred to * items inside the "that" tag.
    /// </summary>
    [HandlesAimlTag(@"thatstar")]
    [UsedImplicitly, PublicAPI]
    public sealed class ThatStarTagHandler : AimlTagHandler
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AimlTagHandler" /> class.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        public ThatStarTagHandler([NotNull] TagHandlerParameters parameters) : base(parameters)
        {
        }

        /// <summary>
        ///     Processes the input text and returns the processed value.
        /// </summary>
        /// <returns>The processed output</returns>
        protected override string ProcessChange()
        {
            // Handle case of no items in 
            if (Query.ThatStar.Count <= 0)
            {
                Error(string.Format(Locale,
                                    @"Encountered a thatstar query with no items in Query.ThatStar on request: {0}",
                                    Request.RawInput));

                return string.Empty;
            }

            // If there's no index, just return the first one
            if (!HasAttribute("index")) { return Query.ThatStar[0].NonNull(); }

            // With an index, return the element at the specified index.
            var index = GetAttribute("index").AsInt();
            if (index > 0) { return Query.ThatStar[index - 1].NonNull(); }

            // Nice one, AIML author; looks like a 0 or negative index was specified. Log it and return.
            Error(string.Format(Locale,
                                Resources.ThatStarTagHandlerProcessChangeInvalidIndex.NonNull(),
                                GetAttribute("index"),
                                Request.RawInput));

            return string.Empty;
        }
    }
}