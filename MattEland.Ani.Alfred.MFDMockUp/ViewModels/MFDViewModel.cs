// ---------------------------------------------------------
// MFDViewModel.cs
// 
// Created on:      10/18/2015 at 12:18 AM
// Last Modified:   10/18/2015 at 12:18 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------
using MattEland.Ani.Alfred.MFDMockUp.Models;
using MattEland.Common.Annotations;
using System;
namespace MattEland.Ani.Alfred.MFDMockUp.ViewModels
{
    /// <summary>
    ///     A ViewModel for a <see cref="MultifunctionDisplay"/>.
    /// </summary>
    [PublicAPI]
    public class MFDViewModel
    {
        /// <summary>
        ///     Gets the <see cref="MultifunctionDisplay"/> model backing this view model.
        /// </summary>
        /// <value>
        ///     The model.
        /// </value>
        [NotNull]
        public MultifunctionDisplay Model
        {
            get
            {
                return _mfd;
            }
        }

        /// <summary>
        ///     The MFD model backing store.
        /// </summary>
        [NotNull]
        private readonly MultifunctionDisplay _mfd;

        /// <summary>
        /// Initializes a new instance of the <see cref="MFDViewModel"/> class.
        /// </summary>
        /// <param name="mfd">The multifunction display.</param>
        public MFDViewModel([NotNull] MultifunctionDisplay mfd)
        {
            _mfd = mfd;
        }
    }
}