// ---------------------------------------------------------
// AlfredEventLogPage.cs
// 
// Created on:      08/19/2015 at 9:31 PM
// Last Modified:   08/25/2015 at 12:06 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.Core.Definitions;

namespace MattEland.Ani.Alfred.Core.Pages
{
    /// <summary>
    ///     An event logging page. This will need a special client-side implementation to list out the
    ///     details
    /// </summary>
    public sealed class AlfredEventLogPage : AlfredPage
    {
        [NotNull]
        private readonly IConsole _console;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AlfredEventLogPage" /> class.
        /// </summary>
        /// <param name="console">The console.</param>
        /// <param name="name">The name.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public AlfredEventLogPage([NotNull] IConsole console, [NotNull] string name)
            : base(name, "Log")
        {
            if (console == null) { throw new ArgumentNullException(nameof(console)); }
            _console = console;
        }

        /// <summary>
        ///     Gets the console.
        /// </summary>
        /// <value>The console.</value>
        [NotNull]
        [UsedImplicitly]
        public new IConsole Console
        {
            get
            {
                /* Use the base console by default, but fallback to this instance's
                console if no Console is available yet from Alfred. This is useful 
                for logging during initialization. */

                return base.Console ?? _console;
            }
        }

        /// <summary>
        ///     Gets the console events.
        /// </summary>
        /// <value>The events.</value>
        [NotNull]
        [ItemNotNull]
        [UsedImplicitly]
        public IEnumerable<IConsoleEvent> Events
        {
            get { return _console.Events; }
        }

        /// <summary>
        ///     Gets the children of this component. Depending on the type of component this is, the children
        ///     will
        ///     vary in their own types.
        /// </summary>
        /// <value>The children.</value>
        public override IEnumerable<IAlfredComponent> Children
        {
            get { yield break; }
        }

        /// <summary>
        ///     Gets the property providers nested inside of this property provider.
        /// </summary>
        /// <value>The property providers.</value>
        public override IEnumerable<IPropertyProvider> PropertyProviders
        {
            get
            {
                return Events.Cast<IPropertyProvider>();
            }
        }
    }
}