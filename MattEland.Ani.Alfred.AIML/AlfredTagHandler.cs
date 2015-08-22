// ---------------------------------------------------------
// AlfredTagHandler.cs
// 
// Created on:      08/17/2015 at 10:57 PM
// Last Modified:   08/18/2015 at 1:03 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Xml;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml.TagHandlers;
using MattEland.Ani.Alfred.Chat.Aiml.Utils;
using MattEland.Ani.Alfred.Core;
using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Common;

namespace MattEland.Ani.Alfred.Chat
{
    /// <summary>
    /// A TagHandler able to invoke shell events
    /// </summary>
    [HandlesAimlTag("shell")]
    public sealed class ShellTagHandler : AimlTagHandler
    {

        /// <summary>
        ///     Initializes a new instance of the <see cref="ShellTagHandler" /> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="parameters"/> is <see langword="null" />.</exception>
        public ShellTagHandler([NotNull] TagHandlerParameters parameters) : base(parameters)
        {
        }

        /// <summary>
        ///     Processes the input text and returns the processed value.
        /// </summary>
        /// <returns>The processed output</returns>
        [NotNull]
        protected override string ProcessChange()
        {
            var element = TemplateElement;
            var recipient = ChatEngine.Owner as IShellCommandRecipient;

            // If there's no target or no recipient, don't bother
            if (!element.HasAttribute("target") || recipient == null)
            {
                return string.Empty;
            }

            // Build a command
            var name = GetAttributeSafe(element, "command");
            if (name.IsEmpty())
            {
                name = "nav";
            }
            var target = GetAttributeSafe(element, "target");
            var data = GetAttributeSafe(element, "data");

            var command = new ShellCommand(name, target, data);

            // Send the command on to the owner
            var redirect = recipient.ProcessShellCommand(command);

            // If no redirect was specified, use empty
            if (!redirect.HasText())
            {
                return string.Empty;
            }

            // Execute a redirect to the result of the star operation
            try
            {
                var node = BuildNode(string.Format(Locale, "<srai>{0}</srai>", redirect).NonNull());
                var parameters = GetTagHandlerParametersForNode(node);
                return new RedirectTagHandler(parameters).Transform();
            }
            catch (XmlException xmlEx)
            {
                Log(string.Format(Locale, "Could not redirect to output due to bad XML: " + redirect, xmlEx.Message), LogLevel.Error);
                return string.Empty;
            }

        }
    }

    /// <summary>
    ///     A tag handler that handles the custom "alfred" tag that integrates Alfred into the AIML system
    ///     and allows AIML files to issue commands to Alfred modules.
    /// </summary>
    [HandlesAimlTag("alfred")]
    public class AlfredTagHandler : AimlTagHandler
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AlfredTagHandler" /> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="parameters" /> is <see langword="null" />.</exception>
        public AlfredTagHandler([NotNull] TagHandlerParameters parameters) : base(parameters)
        {
        }

        /// <summary>
        ///     Processes the input text and returns the processed value.
        /// </summary>
        /// <returns>The processed output</returns>
        protected override string ProcessChange()
        {
            var result = new AlfredCommandResult();

            var element = TemplateElement;
            var recipient = ChatEngine.Owner as IAlfredCommandRecipient;

            if (element.HasAttribute("command") && recipient != null)
            {
                // Build a command
                var name = GetAttributeSafe(element, "command");
                var subsystem = GetAttributeSafe(element, "subsystem");
                var data = GetAttributeSafe(element, "data");

                var command = new ChatCommand(subsystem, name, data);

                // Send the command on to the owner. This may modify result or carry out other actions.
                recipient.ProcessAlfredCommand(command, result);
            }

            // Get the output value from the result in case it was set externally
            return result.Output.NonNull();
        }
    }
}