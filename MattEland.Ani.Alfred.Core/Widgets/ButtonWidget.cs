// ---------------------------------------------------------
// ButtonWidget.cs
// 
// Created on:      08/03/2015 at 1:10 AM
// Last Modified:   08/03/2015 at 1:51 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using JetBrains.Annotations;

namespace MattEland.Ani.Alfred.Core.Widgets
{
    /// <summary>
    ///     Represents a button that's clickable in the user interface.
    /// </summary>
    public sealed class ButtonWidget : AlfredWidget
    {
        /// <summary>
        ///     The command that is executed when the button is clicked.
        /// </summary>
        [CanBeNull]
        private AlfredCommand _clickCommand;

        /// <summary>
        /// The text of the button
        /// </summary>
        [CanBeNull]
        private string _text;

        /// <summary>
        ///     Initializes a new instance of the
        ///     <see
        ///         cref="ButtonWidget" />
        ///     class.
        /// </summary>
        public ButtonWidget()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the
        ///     <see
        ///         cref="ButtonWidget" />
        ///     class.
        /// </summary>
        /// <param
        ///     name="text">
        ///     The button's text.
        /// </param>
        public ButtonWidget([CanBeNull] string text) : this()
        {
            _text = text;
        }

        /// <summary>
        ///     Gets or sets the text of the button.
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

        /// <summary>
        ///     Gets or sets the command that is executed when the button is clicked.
        /// </summary>
        /// <value>The click command.</value>
        [CanBeNull]
        public AlfredCommand ClickCommand
        {
            get { return _clickCommand; }
            set
            {
                if (!Equals(value, _clickCommand))
                {
                    _clickCommand = value;
                    OnPropertyChanged(nameof(ClickCommand));
                }
            }
        }
    }
}