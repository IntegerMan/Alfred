// ---------------------------------------------------------
// SystemPerformanceScreenModel.cs
// 
// Created on:      11/04/2015 at 9:06 PM
// Last Modified:   11/05/2015 at 1:57 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

using Assisticant.Collections;

using MattEland.Ani.Alfred.Core.Modules.SysMonitor;
using MattEland.Ani.Alfred.Core.Widgets;
using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.Models.Screens
{
    /// <summary>
    ///     A data Model for the system performance screen. This class cannot be inherited.
    /// </summary>
    public sealed class SystemPerformanceScreenModel : ScreenModel
    {
        /// <summary>
        ///     The system monitoring subsystem.
        /// </summary>
        [NotNull]
        private readonly SystemMonitoringSubsystem _subsystem;

        /// <summary>
        ///     The performance widgets.
        /// </summary>
        [NotNull, ItemNotNull]
        private readonly ObservableList<WidgetBase> _widgets;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <param name="subsystem"> The system monitoring subsystem. </param>
        public SystemPerformanceScreenModel([NotNull] SystemMonitoringSubsystem subsystem)
            : base("PERF")
        {
            Contract.Requires(subsystem != null);

            _subsystem = subsystem;
            _widgets = new ObservableList<WidgetBase>();
        }

        /// <summary>
        ///     Contains code contract invariants that describe facts about this class that will be true
        ///     after any public method in this class is called.
        /// </summary>
        [ContractInvariantMethod]
        private void ClassInvariants()
        {
            //- Subsystem cannot be null
            Contract.Invariant(_subsystem != null);

            //- Widgets is a collection without any null entity in it
            Contract.Invariant(_widgets != null);
            Contract.Invariant(_widgets.All(w => w != null));
        }

        /// <summary>
        ///     Process the screen state and outputs any resulting information to the processorResult.
        /// </summary>
        /// <param name="processor"> The processor. </param>
        /// <param name="processorResult"> The processor result. </param>
        protected override void ProcessScreenState(MFDProcessor processor,
                                                   MFDProcessorResult processorResult)
        {
        }
    }
}