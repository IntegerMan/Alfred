// ---------------------------------------------------------
// sentence.cs
// 
// Created on:      08/12/2015 at 10:52 PM
// Last Modified:   08/12/2015 at 11:59 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;
using MattEland.Common;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    public class sentence : AimlTagHandler
    {
        public sentence([NotNull] TagHandlerParameters parameters)
            : base(parameters)
        {
        }

        protected override string ProcessChange()
        {
            if (TemplateNode.Name.ToLower() != "sentence")
            {
                return string.Empty;
            }
            if (TemplateNode.InnerText.Length > 0)
            {
                var stringBuilder = new StringBuilder();
                var chArray = TemplateNode.InnerText.Trim().ToCharArray();
                var flag = true;
                for (var index = 0; index < chArray.Length; ++index)
                {
                    var input = Convert.ToString(chArray[index]);
                    if (ChatEngine.Splitters.Contains(input))
                    {
                        flag = true;
                    }
                    if (new Regex("[a-zA-Z]").IsMatch(input))
                    {
                        if (flag)
                        {
                            stringBuilder.Append(input.ToUpper(Locale));
                            flag = false;
                        }
                        else
                        {
                            stringBuilder.Append(input.ToLower(Locale));
                        }
                    }
                    else
                    {
                        stringBuilder.Append(input);
                    }
                }
                return stringBuilder.ToString();
            }
            var node = GetNode("<star/>");
            var parameters = GetTagHandlerParametersForNode(node);
            TemplateNode.InnerText = new star(parameters).Transform().NonNull();

            if (TemplateNode.InnerText.IsEmpty())
            {
                return string.Empty;
            }
            return ProcessChange();
        }
    }
}