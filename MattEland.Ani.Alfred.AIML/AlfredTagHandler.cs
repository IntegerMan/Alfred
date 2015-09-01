// ---------------------------------------------------------
// AlfredTagHandler.cs
// 
// Created on:      08/19/2015 at 9:31 PM
// Last Modified:   08/23/2015 at 11:07 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;

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

            var recipient = ChatEngine.Owner as IAlfredCommandRecipient;

            // Check to make sure we have a recipient to talk to
            if (recipient == null)
            {
                Log(Resources.AlfredTagHandlerProcessChangeNoRecipient, LogLevel.Warning);

                return result.Output.NonNull();
            }

            // Build a command
            var name = GetAttribute("command");
            var subsystem = GetAttribute("subsystem");
            var data = GetAttribute("data");

            var command = new ChatCommand(subsystem, name, data);

            // Send the command on to the owner. This may modify result or carry out other actions.
            recipient.ProcessAlfredCommand(command, result);

            // Get the output value from the result in case it was set externally
            return result.Output.NonNull();
        }

    }
}