// ---------------------------------------------------------
// EventLogPage.cs
// 
// Created on:      08/19/2015 at 9:31 PM
// Last Modified:   09/03/2015 at 5:41 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Common.Providers;

namespace MattEland.Ani.Alfred.Core.Pages
{
    /// <summary>
    ///     An event logging page. This will need a special client-side implementation to list out
    ///     the details
    /// </summary>
    public sealed class EventLogPage : AlfredPage
    {
        [CanBeNull]
        private IConsole _console;

        /// <summary>
        ///     Initializes a new instance of the <see cref="EventLogPage" /> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="name">The name.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when one or more required arguments are null.
        /// </exception>
        public EventLogPage([NotNull] IAlfredContainer container, [NotNull] string name)
            : base(container, name, "Log")
        {
            if (container == null) { throw new ArgumentNullException(nameof(container)); }

            ReceivedEvents = container.ProvideCollection<IPropertyProvider>();
        }

        /// <summary>
        ///     Gets the console.
        /// </summary>
        /// <value>
        /// The console.
        /// </value>
        [CanBeNull]
        private IConsole Console
        {
            get
            {
                if (_console == null)
                {
                    _console = Container.Console;
                }

                return _console;
            }
        }

        /// <summary>
        ///     The events that have already been received and logged.
        /// </summary>
        /// <remarks>
        ///     TODO: It might make sense to purge events older than a day if this app perpetually
        ///     runs.
        /// </remarks>
        /// <value>
        /// The received events.
        /// </value>
        [NotNull]
        private ICollection<IPropertyProvider> ReceivedEvents { get; }

        /// <summary>
        ///     The last time from a logged event that has been moved to the _providers collection.
        /// </summary>
        public DateTime LastTimeLogged { get; private set; } = DateTime.MinValue;

        /// <summary>
        ///     Gets the console events.
        /// </summary>
        /// <value>
        /// The events.
        /// </value>
        [NotNull]
        [ItemNotNull]
        [UsedImplicitly]
        public IEnumerable<IConsoleEvent> Events
        {
            get
            {
                // If we don't have a console, we'll have to just provide an empty collection
                return Console?.Events ?? Container.ProvideCollection<IConsoleEvent>();
            }
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
            get { yield break; }
        }

        /// <summary>
        ///     Gets the property providers nested inside of this property provider.
        /// </summary>
        /// <value>
        /// The property providers.
        /// </value>
        public override IEnumerable<IPropertyProvider> PropertyProviders
        {
            get { return ReceivedEvents; }
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
        ///     Adds new events to the providers collection. This is necessary because we're doing a
        ///     cast to <see cref="IPropertyProvider" /> and can't rely on any observable collection
        ///     to relay this information.
        /// </summary>
        private void AddNewEventsToProviders()
        {
            // Find new (since last check) events that are IPropertyProviders
            var consoleEvents = Events.Where(e => e.Time > LastTimeLogged && e is IPropertyProvider);
            var newEvents = consoleEvents.ToList();

            // Nothing to do if nothing is new
            if (!newEvents.Any()) { return; }

            // Gets the events in order with casting
            AddNewEvents(newEvents);
        }

        /// <summary>
        ///     Adds the newly-detected events to log page.
        /// </summary>
        /// <param name="newEvents">The new events.</param>
        private void AddNewEvents([NotNull] IList<IConsoleEvent> newEvents)
        {
            var newProviders = newEvents.OrderBy(e => e.Time).Cast<IPropertyProvider>();

            foreach (var provider in newProviders.Where(provider => provider != null))
            {
                ReceivedEvents.Add(provider);
            }

            // Update our last logged time
            LastTimeLogged = newEvents.Max(e => e.Time);
        }
    }
}