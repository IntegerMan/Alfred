// ---------------------------------------------------------
// bot.cs
// 
// Created on:      08/12/2015 at 10:40 PM
// Last Modified:   08/12/2015 at 11:03 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System.Xml;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    public class bot : AimlTagHandler
    {
        public bot(Bot bot, User user, SubQuery query, Request request, Result result, XmlNode templateNode)
            : base(bot, user, query, request, result, templateNode)
        {
        }

        protected override string ProcessChange()
        {
            if (templateNode.Name.ToLower() == "bot" && templateNode.Attributes.Count == 1 &&
                templateNode.Attributes[0].Name.ToLower() == "name")
            {
                return bot.GlobalSettings.grabSetting(templateNode.Attributes["name"].Value);
            }
            return string.Empty;
        }
    }
}