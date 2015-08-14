// ---------------------------------------------------------
// person2.cs
// 
// Created on:      08/12/2015 at 10:51 PM
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
    public class person2 : AimlTagHandler
    {
        public person2([NotNull] TagHandlerParameters parameters)
            : base(parameters)
        {
        }

        protected override string ProcessChange()
        {
            if (TemplateNode.Name.ToLower() != "person2")
            {
                return string.Empty;
            }
            if (TemplateNode.InnerText.Length > 0)
            {
                return TextSubstitutionTransformer.Substitute(ChatEngine.Person2Substitutions, TemplateNode.InnerText);
            }

            var node = GetNode("<star/>");
            var parameters = GetTagHandlerParametersForNode(node);
            TemplateNode.InnerText = new star(parameters).Transform().NonNull();

            if (TemplateNode.InnerText.Length > 0)
            {
                return ProcessChange();
            }
            return string.Empty;
        }
    }
}