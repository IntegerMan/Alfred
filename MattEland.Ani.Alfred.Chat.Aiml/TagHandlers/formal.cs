// ---------------------------------------------------------
// formal.cs
// 
// Created on:      08/12/2015 at 10:44 PM
// Last Modified:   08/12/2015 at 11:03 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System.Text;
using System.Xml;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    public class formal : AimlTagHandler
    {
        public formal(Bot bot, User user, SubQuery query, Request request, Result result, XmlNode templateNode)
            : base(bot, user, query, request, result, templateNode)
        {
        }

        protected override string ProcessChange()
        {
            if (!(templateNode.Name.ToLower() == "formal"))
            {
                return string.Empty;
            }
            var stringBuilder = new StringBuilder();
            if (templateNode.InnerText.Length > 0)
            {
                foreach (var str1 in templateNode.InnerText.ToLower().Split())
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