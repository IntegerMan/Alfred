using JetBrains.Annotations;

namespace MattEland.Ani.Alfred.Core.Widgets
{
    /// <summary>
    /// Represents a simple textual display on the user interface.
    /// </summary>
    public class TextWidget : AlfredWidget
    {
        [CanBeNull]
        private string _text;

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
            _text = text;
        }

        /// <summary>
        /// Gets or sets the text of the widget.
        /// </summary>
        /// <value>The text.</value>
        [CanBeNull]
        public string Text
        {
            get { return _text; }
            set
            {
                if (value == _text)
                {
                    return;
                }
                _text = value;
                OnPropertyChanged(nameof(Text));
            }
        }
    }
}