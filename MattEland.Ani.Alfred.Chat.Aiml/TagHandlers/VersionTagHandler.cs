// ---------------------------------------------------------
// VersionTagHandler.cs
// 
// Created on:      08/12/2015 at 11:01 PM
// Last Modified:   08/15/2015 at 1:33 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;
using MattEland.Common;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    /// <summary>
    ///     An AIML tag handler for the AIML "version" tag. This outputs the bot's version.
    /// </summary>
    [HandlesAimlTag("version")]
    public class VersionTagHandler : AimlTagHandler
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AimlTagHandler" /> class.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        public VersionTagHandler([NotNull] TagHandlerParameters parameters)
            : base(parameters)
        {
        }

        /// <summary>
        ///     Processes the input text and returns the processed value.
        /// </summary>
        /// <returns>The processed output</returns>
        protected override string ProcessChange()
        {
            if (TemplateNode.Name.Matches("version"))
            {
                // Return the version or our engine's assembly version if it's not set
                var version = ChatEngine.GlobalSettings.GetValue("version");

                return version.HasText() ? version : ChatEngine.GetAssemblyVersion().ToString();
            }

            return string.Empty;
        }
    }
}