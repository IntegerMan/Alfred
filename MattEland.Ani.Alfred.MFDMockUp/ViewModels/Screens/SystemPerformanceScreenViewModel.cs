using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MattEland.Ani.Alfred.MFDMockUp.Models.Screens;
using MattEland.Ani.Alfred.PresentationCommon.Helpers;
using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.ViewModels.Screens
{
    [ViewModelFor(typeof(SystemPerformanceScreenModel))]
    public sealed class SystemPerformanceScreenViewModel : ScreenViewModel
    {
        [NotNull]
        private readonly SystemPerformanceScreenModel _model;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <param name="screenModel"> The screen model. </param>
        public SystemPerformanceScreenViewModel([NotNull] SystemPerformanceScreenModel screenModel)
            : base(screenModel)
        {
            Contract.Requires(screenModel != null);

            _model = screenModel;
        }
    }
}
