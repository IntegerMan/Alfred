// ---------------------------------------------------------
// WidgetCreationParameters.cs
// 
// Created on:      08/24/2015 at 11:32 PM
// Last Modified:   08/24/2015 at 11:32 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Globalization;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Console;

namespace MattEland.Ani.Alfred.Core.Widgets
{
    /// <summary>
    ///     Contains common parameters needed to create any <see cref="AlfredWidget" />
    /// </summary>
    public class WidgetCreationParameters
    {

        /// <summary>
        ///     Initializes a new instance of the <see cref="WidgetCreationParameters" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="console">The console.</param>
        /// <param name="locale">The locale.</param>
        public WidgetCreationParameters(
            [NotNull] string name,
            [CanBeNull] IConsole console = null,
            [CanBeNull] CultureInfo locale = null)
        {
            Name = name;
            Console = console;
            Locale = locale ?? CultureInfo.CurrentCulture;
        }

        /// <summary>
        ///     Gets the name of this instance of the widget.
        /// </summary>
        /// <value>The name.</value>
        [NotNull]
        public string Name { get; }

        /// <summary>
        ///     Gets the logging console.
        /// </summary>
        /// <value>The console.</value>
        [CanBeNull]
        public IConsole Console { get; }

        /// <summary>
        ///     Gets the locale.
        /// </summary>
        /// <value>The locale.</value>
        [NotNull]
        public CultureInfo Locale { get; }
    }
}