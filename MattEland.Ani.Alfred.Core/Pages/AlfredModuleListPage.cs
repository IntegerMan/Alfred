// ---------------------------------------------------------
// AlfredModuleListPage.cs
// 
// Created on:      08/08/2015 at 7:07 PM
// Last Modified:   08/08/2015 at 7:07 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace MattEland.Ani.Alfred.Core.Pages
{
    /// <summary>
    /// A page grouping together multiple module collections of widgets
    /// </summary>
    public class AlfredModuleListPage : AlfredPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AlfredModuleListPage"/> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public AlfredModuleListPage([NotNull] IPlatformProvider provider)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            _modules = provider.CreateCollection<AlfredModule>();
        }

        [NotNull, ItemNotNull]
        private readonly ICollection<AlfredModule> _modules;

        /// <summary>
        /// Gets the modules.
        /// </summary>
        /// <value>The modules.</value>
        [NotNull, ItemNotNull]
        public IEnumerable<AlfredModule> Modules
        {
            get { return _modules; }
        }

        /// <summary>
        /// Registers the specified module.
        /// </summary>
        /// <param name="module">The module.</param>
        public void Register([NotNull] AlfredModule module)
        {
            _modules.AddSafe(module);
        }
    }
}