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
        public srai(ChatEngine chatEngine, User user, SubQuery query, Request request, Result result, XmlNode templateNode)
            : base(chatEngine, user, query, request, result, templateNode)
        {
        }

        protected override string ProcessChange()
        {
            if (!(TemplateNode.Name.ToLower() == "srai") || TemplateNode.InnerText.Length <= 0)
            {
                return string.Empty;
            }

            var request = new Request(TemplateNode.InnerText, User, ChatEngine, this.Request);

            var result = ChatEngine.Chat(request);

            this.Request.CheckForTimedOut();

            return result.Output;
        }
    }
}