using MattEland.Ani.Alfred.MFDMockUp.Models;
using MattEland.Ani.Alfred.PresentationCommon.Helpers;
using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.ViewModels.Screens
{
    /// <summary>
    ///     A ViewModel for the home screen.
    /// </summary>
    [ViewModelFor(typeof(HomeScreenModel))]
    public class HomeScreenViewModel
    {
        /// <summary>
        ///     The model.
        /// </summary>
        [NotNull]
        private readonly HomeScreenViewModel _model;

        /// <summary>
        ///     Initializes a new instance of the BootupScreenViewModel class.
        /// </summary>
        /// <param name="model"> The model. </param>
        public HomeScreenViewModel([NotNull] HomeScreenViewModel model)
        {
            _model = model;
        }

    }
}