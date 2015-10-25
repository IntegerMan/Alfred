// ---------------------------------------------------------
// SeparatorModel.cs
// 
// Created on:      10/21/2015 at 12:49 AM
// Last Modified:   10/21/2015 at 12:49 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using Assisticant.Fields;

using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.Models.Buttons
{
    /// <summary>
    ///     A data model for a MFD button strip separator.
    /// </summary>
    public class SeparatorModel
    {
        /// <summary>
        ///     Initializes a new instance of the SeparatorModel class.
        /// </summary>
        public SeparatorModel() : this(true)
        {

        }

        /// <summary>
        ///     Initializes a new instance of the SeparatorModel class.
        /// </summary>
        /// <param name="isVisible"> true if this instance is visible, false if not. </param>
        public SeparatorModel(bool isVisible)
        {
            _isVisible = new Observable<bool>(isVisible);
        }

        /// <summary>
        ///     The is visible property.
        /// </summary>
        [NotNull]
        private readonly Observable<bool> _isVisible;

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is visible.
        /// </summary>
        /// <value>
        ///     true if this instance is visible, false if not.
        /// </value>
        public bool IsVisible
        {
            get { return _isVisible; }
            set { _isVisible.Value = value; }
        }
    }
}