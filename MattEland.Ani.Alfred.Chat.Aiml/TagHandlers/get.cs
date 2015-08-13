﻿// ---------------------------------------------------------
// get.cs
// 
// Created on:      08/12/2015 at 10:45 PM
// Last Modified:   08/12/2015 at 11:03 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System.Xml;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    public class get : AimlTagHandler
    {
        public get(Bot bot, User user, SubQuery query, Request request, Result result, XmlNode templateNode)
            : base(bot, user, query, request, result, templateNode)
        {
        }

        protected override string ProcessChange()
        {
            if (templateNode.Name.ToLower() == "get" && bot.GlobalSettings.Count > 0 &&
                (templateNode.Attributes.Count == 1 && templateNode.Attributes[0].Name.ToLower() == "name"))
            {
                return user.Predicates.grabSetting(templateNode.Attributes[0].Value);
            }
            return string.Empty;
        }
    }
}