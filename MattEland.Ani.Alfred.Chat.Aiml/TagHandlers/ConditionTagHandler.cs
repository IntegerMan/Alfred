// ---------------------------------------------------------
// ConditionTagHandler.cs
// 
// Created on:      08/12/2015 at 10:42 PM
// Last Modified:   08/14/2015 at 2:35 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;
using MattEland.Common;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    /// <summary>
    /// This is an AimlTagHandler for the "condition" tag that is used for variable and branch evaluation
    /// </summary>
    [HandlesAimlTag("ConditionTagHandler")]
    public class ConditionTagHandler : AimlTagHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionTagHandler" /> class.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        public ConditionTagHandler([NotNull] TagHandlerParameters parameters)
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
            //- Work with an node instead of a node.
            var element = TemplateElement;
            if (element == null)
            {
                return null;
            }

            //- Ensure we're working with the right node type
            if (!element.Name.Matches("ConditionTagHandler"))
            {
                return string.Empty;
            }

            /* There are three different scenarios depending on what attributes are present on the ConditionTagHandler.
            This will work by checking the node's name and value attributes */

            // In this case no name was provided on the ConditionTagHandler so the children will provide the rules
            if (!element.HasAttribute("name"))
            {
                return EvaluateLooseConditionWithChildNodes(element);
            }

            // Grab the attribute values
            var name = element.GetAttribute("name");
            var variableName = User.Predicates.GetValue(name).NonNull();

            // Handle a simple case with a single node with name and value attributes
            if (element.HasAttribute("value"))
            {
                return EvaluateSimpleConditionNode(element, name, variableName);
            }

            // In this case we have an node with a name, but the child li elements have the values
            return EvaluateConditionWithNameAndChildNodes(element, variableName);
        }

        /// <summary>
        /// Handles the scenario where you have a ConditionTagHandler node without name or value. In that case, the first
        /// matching child node is used and many different conditions can be evaluated.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>System.String.</returns>
        private string EvaluateLooseConditionWithChildNodes([NotNull] XmlNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            foreach (XmlElement xmlNode in node.ChildNodes)
            {
                //- Ensure we're looking at valid li's
                if (xmlNode == null || !xmlNode.Name.Matches("li"))
                {
                    continue;
                }

                // If it's an li with no name and value, this is our default option and it should be taken
                if (!xmlNode.HasAttribute("name") || !xmlNode.HasAttribute("value"))
                {
                    return xmlNode.InnerXml;
                }

                //- Grab our name / value
                var name = xmlNode.GetAttribute("name").NonNull();
                var value = xmlNode.GetAttribute("value").NonNull();

                // Check to see if we match the ConditionTagHandler. If we do, use that value
                var input = User.Predicates.GetValue(name).NonNull();
                if (IsRegexMatch(input, value))
                {
                    return xmlNode.InnerXml;
                }
            }

            //- If we got here, no matches were found
            return string.Empty;
        }

        /// <summary>
        /// Evaluates the ConditionTagHandler with name and child nodes.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="variableName">The variable name.</param>
        /// <returns>System.String.</returns>
        private static string EvaluateConditionWithNameAndChildNodes([NotNull] XmlNode node, string variableName)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            // Select XmlElements from the children for each "li" choice
            foreach (var child in from XmlElement xmlNode in node.ChildNodes
                                  where xmlNode != null
                                  where xmlNode.Name.Matches("li")
                                  select xmlNode)
            {
                //- Sanity check
                Debug.Assert(child?.Attributes != null);

                // If it has a value node, we'll compare it to our variableName value
                if (child.HasAttribute("value"))
                {
                    var childValue = child.GetAttribute("value");

                    // Is this the node we're looking for?
                    if (IsRegexMatch(variableName, childValue))
                    {
                        return child.InnerXml;
                    }
                }
                else
                {
                    // It's just a blank li with no value; let's take it as a default / fallback.
                    return child.InnerXml;
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Evaluates a simple ConditionTagHandler node for a ConditionTagHandler tag with both a name and a value.
        /// </summary>
        /// <param name="element">The node.</param>
        /// <param name="name">The name.</param>
        /// <param name="variableName">The variable name.</param>
        /// <returns>The results of the ConditionTagHandler or string.Empty if the ConditionTagHandler failed</returns>
        private static string EvaluateSimpleConditionNode([NotNull] XmlElement element, string name, string variableName)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            // Grab our ConditionTagHandler's value
            var value = element.GetAttribute("value");

            //- Early exit for no value
            if (name.IsNullOrWhitespace() || value.IsNullOrWhitespace())
            {
                return string.Empty;
            }

            // Build and evaluate a regex to see if our value matches the value
            var isMatch = IsRegexMatch(variableName, value);

            // If the match succeeded, use the contents
            return isMatch ? element.InnerXml : string.Empty;
        }

        /// <summary>
        /// Determines whether the two values match according to a test regex.
        /// </summary>
        /// <param name="expected">The variableName.</param>
        /// <param name="value">The value.</param>
        /// <returns>True if the regex matched otherwise false.</returns>
        private static bool IsRegexMatch(string expected, string value)
        {
            var regex = new Regex(value.NonNull().Replace(" ", "\\s").Replace("*", "[\\sA-Z0-9]+"), RegexOptions.IgnoreCase);

            return regex.IsMatch(expected.NonNull());
        }

        /// <summary>
        /// Builds a regular expression for validation purposes.
        /// </summary>
        /// <param name="input">The input value.</param>
        /// <returns>A regular expression</returns>
        [NotNull]
        private static Regex BuildValidationRegex(string input)
        {
            const RegexOptions Options = RegexOptions.IgnoreCase;

            var regex = new Regex(input.NonNull().Replace(" ", @"\s").Replace("*", @"[\sA-Z0-9]+"),
                                  Options);
            return regex;
        }
    }
}