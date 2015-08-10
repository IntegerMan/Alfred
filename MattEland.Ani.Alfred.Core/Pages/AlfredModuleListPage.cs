// ---------------------------------------------------------
// AlfredModuleListPage.cs
// 
// Created on:      08/08/2015 at 7:17 PM
// Last Modified:   08/09/2015 at 10:03 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;

namespace MattEland.Ani.Alfred.Core.Pages
{
    /// <summary>
    ///     A page grouping together multiple module collections of widgets
    /// </summary>
    public sealed class AlfredModuleListPage : AlfredPage
    {
        [NotNull]
        [ItemNotNull]
        private readonly ICollection<IAlfredModule> _modules;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AlfredModuleListPage" /> class.
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

        /// <summary>
        ///     Gets the modules.
        /// </summary>
        /// <value>The modules.</value>
        [NotNull]
        [ItemNotNull]
        public IEnumerable<IAlfredModule> Modules
        {
            get { return _modules; }
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
                foreach (var module in _modules)
                {
                    yield return module;
                }
            }
        }

        /// <summary>
        ///     Registers the specified module.
        /// </summary>
        /// <param name="module">The module.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
        public void Register([NotNull] IAlfredModule module)
        {
            _modules.AddSafe(module);
            module.OnRegistered(AlfredInstance);
        }

        /// <summary>
        ///     Clears the modules list.
        /// </summary>
        public void ClearModules()
        {
            _modules.Clear();
        }

        /// <summary>
        ///     Gets whether or not the component is visible to the user interface.
        /// </summary>
        /// <value>Whether or not the component is visible.</value>
        public override bool IsVisible
        {
            get { return Modules.Any(m => m.Widgets.Any(w => w.IsVisible)); }
        }
    }
}