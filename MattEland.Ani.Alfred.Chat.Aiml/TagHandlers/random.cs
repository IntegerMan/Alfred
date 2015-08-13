// ---------------------------------------------------------
// random.cs
// 
// Created on:      08/12/2015 at 10:52 PM
// Last Modified:   08/12/2015 at 11:59 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Xml;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    public class random : AimlTagHandler
    {
        public random(ChatEngine chatEngine, User user, SubQuery query, Request request, Result result, XmlNode templateNode)
            : base(chatEngine, user, query, request, result, templateNode)
        {
            isRecursive = false;
        }

        protected override string ProcessChange()
        {
            if (templateNode.Name.ToLower() == "random" && templateNode.HasChildNodes)
            {
                var list = new List<XmlNode>();
                foreach (XmlNode xmlNode in templateNode.ChildNodes)
                {
                    if (xmlNode.Name == "li")
                    {
                        list.Add(xmlNode);
                    }
                }
                if (list.Count > 0)
                {
                    var random = new Random();
                    return list[random.Next(list.Count)].InnerXml;
                }
            }
            return string.Empty;
        }
    }
}