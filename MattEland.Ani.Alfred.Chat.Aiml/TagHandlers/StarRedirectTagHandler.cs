// ---------------------------------------------------------
// sr.cs
// 
// Created on:      08/12/2015 at 10:54 PM
// Last Modified:   08/12/2015 at 11:59 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Xml;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;
using MattEland.Ani.Alfred.Core.Console;
using MattEland.Common;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    /// <summary>
    /// An AIML tag handler that handles the "sr" shortcut tag which is the equivalent of an srai to a star tag.
    /// </summary>
    [HandlesAimlTag("sr")]
    public class StarRedirectTagHandler : AimlTagHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AimlTagHandler" /> class.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        public StarRedirectTagHandler([NotNull] TagHandlerParameters parameters)
            : base(parameters)
        {
        }

        /// <summary>
        /// Processes the input text and returns the processed value.
        /// </summary>
        /// <returns>The processed output</returns>
        protected override string ProcessChange()
        {
            if (!TemplateNode.Name.Matches("sr"))
            {
                return string.Empty;
            }

            // Execute a star operation
            var star = BuildStarTagHandler();
            var starResult = star.Transform();

            // Execute a redirect to the result of the star operation
            try
            {
                var node = BuildNode(string.Format(Locale, "<srai>{0}</srai>", starResult).NonNull());
                var parameters = GetTagHandlerParametersForNode(node);
                return new RedirectTagHandler(parameters).Transform();

            }
            catch (XmlException xmlEx)
            {
                Log(string.Format(Locale, Resources.SrTagHandlerError, xmlEx.Message), LogLevel.Error);
            }

            return string.Empty;
        }
    }
}