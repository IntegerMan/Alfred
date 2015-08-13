// ---------------------------------------------------------
// sentence.cs
// 
// Created on:      08/12/2015 at 10:52 PM
// Last Modified:   08/12/2015 at 11:59 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    public class sentence : AimlTagHandler
    {
        public sentence(ChatEngine chatEngine, User user, SubQuery query, Request request, Result result, XmlNode templateNode)
            : base(chatEngine, user, query, request, result, templateNode)
        {
        }

        protected override string ProcessChange()
        {
            if (!(templateNode.Name.ToLower() == "sentence"))
            {
                return string.Empty;
            }
            if (templateNode.InnerText.Length > 0)
            {
                var stringBuilder = new StringBuilder();
                var chArray = templateNode.InnerText.Trim().ToCharArray();
                var flag = true;
                for (var index = 0; index < chArray.Length; ++index)
                {
                    var input = Convert.ToString(chArray[index]);
                    if (ChatEngine.Splitters.Contains(input))
                    {
                        flag = true;
                    }
                    if (new Regex("[a-zA-Z]").IsMatch(input))
                    {
                        if (flag)
                        {
                            stringBuilder.Append(input.ToUpper(Locale));
                            flag = false;
                        }
                        else
                        {
                            stringBuilder.Append(input.ToLower(Locale));
                        }
                    }
                    else
                    {
                        stringBuilder.Append(input);
                    }
                }
                return stringBuilder.ToString();
            }
            templateNode.InnerText = new star(ChatEngine, user, query, request, result, getNode("<star/>")).Transform();
            if (templateNode.InnerText.Length > 0)
            {
                return ProcessChange();
            }
            return string.Empty;
        }
    }
}