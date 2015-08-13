// ---------------------------------------------------------
// gender.cs
// 
// Created on:      08/12/2015 at 10:45 PM
// Last Modified:   08/12/2015 at 11:59 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Xml;

using MattEland.Ani.Alfred.Chat.Aiml.Normalize;
using MattEland.Ani.Alfred.Chat.Aiml.Utils;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    public class gender : AimlTagHandler
    {
        public gender(ChatEngine chatEngine, User user, SubQuery query, Request request, Result result, XmlNode templateNode)
            : base(chatEngine, user, query, request, result, templateNode)
        {
        }

        protected override string ProcessChange()
        {
            if (!(TemplateNode.Name.ToLower() == "gender"))
            {
                return string.Empty;
            }
            if (TemplateNode.InnerText.Length > 0)
            {
                return TextSubstitutionTransformer.Substitute(ChatEngine.GenderSubstitutions, TemplateNode.InnerText);
            }
            TemplateNode.InnerText =
                new star(ChatEngine, User, Query, Request, Result, GetNode("<star/>")).Transform();
            if (TemplateNode.InnerText.Length > 0)
            {
                return ProcessChange();
            }
            return string.Empty;
        }
    }
}