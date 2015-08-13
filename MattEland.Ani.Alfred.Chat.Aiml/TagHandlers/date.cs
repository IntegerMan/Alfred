// ---------------------------------------------------------
// date.cs
// 
// Created on:      08/12/2015 at 10:43 PM
// Last Modified:   08/12/2015 at 11:59 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Xml;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    public class date : AimlTagHandler
    {
        public date(Bot bot, User user, SubQuery query, Request request, Result result, XmlNode templateNode)
            : base(bot, user, query, request, result, templateNode)
        {
        }

        protected override string ProcessChange()
        {
            if (templateNode.Name.ToLower() == "date")
            {
                return DateTime.Now.ToString(Bot.Locale);
            }
            return string.Empty;
        }
    }
}