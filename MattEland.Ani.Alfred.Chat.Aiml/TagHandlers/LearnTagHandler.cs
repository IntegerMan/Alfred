// ---------------------------------------------------------
// learn.cs
// 
// Created on:      08/12/2015 at 10:49 PM
// Last Modified:   08/12/2015 at 11:59 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.IO;
using System.Security;
using System.Xml;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;
using MattEland.Ani.Alfred.Core.Console;
using MattEland.Common;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    /// <summary>
    /// A tag handler for the AIML "learn" tag that causes the engine to learn new AIML values from a provided file path.
    /// </summary>
    [HandlesAimlTag("learn")]
    public class LearnTagHandler : AimlTagHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AimlTagHandler" /> class.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        public LearnTagHandler([NotNull] TagHandlerParameters parameters)
            : base(parameters)
        {
        }

        /// <summary>
        /// Processes the input text and returns the processed value.
        /// </summary>
        /// <returns>The processed output</returns>
        /// <exception cref="NotSupportedException">file is in an invalid format. </exception>
        protected override string ProcessChange()
        {
            if (TemplateNode.Name.Matches("learn") && TemplateNode.InnerText.HasText())
            {
                var innerText = TemplateNode.InnerText;
                try
                {
                    // This can cause some issues with IO, security, format, etc. catch handles all
                    var fileInfo = new FileInfo(innerText);

                    //- Ensure the file exists
                    if (fileInfo.Exists)
                    {
                        var newAiml = new XmlDocument();

                        // This can cause loads of XML related exceptions. They're dealt with in catch
                        newAiml.Load(innerText);

                        // We've got a document in a decent state - send it to the engine to learn
                        ChatEngine.LoadAimlFile(newAiml, innerText);

                        //- Log for diagnostics
                        Log(string.Format(Locale, "Learn tag invoked on file {0}", fileInfo.FullName), LogLevel.Info);
                    }

                }
                catch (SecurityException ex)
                {
                    Log(string.Format(Locale, Resources.LearnErrorSecurityException, innerText, ex.Message), LogLevel.Error);
                }
                catch (UnauthorizedAccessException ex)
                {
                    Log(string.Format(Locale, Resources.LearnErrorUnauthorizedException, innerText, ex.Message), LogLevel.Error);
                }
                catch (FileNotFoundException ex)
                {
                    Log(string.Format(Locale, Resources.LearnErrorFileNotFound, ex.FileName), LogLevel.Error);
                }
                catch (IOException ex)
                {
                    Log(string.Format(Locale, Resources.LearnErrorIOException, ex.GetType().Name, ex.Message, innerText), LogLevel.Error);
                }
                catch (XmlException ex)
                {
                    Log(string.Format(Locale, Resources.LearnErrorXmlException, ex.Message, innerText), LogLevel.Error);
                }
                catch (NotSupportedException)
                {
                    Log(string.Format(Locale, Resources.LearnErrorNotSupportedException, innerText), LogLevel.Error);
                }
            }

            // Learn never has any output
            return string.Empty;
        }
    }
}