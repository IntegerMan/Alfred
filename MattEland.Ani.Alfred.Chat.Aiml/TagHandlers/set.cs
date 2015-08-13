﻿// ---------------------------------------------------------
// set.cs
// 
// Created on:      08/12/2015 at 10:53 PM
// Last Modified:   08/12/2015 at 11:03 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System.Xml;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    public class set : AimlTagHandler
    {
        public set(Bot bot, User user, SubQuery query, Request request, Result result, XmlNode templateNode)
            : base(bot, user, query, request, result, templateNode)
        {
        }

        protected override string ProcessChange()
        {
            if (!(templateNode.Name.ToLower() == "set") || bot.GlobalSettings.Count <= 0 ||
                (templateNode.Attributes.Count != 1 || !(templateNode.Attributes[0].Name.ToLower() == "name")))
            {
                return string.Empty;
            }
            if (templateNode.InnerText.Length > 0)
            {
                user.Predicates.addSetting(templateNode.Attributes[0].Value, templateNode.InnerText);
                return user.Predicates.grabSetting(templateNode.Attributes[0].Value);
            }
            user.Predicates.removeSetting(templateNode.Attributes[0].Value);
            return string.Empty;
        }
    }
}