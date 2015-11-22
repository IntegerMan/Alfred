// ---------------------------------------------------------
// NotImplementedScreenViewModel.cs
// 
// Created on:      11/21/2015 at 10:00 PM
// Last Modified:   11/21/2015 at 10:00 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using MattEland.Ani.Alfred.MFDMockUp.Models.Screens;
using MattEland.Ani.Alfred.PresentationCommon.Helpers;
using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.ViewModels.Screens
{
    /// <summary>
    ///     A ViewModel for the not implemented screen. This class cannot be inherited.
    /// </summary>
    [ViewModelFor(typeof(NotImplementedScreenModel))]
    public sealed class NotImplementedScreenViewModel : ScreenViewModel
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public NotImplementedScreenViewModel([NotNull] ScreenModel screenModel) : base(screenModel)
        {
        }
    }
}