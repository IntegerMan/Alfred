﻿// ---------------------------------------------------------
// AlfredSubSystem.cs
// 
// Created on:      08/07/2015 at 10:00 PM
// Last Modified:   08/08/2015 at 1:35 AM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

namespace MattEland.Ani.Alfred.Core
{
    /// <summary>
    ///     Represents a subsystem on the Alfred Framework. Subsystems are ways of providing multiple related modules and
    ///     capabilities to Alfred.
    /// </summary>
    public abstract class AlfredSubSystem : AlfredComponent
    {
        [NotNull]
        [ItemNotNull]
        private readonly ICollection<AlfredModule> _modules;

        [NotNull]
        [ItemNotNull]
        private readonly ICollection<AlfredPage> _pages;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AlfredSubSystem" /> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        protected AlfredSubSystem([NotNull] IPlatformProvider provider)
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
            AddToCollectionSafe(module, _modules);
        }

        /// <summary>
        ///     Registers a page.
        /// </summary>
        /// <param name="page">The page.</param>
        protected void Register([NotNull] AlfredPage page)
        {
            AddToCollectionSafe(page, _pages);
        }
    }
}