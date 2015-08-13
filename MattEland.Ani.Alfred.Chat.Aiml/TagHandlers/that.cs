// ---------------------------------------------------------
// that.cs
// 
// Created on:      08/12/2015 at 10:57 PM
// Last Modified:   08/12/2015 at 11:59 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Xml;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    public class that : AimlTagHandler
    {
        public that(ChatEngine chatEngine, User user, SubQuery query, Request request, Result result, XmlNode templateNode)
            : base(chatEngine, user, query, request, result, templateNode)
        {
        }

        protected override string ProcessChange()
        {
            if (templateNode.Name.ToLower() == "that")
            {
                if (templateNode.Attributes.Count == 0)
                {
                    return user.getThat();
                }
                if (templateNode.Attributes.Count == 1 && templateNode.Attributes[0].Name.ToLower() == "index")
                {
                    if (templateNode.Attributes[0].Value.Length > 0)
                    {
                        try
                        {
                            var strArray = templateNode.Attributes[0].Value.Split(",".ToCharArray());
                            if (strArray.Length == 2)
                            {
                                var num1 = Convert.ToInt32(strArray[0].Trim());
                                var num2 = Convert.ToInt32(strArray[1].Trim());
                                if (num1 > 0 & num2 > 0)
                                {
                                    return user.getThat(num1 - 1, num2 - 1);
                                }
                                ChatEngine.writeToLog("ERROR! An input tag with a badly formed index (" +
                                               templateNode.Attributes[0].Value +
                                               ") was encountered processing the input: " + request.RawInput);
                            }
                            else
                            {
                                var num = Convert.ToInt32(templateNode.Attributes[0].Value.Trim());
                                if (num > 0)
                                {
                                    return user.getThat(num - 1);
                                }
                                ChatEngine.writeToLog("ERROR! An input tag with a badly formed index (" +
                                               templateNode.Attributes[0].Value +
                                               ") was encountered processing the input: " + request.RawInput);
                            }
                        }
                        catch
                        {
                            ChatEngine.writeToLog("ERROR! An input tag with a bady formed index (" +
                                           templateNode.Attributes[0].Value + ") was encountered processing the input: " +
                                           request.RawInput);
                        }
                    }
                }
            }
            return string.Empty;
        }
    }
}