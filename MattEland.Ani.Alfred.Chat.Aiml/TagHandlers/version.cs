// ---------------------------------------------------------
// version.cs
// 
// Created on:      08/12/2015 at 11:01 PM
// Last Modified:   08/12/2015 at 11:59 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Xml;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    public class version : AimlTagHandler
    {
        public version([NotNull] TagHandlerParameters parameters)
            : base(parameters)
        {
        }

        protected override string ProcessChange()
        {
            if (TemplateNode.Name.ToLower() == "version")
            {
                return ChatEngine.GlobalSettings.GetValue("version");
            }

            return string.Empty;
        }
    }
}