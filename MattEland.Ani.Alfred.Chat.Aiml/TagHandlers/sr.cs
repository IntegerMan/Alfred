// ---------------------------------------------------------
// sr.cs
// 
// Created on:      08/12/2015 at 10:54 PM
// Last Modified:   08/12/2015 at 11:59 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Xml;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    public class sr : AimlTagHandler
    {
        public sr([NotNull] TagHandlerParameters parameters)
            : base(parameters)
        {
        }

        protected override string ProcessChange()
        {
            if (TemplateNode.Name.ToLower() == "sr")
            {
                var node = GetNode("<star/>");
                var parameters = GetTagHandlerParametersForNode(node);
                var starResult = new star(parameters).Transform();

                node = GetNode($"<srai>{starResult}</srai>");
                parameters = GetTagHandlerParametersForNode(node);
                return new RedirectTagHandler(parameters).Transform();
            }

            return string.Empty;
        }
    }
}