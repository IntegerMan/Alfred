// ---------------------------------------------------------
// thatstar.cs
// 
// Created on:      08/12/2015 at 10:58 PM
// Last Modified:   08/12/2015 at 11:59 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Xml;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    public class thatstar : AimlTagHandler
    {
        public thatstar(ChatEngine chatEngine, User user, SubQuery query, Request request, Result result, XmlNode templateNode)
            : base(chatEngine, user, query, request, result, templateNode)
        {
        }

        protected override string ProcessChange()
        {
            if (TemplateNode.Name.ToLower() == "thatstar")
            {
                if (TemplateNode.Attributes.Count == 0)
                {
                    if (Query.ThatStar.Count > 0)
                    {
                        return Query.ThatStar[0];
                    }
                    ChatEngine.writeToLog(
                                   "ERROR! An out of bounds index to thatstar was encountered when processing the input: " +
                                   Request.RawInput);
                }
                else if (TemplateNode.Attributes.Count == 1 && TemplateNode.Attributes[0].Name.ToLower() == "index")
                {
                    if (TemplateNode.Attributes[0].Value.Length > 0)
                    {
                        try
                        {
                            var num = Convert.ToInt32(TemplateNode.Attributes[0].Value.Trim());
                            if (Query.ThatStar.Count > 0)
                            {
                                if (num > 0)
                                {
                                    return Query.ThatStar[num - 1];
                                }
                                ChatEngine.writeToLog("ERROR! An input tag with a bady formed index (" +
                                               TemplateNode.Attributes[0].Value +
                                               ") was encountered processing the input: " + Request.RawInput);
                            }
                            else
                            {
                                ChatEngine.writeToLog(
                                               "ERROR! An out of bounds index to thatstar was encountered when processing the input: " +
                                               Request.RawInput);
                            }
                        }
                        catch
                        {
                            ChatEngine.writeToLog("ERROR! A thatstar tag with a bady formed index (" +
                                           TemplateNode.Attributes[0].Value + ") was encountered processing the input: " +
                                           Request.RawInput);
                        }
                    }
                }
            }
            return string.Empty;
        }
    }
}