﻿// ---------------------------------------------------------
// person.cs
// 
// Created on:      08/12/2015 at 10:50 PM
// Last Modified:   08/12/2015 at 11:03 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System.Xml;

using MattEland.Ani.Alfred.Chat.Aiml.Normalize;
using MattEland.Ani.Alfred.Chat.Aiml.Utils;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    public class person : AimlTagHandler
    {
        public person(Bot bot, User user, SubQuery query, Request request, Result result, XmlNode templateNode)
            : base(bot, user, query, request, result, templateNode)
        {
        }

        protected override string ProcessChange()
        {
            if (!(templateNode.Name.ToLower() == "person"))
            {
                return string.Empty;
            }
            if (templateNode.InnerText.Length > 0)
            {
                return ApplySubstitutions.Substitute(bot, bot.PersonSubstitutions, templateNode.InnerText);
            }
            templateNode.InnerText =
                new star(bot, user, query, request, result, AimlTagHandler.getNode("<star/>")).Transform();
            if (templateNode.InnerText.Length > 0)
            {
                return ProcessChange();
            }
            return string.Empty;
        }
    }
}