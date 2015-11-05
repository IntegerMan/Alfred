using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Assisticant.Collections;

using MattEland.Ani.Alfred.MFDMockUp.Models.Screens;
using MattEland.Ani.Alfred.PresentationCommon.Helpers;
using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.ViewModels.Screens
{
    /// <summary>
    ///     A ViewModel for the system performance screen. This class cannot be inherited.
    /// </summary>
    [ViewModelFor(typeof(SystemPerformanceScreenModel))]
    public sealed class SystemPerformanceScreenViewModel : ScreenViewModel
    {
        /// <summary>
        ///     The performance screen model.
        /// </summary>
        [NotNull]
        private readonly SystemPerformanceScreenModel _model;

        /// <summary>
        ///     The performance widgets collection.
        /// </summary>
        [NotNull]
        private readonly ObservableList<object> _performanceWidgets;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <param name="screenModel"> The screen model. </param>
        public SystemPerformanceScreenViewModel([NotNull] SystemPerformanceScreenModel screenModel)
            : base(screenModel)
        {
            Contract.Requires(screenModel != null);

            _model = screenModel;
            _performanceWidgets = new ObservableList<object>();
        }

        /// <summary>
        ///     Gets the performance widgets.
        /// </summary>
        /// <value>
        ///     The performance widgets.
        /// </value>
        [NotNull, ItemNotNull]
        public IEnumerable<object> PerformanceWidgets
        {
            get { return _performanceWidgets; }
        }
    }
}
