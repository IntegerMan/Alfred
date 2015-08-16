// ---------------------------------------------------------
// topicstar.cs
// 
// Created on:      08/12/2015 at 10:59 PM
// Last Modified:   08/12/2015 at 11:59 PM
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
    /// A tag handler for the AIML "topicstar" tag. This returns the output of the specified * value within the topic node.
    /// </summary>
    [HandlesAimlTag("topicstar")]
    public class TopicStarTagHandler : AimlTagHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AimlTagHandler" /> class.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        public TopicStarTagHandler([NotNull] TagHandlerParameters parameters)
            : base(parameters)
        {
        }

        /// <summary>
        /// Processes the input text and returns the processed value.
        /// </summary>
        /// <returns>The processed output</returns>
        protected override string ProcessChange()
        {
            var element = TemplateElement;
            if (element != null && element.Name.Matches("topicstar"))
            {
                // Validate that we have TopicStar items to work with
                var topicStar = Query.TopicStar;
                if (topicStar.Count <= 0)
                {
                    Log(string.Format(Locale, Resources.TopicStarErrorNoItems, Request.RawInput), LogLevel.Error);

                    return string.Empty;
                }

                // When no index is specified return the first item.
                if (!element.HasAttribute("index"))
                {
                    return topicStar[0];
                }

                // Grab the item at the specified array index
                var index = element.GetAttribute("index").AsInt();
                if (index.IsWithinBoundsOf(topicStar))
                {
                    return topicStar[index - 1];
                }

                // Log the out of range failure
                Log(string.Format(Locale, Resources.TopicStarErrorOutOfRange, index, Request.RawInput), LogLevel.Error);
            }

            return string.Empty;
        }
    }
}