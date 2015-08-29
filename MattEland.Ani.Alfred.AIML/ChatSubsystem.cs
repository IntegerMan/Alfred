// ---------------------------------------------------------
// ChatSubsystem.cs
// 
// Created on:      08/19/2015 at 9:31 PM
// Last Modified:   08/25/2015 at 9:54 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Subsystems;
using MattEland.Common;

using Res = MattEland.Ani.Alfred.Chat.Resources;

namespace MattEland.Ani.Alfred.Chat
{
    /// <summary>
    ///     The Chat subsystem for Alfred. Presents a chatting mechanism using AIML powered conversation
    ///     bots.
    /// </summary>
    public sealed class ChatSubsystem : AlfredSubsystem
    {

        [NotNull]
        private readonly ChatPage _chatPage;

        [NotNull]
        private readonly AlfredCommandRouter _commandRouter = new AlfredCommandRouter();

        /// <summary>
        ///     Initializes a new instance of the <see cref="AlfredSubsystem" /> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="console">The console.</param>
        /// <param name="engineName">Name of the chat engine.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public ChatSubsystem(
            [NotNull] IPlatformProvider provider,
            [CanBeNull] IConsole console,
            [NotNull] string engineName) : base(console)
        {
            // Instantiate composite objects
            ChatHandler = new AimlStatementHandler(engineName, Container);
            _chatPage = new ChatPage(Res.ChatModuleName.NonNull(), ChatHandler);
        }

        /// <summary>
        ///     Gets the chat handler.
        /// </summary>
        /// <value>The chat handler.</value>
        [NotNull]
        public AimlStatementHandler ChatHandler
        {
            [DebuggerStepThrough]
            get;
        }

        /// <summary>
        ///     Gets or sets whether the page should be registered. If not, the chat functionality will just be
        ///     supplied to the
        ///     system via the chat handler.
        ///     Defaults to false.
        /// </summary>
        /// <value>Whether or not the page should be registered.</value>
        public bool RegisterPage
        {
            get;
            [UsedImplicitly]
            set;
        }

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
        ///     Gets the property providers nested inside of this property provider.
        /// </summary>
        /// <value>The property providers.</value>
        public override IEnumerable<IPropertyProvider> PropertyProviders
        {
            get
            {
                // We still want the default nodes
                foreach (var provider in base.PropertyProviders) { yield return provider; }

                foreach (var provider in ChatHandler.PropertyProviders) { yield return provider; }
            }
        }

        /// <summary>
        ///     Handles initialization events
        /// </summary>
        /// <param name="alfred">The alfred instance.</param>
        protected override void InitializeProtected(IAlfred alfred)
        {
            // Tell the chat engine where to send its mail
            _commandRouter.Alfred = alfred;
            ChatHandler.UpdateOwner(_commandRouter);
        }

        /// <summary>
        ///     Handles shutdown events
        /// </summary>
        protected override void ShutdownProtected()
        {
            // Clear out the owner just in case
            ChatHandler.UpdateOwner(null);
        }

        /// <summary>
        ///     Allows components to define controls
        /// </summary>
        protected override void RegisterControls()
        {
            base.RegisterControls();

            // We don't have console until we're registered, so update it here
            ChatHandler.Console = AlfredInstance?.Console;

            // Register with the rest of the system
            if (RegisterPage) { Register(_chatPage); }
            Register(ChatHandler);
        }

        /// <summary>
        ///     A notification method that is invoked when initialization for Alfred is complete so the UI can
        ///     be fully enabled or adjusted
        /// </summary>
        public override void OnInitializationCompleted()
        {
            /* The chat provider may not be the one that we gave to Alfred or it may be a decorator around it.
               Either way, use Alfred's implementation. */

            var chatProvider = AlfredInstance?.ChatProvider;

            // Say hi so Alfred greets the user
            chatProvider?.DoInitialGreeting();
        }
    }

}