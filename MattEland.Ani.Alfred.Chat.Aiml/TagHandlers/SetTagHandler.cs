// ---------------------------------------------------------
// SetTagHandler.cs
// 
// Created on:      08/19/2015 at 9:31 PM
// Last Modified:   08/24/2015 at 12:39 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;
using MattEland.Common;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    /// <summary>
    ///     A tag handler for handling the AIML "set" tag managing user variables.
    /// </summary>
    [HandlesAimlTag("set")]
    [UsedImplicitly]
    internal sealed class SetTagHandler : AimlTagHandler
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AimlTagHandler" /> class.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        public SetTagHandler([NotNull] TagHandlerParameters parameters) : base(parameters) { }

        /// <summary>
        ///     Processes the input text and returns the processed value.
        /// </summary>
        /// <returns>The processed output</returns>
        protected override string ProcessChange()
        {
            //- Validate conditions
            if (!HasAttribute("name")) { return string.Empty; }

            // Get the setting we're working with
            var settingName = GetAttribute("name");
            if (settingName.IsEmpty()) { return string.Empty; }

            if (Contents.HasText())
            {
                // Add a setting for the specified value
                User.UserVariables.Add(settingName, Contents);

                return Contents;
            }

            // We don't have text. Clear out the setting instead
            User.UserVariables.Remove(settingName);

            return string.Empty;
        }
    }
}