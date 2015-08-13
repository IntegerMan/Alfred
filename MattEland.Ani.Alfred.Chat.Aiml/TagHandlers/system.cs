// ---------------------------------------------------------
// system.cs
// 
// Created on:      08/12/2015 at 10:57 PM
// Last Modified:   08/12/2015 at 11:59 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Xml;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    public class system : AimlTagHandler
    {
        public system(ChatEngine chatEngine, User user, SubQuery query, Request request, Result result, XmlNode templateNode)
            : base(chatEngine, user, query, request, result, templateNode)
        {
        }

        protected override string ProcessChange()
        {
            ChatEngine.writeToLog("The system tag is not implemented in this ChatEngine");
            return string.Empty;
        }
    }
}