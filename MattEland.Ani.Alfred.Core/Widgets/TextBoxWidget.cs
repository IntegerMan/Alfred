using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.Core.Widgets
{
    /// <summary>
    ///     A text box widget. This class cannot be inherited.
    /// </summary>
    public sealed class TextBoxWidget : TextWidgetBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="TextWidgetBase" /> class.
        /// </summary>
        /// <param name="parameters"> Widget creation parameters. </param>
        public TextBoxWidget([NotNull] WidgetCreationParameters parameters) : this(string.Empty, parameters)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="TextWidgetBase" /> class.
        /// </summary>
        /// <param name="text"> The text. </param>
        /// <param name="parameters"> Widget creation parameters. </param>
        public TextBoxWidget(string text, [NotNull] WidgetCreationParameters parameters) : base(parameters)
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
            get { return "Text Box"; }
        }
    }
}