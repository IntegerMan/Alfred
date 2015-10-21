﻿// ---------------------------------------------------------
// ButtonModel.cs
// 
// Created on:      10/20/2015 at 1:34 PM
// Last Modified:   10/20/2015 at 1:34 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Diagnostics;

using Assisticant.Fields;

using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.Models
{
    /// <summary>
    ///     A model for buttons.
    /// </summary>
    public sealed class ButtonModel
    {
        /// <summary>
        ///     Initializes a new instance of the ButtonModel class without a label.
        /// </summary>
        [UsedImplicitly]
        public ButtonModel() : this(string.Empty)
        {

        }

        /// <summary>
        ///     Initializes a new instance of the ButtonModel class.
        /// </summary>
        /// <param name="text"> The text. </param>
        public ButtonModel(string text) : this(text, false, 0)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the ButtonModel class.
        /// </summary>
        /// <param name="text"> The text. </param>
        /// <param name="isSelected"> true if this instance is selected. </param>
        public ButtonModel(string text, bool isSelected) : this(text, isSelected, 0)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the ButtonModel class.
        /// </summary>
        /// <param name="text"> The text. </param>
        /// <param name="isSelected"> true if this instance is selected. </param>
        /// <param name="index"> The button's index. </param>
        public ButtonModel([CanBeNull] string text, bool isSelected, int index)
        {
            _text = new Observable<string>(text);
            _isSelected = new Observable<bool>(isSelected);
            _index = new Observable<int>(index);
        }

        [NotNull]
        private readonly Observable<string> _text;

        [NotNull]
        private readonly Observable<bool> _isSelected;

        [NotNull]
        private readonly Observable<int> _index;

        /// <summary>
        ///     Sets the text of the button
        /// </summary>
        /// <value>
        ///     The text.
        /// </value>
        [NotNull]
        public string Text
        {
            [DebuggerStepThrough]
            get
            { return _text; }
            set { _text.Value = value; }
        }

        /// <summary>
        ///     Sets a value indicating whether this button's label should appear as selected.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is selected, <c>false</c> if not.
        /// </value>
        [NotNull]
        public bool IsSelected
        {
            [DebuggerStepThrough]
            get
            { return _isSelected; }
            set { _isSelected.Value = value; }
        }

        /// <summary>
        ///     Gets or sets the zero-based index of this button within a button strip.
        /// </summary>
        /// <value>
        ///     The button's index.
        /// </value>
        public int Index
        {
            get { return _index; }
            set { _index.Value = value; }
        }
    }
}