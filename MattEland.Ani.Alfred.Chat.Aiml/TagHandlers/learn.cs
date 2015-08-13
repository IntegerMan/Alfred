// ---------------------------------------------------------
// learn.cs
// 
// Created on:      08/12/2015 at 10:49 PM
// Last Modified:   08/12/2015 at 11:59 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.IO;
using System.Xml;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    public class learn : AimlTagHandler
    {
        public learn(Bot bot, User user, SubQuery query, Request request, Result result, XmlNode templateNode)
            : base(bot, user, query, request, result, templateNode)
        {
        }

        protected override string ProcessChange()
        {
            if (templateNode.Name.ToLower() == "learn" && templateNode.InnerText.Length > 0)
            {
                var innerText = templateNode.InnerText;
                if (new FileInfo(innerText).Exists)
                {
                    var newAIML = new XmlDocument();
                    try
                    {
                        newAIML.Load(innerText);
                        Bot.loadAIMLFromXML(newAIML, innerText);
                    }
                    catch
                    {
                        Bot.writeToLog(
                                       "ERROR! Attempted (but failed) to <learn> some new AIML from the following URI: " +
                                       innerText);
                    }
                }
            }
            return string.Empty;
        }
    }
}