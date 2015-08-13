// ---------------------------------------------------------
// gossip.cs
// 
// Created on:      08/12/2015 at 10:46 PM
// Last Modified:   08/12/2015 at 11:59 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Xml;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    public class gossip : AimlTagHandler
    {
        public gossip(ChatEngine chatEngine, User user, SubQuery query, Request request, Result result, XmlNode templateNode)
            : base(chatEngine, user, query, request, result, templateNode)
        {
        }

        protected override string ProcessChange()
        {
            if (templateNode.Name.ToLower() == "gossip" && templateNode.InnerText.Length > 0)
            {
                ChatEngine.writeToLog("GOSSIP from user: " + user.UserID + ", '" + templateNode.InnerText + "'");
            }
            return string.Empty;
        }
    }
}