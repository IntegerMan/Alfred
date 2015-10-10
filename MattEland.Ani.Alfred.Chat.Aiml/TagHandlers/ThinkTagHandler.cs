// ---------------------------------------------------------
// ThinkTagHandler.cs
// 
// Created on:      08/12/2015 at 10:59 PM
// Last Modified:   08/15/2015 at 1:29 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using MattEland.Common.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    /// <summary>
    ///     A tag handler for the AIML think tag. This outputs nothing.
    /// </summary>
    [HandlesAimlTag("think")]
    [UsedImplicitly]
    internal sealed class ThinkTagHandler : AimlTagHandler
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AimlTagHandler" /> class.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public ThinkTagHandler([NotNull] TagHandlerParameters parameters)
            : base(parameters)
        {
        }

        /// <summary>
        ///     Processes the input text and returns the processed value.
        /// </summary>
        /// <returns>The processed output</returns>
        protected override string ProcessChange()
        {
            //? TODO: Do statements inside of Think get executed still?
            //? If so, shouldn't it run them via a srai and just not output anything?

            return string.Empty;
        }
    }
}