// ---------------------------------------------------------
// GossipTagHandler.cs
// 
// Created on:      08/12/2015 at 10:46 PM
// Last Modified:   08/12/2015 at 11:59 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;
using MattEland.Ani.Alfred.Core.Console;
using MattEland.Common;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    /// <summary>
    /// A tag handler for the AIML "gossip" tag. 
    /// 
    /// This implementation simply logs the contents of the gossip tag to the logger.
    /// </summary>
    [HandlesAimlTag("gossip")]
    public class GossipTagHandler : AimlTagHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AimlTagHandler" /> class.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        public GossipTagHandler([NotNull] TagHandlerParameters parameters)
            : base(parameters)
        {
        }

        /// <summary>
        /// Processes the input text and returns the processed value.
        /// </summary>
        /// <returns>The processed output</returns>
        protected override string ProcessChange()
        {
            var node = TemplateNode;

            //- Ensure we have something to care about
            if (node.Name.Matches("gossip") && node.InnerText.HasText())
            {
                Log(string.Format(Locale, "GOSSIP from user: {0}, '{1}'", User.Id, node.InnerText), LogLevel.Info);
            }

            return string.Empty;
        }
    }
}