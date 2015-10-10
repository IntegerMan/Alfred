// ---------------------------------------------------------
// ConsoleEventExplorerDecorator.cs
// 
// Created on:      08/26/2015 at 12:31 PM
// Last Modified:   08/26/2015 at 12:31 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;

using MattEland.Common.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;

namespace MattEland.Ani.Alfred.Core.Console
{
    /// <summary>
    ///     A decorator that wraps an <see cref="IConsoleEvent" /> to expose its properties while providing
    ///     an <see cref="IPropertyProvider" /> implementation that allows the explorer to render nodes for
    ///     events.
    /// </summary>
    public sealed class ConsoleEventExplorerDecorator : IConsoleEvent, IPropertyProvider
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ConsoleEventExplorerDecorator" /> class.
        /// </summary>
        /// <param name="consoleEvent">The console event.</param>
        /// <exception cref="ArgumentNullException"><paramref name="consoleEvent"/> is <see langword="null" />.</exception>
        public ConsoleEventExplorerDecorator([NotNull] IConsoleEvent consoleEvent)
        {
            if (consoleEvent == null) { throw new ArgumentNullException(nameof(consoleEvent)); }

            ConsoleEvent = consoleEvent;
        }

        /// <summary>
        ///     Gets the decorated console event.
        /// </summary>
        /// <value>The console event.</value>
        [NotNull]
        private IConsoleEvent ConsoleEvent { get; }

        /// <summary>
        ///     Gets the logging level.
        /// </summary>
        /// <value>The level.</value>
        public LogLevel Level
        {
            get { return ConsoleEvent.Level; }
        }

        /// <summary>
        ///     Gets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message
        {
            get { return ConsoleEvent.Message; }
        }

        /// <summary>
        ///     Gets the time in local system time.
        /// </summary>
        /// <value>The time.</value>
        public DateTime Time
        {
            get { return ConsoleEvent.Time; }
        }

        /// <summary>
        ///     Gets the title of the event.
        /// </summary>
        /// <value>The title.</value>
        public string Title
        {
            get { return ConsoleEvent.Title; }
        }

        /// <summary>
        ///     Gets the time the event was logged in UTC.
        /// </summary>
        /// <value>The time the event was logged in UTC.</value>
        public DateTime UtcTime
        {
            get { return ConsoleEvent.UtcTime; }
        }

        /// <summary>
        ///     Gets the display name for use in the user interface.
        /// </summary>
        /// <value>The display name.</value>
        public string DisplayName
        {
            get { return Name; }
        }

        /// <summary>
        ///     Gets the name of the broad categorization or type for this instance.
        /// </summary>
        /// <example>
        ///     Some examples of ItemTypeName values might be "Folder", "Application", "User", etc.
        /// </example>
        /// <value>The item type's name.</value>
        public string ItemTypeName
        {
            get
            {
                return string.Format(CultureInfo.InvariantCulture, "{0} Event", Level);
                ;
            }
        }

        /// <summary>
        ///     Gets the name of the item.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return string.Format(CultureInfo.InvariantCulture, "{0}: {1}", Title, Message); }
        }

        /// <summary>
        ///     Gets a list of <see cref="IPropertyItem"/> objects provided by this node.
        /// </summary>
        /// <returns>The properties</returns>
        public IEnumerable<IPropertyItem> Properties
        {
            get
            {
                yield return new AlfredProperty("Title", Title);
                yield return new AlfredProperty("Message", Message);
                yield return new AlfredProperty("Level", Level);
                yield return new AlfredProperty("Created", Time);
            }
        }

        /// <summary>
        ///     Gets the property providers.
        /// </summary>
        /// <value>The property providers.</value>
        public IEnumerable<IPropertyProvider> PropertyProviders
        {
            get { yield break; }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString() { return ConsoleEvent.ToString(); }
    }
}