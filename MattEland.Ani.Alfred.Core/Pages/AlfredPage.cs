// ---------------------------------------------------------
// AlfredPage.cs
// 
// Created on:      08/11/2015 at 9:44 PM
// Last Modified:   08/11/2015 at 9:45 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;

namespace MattEland.Ani.Alfred.Core.Pages
{
    /// <summary>
    ///     A page that can be used in Alfred
    /// </summary>
    public abstract class AlfredPage : AlfredComponent, IAlfredPage
    {
        [NotNull]
        private readonly string _name;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AlfredPage" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        protected AlfredPage([NotNull] string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            _name = name;
        }

        /// <summary>
        ///     Gets whether or not the component is visible to the user interface.
        /// </summary>
        /// <value>Whether or not the component is visible.</value>
        public override bool IsVisible
        {
            get { return Status == AlfredStatus.Online; }
        }

        /// <summary>
        ///     Gets or sets the name of the page.
        /// </summary>
        /// <value>The name.</value>
        [NotNull]
        public override string Name
        {
            get { return _name; }
        }

        /// <summary>
        ///     Gets a value indicating whether this is a root level page that should show on the navigator.
        /// </summary>
        /// <value><c>true</c> if this page is root level; otherwise, <c>false</c>.</value>
        public bool IsRootLevel
        {
            get { return true; }
        }

        /// <summary>
        ///     Handles a chat command that may be intended for this page or one of its modules.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="result">The default system response. This should be modified and returned.</param>
        /// <returns><c>true</c> if the command was handled, <c>false</c> otherwise.</returns>
        public virtual bool HandleChatCommand(ChatCommand command, AlfredCommandResult result)
        {
            return false;
        }
    }

}