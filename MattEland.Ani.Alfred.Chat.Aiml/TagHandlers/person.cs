// ---------------------------------------------------------
// person.cs
// 
// Created on:      08/12/2015 at 10:50 PM
// Last Modified:   08/12/2015 at 11:59 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Xml;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml.Normalize;
using MattEland.Ani.Alfred.Chat.Aiml.Utils;
using MattEland.Common;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    public class person : AimlTagHandler
    {
        public person([NotNull] TagHandlerParameters parameters)
            : base(parameters)
        {
        }

        protected override string ProcessChange()
        {
            if (!(TemplateNode.Name.ToLower() == "person"))
            {
                return string.Empty;
            }
            if (TemplateNode.InnerText.Length > 0)
            {
                return TextSubstitutionTransformer.Substitute(ChatEngine.PersonSubstitutions, TemplateNode.InnerText);
            }

            var templateNode = GetNode("<star/>");
            var parameters = GetTagHandlerParametersForNode(templateNode);
            var star = new star(parameters);
            TemplateNode.InnerText = star.Transform().NonNull();
            if (TemplateNode.InnerText.Length > 0)
            {
                return ProcessChange();
            }
            return string.Empty;
        }

    }
}