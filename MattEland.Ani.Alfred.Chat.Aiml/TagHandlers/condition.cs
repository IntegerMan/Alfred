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

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    public class condition : AimlTagHandler
    {
        public condition([NotNull] TagHandlerParameters parameters)
            : base(parameters)
        {
            IsRecursive = false;
        }

        protected override string ProcessChange()
        {
            if (TemplateNode.Name.ToLower() == "condition")
            {
                if (TemplateNode.Attributes.Count == 2)
                {
                    var name = "";
                    var str = "";
                    if (TemplateNode.Attributes[0].Name == "name")
                    {
                        name = TemplateNode.Attributes[0].Value;
                    }
                    else if (TemplateNode.Attributes[0].Name == "value")
                    {
                        str = TemplateNode.Attributes[0].Value;
                    }
                    if (TemplateNode.Attributes[1].Name == "name")
                    {
                        name = TemplateNode.Attributes[1].Value;
                    }
                    else if (TemplateNode.Attributes[1].Name == "value")
                    {
                        str = TemplateNode.Attributes[1].Value;
                    }
                    if (name.Length > 0 & str.Length > 0)
                    {
                        var input = User.Predicates.GetValue(name);
                        if (
                            new Regex(str.Replace(" ", "\\s").Replace("*", "[\\sA-Z0-9]+"), RegexOptions.IgnoreCase)
                                .IsMatch(input))
                        {
                            return TemplateNode.InnerXml;
                        }
                    }
                }
                else if (TemplateNode.Attributes.Count == 1)
                {
                    if (TemplateNode.Attributes[0].Name == "name")
                    {
                        var name = TemplateNode.Attributes[0].Value;
                        foreach (XmlNode xmlNode in TemplateNode.ChildNodes)
                        {
                            if (xmlNode.Name.ToLower() == "li")
                            {
                                if (xmlNode.Attributes.Count == 1)
                                {
                                    if (xmlNode.Attributes[0].Name.ToLower() == "value")
                                    {
                                        var input = User.Predicates.GetValue(name);
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
                else if (TemplateNode.Attributes.Count == 0)
                {
                    foreach (XmlNode xmlNode in TemplateNode.ChildNodes)
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
                                    var input = User.Predicates.GetValue(name);
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