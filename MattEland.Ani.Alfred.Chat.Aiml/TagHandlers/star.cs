// ---------------------------------------------------------
// star.cs
// 
// Created on:      08/12/2015 at 10:56 PM
// Last Modified:   08/12/2015 at 11:03 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.Xml;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    public class star : AimlTagHandler
    {
        public star(Bot bot, User user, SubQuery query, Request request, Result result, XmlNode templateNode)
            : base(bot, user, query, request, result, templateNode)
        {
        }

        protected override string ProcessChange()
        {
            if (templateNode.Name.ToLower() == "star")
            {
                if (query.InputStar.Count > 0)
                {
                    if (templateNode.Attributes.Count == 0)
                    {
                        return query.InputStar[0];
                    }
                    if (templateNode.Attributes.Count == 1)
                    {
                        if (templateNode.Attributes[0].Name.ToLower() == "index")
                        {
                            try
                            {
                                var index = Convert.ToInt32(templateNode.Attributes[0].Value) - 1;
                                if (index >= 0 & index < query.InputStar.Count)
                                {
                                    return query.InputStar[index];
                                }
                                bot.writeToLog("InputStar out of bounds reference caused by input: " + request.rawInput);
                            }
                            catch
                            {
                                bot.writeToLog(
                                               "Index set to non-integer value whilst processing star tag in response to the input: " +
                                               request.rawInput);
                            }
                        }
                    }
                }
                else
                {
                    bot.writeToLog(
                                   "A star tag tried to reference an empty InputStar collection when processing the input: " +
                                   request.rawInput);
                }
            }
            return string.Empty;
        }
    }
}