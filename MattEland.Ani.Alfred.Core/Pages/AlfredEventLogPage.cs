// ---------------------------------------------------------
// AlfredEventLogPage.cs
// 
// Created on:      08/19/2015 at 9:31 PM
// Last Modified:   08/26/2015 at 1:09 PM
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

        [NotNull]
        private readonly ICollection<IPropertyProvider> _providers;

        /// <summary>
        ///     The last time from a logged event that has been moved to the _providers collection.
        /// </summary>
        private DateTime _lastTimeLogged = DateTime.MinValue;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AlfredEventLogPage" /> class.
        /// </summary>
        /// <param name="platform">The platform provider.</param>
        /// <param name="console">The console.</param>
        /// <param name="name">The name.</param>
        /// <exception cref="System.ArgumentNullException">
        /// </exception>
        public AlfredEventLogPage(
            [NotNull] IPlatformProvider platform,
            [NotNull] IConsole console,
            [NotNull] string name) : base(name, "Log")
        {
            //- Validate
            if (platform == null) { throw new ArgumentNullException(nameof(platform)); }
            if (console == null) { throw new ArgumentNullException(nameof(console)); }

            _console = console;
            _providers = platform.CreateCollection<IPropertyProvider>();
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
            get { return _providers; }
        }

        /// <summary>
        ///     Updates the component
        /// </summary>
        protected override void UpdateProtected()
        {
            base.UpdateProtected();

            AddNewEventsToProviders();
        }

        /// <summary>
        ///     Adds new events to the providers collection. This is necessary because we're doing a cast to
        ///     IPropertyProvider and can't rely on any observable collection to relay this information.
        /// </summary>
        private void AddNewEventsToProviders()
        {
            // Find new events that are IPropertyProviders
            var newEvents =
                _console.Events.Where(e => e.Time > _lastTimeLogged && e is IPropertyProvider)
                        .ToList();

            if (newEvents.Any())
            {
                // Gets the events in order with casting
                var newProviders = newEvents.OrderBy(e => e.Time).Cast<IPropertyProvider>();

                foreach (var provider in newProviders.Where(provider => provider != null))
                {
                    _providers.Add(provider);
                }

                // Update our last logged time
                _lastTimeLogged = newEvents.Max(e => e.Time);
            }
        }
    }
}