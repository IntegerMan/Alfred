// ---------------------------------------------------------
// ButtonWidget.cs
// 
// Created on:      08/03/2015 at 1:10 AM
// Last Modified:   08/03/2015 at 2:16 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;

using JetBrains.Annotations;

namespace MattEland.Ani.Alfred.Core.Widgets
{
    /// <summary>
    ///     Represents a button that's clickable in the user interface.
    /// </summary>
    public sealed class ButtonWidget : AlfredTextWidget
    {
        /// <summary>
        ///     The command that is executed when the button is clicked.
        /// </summary>
        [CanBeNull]
        private AlfredCommand _clickCommand;

        /// <summary>
        ///     Initializes a new instance of the
        ///     <see
        ///         cref="ButtonWidget" />
        ///     class.
        /// </summary>
        public ButtonWidget() : this(null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ButtonWidget"/> class.
        /// </summary>
        /// <param name="clickCommand">The command.</param>
        public ButtonWidget([CanBeNull] AlfredCommand clickCommand) : this(null, clickCommand)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ButtonWidget"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        public ButtonWidget([CanBeNull] string text) : this(text, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ButtonWidget"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="clickCommand">The click command.</param>
        public ButtonWidget([CanBeNull] string text, [CanBeNull] AlfredCommand clickCommand)
        {
            ClickCommand = clickCommand;
            Text = text;
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

        /// <summary>
        ///     Simulates a button click
        /// </summary>
        /// <exception
        ///     cref="System.InvalidOperationException">
        ///     Tried to click the button when CanExecute on ClickCommand returned false.
        /// </exception>
        public void Click()
        {
            if (ClickCommand != null)
            {
                if (!ClickCommand.CanExecute(this))
                {
                    throw new InvalidOperationException(
                        "Tried to click the button when CanExecute on ClickCommand returned false.");
                }

                ClickCommand.Execute(this);
            }
        }
    }
}