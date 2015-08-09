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

using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.Core.Modules;

namespace MattEland.Ani.Alfred.Core.Pages
{
    /// <summary>
    /// A page grouping together multiple module collections of widgets
    /// </summary>
    public class AlfredModuleListPage : AlfredPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AlfredModuleListPage" /> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="name">The name.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public AlfredModuleListPage([NotNull] IPlatformProvider provider, [NotNull] string name) : base(name)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            _modules = provider.CreateCollection<IAlfredModule>();
        }

        [NotNull, ItemNotNull]
        private readonly ICollection<IAlfredModule> _modules;

        /// <summary>
        /// Gets the modules.
        /// </summary>
        /// <value>The modules.</value>
        [NotNull, ItemNotNull]
        public IEnumerable<IAlfredModule> Modules
        {
            get { return _modules; }
        }

        /// <summary>
        /// Registers the specified module.
        /// </summary>
        /// <param name="module">The module.</param>
        public void Register([NotNull] IAlfredModule module)
        {
            _modules.AddSafe(module);
            module.OnRegistered(AlfredInstance);
        }


        /// <summary>
        /// Gets the children of this component. Depending on the type of component this is, the children will
        /// vary in their own types.
        /// </summary>
        /// <value>The children.</value>
        public override IEnumerable<IAlfredComponent> Children
        {
            get
            {
                foreach (var module in _modules)
                {
                    yield return module;
                }
            }
        }

        /// <summary>
        /// Clears the modules list.
        /// </summary>
        public void ClearModules()
        {
            _modules.Clear();
        }
    }
}