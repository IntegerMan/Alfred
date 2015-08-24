// ---------------------------------------------------------
// BotTagHandler.cs
// 
// Created on:      08/12/2015 at 10:40 PM
// Last Modified:   08/14/2015 at 10:45 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;
using MattEland.Common;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    /// <summary>
    /// An AIML Tag handler that handles the "bot" tag. This allows the system to interact with system variables.
    /// </summary>
    [HandlesAimlTag("bot"), UsedImplicitly]
    public class BotTagHandler : AimlTagHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AimlTagHandler" /> class.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        public BotTagHandler([NotNull] TagHandlerParameters parameters)
            : base(parameters)
        {
        }

        /// <summary>
        ///     Processes the input text and returns the processed value.
        /// </summary>
        /// <returns>The processed output</returns>
        protected override string ProcessChange()
        {
            // Ensure we have an element to work with
            if (!HasAttribute("name")) { return string.Empty; }

            // Grab the attribute and use its value to GetTagHandler a setting value
            var value = GetAttribute("name");

            return value.HasText() ? GetGlobalSetting(value) : string.Empty;
        }

    }
}