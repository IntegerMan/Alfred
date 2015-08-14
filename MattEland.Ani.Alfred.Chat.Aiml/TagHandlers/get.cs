// ---------------------------------------------------------
// get.cs
// 
// Created on:      08/12/2015 at 10:45 PM
// Last Modified:   08/12/2015 at 11:59 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Xml;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    public class get : AimlTagHandler
    {
        public get([NotNull] TagHandlerParameters parameters)
            : base(parameters)
        {
        }

        protected override string ProcessChange()
        {
            if (TemplateNode.Name.ToLower() == "get" && ChatEngine.GlobalSettings.Count > 0 &&
                (TemplateNode.Attributes.Count == 1 && TemplateNode.Attributes[0].Name.ToLower() == "name"))
            {
                return User.Predicates.GetValue(TemplateNode.Attributes[0].Value);
            }
            return string.Empty;
        }
    }
}