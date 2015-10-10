// ---------------------------------------------------------
// ModuleListPage.cs
// 
// Created on:      08/19/2015 at 9:31 PM
// Last Modified:   09/13/2015 at 3:56 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using MattEland.Common.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Common;
using MattEland.Common.Providers;

namespace MattEland.Ani.Alfred.Core.Pages
{
    /// <summary>
    ///     A page grouping together multiple module collections of widgets
    /// </summary>
    [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
    public sealed class ModuleListPage : ModulePageBase
    {
        /// <summary>
        ///     The modules.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        private readonly ICollection<IAlfredModule> _modules;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ModuleListPage" /> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="name">The name.</param>
        /// <param name="id">The identifier.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when one or more required arguments are null.
        /// </exception>
        public ModuleListPage([NotNull] IAlfredContainer container,
                              [NotNull] string name,
                              [NotNull] string id) : base(container, name, id)
        {
            //- Validate
            if (container == null) throw new ArgumentNullException(nameof(container));

            _modules = container.ProvideCollection<IAlfredModule>();

            // By default use Horizontal wrap layout
            LayoutType = LayoutType.HorizontalWrapPanel;
        }

        /// <summary>
        ///     Gets the modules.
        /// </summary>
        /// <value>
        ///     The modules.
        /// </value>
        public override IEnumerable<IAlfredModule> Modules
        {
            get { return _modules; }
        }

        /// <summary>
        ///     Registers the specified module.
        /// </summary>
        /// <param name="module">The module.</param>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods",
            MessageId = "0")]
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
    }
}