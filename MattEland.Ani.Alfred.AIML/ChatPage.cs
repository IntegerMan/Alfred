using System.Collections.Generic;
using System.Diagnostics;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Pages;
using MattEland.Common.Providers;

namespace MattEland.Ani.Alfred.Chat
{
    /// <summary>
    /// A custom page for interacting with Alfred via Chat
    /// </summary>
    public sealed class ChatPage : Page
    {
        [NotNull]
        private readonly IChatProvider _chatHandler;

        /// <summary>
        /// Gets the chat handler.
        /// </summary>
        /// <value>The chat handler.</value>
        [NotNull]
        public IChatProvider ChatHandler
        {
            [DebuggerStepThrough]
            get
            { return _chatHandler; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Page" /> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="name">The name.</param>
        /// <param name="chatHandler">The input handler</param>
        internal ChatPage([NotNull] IObjectContainer container, [NotNull] string name, [NotNull] IChatProvider chatHandler) : base(container, name, "Chat")
        {
            _chatHandler = chatHandler;
        }

        /// <summary>
        /// Gets the children of this component. Depending on the type of component this is, the children will
        /// vary in their own types.
        /// </summary>
        /// <value>The children.</value>
        public override IEnumerable<IAlfredComponent> Children
        {
            get { yield break; }
        }

    }

}