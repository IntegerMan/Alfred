// ---------------------------------------------------------
// LearnTagHandler.cs
// 
// Created on:      08/19/2015 at 9:31 PM
// Last Modified:   08/23/2015 at 4:15 PM
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
    ///     A tag handler for the AIML "learn" tag that causes the engine to learn new AIML values from a
    ///     provided file path.
    /// </summary>
    [HandlesAimlTag("learn")]
    public class LearnTagHandler : AimlTagHandler
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AimlTagHandler" /> class.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        public LearnTagHandler([NotNull] TagHandlerParameters parameters)
            : base(parameters)
        {
        }

        /// <summary>
        ///     Processes the input text and returns the processed value.
        /// </summary>
        /// <returns>The processed output</returns>
        protected override string ProcessChange()
        {
            if (TemplateNode.Name.Matches("learn") && TemplateNode.InnerText.HasText())
            {
                try
                {
                    LearnFromDocument(TemplateNode.InnerText);
                }
                catch (SecurityException ex)
                {
                    Log(string.Format(Locale,
                                      Resources.LearnErrorSecurityException.NonNull(),
                                      TemplateNode.InnerText,
                                      ex.Message),
                        LogLevel.Error);
                }
                catch (UnauthorizedAccessException ex)
                {
                    Log(string.Format(Locale,
                                      Resources.LearnErrorUnauthorizedException.NonNull(),
                                      TemplateNode.InnerText,
                                      ex.Message),
                        LogLevel.Error);
                }
                catch (FileNotFoundException ex)
                {
                    Log(string.Format(Locale,
                                      Resources.LearnErrorFileNotFound.NonNull(),
                                      ex.FileName),
                        LogLevel.Error);
                }
                catch (IOException ex)
                {
                    Log(string.Format(Locale,
                                      Resources.LearnErrorIOException.NonNull(),
                                      ex.GetType().Name,
                                      ex.Message,
                                      TemplateNode.InnerText),
                        LogLevel.Error);
                }
                catch (XmlException ex)
                {
                    Log(string.Format(Locale,
                                      Resources.LearnErrorXmlException.NonNull(),
                                      ex.Message,
                                      TemplateNode.InnerText),
                        LogLevel.Error);
                }
                catch (NotSupportedException)
                {
                    Log(string.Format(Locale,
                                      Resources.LearnErrorNotSupportedException.NonNull(),
                                      TemplateNode.InnerText),
                        LogLevel.Error);
                }
            }

            // Learn never has any output
            return string.Empty;
        }

        /// <summary>
        ///     Loads AIML contents from the specified path
        /// </summary>
        /// <param name="path">The path.</param>
        private void LearnFromDocument([NotNull] string path)
        {
            // This can cause some issues with IO, security, format, etc. catch handles all
            var fileInfo = new FileInfo(path);

            //- Ensure the file exists
            if (fileInfo.Exists)
            {
                //- Log for diagnostics
                Log(string.Format(Locale,
                                  "Learn tag invoked on file {0}",
                                  fileInfo.FullName),
                                  LogLevel.Info);

                // This can cause loads of XML related exceptions.
                var newAiml = new XmlDocument();
                newAiml.Load(path);

                // We've got a document in a decent state - send it to the engine to learn
                ChatEngine.LoadAimlFile(newAiml);
            }
            else
            {
                //- Log the failure
                Log(string.Format(Locale,
                                  "Tried to learn from {0} but could not find file.",
                                  fileInfo.FullName),
                                  LogLevel.Warning);
            }
        }
    }
}