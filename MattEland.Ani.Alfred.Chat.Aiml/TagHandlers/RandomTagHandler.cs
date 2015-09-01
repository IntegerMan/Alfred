// ---------------------------------------------------------
// RandomTagHandler.cs
// 
// Created on:      08/12/2015 at 10:52 PM
// Last Modified:   08/18/2015 at 4:41 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Diagnostics;
using System.Linq;
using System.Xml;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;
using MattEland.Ani.Alfred.Core.Console;
using MattEland.Common;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    /// <summary>
    ///     A tag handler that handles the AIML random tag.
    /// </summary>
    /// <remarks>
    ///     See "http://www.alicebot.org/documentation/aiml-reference.html#random" for more information on
    ///     the random tag.
    /// </remarks>
    [HandlesAimlTag("random"), UsedImplicitly]
    public class RandomTagHandler : AimlTagHandler
    {
        [NotNull]
        private static readonly Random _randomizer = new Random();

        /// <summary>
        ///     Initializes a new instance of the <see cref="RandomTagHandler" /> class.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        [UsedImplicitly]
        public RandomTagHandler([NotNull] TagHandlerParameters parameters)
            : base(parameters)
        {
            IsRecursive = false;
        }

        /// <summary>
        ///     Processes the input text and returns the processed value.
        /// </summary>
        /// <returns>The processed output</returns>
        protected override string ProcessChange()
        {
            // Don't return anything if there aren't any children.
            if (!HasChildNodes)
            {
                return string.Empty;
            }

            //- Looking at the children...
            var childNodes = ChildNodes.Cast<XmlNode>();
            var list = childNodes.Where(xmlNode => xmlNode?.Name == "li").ToList();

            //- Ensure we have items
            if (list.Count <= 0)
            {
                return string.Empty;
            }

            // Grab a random element and return its contents
            var index = _randomizer.Next(list.Count);
            var node = list[index];
            Debug.Assert(node != null);

            try
            {
                return node.InnerXml.NonNull();

            }
            catch (XmlException xmlException)
            {
                Log("Problem getting node XML: " + xmlException.BuildDetailsMessage(), LogLevel.Error);

                return string.Empty;
            }
        }
    }
}