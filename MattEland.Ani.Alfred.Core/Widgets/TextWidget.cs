using JetBrains.Annotations;

namespace MattEland.Ani.Alfred.Core.Widgets
{
    /// <summary>
    /// Represents a simple textual display on the user interface.
    /// </summary>
    public sealed class TextWidget : AlfredTextWidget
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextWidget"/> class.
        /// </summary>
        public TextWidget() : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextWidget"/> class.
        /// </summary>
        /// <param name="text">The text for the widget.</param>
        public TextWidget([CanBeNull] string text)
        {
            Text = text;
        }
    }
}