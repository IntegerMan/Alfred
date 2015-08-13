﻿// ---------------------------------------------------------
// formal.cs
// 
// Created on:      08/12/2015 at 10:44 PM
// Last Modified:   08/12/2015 at 11:59 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Text;
using System.Xml;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    public class formal : AimlTagHandler
    {
        public formal(ChatEngine chatEngine, User user, SubQuery query, Request request, Result result, XmlNode templateNode)
            : base(chatEngine, user, query, request, result, templateNode)
        {
        }

        protected override string ProcessChange()
        {
            if (!(TemplateNode.Name.ToLower() == "formal"))
            {
                return string.Empty;
            }
            var stringBuilder = new StringBuilder();
            if (TemplateNode.InnerText.Length > 0)
            {
                foreach (var str1 in TemplateNode.InnerText.ToLower().Split())
                {
                    var str2 = str1.Substring(0, 1).ToUpper();
                    if (str1.Length > 1)
                    {
                        str2 += str1.Substring(1);
                    }
                    stringBuilder.Append(str2 + " ");
                }
            }
            return stringBuilder.ToString().Trim();
        }
    }
}