// ---------------------------------------------------------
// ModulePageBase.cs
// 
// Created on:      09/13/2015 at 3:52 PM
// Last Modified:   09/13/2015 at 3:55 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Common.Providers;
using MattEland.Common;

namespace MattEland.Ani.Alfred.Core.Pages
{
    /// <summary>
    ///     A page containing a collection of modules
    /// </summary>
    public abstract class ModulePageBase : AlfredPage
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ModulePageBase" /> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="name">The name.</param>
        /// <param name="id">The identifier.</param>
        protected ModulePageBase(IObjectContainer container, string name, string id)
            : base(container, name, id)
        {
        }

        /// <summary>
        ///     Gets the modules.
        /// </summary>
        /// <value>
        /// The modules.
        /// </value>
        [NotNull]
        [ItemNotNull]
        public abstract IEnumerable<IAlfredModule> Modules { get; }

        /// <summary>
        ///     Gets the children of this page. For <see cref="ModulePageBase" /> pages, this will be the
        ///     contents of <see cref="ModulePageBase.Modules" /> .
        /// </summary>
        /// <value>
        ///     The children.
        /// </value>
        public override IEnumerable<IAlfredComponent> Children
        {
            get { return Modules; }
        }

        /// <summary>
        ///     Gets whether or not the component is visible to the user interface.
        /// </summary>
        /// <value>
        /// Whether or not the component is visible.
        /// </value>
        public override bool IsVisible
        {
            get
            {
                // Display if any module has a visible widget
                var anyVisibleModules = Modules.Any(m => m.Widgets.Any(w => w.IsVisible));

                return base.IsVisible && anyVisibleModules;
            }
        }

        /// <summary>
        ///     Processes an Alfred Command. If the <paramref name="command" /> is handled,
        ///     <paramref name="result" /> should be modified accordingly and the method should
        ///     return true. Returning <see langword="false" /> will not stop the message from being
        ///     propagated.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="result">
        /// The result. If the <paramref name="command" /> was handled, this should be updated.
        /// </param>
        /// <returns>
        ///     <c>True</c> if the <paramref name="command" /> was handled; otherwise false.
        /// </returns>
        public override bool ProcessAlfredCommand(ChatCommand command, ICommandResult result)
        {
            return Modules.Any(module => module.ProcessAlfredCommand(command, result))
                   || base.ProcessAlfredCommand(command, result);
        }

        /// <summary>
        /// Finds the module by its name
        /// </summary>
        /// <param name="name">Name of the module.</param>
        /// <returns>The module or null if no module found.</returns>
        [CanBeNull]
        public IAlfredComponent FindModuleByName(string name)
        {
            return Children.FirstOrDefault(m => m.Name.Matches(name));
        }
    }
}