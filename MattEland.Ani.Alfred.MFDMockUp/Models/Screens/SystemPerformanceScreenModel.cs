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
using MattEland.Ani.Alfred.MFDMockUp.Models.Buttons;
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

        private readonly List<ButtonModel> _commandButtons;

        [NotNull]
        private readonly Observable<bool> _showMemory;
        [NotNull]
        private readonly Observable<bool> _showCPU;
        [NotNull]
        private readonly Observable<bool> _showDiskRead;
        [NotNull]
        private readonly Observable<bool> _showDiskWrite;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <param name="subsystem"> The system monitoring subsystem. </param>
        public SystemPerformanceScreenModel([NotNull] SystemMonitoringSubsystem subsystem)
            : base("PERF")
        {
            Contract.Requires(subsystem != null);

            Subsystem = subsystem;

            //- Set Up Observables
            _widgets = new ObservableList<IWidget>();
            _isOnline = new Observable<bool>();
            _showMemory = new Observable<bool>(true);
            _showCPU = new Observable<bool>(true);
            _showDiskRead = new Observable<bool>(true);
            _showDiskWrite = new Observable<bool>(true);


            // Set up the filtering buttons
            var memButton = new ActionButtonModel("MEM", () => ShowMemory = !ShowMemory, () => ShowMemory);
            var cpuButton = new ActionButtonModel("CPU", () => ShowCPU = !ShowCPU, () => ShowCPU);
            var diskReadButton = new ActionButtonModel("DSKR", () => ShowDiskRead = !ShowDiskRead, () => ShowDiskRead);
            var diskWriteButton = new ActionButtonModel("DSKW", () => ShowDiskWrite = !ShowDiskWrite, () => ShowDiskWrite);

            // Build out the command list
            _commandButtons = new List<ButtonModel>
            {
                memButton,
                cpuButton,
                BuildModeSwitchButton(),
                diskReadButton,
                diskWriteButton
            };

        }

        /// <summary>
        ///     Gets or sets a value indicating whether memory is shown.
        /// </summary>
        /// <value>
        ///     true if memory usage is shown, false if not.
        /// </value>
        public bool ShowMemory
        {
            get { return _showMemory; }
            set
            {
                if (ShowMemory == value) return;

                _showMemory.Value = value;
                UpdateWidgetsCollection();
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether CPU usage is shown.
        /// </summary>
        /// <value>
        ///     true if CPU usage is shown, false if not.
        /// </value>
        public bool ShowCPU
        {
            get { return _showCPU; }
            set
            {
                if (ShowCPU == value) return;

                _showCPU.Value = value;
                UpdateWidgetsCollection();
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether disk read usage is shown.
        /// </summary>
        /// <value>
        ///     true if disk read usage is shown, false if not.
        /// </value>
        public bool ShowDiskRead
        {
            get { return _showDiskRead; }
            set
            {
                if (ShowDiskRead == value) return;

                _showDiskRead.Value = value;
                UpdateWidgetsCollection();
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether disk write usage is shown.
        /// </summary>
        /// <value>
        ///     true if disk write usage is shown, false if not.
        /// </value>
        public bool ShowDiskWrite
        {
            get { return _showDiskWrite; }
            set
            {
                if (ShowDiskWrite == value) return;

                _showDiskWrite.Value = value;
                UpdateWidgetsCollection();
            }
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
        public IEnumerable<IWidget> Widgets
        {
            [DebuggerStepThrough]
            get
            {
                Contract.Ensures(Contract.Result<IEnumerable<IWidget>>() != null);

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
            if (!_widgets.Any())
            {
                UpdateWidgetsCollection();
            }
        }

        /// <summary>
        ///     Repopulates the widgets collection based on which widgets should be visible.
        /// </summary>
        private void UpdateWidgetsCollection()
        {
            // Reset the collection
            _widgets.Clear();

            // Add Widgets that are enabled
            if (ShowMemory)
            {
                foreach (var widget in Subsystem.MemoryModule.Widgets)
                {
                    _widgets.Add(widget);
                }
            }
            if (ShowCPU)
            {
                foreach (var widget in Subsystem.CpuModule.Widgets)
                {
                    _widgets.Add(widget);
                }
            }
            if (ShowDiskRead)
            {
                _widgets.Add(Subsystem.DiskModule.ReadWidget);
            }
            if (ShowDiskWrite)
            {
                _widgets.Add(Subsystem.DiskModule.WriteWidget);
            }
        }

        /// <summary>
        ///     Gets the command buttons associated with this screen. Return null for default commands.
        /// </summary>
        /// <value>
        ///     The command buttons.
        /// </value>
        public override IList<ButtonModel> CommandButtons
        {
            get
            {
                return _commandButtons;
            }
        }


    }
}