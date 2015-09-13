// ---------------------------------------------------------
// ButtonWidget.cs
// 
// Created on:      08/19/2015 at 9:31 PM
// Last Modified:   08/24/2015 at 11:26 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;

namespace MattEland.Ani.Alfred.Core.Widgets
{
    /// <summary>
    ///     Represents a button that the user can click in the user interface.
    /// </summary>
    public sealed class ButtonWidget : TextWidgetBase
    {
        /// <summary>
        ///     The command that is executed when the button is clicked.
        /// </summary>
        [CanBeNull]
        private IAlfredCommand _clickCommand;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TextWidgetBase" /> class.
        /// </summary>
        /// <param name="parameters"> The parameters. </param>
        public ButtonWidget([NotNull] WidgetCreationParameters parameters) : base(parameters)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ButtonWidget" /> class.
        /// </summary>
        /// <param name="text"> The text. </param>
        /// <param name="clickCommand"> The click command. </param>
        /// <param name="parameters"> The parameters. </param>
        public ButtonWidget(
            [CanBeNull] string text,
            [CanBeNull] IAlfredCommand clickCommand,
            [NotNull] WidgetCreationParameters parameters) : base(parameters)
        {
            ClickCommand = clickCommand;
            Text = text;
        }

        /// <summary>
        ///     Gets or sets the command that is executed when the button is clicked.
        /// </summary>
        /// <value>
        ///     The click command.
        /// </value>
        [CanBeNull]
        public IAlfredCommand ClickCommand
        {
            get
            {
                //Debug.Assert(_clickCommand != null);
                return _clickCommand;
            }
            set
            {
                if (Equals(value, _clickCommand)) return;

                // Perform the change and notify anyone who is interested
                _clickCommand = value;
                OnPropertyChanged(nameof(ClickCommand));
            }
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
            get { return "Button"; }
        }

        /// <summary>
        ///     Simulates a button click.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        ///     Tried to click the button when CanExecute on <see cref="ClickCommand"/> returned false.
        /// </exception>
        [SuppressMessage("ReSharper", "CatchAllClause")]
        public void Click()
        {
            if (ClickCommand == null) return;

            if (!ClickCommand.CanExecute(this))
            {
                throw new InvalidOperationException("Tried to click the button when CanExecute on ClickCommand returned false.");
            }

            try
            {
                ClickCommand.Execute(this);
            }
            catch (Exception exception)
            {
                HandleCallbackException(exception, nameof(Click));
            }
        }
    }
}