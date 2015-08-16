// ---------------------------------------------------------
// set.cs
// 
// Created on:      08/12/2015 at 10:53 PM
// Last Modified:   08/15/2015 at 1:16 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;
using MattEland.Common;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    /// <summary>
    /// A tag handler for handling the AIML "set" tag managing user variables.
    /// </summary>
    [HandlesAimlTag("set")]
    public class SetTagHandler : AimlTagHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AimlTagHandler" /> class.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        public SetTagHandler([NotNull] TagHandlerParameters parameters)
            : base(parameters)
        {
        }

        /// <summary>
        /// Processes the input text and returns the processed value.
        /// </summary>
        /// <returns>The processed output</returns>
        protected override string ProcessChange()
        {
            //- Validate conditions
            var element = TemplateElement;
            if (element == null || !element.Name.Matches("set") || (!element.HasAttribute("name")))
            {
                return string.Empty;
            }

            // Get the settng we're working with
            var settingName = element.GetAttribute("name");
            if (settingName.IsEmpty())
            {
                return string.Empty;
            }

            var innerText = element.InnerText;
            if (innerText.HasText())
            {
                // Add a setting for the specified value
                User.UserVariables.Add(settingName, innerText);

                return innerText;
            }

            // We don't have text. Clear out the setting instead
            User.UserVariables.Remove(settingName);

            return string.Empty;
        }
    }
}