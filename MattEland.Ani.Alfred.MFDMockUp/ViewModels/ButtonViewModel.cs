﻿// ---------------------------------------------------------
// ButtonViewModel.cs
// 
// Created on:      10/20/2015 at 1:42 PM
// Last Modified:   10/20/2015 at 1:42 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using MattEland.Ani.Alfred.MFDMockUp.Models;
using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.ViewModels
{
    /// <summary>
    ///     A view model for the <see cref="ButtonModel"/> model used for presenting a MFD button and
    ///     its associated label.
    /// </summary>
    public class ButtonViewModel
    {
        [NotNull]
        private readonly ButtonModel _model;

        /// <summary>
        ///     Initializes a new instance of the ButtonViewModel class.
        /// </summary>
        /// <param name="model"> The model. </param>
        public ButtonViewModel([NotNull] ButtonModel model)
        {
            _model = model;
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
    }
}