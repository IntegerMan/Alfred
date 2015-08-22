// ---------------------------------------------------------
// AlfredModuleListPage.cs
// 
// Created on:      08/19/2015 at 9:31 PM
// Last Modified:   08/22/2015 at 12:03 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Common;

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
        /// <param name="id">The identifier.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public AlfredModuleListPage([NotNull] IPlatformProvider provider,
                                    [NotNull] string name,
                                    [NotNull] string id) : base(name, id)
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
        ///     Gets the children of this component. Depending on the type of component this is, the children
        ///     will
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
        ///     Gets whether or not the component is visible to the user interface.
        /// </summary>
        /// <value>Whether or not the component is visible.</value>
        public override bool IsVisible
        {
            get
            {
                return base.IsVisible &&
                       Modules.Any(m => m.Widgets != null && m.Widgets.Any(w => w != null && w.IsVisible));
            }
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

        /// <summary>
        ///     Processes an Alfred Command. If the command is handled, result should be modified accordingly
        ///     and the method should return true. Returning false will not stop the message from being
        ///     propogated.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="result">The result. If the command was handled, this should be updated.</param>
        /// <returns><c>True</c> if the command was handled; otherwise false.</returns>
        public override bool ProcessAlfredCommand(ChatCommand command, AlfredCommandResult result)
        {

            foreach (var module in Modules)
            {
                if (module.ProcessAlfredCommand(command, result))
                {
                    return true;
                }
            }

            return base.ProcessAlfredCommand(command, result);
        }
    }
}