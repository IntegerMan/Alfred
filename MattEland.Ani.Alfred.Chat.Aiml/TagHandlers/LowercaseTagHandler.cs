// ---------------------------------------------------------
// LowercaseTagHandler.cs
// 
// Created on:      08/12/2015 at 10:49 PM
// Last Modified:   08/15/2015 at 12:38 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;
using MattEland.Common;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    /// <summary>
    ///     A tag handler responding to the AIML "lowercase" tag. This formats all text in the node in
    ///     lower case.
    /// </summary>
    [HandlesAimlTag("lowercase")]
    public class LowercaseTagHandler : AimlTagHandler
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AimlTagHandler" /> class.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        public LowercaseTagHandler([NotNull] TagHandlerParameters parameters)
            : base(parameters)
        {
        }

        /// <summary>
        ///     Processes the input text and returns the processed value.
        /// </summary>
        /// <returns>The processed output</returns>
        protected override string ProcessChange()
        {
            if (TemplateNode.Name.Matches("lowercase"))
            {
                // This is a very simple operation using the user's locale
                return TemplateNode.InnerText.ToLower(Locale);
            }

            return string.Empty;
        }
    }
}