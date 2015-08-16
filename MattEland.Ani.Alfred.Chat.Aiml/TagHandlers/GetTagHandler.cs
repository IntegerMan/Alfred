// ---------------------------------------------------------
// GetTagHandler.cs
// 
// Created on:      08/12/2015 at 10:45 PM
// Last Modified:   08/14/2015 at 10:15 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;
using MattEland.Common;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    /// <summary>
    /// A TagHandler for the AIML "get" tag.
    /// </summary>
    [HandlesAimlTag("get")]
    public class GetTagHandler : AimlTagHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AimlTagHandler" /> class.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        public GetTagHandler([NotNull] TagHandlerParameters parameters)
            : base(parameters)
        {
        }

        /// <summary>
        ///     Processes the input text and returns the processed value.
        /// </summary>
        /// <returns>The processed output</returns>
        protected override string ProcessChange()
        {
            var node = TemplateElement;

            //- Ensure correct tag and that the node has the attribute we need
            if (node == null || !node.Name.Matches("get") || !node.HasAttribute("name"))
            {
                return string.Empty;
            }

            // Grab the name of the requested variable from the tag
            var variableName = node.GetAttribute("name").NonNull();

            //- Safety check for junk input
            if (variableName.IsEmpty())
            {
                return string.Empty;
            }

            // Return the value of the variable
            return User.UserVariables.GetValue(variableName);

        }
    }
}