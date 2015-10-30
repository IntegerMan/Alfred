// ---------------------------------------------------------
// SeparatorViewModel.cs
// 
// Created on:      10/21/2015 at 12:49 AM
// Last Modified:   10/21/2015 at 12:49 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Windows;

using MattEland.Ani.Alfred.MFDMockUp.Models;
using MattEland.Ani.Alfred.MFDMockUp.Models.Buttons;
using MattEland.Ani.Alfred.PresentationCommon.Helpers;
using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.ViewModels.Buttons
{
    /// <summary>
    ///     A separator view model representing a separator in the button list
    /// </summary>
    [ViewModelFor(typeof(SeparatorModel))]
    [PublicAPI]
    public sealed class SeparatorViewModel
    {
        /// <summary>
        ///     The model.
        /// </summary>
        [NotNull]
        private readonly SeparatorModel _model;

        /// <summary>
        ///     Initializes a new instance of the SeparatorViewModel class.
        /// </summary>
        /// <param name="model"> The model. </param>
        public SeparatorViewModel([NotNull] SeparatorModel model)
        {
            _model = model;
        }

        /// <summary>
        ///     Gets the visibility of the separator
        /// </summary>
        /// <value>
        ///     The visibility.
        /// </value>
        public Visibility Visibility
        {
            get
            {
                return _model.IsVisible ? Visibility.Visible : Visibility.Collapsed;
            }
        }
    }
}