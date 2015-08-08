// ---------------------------------------------------------
// AlfredSubSystem.cs
// 
// Created on:      08/07/2015 at 10:00 PM
// Last Modified:   08/07/2015 at 10:35 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Console;

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
        /// Registers a module.
        /// </summary>
        /// <param name="module">The module.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        protected void RegisterModule([NotNull] AlfredModule module)
        {
            if (module == null)
            {
                throw new ArgumentNullException(nameof(module));
            }

            // This shouldn't happen, but I want to check to make sure
            if (Modules.Contains(module))
            {
                throw new InvalidOperationException("The specified module was already part of the collection");
            }

            _modules.Add(module);
        }

        /// <summary>
        ///     Gets whether or not the module is visible to the user interface.
        /// </summary>
        /// <value>Whether or not the module is visible.</value>
        public override bool IsVisible
        {
            get { return true; }
        }
    }
}