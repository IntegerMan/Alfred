using MattEland.Ani.Alfred.PresentationCommon.Helpers;
using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.ViewModels.Screens
{
    /// <summary>
    ///     A ViewModel for the bootup screen.
    /// </summary>
    [ViewModelFor(typeof(VersionInfoScreenViewModel))]
    public class VersionInfoScreenViewModel
    {
        /// <summary>
        ///     The model.
        /// </summary>
        [NotNull]
        private readonly VersionInfoScreenViewModel _model;

        /// <summary>
        ///     Initializes a new instance of the BootupScreenViewModel class.
        /// </summary>
        /// <param name="model"> The model. </param>
        public VersionInfoScreenViewModel([NotNull] VersionInfoScreenViewModel model)
        {
            _model = model;
        }

    }
}