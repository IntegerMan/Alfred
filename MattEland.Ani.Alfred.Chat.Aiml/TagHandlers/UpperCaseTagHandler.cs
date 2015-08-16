// ---------------------------------------------------------
// uppercase.cs
// 
// Created on:      08/12/2015 at 11:00 PM
// Last Modified:   08/12/2015 at 11:59 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;
using MattEland.Common;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    /// <summary>
    /// A tag handler for the AIML "uppercase" tag. This outputs its text in upper case.
    /// </summary>
    [HandlesAimlTag("uppercase")]
    public class UppercaseTagHandler : AimlTagHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AimlTagHandler" /> class.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        public UppercaseTagHandler([NotNull] TagHandlerParameters parameters)
            : base(parameters)
        {
        }

        /// <summary>
        /// Processes the input text and returns the processed value.
        /// </summary>
        /// <returns>The processed output</returns>
        protected override string ProcessChange()
        {
            if (TemplateNode.Name.Matches("uppercase"))
            {
                return TemplateNode.InnerText.ToUpper(Locale);
            }

            return string.Empty;
        }
    }
}