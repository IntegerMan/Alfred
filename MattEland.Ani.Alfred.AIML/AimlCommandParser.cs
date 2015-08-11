// ---------------------------------------------------------
// AimlCommandParser.cs
// 
// Created on:      08/11/2015 at 6:23 PM
// Last Modified:   08/11/2015 at 6:27 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Xml;
using System.Xml.Linq;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.Core.Definitions;

namespace MattEland.Ani.Alfred.Chat
{
    /// <summary>
    ///     A utility class for parsing AIML templates and extracting commands
    /// </summary>
    /// <remarks>
    ///     These methods are intended to aid interpreting a string representation of an template and translating
    ///     it into a ChatCommand. This looks for &lt;oob&gt; tags in the template and an &lt;alfred&gt; tag inside of that.
    ///     The alfred tag should have a subsystem, command, and optionally a data attribute.
    /// </remarks>
    internal static class AimlCommandParser
    {
        /// <summary>
        ///     Gets the first command present from the template and returns that node in XML format.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <param name="console">The console.</param>
        /// <returns>The chat command or null if there was no command</returns>
        internal static ChatCommand GetCommandFromTemplate([CanBeNull] string template, [CanBeNull] IConsole console)
        {
            // Do a bunch of trimming and interpreting to find our XML
            var commandXml = GetCommandXmlFromTemplate(template, console);
            if (commandXml == null)
            {
                return ChatCommand.Empty;
            }

            // Load the XML as a document so we can manipulate the elements easier
            var commandNode = GetCommandNodeXElement(commandXml, console);
            if (commandNode == null)
            {
                return ChatCommand.Empty;
            }

            var subsystem = commandNode.Attribute("subsystem")?.Value;
            var command = commandNode.Attribute("command")?.Value;
            var data = commandNode.Attribute("data")?.Value;

            console?.Log(Resources.ChatOutputHeader, "Received OOB Command: " + command, LogLevel.Info);

            return new ChatCommand(subsystem, command, data);
        }

        /// <summary>
        ///     Gets the command node Xml Element.
        /// </summary>
        /// <param name="commandXml">The command XML.</param>
        /// <param name="console">The console.</param>
        /// <returns>An XElement representing a command.</returns>
        private static XElement GetCommandNodeXElement([NotNull] string commandXml, [CanBeNull] IConsole console)
        {

            XDocument xdoc;
            try
            {
                xdoc = XDocument.Parse(commandXml);
            }
            catch (XmlException ex)
            {
                var message = string.Format(CultureInfo.CurrentCulture,
                                            Resources.ErrorParsingCommand,
                                            ex.Message,
                                            commandXml);

                console?.Log(Resources.ChatOutputHeader,
                             message,
                             LogLevel.Error);

                return null;
            }

            // Grab the OOB root tag out of the document
            var oobElement = xdoc.Root;
            if (oobElement == null)
            {
                console?.Log(Resources.ChatOutputHeader, "OOB command had no root element", LogLevel.Error);

                return null;
            }

            // Return either the XML of the first node or the value of an text value
            var xElement = oobElement.FirstNode as XElement;

            // Validate our element
            if (xElement == null || xElement.Name.LocalName?.ToLowerInvariant() != "alfred")
            {
                console?.Log(Resources.ChatOutputHeader, "OOB root element was not an alfred XElement", LogLevel.Error);
                return null;
            }

            return xElement;
        }

        /// <summary>
        ///     Gets the command XML from a template.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <param name="console">The console.</param>
        /// <returns>The XML without noise of periphery characters</returns>
        [SuppressMessage("ReSharper", "SuggestVarOrType_BuiltInTypes")]
        [CanBeNull]
        private static string GetCommandXmlFromTemplate([CanBeNull] string template, [CanBeNull] IConsole console)
        {
            // Early exit if it's empty
            if (template == null)
            {
                return null;
            }

            // Set up constants
            const StringComparison ComparisonType = StringComparison.OrdinalIgnoreCase;
            const string StartTag = "<oob";
            const string EndTag = "</oob>";
            const string SelfClosingTagEnd = "/>";

            // Figure out where we start
            var start = template.IndexOf(StartTag, ComparisonType);

            // If we don't have a tag, there's no command and that's fine
            if (start < 0)
            {
                return null;
            }

            // We don't care about the portion of the string before the start so chop it off now
            template = template.Substring(start);

            // Try to find self-closing tags first
            var selfClosingEnd = template.LastIndexOf(SelfClosingTagEnd, ComparisonType);
            if (selfClosingEnd >= 0)
            {
                // We're self-closing. Advance to the end of the tag
                selfClosingEnd = selfClosingEnd + SelfClosingTagEnd.Length;
            }

            // Look for an end tag for our command node
            var end = template.IndexOf(EndTag, ComparisonType);
            if (end >= 0)
            {
                end += EndTag.Length;
            }

            // If we have both a self-closing tag and an end tag, the self-closing probably belongs to
            // an inner XML element. In that case we want to go with the end tag. On the other hand, if
            // we have a self-closing tag and no end tag, we'll want to go with the self-closing tag.

            // That's what this is doing - taking the self-closing tag as the end tag
            if (end <= 0 && selfClosingEnd >= 0)
            {
                end = selfClosingEnd;
            }

            // If we don't have an end at this point, we need to bow out as a tag that was started but not finished
            if (end < 0)
            {
                var message = string.Format(CultureInfo.CurrentCulture, Resources.NoEndTagForOobCommand, template);
                console?.Log(Resources.ChatOutputHeader, message, LogLevel.Error);

                return null;
            }

            // Now we can snip out the extra bits to get our template XML
            return template.Substring(0, end);
        }
    }
}