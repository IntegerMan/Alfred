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
using MattEland.Common.Providers;
using MattEland.Ani.Alfred.Core.Definitions;

namespace MattEland.Ani.Alfred.Core.Widgets
{
    /// <summary>
    ///     Contains common parameters needed to create any <see cref="WidgetBase" />
    /// </summary>
    public class WidgetCreationParameters : IHasContainer<IAlfredContainer>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="WidgetCreationParameters" /> class.
        /// </summary>
        /// <param name="name"> The name. </param>
        /// <param name="container"> The container. </param>
        public WidgetCreationParameters(
            [NotNull] string name,
            [NotNull] IAlfredContainer container)
        {
            Name = name;
            Container = container;
        }

        /// <summary>
        ///     Gets the name of this instance of the widget.
        /// </summary>
        /// <value>The name.</value>
        [NotNull, UsedImplicitly]
        public string Name { get; }

        /// <summary>
        ///     Gets the container.
        /// </summary>
        /// <value>
        ///     The container.
        /// </value>
        [NotNull]
        public IAlfredContainer Container { get; }
    }
}