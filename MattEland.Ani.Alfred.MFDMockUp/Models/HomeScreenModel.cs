using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MattEland.Ani.Alfred.MFDMockUp.Models
{
    /// <summary>
    ///     A data model for the home screen. This class cannot be inherited.
    /// </summary>
    public sealed class HomeScreenModel : ScreenModel
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public HomeScreenModel() : base("HOME")
        {
        }
        /// <summary>
        ///     Process the screen state and outputs any resulting information to the processorResult.
        /// </summary>
        /// <param name="processor"> The processor. </param>
        /// <param name="processorResult"> The processor result. </param>
        protected override void ProcessScreenState(MFDProcessor processor, MFDProcessorResult processorResult)
        {
            // Do nothing
        }
    }
}
