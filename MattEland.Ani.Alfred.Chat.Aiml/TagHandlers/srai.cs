// ---------------------------------------------------------
// srai.cs
// 
// Created on:      08/12/2015 at 10:55 PM
// Last Modified:   08/12/2015 at 11:59 PM
// 
// Last Modified by: Matt Eland
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

            // TODO: The problem with this model is you don't get the full template as it's traced through. This
            // gives the client difficulties in determining what tag was hit. Then again, maybe I can write around
            // this and not even need to know on the outside how it was routed.

            var request = new Request(templateNode.InnerText, user, Bot);
            request.StartedOn = this.request.StartedOn;

            var result = Bot.Chat(request);
            this.request.hasTimedOut = request.hasTimedOut;
            return result.Output;
        }
    }
}