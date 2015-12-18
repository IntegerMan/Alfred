// ---------------------------------------------------------
// BuiltInTestsScreenViewModel.cs
// 
// Created on:      12/16/2015 at 10:28 PM
// Last Modified:   12/16/2015 at 10:28 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;

using MattEland.Ani.Alfred.Core.Definitions;
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

        [NotNull, ItemNotNull]
        private readonly ICollection<FaultIndicatorViewModel> _activeFaults;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:BuiltInTestsScreenViewModel"/> class.
        /// </summary>
        public BuiltInTestsScreenViewModel([NotNull] BuiltInTestsScreenModel screenModel)
            : base(screenModel)
        {
            if (screenModel == null) throw new ArgumentNullException(nameof(screenModel));

            _model = screenModel;
            _activeFaults = Container.ProvideCollection<FaultIndicatorViewModel>();
        }

        public IEnumerable<FaultIndicatorViewModel> FaultsOnCurrentPage
        {
            get
            {
                return _activeFaults;
            }
        }
    }
}