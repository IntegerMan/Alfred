// ---------------------------------------------------------
// ButtonWidget.cs
// 
// Created on:      08/03/2015 at 1:10 AM
// Last Modified:   08/03/2015 at 1:11 AM
// Original author: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;

using JetBrains.Annotations;

namespace MattEland.Ani.Alfred.Core.Widgets
{
    /// <summary>
    ///     Represents a button that's clickable in the user interface.
    /// </summary>
    public sealed class ButtonWidget : AlfredWidget
    {
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
        /// Initializes a new instance of the <see cref="ButtonWidget"/> class.
        /// </summary>
        /// <param name="text">The button's text.</param>
        public ButtonWidget([CanBeNull] string text) : this()
        {
            _text = text;
        }

        [CanBeNull]
        private string _text;

        /// <summary>
        /// Gets or sets the text of the button.
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