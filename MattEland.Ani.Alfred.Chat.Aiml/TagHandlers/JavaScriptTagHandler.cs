// ---------------------------------------------------------
// JavaScriptTagHandler.cs
// 
// Created on:      08/12/2015 at 10:48 PM
// Last Modified:   08/12/2015 at 11:59 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;
using MattEland.Ani.Alfred.Core.Console;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    /// <summary>
    /// A TagHandler for AIML "javascript" tags. JavaScript tags are not currently supported.
    /// </summary>
    [HandlesAimlTag("javascript")]
    public class JavaScriptTagHandler : AimlTagHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AimlTagHandler" /> class.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        public JavaScriptTagHandler([NotNull] TagHandlerParameters parameters)
            : base(parameters)
        {
        }

        /// <summary>
        /// Processes the input text and returns the processed value.
        /// </summary>
        /// <returns>The processed output</returns>
        protected override string ProcessChange()
        {
            // TODO: If this ever is ported to use web pages, it might be worth revisiting this tag handler

            Log(Resources.JavaScriptNotSupportedMessage, LogLevel.Warning);

            return string.Empty;
        }
    }
}