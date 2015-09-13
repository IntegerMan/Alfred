﻿// ---------------------------------------------------------
// ModuleListPage.cs
// 
// Created on:      08/19/2015 at 9:31 PM
// Last Modified:   09/11/2015 at 10:11 PM
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
using MattEland.Common.Providers;

namespace MattEland.Ani.Alfred.Core.Pages
{
    /// <summary>
    ///     A page grouping together multiple module collections of widgets
    /// </summary>
    [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
    public sealed class ModuleListPage : AlfredPage
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
        public ModuleListPage([NotNull] IObjectContainer container,
                              [NotNull] string name,
                              [NotNull] string id) : base(container, name, id)
        {
            //- Validate
            if (container == null) throw new ArgumentNullException(nameof(container));

            _modules = container.ProvideCollection<IAlfredModule>();
        }

        /// <summary>
        ///     Gets the modules.
        /// </summary>
        /// <value>
        /// The modules.
        /// </value>
        [NotNull]
        [ItemNotNull]
        public IEnumerable<IAlfredModule> Modules
        {
            get { return _modules; }
        }

        /// <summary>
        ///     Gets the children of this component. Depending on the type of component this is, the
        ///     children will vary in their own types.
        /// </summary>
        /// <value>
        /// The children.
        /// </value>
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
        /// <value>
        /// Whether or not the component is visible.
        /// </value>
        public override bool IsVisible
        {
            get
            {
                // Display if any module has a visible widget
                var baseVisbility = base.IsVisible;
                var anyVisibleModules = Modules.Any(m => m.Widgets.Any(w => w.IsVisible));

                return baseVisbility && anyVisibleModules;
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