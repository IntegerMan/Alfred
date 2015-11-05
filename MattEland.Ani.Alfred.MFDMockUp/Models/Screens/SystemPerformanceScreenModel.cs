using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MattEland.Ani.Alfred.MFDMockUp.Models.Screens
{
    /// <summary>
    ///     A data Model for the system performance screen. This class cannot be inherited.
    /// </summary>
    public sealed class SystemPerformanceScreenModel : ScreenModel
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public SystemPerformanceScreenModel() : base("PERF")
        {
        }

        /// <summary>
        ///     Process the screen state and outputs any resulting information to the processorResult.
        /// </summary>
        /// <param name="processor"> The processor. </param>
        /// <param name="processorResult"> The processor result. </param>
        protected override void ProcessScreenState(MFDProcessor processor, MFDProcessorResult processorResult)
        {
        }
    }
}
