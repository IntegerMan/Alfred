// ---------------------------------------------------------
// size.cs
// 
// Created on:      08/12/2015 at 10:54 PM
// Last Modified:   08/12/2015 at 11:03 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.Xml;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    public class size : AimlTagHandler
    {
        public size(Bot bot, User user, SubQuery query, Request request, Result result, XmlNode templateNode)
            : base(bot, user, query, request, result, templateNode)
        {
        }

        protected override string ProcessChange()
        {
            if (templateNode.Name.ToLower() == "size")
            {
                return Convert.ToString(bot.Size);
            }
            return string.Empty;
        }
    }
}