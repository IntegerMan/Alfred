// ---------------------------------------------------------
// SystemPerformanceScreenModel.cs
// 
// Created on:      11/04/2015 at 9:06 PM
// Last Modified:   11/05/2015 at 4:22 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;

using Assisticant.Collections;
using Assisticant.Fields;

using MattEland.Ani.Alfred.Core.Modules.SysMonitor;
using MattEland.Common.Annotations;
using MattEland.Presentation.Logical.Widgets;

namespace MattEland.Ani.Alfred.MFDMockUp.Models.Screens
{
    /// <summary>
    ///     A data Model for the system performance screen. This class cannot be inherited.
    /// </summary>
    public sealed class SystemPerformanceScreenModel : ScreenModel
    {

        /// <summary>
        ///     Whether or not the subsystem is online.
        /// </summary>
        [NotNull]
        private readonly Observable<bool> _isOnline;

        /// <summary>
        ///     The performance widgets.
        /// </summary>
        [NotNull, ItemNotNull]
        private readonly ObservableList<IWidget> _widgets;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <param name="subsystem"> The system monitoring subsystem. </param>
        public SystemPerformanceScreenModel([NotNull] SystemMonitoringSubsystem subsystem)
            : base("PERF")
        {
            Contract.Requires(subsystem != null);

            Subsystem = subsystem;
            _widgets = new ObservableList<IWidget>();
            _isOnline = new Observable<bool>();
        }

        /// <summary>
        ///     The system monitoring subsystem.
        /// </summary>
        [NotNull]
        public SystemMonitoringSubsystem Subsystem { get; }

        /// <summary>
        ///     The performance widgets.
        /// </summary>
        [NotNull, ItemNotNull]
        public ObservableList<IWidget> Widgets
        {
            [DebuggerStepThrough]
            get
            {
                Contract.Ensures(Contract.Result<ObservableList<IWidget>>() != null);

                return _widgets;
            }
        }

        /// <summary>
        ///     Gets a value indicating whether the subsystem is online.
        /// </summary>
        /// <value>
        ///     true if the subsystem is online, false if not.
        /// </value>
        public bool IsSubsystemOnline
        {
            get { return _isOnline; }
        }


        /// <summary>
        ///     Contains code contract invariants that describe facts about this class that will be true
        ///     after any public method in this class is called.
        /// </summary>
        [ContractInvariantMethod]
        private void ClassInvariants()
        {
            //- Subsystem cannot be null
            Contract.Invariant(Subsystem != null);

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
            /* Since the subsystem doesn't raise NPC we can't bind to it directly, 
               but we can update our internal values every cycle. */
            _isOnline.Value = Subsystem.IsOnline;

            // Ensure we have widgets for everything present.
            UpdateWidgetsCollection();
        }

        /// <summary>
        ///     Updates the widgets collection by adding new widgets or removing old widgets when offline.
        /// </summary>
        private void UpdateWidgetsCollection()
        {
            var numWidgetsInDisplay = _widgets.Count;

            // If we're offline, make sure that we have all widgets cleared
            if (!Subsystem.IsOnline)
            {
                if (numWidgetsInDisplay > 0)
                {
                    _widgets.Clear();
                }
                return;
            }

            // If we've already populated widgets on the prior run, don't worry about it
            if (numWidgetsInDisplay != 0) return;

            // Copy all widgets from all modules into the widgets collection
            foreach (var widget in Subsystem.SystemModules.SelectMany(module => module.Widgets))
            {
                _widgets.Add(widget);
            }
        }
    }
}