// ---------------------------------------------------------
// input.cs
// 
// Created on:      08/12/2015 at 10:47 PM
// Last Modified:   08/12/2015 at 11:59 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Xml;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;
using MattEland.Ani.Alfred.Core.Console;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    public class input : AimlTagHandler
    {
        public input(ChatEngine chatEngine, User user, SubQuery query, Request request, Result result, XmlNode templateNode)
            : base(chatEngine, user, query, request, result, templateNode)
        {
        }

        protected override string ProcessChange()
        {
            if (TemplateNode.Name.ToLower() == "input")
            {
                if (TemplateNode.Attributes.Count == 0)
                {
                    return User.GetInputSentence();
                }
                if (TemplateNode.Attributes.Count == 1 && TemplateNode.Attributes[0].Name.ToLower() == "index")
                {
                    if (TemplateNode.Attributes[0].Value.Length > 0)
                    {
                        try
                        {
                            var strArray = TemplateNode.Attributes[0].Value.Split(",".ToCharArray());
                            if (strArray.Length == 2)
                            {
                                var num1 = Convert.ToInt32(strArray[0].Trim());
                                var num2 = Convert.ToInt32(strArray[1].Trim());
                                if (num1 > 0 & num2 > 0)
                                {
                                    return User.GetInputSentence(num1 - 1, num2 - 1);
                                }
                                Log("An input tag with a bady formed index (" +
                                               TemplateNode.Attributes[0].Value +
                                               ") was encountered processing the input: " + Request.RawInput, LogLevel.Error);
                            }
                            else
                            {
                                var num = Convert.ToInt32(TemplateNode.Attributes[0].Value.Trim());
                                if (num > 0)
                                {
                                    return User.GetInputSentence(num - 1);
                                }
                                Log("An input tag with a bady formed index (" +
                                               TemplateNode.Attributes[0].Value +
                                               ") was encountered processing the input: " + Request.RawInput, LogLevel.Error);
                            }
                        }
                        catch
                        {
                            Log("An input tag with a bady formed index (" +
                                           TemplateNode.Attributes[0].Value + ") was encountered processing the input: " +
                                           Request.RawInput, LogLevel.Error);
                        }
                    }
                }
            }
            return string.Empty;
        }
    }
}