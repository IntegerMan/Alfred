// ---------------------------------------------------------
// uppercase.cs
// 
// Created on:      08/12/2015 at 11:00 PM
// Last Modified:   08/12/2015 at 11:03 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System.Xml;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    public class uppercase : AimlTagHandler
    {
        public uppercase(Bot bot, User user, SubQuery query, Request request, Result result, XmlNode templateNode)
            : base(bot, user, query, request, result, templateNode)
        {
        }

        protected override string ProcessChange()
        {
            if (templateNode.Name.ToLower() == "uppercase")
            {
                return templateNode.InnerText.ToUpper(bot.Locale);
            }
            return string.Empty;
        }
    }
}