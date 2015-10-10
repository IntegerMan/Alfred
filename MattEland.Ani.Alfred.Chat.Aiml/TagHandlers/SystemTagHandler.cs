// ---------------------------------------------------------
// SystemTagHandler.cs
// 
// Created on:      08/12/2015 at 10:57 PM
// Last Modified:   08/15/2015 at 1:42 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using MattEland.Common.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;
using MattEland.Ani.Alfred.Core.Console;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    /// <summary>
    ///     A tag handler for the AIML "system" tag. This tag currently does nothing.
    /// </summary>
    [HandlesAimlTag("system")]
    [UsedImplicitly]
    public sealed class SystemTagHandler : AimlTagHandler
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AimlTagHandler" /> class.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        public SystemTagHandler([NotNull] TagHandlerParameters parameters)
            : base(parameters)
        {
        }

        /// <summary>
        ///     Processes the input text and returns the processed value.
        /// </summary>
        /// <returns>The processed output</returns>
        protected override string ProcessChange()
        {
            /* TODO: I'll likely want to support this very very soon, unless I decide to go with 
               a custom tag of a different type If a straight implementation was wanted, 
               http://stackoverflow.com/questions/15234448/run-shell-commands-using-c-sharp-and-get-the-info-into-string 
               is a nice starting place. */

            Log(Resources.SystemNotImplemented, LogLevel.Warning);

            return string.Empty;
        }
    }
}