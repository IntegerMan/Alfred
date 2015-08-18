// ---------------------------------------------------------
// AlfredTagHandler.cs
// 
// Created on:      08/17/2015 at 10:57 PM
// Last Modified:   08/17/2015 at 10:59 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml.TagHandlers;
using MattEland.Ani.Alfred.Chat.Aiml.Utils;

namespace MattEland.Ani.Alfred.Chat
{
    /// <summary>
    ///     A tag handler that handles the custom "alfred" tag that integrates Alfred into the AIML system
    ///     and allows AIML files to issue commands to Alfred modules.
    /// </summary>
    [HandlesAimlTag("alfred")]
    public class AlfredTagHandler : AimlTagHandler
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AlfredTagHandler" /> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="parameters" /> is <see langword="null" />.</exception>
        public AlfredTagHandler([NotNull] TagHandlerParameters parameters) : base(parameters)
        {
        }

        /// <summary>
        ///     Processes the input text and returns the processed value.
        /// </summary>
        /// <returns>The processed output</returns>
        protected override string ProcessChange()
        {
            throw new NotImplementedException("Working on it...");
        }
    }
}