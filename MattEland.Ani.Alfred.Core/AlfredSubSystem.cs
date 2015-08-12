// ---------------------------------------------------------
// AlfredSubSystem.cs
// 
// Created on:      08/07/2015 at 10:00 PM
// Last Modified:   08/12/2015 at 1:50 AM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;

namespace MattEland.Ani.Alfred.Core
{
    /// <summary>
    ///     Represents a subsystem on the Alfred Framework. Subsystems are ways of providing multiple related modules and
    ///     capabilities to Alfred.
    /// </summary>
    public abstract class AlfredSubsystem : AlfredComponent, IAlfredSubsystem
    {
        [NotNull]
        [ItemNotNull]
        private readonly ICollection<IAlfredPage> _pages;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AlfredSubsystem" /> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        protected AlfredSubsystem([NotNull] IPlatformProvider provider)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            _pages = provider.CreateCollection<IAlfredPage>();
        }

        /// <summary>
        ///     Gets whether or not the module is visible to the user interface.
        /// </summary>
        /// <value>Whether or not the module is visible.</value>
        public override bool IsVisible
        {
            get { return true; }
        }

        /// <summary>
        ///     Gets the children of this component. Depending on the type of component this is, the children will
        ///     vary in their own types.
        /// </summary>
        /// <value>The children.</value>
        public override IEnumerable<IAlfredComponent> Children
        {
            get
            {
                foreach (var page in _pages)
                {
                    yield return page;
                }
            }
        }

        /// <summary>
        ///     Gets the root-level pages provided by this subsystem.
        /// </summary>
        /// <value>The root-level pages.</value>
        public IEnumerable<IAlfredPage> RootPages
        {
            get { return _pages.Where(page => page.IsRootLevel); }
        }

        /// <summary>
        ///     Gets the identifier for the subsystem to be used in command routing.
        /// </summary>
        /// <value>The identifier for the subsystem.</value>
        [NotNull]
        public abstract string Id { get; }

        /// <summary>
        ///     Handles a chat command that may be intended for this module.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="result">The default system response. This should be modified and returned.</param>
        /// <returns><c>true</c> if the command was handled, <c>false</c> otherwise.</returns>
        public virtual bool HandleChatCommand(ChatCommand command, [NotNull] AlfredCommandResult result)
        {
            return false;
        }

        /// <summary>
        ///     Gets the pages associated with this subsystem
        /// </summary>
        /// <value>The pages.</value>
        [ItemNotNull]
        public IEnumerable<IAlfredPage> Pages
        {
            get { return _pages; }
        }

        /// <summary>
        ///     Registers a page.
        /// </summary>
        /// <param name="page">The page.</param>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
        protected void Register([NotNull] IAlfredPage page)
        {
            _pages.AddSafe(page);
            page.OnRegistered(AlfredInstance);
        }

        /// <summary>
        ///     Clears all child collections
        /// </summary>
        protected override void ClearChildCollections()
        {
            base.ClearChildCollections();

            _pages.Clear();
        }
    }

}