using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MattEland.Ani.Alfred.MFDMockUp.Models;
using MattEland.Ani.Alfred.PresentationCommon.Helpers;
using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.ViewModels.Screens
{
    /// <summary>
    ///     A ViewModel for the bootup screen.
    /// </summary>
    [ViewModelFor(typeof(BootupScreenModel))]
    public class BootupScreenViewModel
    {
        /// <summary>
        ///     The model.
        /// </summary>
        [NotNull]
        private readonly BootupScreenModel _model;

        /// <summary>
        ///     Initializes a new instance of the BootupScreenViewModel class.
        /// </summary>
        /// <param name="model"> The model. </param>
        public BootupScreenViewModel([NotNull] BootupScreenModel model)
        {
            _model = model;
        }

        public double Progress { get { return _model.Progress; } }
    }

}
