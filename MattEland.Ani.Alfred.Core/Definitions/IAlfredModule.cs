// ---------------------------------------------------------
// IAlfredModule.cs
// 
// Created on:      08/09/2015 at 6:17 PM
// Last Modified:   08/09/2015 at 6:19 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;

using MattEland.Common.Annotations;
using MattEland.Presentation.Logical.Widgets;

namespace MattEland.Ani.Alfred.Core.Definitions
{
    /// <summary>
    ///     Represents a module belonging to a page or subsystem in Alfred.
    /// </summary>
    public interface IAlfredModule : IAlfredComponent, IAlfredCommandRecipient
    {
        /// <summary>
        ///     Gets the user interface widgets for the module.
        /// </summary>
        /// <value>The user interface widgets.</value>
        [NotNull, ItemNotNull]
        IEnumerable<IWidget> Widgets { get; }

        /// <summary>
        ///     Gets the type of the layout.
        /// </summary>
        /// <value>
        ///     The type of the layout.
        /// </value>
        LayoutType LayoutType { get; }

        /// <summary>
        ///     Registers a widget for the module.
        /// </summary>
        /// <param name="widget">
        ///     The widget.
        /// </param>
        void Register([NotNull] IWidget widget);

        /// <summary>
        ///     Registers multiple widgets at once.
        /// </summary>
        /// <param name="widgets">
        ///     The widgets.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="widgets"/> is <see langword="null" />.</exception>
        void Register([NotNull, ItemNotNull] IEnumerable<IWidget> widgets);
    }
}