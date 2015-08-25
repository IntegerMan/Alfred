// ---------------------------------------------------------
// AlfredTextWidget.cs
// 
// Created on:      08/19/2015 at 9:31 PM
// Last Modified:   08/24/2015 at 11:35 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using JetBrains.Annotations;

namespace MattEland.Ani.Alfred.Core.Widgets
{
    /// <summary>
    ///     Represents a widget that operates off of a Text field to present its contents.
    /// </summary>
    public abstract class AlfredTextWidget : AlfredWidget
    {

        [CanBeNull]
        private string _text;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AlfredTextWidget" /> class.
        /// </summary>
        protected AlfredTextWidget([NotNull] WidgetCreationParameters parameters)
            : base(parameters)
        {
        }

        /// <summary>
        ///     Gets or sets the text of the widget.
        /// </summary>
        /// <value>The text.</value>
        [CanBeNull]
        public string Text
        {
            get { return _text; }
            set
            {
                if (value != _text)
                {
                    _text = value;
                    OnPropertyChanged(nameof(Text));
                }
            }
        }
    }
}