// ---------------------------------------------------------
// BuiltInTestsScreenViewModel.cs
// 
// Created on:      12/16/2015 at 10:28 PM
// Last Modified:   12/16/2015 at 10:28 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;

using MattEland.Ani.Alfred.MFDMockUp.Models.Screens;
using MattEland.Ani.Alfred.PresentationCommon.Helpers;
using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.ViewModels.Screens
{
    /// <summary>
    ///     A ViewModel for the built in tests screen. This class cannot be inherited.
    /// </summary>
    [ViewModelFor(typeof(BuiltInTestsScreenModel))]
    [UsedImplicitly]
    public sealed class BuiltInTestsScreenViewModel : ScreenViewModel
    {
        [NotNull]
        private readonly BuiltInTestsScreenModel _model;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:BuiltInTestsScreenViewModel"/> class.
        /// </summary>
        public BuiltInTestsScreenViewModel([NotNull] BuiltInTestsScreenModel screenModel) : base(screenModel)
        {
            if (screenModel == null) throw new ArgumentNullException(nameof(screenModel));

            _model = screenModel;
        }
    }
}