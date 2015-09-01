// ---------------------------------------------------------
// DateTagHandler.cs
// 
// Created on:      08/19/2015 at 9:31 PM
// Last Modified:   08/24/2015 at 12:35 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    /// <summary>
    ///     An AimlTagHandler for the "date" tag. This provides an easy way of giving the time and date.
    /// </summary>
    [HandlesAimlTag("date")]
    [UsedImplicitly]
    public class DateTagHandler : AimlTagHandler
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="DateTagHandler" /> class.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        public DateTagHandler([NotNull] TagHandlerParameters parameters) : base(parameters) { }

        /// <summary>
        ///     Processes the input text and returns the processed value.
        /// </summary>
        /// <returns>The processed output</returns>
        protected override string ProcessChange()
        {
            var formatString = "f"; // Full date and time string

            /* By default we'll use the full date and time, but look for a format attribute
            to provide a custom format */
            if (HasAttribute("format")) { formatString = GetAttribute("format"); }

            // Return the current time with formatting applied
            var now = DateTime.Now;
            return now.ToString(formatString, Locale);
        }
    }
}