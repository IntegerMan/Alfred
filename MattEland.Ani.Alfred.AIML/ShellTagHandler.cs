using System;
using System.Xml;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml.TagHandlers;
using MattEland.Ani.Alfred.Chat.Aiml.Utils;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Common;

namespace MattEland.Ani.Alfred.Chat
{
    /// <summary>
    ///     A TagHandler able to invoke shell events
    /// </summary>
    [HandlesAimlTag("shell")]
    public sealed class ShellTagHandler : AimlTagHandler
    {

        /// <summary>
        ///     Initializes a new instance of the <see cref="ShellTagHandler" /> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="parameters" /> is <see langword="null" />.</exception>
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
            var recipient = ChatEngine.Owner as IShellCommandRecipient;

            // If there's no target or no recipient, don't bother
            if (!HasAttribute("target") || recipient == null)
            {
                return string.Empty;
            }

            // Build a command
            var name = GetAttribute("command");
            if (name.IsEmpty())
            {
                name = "nav";
            }
            var target = GetAttribute("target");
            var data = GetAttribute("data");

            var command = new ShellCommand(name, target, data);

            // Send the command on to the owner
            var redirect = recipient.ProcessShellCommand(command);

            // If no redirect was specified, use empty otherwise execute a redirect to the result of the star operation
            return !redirect.HasText() ? string.Empty : DoRedirect(redirect);

        }

    }
}