// ---------------------------------------------------------
// AlfredChatSubsystem.cs
// 
// Created on:      08/09/2015 at 11:09 PM
// Last Modified:   08/12/2015 at 3:54 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System.Diagnostics;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core;
using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Subsystems;
using MattEland.Common;

using Res = MattEland.Ani.Alfred.Chat.Resources;

namespace MattEland.Ani.Alfred.Chat
{
    /// <summary>
    ///     The Chat subsystem for Alfred. Presents a chatting mechanism using AIML powered conversation bots.
    /// </summary>
    public sealed class AlfredChatSubsystem : AlfredSubsystem
    {
        [NotNull]
        private readonly AimlStatementHandler _chatHandler;

        /// <summary>
        /// Gets the chat handler.
        /// </summary>
        /// <value>The chat handler.</value>
        [NotNull]
        public AimlStatementHandler ChatHandler
        {
            [DebuggerStepThrough]
            get
            { return _chatHandler; }
        }

        [NotNull]
        private readonly ChatPage _chatPage;

        [NotNull]
        private readonly AlfredCommandRouter _commandRouter = new AlfredCommandRouter();

        /// <summary>
        /// Handles initialization events
        /// </summary>
        /// <param name="alfred">The alfred instance.</param>
        protected override void InitializeProtected(IAlfred alfred)
        {
            // Tell the chat engine where to send its mail
            _commandRouter.Alfred = alfred;
            _chatHandler.UpdateOwner(_commandRouter);
        }

        /// <summary>
        ///     Handles shutdown events
        /// </summary>
        protected override void ShutdownProtected()
        {
            // Clear out the owner just in case
            _chatHandler.UpdateOwner(null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AlfredSubsystem" /> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="console">The console.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public AlfredChatSubsystem([NotNull] IPlatformProvider provider,
                                   [CanBeNull] IConsole console) : base(provider, console)
        {
            _chatHandler = new AimlStatementHandler(console);
            _chatPage = new ChatPage(Res.ChatModuleName.NonNull(), _chatHandler);
        }

        /// <summary>
        ///     Gets or sets whether the page should be registered. If not, the chat functionality will just be supplied to the
        ///     system via the chat handler.
        ///     Defaults to false.
        /// </summary>
        /// <value>Whether or not the page should be registered.</value>
        public bool RegisterPage { get; set; }

        /// <summary>
        ///     Gets the name of the component.
        /// </summary>
        /// <value>The name of the component.</value>
        [NotNull]
        public override string Name
        {
            get { return Res.ChatModuleName.NonNull(); }
        }

        /// <summary>
        ///     Gets the identifier for the subsystem to be used in command routing.
        /// </summary>
        /// <value>The identifier for the subsystem.</value>
        [NotNull]
        public override string Id
        {
            get { return "Chat"; }
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
            AlfredInstance?.ChatProvider?.DoInitialGreeting();
        }
    }

}