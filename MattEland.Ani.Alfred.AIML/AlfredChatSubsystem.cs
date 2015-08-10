using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core;
using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.Core.Definitions;

using Res = MattEland.Ani.Alfred.Chat.Resources;

namespace MattEland.Ani.Alfred.Chat
{
    /// <summary>
    /// The Chat subsystem for Alfred. Presents a chatting mechanism using AIML powered conversation bots.
    /// </summary>
    public class AlfredChatSubsystem : AlfredSubsystem, IUserStatementHandler
    {
        [NotNull]
        private readonly ChatPage _chatPage;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AlfredSubsystem" /> class.
        /// </summary>
        public AlfredChatSubsystem() : this(new SimplePlatformProvider())
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AlfredSubsystem" /> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public AlfredChatSubsystem([NotNull] IPlatformProvider provider) : base(provider)
        {
            _chatPage = new ChatPage(Res.ChatModuleName.NonNull(), this);
        }

        /// <summary>
        /// Allows components to define controls
        /// </summary>
        protected override void RegisterControls()
        {
            base.RegisterControls();

            Register(_chatPage);
        }

        /// <summary>
        ///     Gets the name of the component.
        /// </summary>
        /// <value>The name of the component.</value>
        public override string Name
        {
            get { return Res.ChatModuleName.NonNull(); }
        }

        /// <summary>
        /// Handles a user statement.
        /// </summary>
        /// <param name="userInput">The user input.</param>
        /// <returns>The response to the user statment</returns>
        public UserStatementResponse HandleUserStatement(string userInput)
        {
            // Log the input to the diagnostic log. Verbose should keep it from being spoken
            Log("Chat.Input", userInput, LogLevel.Verbose);

            // TODO: Actually do some processing here

            var response = new UserStatementResponse(userInput,
                                                     "I'm afraid I don't know how to respond to that, sir.",
                                                     true);

            // Log the output to the diagnostic log. Info should make it spoken if speech is on.
            Log("Chat.Output", response.ResponseText, LogLevel.Info);

            return response;
        }
    }

}
