// ---------------------------------------------------------
// ButtonViewModel.cs
// 
// Created on:      10/20/2015 at 1:42 PM
// Last Modified:   10/20/2015 at 1:42 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Linq;
using System.Windows;

using MattEland.Ani.Alfred.MFDMockUp.Models;
using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.ViewModels
{
    /// <summary>
    ///     A view model for the <see cref="ButtonModel"/> model used for presenting a MFD button and
    ///     its associated label.
    /// </summary>
    public sealed class ButtonViewModel
    {
        [NotNull]
        private readonly ButtonModel _model;

        [NotNull]
        private readonly MFDButtonStripViewModel _parent;

        /// <summary>
        ///     Initializes a new instance of the ButtonViewModel class.
        /// </summary>
        /// <param name="model"> The model. </param>
        public ButtonViewModel([NotNull] ButtonModel model, [NotNull] MFDButtonStripViewModel parent)
        {
            _model = model;
            _parent = parent;
        }

        /// <summary>
        ///     Gets or sets the text of the button.
        /// </summary>
        /// <value>
        ///     The text of the button.
        /// </value>
        public string Text
        {
            get { return _model.Text; }
            set { _model.Text = value; }
        }

        /// <summary>
        ///     Gets the tool tip text to display when the mouse is over the button.
        /// </summary>
        /// <value>
        ///     The tool tip text.
        /// </value>
        public string ToolTipText
        {
            get { return _model.Text; }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether this button is selected.
        /// </summary>
        /// <value>
        ///     true if this button is selected, false if not.
        /// </value>
        public bool IsSelected
        {
            get
            {
                return _model.IsSelected;
            }
            set
            {
                _model.IsSelected = value;
            }
        }

        /// <summary>
        ///     Gets the Visibility of the separator between buttons. This separator should not be
        ///     rendered between items.
        /// </summary>
        /// <value>
        ///     The separator visibility.
        /// </value>
        public Visibility SeparatorVisibility
        {
            get
            {
                if (Index + 1 < _parent.Buttons.Count())
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
        }

        /// <summary>
        ///     Gets the zero-based index of this button within a button strip.
        /// </summary>
        /// <value>
        ///     The index.
        /// </value>
        public int Index
        {
            get { return _model.Index; }
        }
    }
}