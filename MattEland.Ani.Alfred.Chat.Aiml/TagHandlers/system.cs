// ---------------------------------------------------------
// system.cs
// 
// Created on:      08/12/2015 at 10:57 PM
// Last Modified:   08/12/2015 at 11:03 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System.Xml;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    public class system : AimlTagHandler
    {
        public system(Bot bot, User user, SubQuery query, Request request, Result result, XmlNode templateNode)
            : base(bot, user, query, request, result, templateNode)
        {
        }

        protected override string ProcessChange()
        {
            bot.writeToLog("The system tag is not implemented in this bot");
            return string.Empty;
        }
    }
}