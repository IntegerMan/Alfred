// ---------------------------------------------------------
// learn.cs
// 
// Created on:      08/12/2015 at 10:49 PM
// Last Modified:   08/12/2015 at 11:59 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.IO;
using System.Xml;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;
using MattEland.Ani.Alfred.Core.Console;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    public class learn : AimlTagHandler
    {
        public learn([NotNull] TagHandlerParameters parameters)
            : base(parameters)
        {
        }

        protected override string ProcessChange()
        {
            if (TemplateNode.Name.ToLower() == "learn" && TemplateNode.InnerText.Length > 0)
            {
                var innerText = TemplateNode.InnerText;
                if (new FileInfo(innerText).Exists)
                {
                    var newAIML = new XmlDocument();
                    try
                    {
                        newAIML.Load(innerText);
                        ChatEngine.LoadAIMLFromXML(newAIML, innerText);
                    }
                    catch
                    {
                        Log("Attempted (but failed) to <learn> some new AIML from the following URI: " +
                                       innerText, LogLevel.Error);
                    }
                }
            }
            return string.Empty;
        }
    }
}