﻿// ---------------------------------------------------------
// GossipTagHandler.cs
// 
// Created on:      08/19/2015 at 9:31 PM
// Last Modified:   08/24/2015 at 12:06 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using MattEland.Common.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;
using MattEland.Ani.Alfred.Core.Console;
using MattEland.Common;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    /// <summary>
    ///     A tag handler for the AIML "gossip" tag.
    /// 
    ///     This implementation simply logs the contents of the gossip tag to the logger.
    /// </summary>
    [HandlesAimlTag("gossip")]
    [UsedImplicitly]
    internal sealed class GossipTagHandler : AimlTagHandler
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AimlTagHandler" /> class.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public GossipTagHandler([NotNull] TagHandlerParameters parameters) : base(parameters)
        {
        }

        /// <summary>
        ///     Processes the input text and returns the processed value.
        /// </summary>
        /// <returns>The processed output</returns>
        protected override string ProcessChange()
        {
            //- Ensure we have something to care about
            if (!Contents.HasText()) { return string.Empty; }

            // Build a message for the log and add the entry. No other action will be taken.
            var message = string.Format(Locale,
                                        Resources.GossipTagHandleLogMessage.NonNull(),
                                        User.Name,
                                        Contents);

            Log(message, LogLevel.Info);

            return string.Empty;
        }
    }
}