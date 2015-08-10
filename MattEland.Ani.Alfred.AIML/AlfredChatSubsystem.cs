using System;
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
    public class AlfredChatSubsystem : AlfredSubsystem
    {
        [NotNull]
        private readonly ChatPage _chatPage;

        [NotNull]
        private readonly AimlStatementHandler _chatHandler;

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
            _chatHandler = new AimlStatementHandler();
            _chatPage = new ChatPage(Res.ChatModuleName.NonNull(), _chatHandler);
        }

        /// <summary>
        /// Allows components to define controls
        /// </summary>
        protected override void RegisterControls()
        {
            base.RegisterControls();

            // We don't have console until we're registered, so update it here
            _chatHandler.Console = AlfredInstance?.Console;

            // Register our chat page
            Register(_chatPage);
            Register(_chatHandler);

            //? There may be some need to integrate this into AlfredProvider proper for voice handling, but not quite yet
        }

        /// <summary>
        ///     A notification method that is invoked when initialization for Alfred is complete so the UI can be fully enabled or
        ///     adjusted
        /// </summary>
        public override void OnInitializationCompleted()
        {
            // Say hi so Alfred greets the user
            _chatHandler.DoInitialGreeting();
        }

        /// <summary>
        ///     Gets the name of the component.
        /// </summary>
        /// <value>The name of the component.</value>
        public override string Name
        {
            get { return Res.ChatModuleName.NonNull(); }
        }

    }

}
