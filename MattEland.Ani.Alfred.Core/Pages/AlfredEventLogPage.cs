// ---------------------------------------------------------
// AlfredEventLogPage.cs
// 
// Created on:      08/08/2015 at 7:23 PM
// Last Modified:   08/08/2015 at 7:23 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Console;

namespace MattEland.Ani.Alfred.Core.Pages
{
    /// <summary>
    /// An event logging page. This will need a special client-side implementation to list out the details
    /// </summary>
    public class AlfredEventLogPage : AlfredPage
    {
        [NotNull]
        private readonly IConsole _console;

        /// <summary>
        /// Initializes a new instance of the <see cref="AlfredEventLogPage" /> class.
        /// </summary>
        /// <param name="console">The console.</param>
        /// <param name="name">The name.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public AlfredEventLogPage([NotNull] IConsole console, [NotNull] string name) : base(name)
        {
            if (console == null)
            {
                throw new ArgumentNullException(nameof(console));
            }
            _console = console;
        }

        /// <summary>
        /// Gets the console.
        /// </summary>
        /// <value>The console.</value>
        [NotNull]
        [UsedImplicitly]
        public IConsole Console
        {
            get { return _console; }
        }

        /// <summary>
        /// Gets the console events.
        /// </summary>
        /// <value>The events.</value>
        [NotNull, ItemNotNull]
        [UsedImplicitly]
        public IEnumerable<ConsoleEvent> Events
        {
            get { return _console.Events; }
        }


        /// <summary>
        /// Gets the children of this component. Depending on the type of component this is, the children will
        /// vary in their own types.
        /// </summary>
        /// <value>The children.</value>
        public override IEnumerable<AlfredComponent> Children
        {
            get
            {
                yield break;
            }
        }

    }
}