// ---------------------------------------------------------
// date.cs
// 
// Created on:      08/12/2015 at 10:43 PM
// Last Modified:   08/12/2015 at 11:59 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Xml;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    public class date : AimlTagHandler
    {
        public date([NotNull] TagHandlerParameters parameters)
            : base(parameters)
        {
        }

        protected override string ProcessChange()
        {
            if (TemplateNode.Name.ToLower() == "date")
            {
                return DateTime.Now.ToString(ChatEngine.Locale);
            }
            return string.Empty;
        }
    }
}