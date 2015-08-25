// ---------------------------------------------------------
// ButtonWidget.cs
// 
// Created on:      08/19/2015 at 9:31 PM
// Last Modified:   08/24/2015 at 11:26 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Console;
using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Common;

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
        /// Initializes a new instance of the <see cref="AlfredTextWidget" /> class.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        public ButtonWidget([NotNull] WidgetCreationParameters parameters) : base(parameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ButtonWidget" /> class.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="clickCommand">The click command.</param>
        /// <param name="parameters">The parameters.</param>
        public ButtonWidget(
            [CanBeNull] string text,
            [CanBeNull] AlfredCommand clickCommand,
            [NotNull] WidgetCreationParameters parameters) : base(parameters)
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
        ///     Simulates a button click
        /// </summary>
        /// <exception cref="System.InvalidOperationException">
        ///     Tried to click the button when CanExecute on ClickCommand returned false.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     Tried to click the button when CanExecute on
        ///     ClickCommand returned false.
        /// </exception>
        [SuppressMessage("ReSharper", "CatchAllClause")]
        public void Click()
        {
            if (ClickCommand != null)
            {
                if (!ClickCommand.CanExecute(this))
                {
                    throw new InvalidOperationException(
                        Resources.ButtonWidgetClickCantExecuteErrorMessage);
                }

                try
                {
                    ClickCommand.Execute(this);
                }
                catch (Exception exception)
                {
                    Error("Button.Click", exception.BuildDetailsMessage());
                }
            }
        }
    }
}