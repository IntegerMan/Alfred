// ---------------------------------------------------------
// javascript.cs
// 
// Created on:      08/12/2015 at 10:48 PM
// Last Modified:   08/12/2015 at 11:59 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Xml;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;
using MattEland.Ani.Alfred.Core.Console;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    public class javascript : AimlTagHandler
    {
        public javascript(ChatEngine chatEngine, User user, SubQuery query, Request request, Result result, XmlNode templateNode)
            : base(chatEngine, user, query, request, result, templateNode)
        {
        }

        protected override string ProcessChange()
        {
            Log("The javascript tag is not implemented in this ChatEngine", LogLevel.Warning);
            return string.Empty;
        }
    }
}