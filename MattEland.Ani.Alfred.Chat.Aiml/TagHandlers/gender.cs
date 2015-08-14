// ---------------------------------------------------------
// gender.cs
// 
// Created on:      08/12/2015 at 10:45 PM
// Last Modified:   08/12/2015 at 11:59 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Xml;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml.Normalize;
using MattEland.Ani.Alfred.Chat.Aiml.Utils;
using MattEland.Common;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    public class gender : AimlTagHandler
    {
        public gender([NotNull] TagHandlerParameters parameters)
            : base(parameters)
        {
        }

        protected override string ProcessChange()
        {
            if (TemplateNode.Name.ToLower() != "gender")
            {
                return string.Empty;
            }
            if (TemplateNode.InnerText.Length > 0)
            {
                return TextSubstitutionTransformer.Substitute(ChatEngine.GenderSubstitutions, TemplateNode.InnerText);
            }

            var node = GetNode("<star/>");
            var parameters = GetTagHandlerParametersForNode(node);

            TemplateNode.InnerText = new star(parameters).Transform().NonNull();

            if (TemplateNode.InnerText.IsNullOrEmpty())
            {
                return string.Empty;
            }
            return ProcessChange();
        }
    }
}