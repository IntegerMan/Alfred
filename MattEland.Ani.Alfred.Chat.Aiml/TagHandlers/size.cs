// ---------------------------------------------------------
// size.cs
// 
// Created on:      08/12/2015 at 10:54 PM
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
    public class size : AimlTagHandler
    {
        public size([NotNull] TagHandlerParameters parameters)
            : base(parameters)
        {
        }

        protected override string ProcessChange()
        {
            if (TemplateNode.Name.ToLower() == "size")
            {
                return Convert.ToString(ChatEngine.Size);
            }
            return string.Empty;
        }
    }
}