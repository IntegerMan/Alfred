// ---------------------------------------------------------
// DateTagHandler.cs
// 
// Created on:      08/12/2015 at 10:43 PM
// Last Modified:   08/14/2015 at 3:13 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;
using MattEland.Common;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    /// <summary>
    /// An AimlTagHandler for the "date" tag. This provides an easy way of giving the time and date.
    /// </summary>
    [HandlesAimlTag("date")]
    public class DateTagHandler : AimlTagHandler
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="DateTagHandler" /> class.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        public DateTagHandler([NotNull] TagHandlerParameters parameters)
            : base(parameters)
        {
        }

        /// <summary>
        ///     Processes the input text and returns the processed value.
        /// </summary>
        /// <returns>The processed output</returns>
        protected override string ProcessChange()
        {
            var isDateNode = TemplateNode.Name.Matches("date");

            // Format using long date formatting for the culture
            const string DateFormatString = "D"; // TODO: Add support for a format attribute

            return isDateNode ? DateTime.Now.ToString(DateFormatString, Locale) : string.Empty;

        }
    }
}