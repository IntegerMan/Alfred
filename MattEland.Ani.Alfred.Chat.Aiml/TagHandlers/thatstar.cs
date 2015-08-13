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
            if (templateNode.Name.ToLower() == "thatstar")
            {
                if (templateNode.Attributes.Count == 0)
                {
                    if (query.ThatStar.Count > 0)
                    {
                        return query.ThatStar[0];
                    }
                    ChatEngine.writeToLog(
                                   "ERROR! An out of bounds index to thatstar was encountered when processing the input: " +
                                   request.RawInput);
                }
                else if (templateNode.Attributes.Count == 1 && templateNode.Attributes[0].Name.ToLower() == "index")
                {
                    if (templateNode.Attributes[0].Value.Length > 0)
                    {
                        try
                        {
                            var num = Convert.ToInt32(templateNode.Attributes[0].Value.Trim());
                            if (query.ThatStar.Count > 0)
                            {
                                if (num > 0)
                                {
                                    return query.ThatStar[num - 1];
                                }
                                ChatEngine.writeToLog("ERROR! An input tag with a bady formed index (" +
                                               templateNode.Attributes[0].Value +
                                               ") was encountered processing the input: " + request.RawInput);
                            }
                            else
                            {
                                ChatEngine.writeToLog(
                                               "ERROR! An out of bounds index to thatstar was encountered when processing the input: " +
                                               request.RawInput);
                            }
                        }
                        catch
                        {
                            ChatEngine.writeToLog("ERROR! A thatstar tag with a bady formed index (" +
                                           templateNode.Attributes[0].Value + ") was encountered processing the input: " +
                                           request.RawInput);
                        }
                    }
                }
            }
            return string.Empty;
        }
    }
}