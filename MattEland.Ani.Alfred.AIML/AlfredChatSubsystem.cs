// ---------------------------------------------------------
// AlfredChatSubsystem.cs
// 
// Created on:      08/09/2015 at 11:09 PM
// Last Modified:   08/10/2015 at 9:33 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core;
using MattEland.Ani.Alfred.Core.Definitions;

using Res = MattEland.Ani.Alfred.Chat.Resources;

namespace MattEland.Ani.Alfred.Chat
{
    /// <summary>
    ///     The Chat subsystem for Alfred. Presents a chatting mechanism using AIML powered conversation bots.
    /// </summary>
    public class AlfredChatSubsystem : AlfredSubsystem
    {
        [NotNull]
        private readonly AimlStatementHandler _chatHandler;

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
            _chatHandler = new AimlStatementHandler();
            _chatPage = new ChatPage(Res.ChatModuleName.NonNull(), _chatHandler);
        }

        /// <summary>
        ///     Gets or sets whether the page should be registered. If not, the chat functionality will just be supplied to the
        ///     system via the chat handler.
        /// 
        ///     Defaults to false.
        /// </summary>
        /// <value>Whether or not the page should be registered.</value>
        public bool RegisterPage { get; set; }

        /// <summary>
        ///     Gets the name of the component.
        /// </summary>
        /// <value>The name of the component.</value>
        public override string Name
        {
            get { return Res.ChatModuleName.NonNull(); }
        }

        /// <summary>
        ///     Allows components to define controls
        /// </summary>
        protected override void RegisterControls()
        {
            base.RegisterControls();

            // We don't have console until we're registered, so update it here
            _chatHandler.Console = AlfredInstance?.Console;

            // Register with the rest of the system
            if (RegisterPage)
            {
                Register(_chatPage);
            }
            Register(_chatHandler);
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
    }

}