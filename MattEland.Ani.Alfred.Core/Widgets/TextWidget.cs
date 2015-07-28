namespace MattEland.Ani.Alfred.Core.Widgets
{
    /// <summary>
    /// Represents a simple textual display on the user interface.
    /// </summary>
    public class TextWidget : AlfredWidget
    {
        private string _text;

        /// <summary>
        /// Gets or sets the text of the widget.
        /// </summary>
        /// <value>The text.</value>
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