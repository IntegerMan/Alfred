// ---------------------------------------------------------
// WarningWidget.cs
// 
// Created on:      08/19/2015 at 9:31 PM
// Last Modified:   08/24/2015 at 11:39 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using JetBrains.Annotations;

namespace MattEland.Ani.Alfred.Core.Widgets
{
    /// <summary>
    ///     A widget used for displaying warnings or errors to the user.
    /// </summary>
    public sealed class WarningWidget : TextWidgetBase
    {

        /// <summary>
        ///     Initializes a new instance of the <see cref="TextWidgetBase" /> class.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        public WarningWidget([NotNull] WidgetCreationParameters parameters)
            : this(null, parameters)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="TextWidgetBase" /> class.
        /// </summary>
        /// <param name="text">The text of the control.</param>
        /// <param name="parameters">The parameters.</param>
        public WarningWidget([CanBeNull] string text, [NotNull] WidgetCreationParameters parameters)
            : base(parameters)
        {
            Text = text;
        }

        /// <summary>
        ///     Gets the name of the broad categorization or type that this item is.
        /// </summary>
        /// <example>
        ///     Some examples of ItemTypeName values might be "Folder", "Application", "User", etc.
        /// </example>
        /// <value>The item type's name.</value>
        public override string ItemTypeName
        {
            get { return "Warning Label"; }
        }
    }
}