// ---------------------------------------------------------
// MFDSelection.cs
// 
// Created on:      10/18/2015 at 12:22 AM
// Last Modified:   10/18/2015 at 12:22 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using Assisticant.Fields;
using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.Models
{
    /// <summary>
    ///     Represents the selection of a Multifunction Display (MFD) among many present in a
    ///     <see cref="Workspace"/>.
    /// </summary>
    public sealed class MFDSelection
    {
        /// <summary>
        ///     The selected MFD.
        /// </summary>
        [NotNull]
        private readonly Observable<MultifunctionDisplay> _selectedMFD;

        /// <summary>
        /// Initializes a new instance of the <see cref="MFDSelection"/> class.
        /// </summary>
        public MFDSelection()
        {
            _selectedMFD = new Observable<MultifunctionDisplay>();
        }

        /// <summary>
        ///     Gets or sets the selected multifunction display (MFD).
        /// </summary>
        /// <value>
        ///     The selected MFD.
        /// </value>
        [CanBeNull]
        public MultifunctionDisplay SelectedMFD
        {
            get { return _selectedMFD; }
            set { _selectedMFD.Value = value; }
        }
    }
}