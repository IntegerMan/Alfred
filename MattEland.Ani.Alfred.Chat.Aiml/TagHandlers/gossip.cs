// ---------------------------------------------------------
// gossip.cs
// 
// Created on:      08/12/2015 at 10:46 PM
// Last Modified:   08/12/2015 at 11:59 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Xml;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;
using MattEland.Ani.Alfred.Core.Console;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    public class gossip : AimlTagHandler
    {
        public gossip([NotNull] TagHandlerParameters parameters)
            : base(parameters)
        {
        }

        protected override string ProcessChange()
        {
            if (TemplateNode.Name.ToLower() == "gossip" && TemplateNode.InnerText.Length > 0)
            {
                Log("GOSSIP from user: " + User.Id + ", '" + TemplateNode.InnerText + "'", LogLevel.Info);
            }
            return string.Empty;
        }
    }
}