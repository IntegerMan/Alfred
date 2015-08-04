// ---------------------------------------------------------
// AlfredTextWidget.cs
// 
// Created on:      08/03/2015 at 2:46 PM
// Last Modified:   08/04/2015 at 3:04 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using JetBrains.Annotations;

namespace MattEland.Ani.Alfred.Core.Widgets
{
    /// <summary>
    ///     Represents a widget that operates off of a Text field to present its contents.
    /// </summary>
    public abstract class AlfredTextWidget : AlfredWidget
    {
        // Choosing to not include short-cut constructors to give base classes more freedom in defining their constructors

        [CanBeNull]
        private string _text;

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