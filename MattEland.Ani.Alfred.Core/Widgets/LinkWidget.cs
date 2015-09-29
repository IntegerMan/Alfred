using System;
using System.Collections.Generic;
using System.Linq;

namespace MattEland.Ani.Alfred.Core.Widgets
{
    /// <summary>
    /// Represents a hyperlink control in the user interface
    /// </summary>
    public sealed class LinkWidget : TextWidgetBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LinkWidget" /> class.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        public LinkWidget(WidgetCreationParameters parameters) : base(parameters)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LinkWidget" /> class.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="parameters">The parameters.</param>
        public LinkWidget(string text, WidgetCreationParameters parameters) : base(text, parameters)
        {

        }

        /// <summary>
        /// Gets the name of the broad categorization or type that this item is.
        /// </summary>
        /// <example>
        /// Some examples of ItemTypeName values might be "Folder", "Application", "User", etc.
        /// </example>
        /// <value>The item type's name.</value>
        public override string ItemTypeName
        {
            get
            {
                return "Hyperlink";
            }
        }
    }
}
