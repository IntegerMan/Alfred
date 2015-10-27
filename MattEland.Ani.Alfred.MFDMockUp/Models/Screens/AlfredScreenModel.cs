using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MattEland.Ani.Alfred.Core;
using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.Models.Screens
{
    /// <summary>
    ///     A data model for the Alfred status screen. This class cannot be inherited.
    /// </summary>
    public sealed class AlfredScreenModel : ScreenModel
    {
        /// <summary>
        ///     Initializes a new instance of the AlfredScreenModel class.
        /// </summary>
        public AlfredScreenModel([NotNull] AlfredApplication alfredApplication) : base("ALF")
        {
            Contract.Requires(alfredApplication != null);

            AlfredApplication = alfredApplication;
        }

        /// <summary>
        ///     Process the screen state and outputs any resulting information to the processorResult.
        /// </summary>
        /// <param name="processor"> The processor. </param>
        /// <param name="processorResult"> The processor result. </param>
        protected override void ProcessScreenState(MFDProcessor processor, MFDProcessorResult processorResult)
        {
            // No operation (yet)
        }

        [NotNull]
        public AlfredApplication AlfredApplication
        {
            get;
        }
    }
}
