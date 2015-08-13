// ---------------------------------------------------------
// srai.cs
// 
// Created on:      08/12/2015 at 10:55 PM
// Last Modified:   08/12/2015 at 11:03 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System.Xml;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    public class srai : AimlTagHandler
    {
        public srai(Bot bot, User user, SubQuery query, Request request, Result result, XmlNode templateNode)
            : base(bot, user, query, request, result, templateNode)
        {
        }

        protected override string ProcessChange()
        {
            if (!(templateNode.Name.ToLower() == "srai") || templateNode.InnerText.Length <= 0)
            {
                return string.Empty;
            }
            var request = new Request(templateNode.InnerText, user, bot);
            request.StartedOn = this.request.StartedOn;
            var result = bot.Chat(request);
            this.request.hasTimedOut = request.hasTimedOut;
            return result.Output;
        }
    }
}