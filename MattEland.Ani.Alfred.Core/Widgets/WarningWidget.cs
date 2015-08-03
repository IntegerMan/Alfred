using JetBrains.Annotations;

namespace MattEland.Ani.Alfred.Core.Widgets
{
    /// <summary>
    /// A widget used for displaying warnings or errors to the user.
    /// </summary>
    public sealed class WarningWidget : AlfredTextWidget
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WarningWidget"/> class.
        /// </summary>
        public WarningWidget() : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WarningWidget"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        public WarningWidget([CanBeNull] string text)
        {
            Text = text;
        }
    }
}