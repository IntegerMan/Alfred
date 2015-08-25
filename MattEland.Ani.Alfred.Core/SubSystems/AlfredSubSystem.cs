// ---------------------------------------------------------
// AlfredSubSystem.cs
// 
// Created on:      08/07/2015 at 10:00 PM
// Last Modified:   08/16/2015 at 2:21 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Common;

namespace MattEland.Ani.Alfred.Core.Subsystems
{
    /// <summary>
    ///     Represents a subsystem on the Alfred Framework. Subsystems are ways of providing multiple
    ///     related modules and
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
        /// <param name="console">The console.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        protected AlfredSubsystem([NotNull] IPlatformProvider provider,
                                  [CanBeNull] IConsole console = null) : base(console)
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
        ///     Gets the children of this component. Depending on the type of component this is, the children
        ///     will
        ///     vary in their own types.
        /// </summary>
        /// <value>The children.</value>
        public override IEnumerable<IAlfredComponent> Children
        {
            get { return _pages; }
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
        protected void Register([NotNull] IAlfredPage page)
        {
            if (page == null) { throw new ArgumentNullException(nameof(page)); }
            _pages.AddSafe(page);

            if (AlfredInstance == null)
            {
                throw new InvalidOperationException("Cannot register page without an Alfred instance");
            }

            AlfredInstance.Register(page);
        }

        /// <summary>
        ///     Clears all child collections
        /// </summary>
        protected override void ClearChildCollections()
        {
            base.ClearChildCollections();

            _pages.Clear();
        }

        /// <summary>
        /// Processes an Alfred Command. If the command is handled, result should be modified accordingly and the method should return true. Returning false will not stop the message from being propogated.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="result">The result. If the command was handled, this should be updated.</param>
        /// <returns><c>True</c> if the command was handled; otherwise false.</returns>
        public virtual bool ProcessAlfredCommand(ChatCommand command,
                                                 [CanBeNull] AlfredCommandResult result)
        {
            /* If there's no result, there's no way any feedback could get back. 
            This is just here to protect against bad input */

            if (result == null) { return false; }

            // Only route messages to sub-components if they don't have a destination
            if (command.Subsystem.IsEmpty() || command.Subsystem.Matches(Id))
            {
                foreach (var page in Pages)
                {
                    if (page.ProcessAlfredCommand(command, result))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Gets the name of the broad categorization or type that this item is.
        /// </summary>
        /// <example>
        /// Some examples of ItemTypeName values might be "Folder", "Application", "User", etc.
        /// </example>
        /// <value>The item type's name.</value>
        public override string ItemTypeName
        {
            get { return "Subsystem"; }
        }
    }

}