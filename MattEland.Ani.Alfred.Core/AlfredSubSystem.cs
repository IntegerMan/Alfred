// ---------------------------------------------------------
// AlfredSubsystem.cs
// 
// Created on:      08/07/2015 at 10:00 PM
// Last Modified:   08/08/2015 at 1:35 AM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Pages;

namespace MattEland.Ani.Alfred.Core
{
    /// <summary>
    ///     Represents a subsystem on the Alfred Framework. Subsystems are ways of providing multiple related modules and
    ///     capabilities to Alfred.
    /// </summary>
    public abstract class AlfredSubsystem : AlfredComponent
    {
        [NotNull]
        [ItemNotNull]
        private readonly ICollection<AlfredModule> _modules;

        [NotNull]
        [ItemNotNull]
        private readonly ICollection<AlfredPage> _pages;

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

            _modules = provider.CreateCollection<AlfredModule>();
            _pages = provider.CreateCollection<AlfredPage>();
        }

        /// <summary>
        ///     Gets the modules associated with this subsystem
        /// </summary>
        /// <value>The modules.</value>
        [NotNull]
        [ItemNotNull]
        public IEnumerable<AlfredModule> Modules
        {
            get { return _modules; }
        }

        /// <summary>
        ///     Gets the pages associated with this subsystem
        /// </summary>
        /// <value>The pages.</value>
        [NotNull]
        [ItemNotNull]
        public IEnumerable<AlfredPage> Pages
        {
            get { return _pages; }
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
        ///     Registers a module.
        /// </summary>
        /// <param name="module">The module.</param>
        protected void Register([NotNull] AlfredModule module)
        {
            _modules.AddSafe(module);
            module.OnRegistered(AlfredInstance);
        }

        /// <summary>
        ///     Registers a page.
        /// </summary>
        /// <param name="page">The page.</param>
        protected void Register([NotNull] AlfredPage page)
        {
            _pages.AddSafe(page);
            page.OnRegistered(AlfredInstance);
        }

        /// <summary>
        /// Gets the children of this component. Depending on the type of component this is, the children will
        /// vary in their own types.
        /// </summary>
        /// <value>The children.</value>
        public override IEnumerable<AlfredComponent> Children
        {
            get
            {
                // Pages are higher up in the hierarchy than modules so they come first
                foreach (var page in _pages)
                {
                    yield return page;
                }

                // Return all modules
                foreach (var module in _modules)
                {
                    yield return module;
                }
            }
        }

        /// <summary>
        /// Clears all child collections
        /// </summary>
        protected override void ClearChildCollections()
        {
            base.ClearChildCollections();

            _pages.Clear();
            _modules.Clear();
        }

    }
}