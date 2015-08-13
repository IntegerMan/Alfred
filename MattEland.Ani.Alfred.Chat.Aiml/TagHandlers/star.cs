// ---------------------------------------------------------
// star.cs
// 
// Created on:      08/12/2015 at 10:56 PM
// Last Modified:   08/12/2015 at 11:59 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Xml;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    public class star : AimlTagHandler
    {
        public star(ChatEngine chatEngine, User user, SubQuery query, Request request, Result result, XmlNode templateNode)
            : base(chatEngine, user, query, request, result, templateNode)
        {
        }

        protected override string ProcessChange()
        {
            if (TemplateNode.Name.ToLower() == "star")
            {
                if (Query.InputStar.Count > 0)
                {
                    if (TemplateNode.Attributes.Count == 0)
                    {
                        return Query.InputStar[0];
                    }
                    if (TemplateNode.Attributes.Count == 1)
                    {
                        if (TemplateNode.Attributes[0].Name.ToLower() == "index")
                        {
                            try
                            {
                                var index = Convert.ToInt32(TemplateNode.Attributes[0].Value) - 1;
                                if (index >= 0 & index < Query.InputStar.Count)
                                {
                                    return Query.InputStar[index];
                                }
                                ChatEngine.writeToLog("InputStar out of bounds reference caused by input: " + Request.RawInput);
                            }
                            catch
                            {
                                ChatEngine.writeToLog(
                                               "Index set to non-integer value whilst processing star tag in response to the input: " +
                                               Request.RawInput);
                            }
                        }
                    }
                }
                else
                {
                    ChatEngine.writeToLog(
                                   "A star tag tried to reference an empty InputStar collection when processing the input: " +
                                   Request.RawInput);
                }
            }
            return string.Empty;
        }
    }
}