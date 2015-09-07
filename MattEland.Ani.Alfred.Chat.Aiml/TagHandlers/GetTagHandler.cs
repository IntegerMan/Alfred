// ---------------------------------------------------------
// GetTagHandler.cs
// 
// Created on:      08/19/2015 at 9:31 PM
// Last Modified:   08/24/2015 at 12:36 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;
using MattEland.Common;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    /// <summary>
    ///     A TagHandler for the AIML "get" tag.
    /// </summary>
    [HandlesAimlTag("get")]
    [UsedImplicitly]
    internal sealed class GetTagHandler : AimlTagHandler
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AimlTagHandler" /> class.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        public GetTagHandler([NotNull] TagHandlerParameters parameters) : base(parameters) { }

        /// <summary>
        ///     Processes the input text and returns the processed value.
        /// </summary>
        /// <returns>The processed output</returns>
        protected override string ProcessChange()
        {
            //- Ensure that the node has the attribute we need
            if (!HasAttribute("name")) { return string.Empty; }

            // Grab the name of the requested variable from the tag
            var variableName = GetAttribute("name");

            //- Safety check for junk input
            if (variableName.IsEmpty()) { return string.Empty; }

            // Return the value of the variable
            return User.UserVariables.GetValue(variableName);
        }
    }
}