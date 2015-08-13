// ---------------------------------------------------------
// bot.cs
// 
// Created on:      08/12/2015 at 10:40 PM
// Last Modified:   08/12/2015 at 11:59 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Xml;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    public class bot : AimlTagHandler
    {
        public bot(ChatEngine chatEngine, User user, SubQuery query, Request request, Result result, XmlNode templateNode)
            : base(chatEngine, user, query, request, result, templateNode)
        {
        }

        protected override string ProcessChange()
        {
            if (templateNode.Name.ToLower() == "ChatEngine" && templateNode.Attributes.Count == 1 &&
                templateNode.Attributes[0].Name.ToLower() == "name")
            {
                return ChatEngine.GlobalSettings.grabSetting(templateNode.Attributes["name"].Value);
            }
            return string.Empty;
        }
    }
}