// ---------------------------------------------------------
// condition.cs
// 
// Created on:      08/12/2015 at 10:42 PM
// Last Modified:   08/12/2015 at 11:59 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Text.RegularExpressions;
using System.Xml;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    public class condition : AimlTagHandler
    {
        public condition(ChatEngine chatEngine, User user, SubQuery query, Request request, Result result, XmlNode templateNode)
            : base(chatEngine, user, query, request, result, templateNode)
        {
            isRecursive = false;
        }

        protected override string ProcessChange()
        {
            if (templateNode.Name.ToLower() == "condition")
            {
                if (templateNode.Attributes.Count == 2)
                {
                    var name = "";
                    var str = "";
                    if (templateNode.Attributes[0].Name == "name")
                    {
                        name = templateNode.Attributes[0].Value;
                    }
                    else if (templateNode.Attributes[0].Name == "value")
                    {
                        str = templateNode.Attributes[0].Value;
                    }
                    if (templateNode.Attributes[1].Name == "name")
                    {
                        name = templateNode.Attributes[1].Value;
                    }
                    else if (templateNode.Attributes[1].Name == "value")
                    {
                        str = templateNode.Attributes[1].Value;
                    }
                    if (name.Length > 0 & str.Length > 0)
                    {
                        var input = user.Predicates.GetValue(name);
                        if (
                            new Regex(str.Replace(" ", "\\s").Replace("*", "[\\sA-Z0-9]+"), RegexOptions.IgnoreCase)
                                .IsMatch(input))
                        {
                            return templateNode.InnerXml;
                        }
                    }
                }
                else if (templateNode.Attributes.Count == 1)
                {
                    if (templateNode.Attributes[0].Name == "name")
                    {
                        var name = templateNode.Attributes[0].Value;
                        foreach (XmlNode xmlNode in templateNode.ChildNodes)
                        {
                            if (xmlNode.Name.ToLower() == "li")
                            {
                                if (xmlNode.Attributes.Count == 1)
                                {
                                    if (xmlNode.Attributes[0].Name.ToLower() == "value")
                                    {
                                        var input = user.Predicates.GetValue(name);
                                        if (
                                            new Regex(
                                                xmlNode.Attributes[0].Value.Replace(" ", "\\s")
                                                                     .Replace("*", "[\\sA-Z0-9]+"),
                                                RegexOptions.IgnoreCase).IsMatch(input))
                                        {
                                            return xmlNode.InnerXml;
                                        }
                                    }
                                }
                                else if (xmlNode.Attributes.Count == 0)
                                {
                                    return xmlNode.InnerXml;
                                }
                            }
                        }
                    }
                }
                else if (templateNode.Attributes.Count == 0)
                {
                    foreach (XmlNode xmlNode in templateNode.ChildNodes)
                    {
                        if (xmlNode.Name.ToLower() == "li")
                        {
                            if (xmlNode.Attributes.Count == 2)
                            {
                                var name = "";
                                var str = "";
                                if (xmlNode.Attributes[0].Name == "name")
                                {
                                    name = xmlNode.Attributes[0].Value;
                                }
                                else if (xmlNode.Attributes[0].Name == "value")
                                {
                                    str = xmlNode.Attributes[0].Value;
                                }
                                if (xmlNode.Attributes[1].Name == "name")
                                {
                                    name = xmlNode.Attributes[1].Value;
                                }
                                else if (xmlNode.Attributes[1].Name == "value")
                                {
                                    str = xmlNode.Attributes[1].Value;
                                }
                                if (name.Length > 0 & str.Length > 0)
                                {
                                    var input = user.Predicates.GetValue(name);
                                    if (
                                        new Regex(str.Replace(" ", "\\s").Replace("*", "[\\sA-Z0-9]+"),
                                                  RegexOptions.IgnoreCase).IsMatch(input))
                                    {
                                        return xmlNode.InnerXml;
                                    }
                                }
                            }
                            else if (xmlNode.Attributes.Count == 0)
                            {
                                return xmlNode.InnerXml;
                            }
                        }
                    }
                }
            }
            return string.Empty;
        }
    }
}