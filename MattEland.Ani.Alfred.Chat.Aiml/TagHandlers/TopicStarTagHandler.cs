// ---------------------------------------------------------
// TopicStarTagHandler.cs
// 
// Created on:      08/19/2015 at 9:31 PM
// Last Modified:   08/24/2015 at 12:45 AM
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
    ///     A tag handler for the AIML "topicstar" tag. This returns the output of the specified * value
    ///     within the topic node.
    /// </summary>
    [HandlesAimlTag("topicstar")]
    [UsedImplicitly]
    public class TopicStarTagHandler : AimlTagHandler
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AimlTagHandler" /> class.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        public TopicStarTagHandler([NotNull] TagHandlerParameters parameters) : base(parameters)
        {
        }

        /// <summary>
        ///     Processes the input text and returns the processed value.
        /// </summary>
        /// <returns>The processed output</returns>
        protected override string ProcessChange()
        {
            // Validate that we have TopicStar items to work with
            var topicStar = Query.TopicStar;
            if (topicStar.Count <= 0)
            {
                Error(string.Format(Locale,
                                    Resources.TopicStarErrorNoItems.NonNull(),
                                    Request.RawInput));

                return string.Empty;
            }

            // When no index is specified return the first item.
            if (!HasAttribute("index")) { return topicStar[0].NonNull(); }

            // Grab the item at the specified array index
            var index = GetAttribute("index").AsInt();
            if (index.IsWithinBoundsOf(topicStar)) { return topicStar[index - 1].NonNull(); }

            // Log the out of range failure
            Error(string.Format(Locale,
                                Resources.TopicStarErrorOutOfRange.NonNull(),
                                index,
                                Request.RawInput));

            return string.Empty;
        }
    }
}