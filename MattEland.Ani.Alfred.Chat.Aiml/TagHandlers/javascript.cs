// ---------------------------------------------------------
// javascript.cs
// 
// Created on:      08/12/2015 at 10:48 PM
// Last Modified:   08/12/2015 at 11:59 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Xml;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    public class javascript : AimlTagHandler
    {
        public javascript(Bot bot, User user, SubQuery query, Request request, Result result, XmlNode templateNode)
            : base(bot, user, query, request, result, templateNode)
        {
        }

        protected override string ProcessChange()
        {
            Bot.writeToLog("The javascript tag is not implemented in this bot");
            return string.Empty;
        }
    }
}